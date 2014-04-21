using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FlowController : MonoBehaviour {

    GameObject _level;
    public int MinFlowDelta = 30;
    public int MaxFlowDelta = 40;
    public int MinFlowTime = 8;
    public int MaxFlowTime = 12;
    public int FlowHeightPerTick = 3;

    int _count = 0;
    int _flowcount = 0;
    int _flowdelta = 0;
    int _flowtime = 0;
    List<GameObject> _lavasource = new List<GameObject>();
    bool[][] _hadlava;

	void Start ()
    {
        _flowdelta = Random.Range(MinFlowDelta, MaxFlowDelta);
        _flowtime = Random.Range(MinFlowTime, MaxFlowTime);
        _hadlava = new bool[Level.LevelWidth][];
        for (int x = 0; x < Level.LevelWidth; x++)
        {
            _hadlava[x] = new bool[Level.LevelHeight];
        }
		StartCoroutine("Eruption");
	}

    public void SetLevel(GameObject level)
    {
        _level = level;
    }
	
	void Update ()
    {
        _count++;
        if (_count == _flowdelta || Input.GetKeyDown(KeyCode.Space))
        {
            _count = 0;
            _flowdelta = Random.Range(MinFlowDelta, MaxFlowDelta);
            // tick all lava, and then flow in more from the stream source.
            for (int y = 0; y < Level.LevelHeight; y++)
            {
              for (int x = 0; x < Level.LevelWidth; x++)
                {
                    _hadlava[x][y] = _level.GetComponent<Level>()._tiles[x][y].GetComponent<Tile>().HasLava;
                }
            }
            for (int y = 0; y < Level.LevelHeight; y++)
            {
                for (int x = 0; x < Level.LevelWidth; x++)
                {
                    if (_hadlava[x][y]) _level.GetComponent<Level>()._tiles[x][y].GetComponent<Tile>().TrickleDown();
                }
            }
			foreach(GameObject tile in _lavasource)
			{
            	tile.GetComponent<Tile>().PatrickFlowIn(FlowHeightPerTick);
			}
            _flowcount++;
            if (_flowcount == _flowtime)
            {
                _flowcount = 0;
                _flowtime = Random.Range(MinFlowTime, MaxFlowTime);
				_lavasource.Clear();
                NewStream();
            }
        }
	}

	IEnumerator Eruption()
	{
		while(true)
		{
			yield return new WaitForSeconds(60);
			_flowcount = 0;
			_flowtime = Random.Range(MinFlowTime, MaxFlowTime);
			for(int i = 0; i < 6; i++)
			{
				NewStream();
			}
		}
	}

    public void NewStream()
    {
        // todo: make this random distribution more uniform. most important RNG.
        // TODO TODO TODO
        GameObject tile = _level.GetComponent<Level>()._tiles[Random.Range(0, Level.LevelWidth - 1)][Level.LevelHeight - 1];
        tile.GetComponent<Tile>().GroundHeight = 2;
        tile.GetComponent<Tile>().LavaHeight = FlowHeightPerTick;
        _lavasource.Add(tile);
    }
}
