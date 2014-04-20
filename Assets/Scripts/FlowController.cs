using UnityEngine;
using System.Collections;

public class FlowController : MonoBehaviour {

    GameObject _level;
    public int MinFlowDelta = 80;
    public int MaxFlowDelta = 100;
    public int MinFlowTime = 8;
    public int MaxFlowTime = 12;

    int _count = 0;
    int _flowcount = 0;
    int _flowdelta = 0;
    int _flowtime = 0;
    GameObject _lavasource;
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
            _lavasource.GetComponent<Tile>().PatrickFlowIn();
            _flowcount++;
            if (_flowcount == _flowtime)
            {
                _flowcount = 0;
                _flowtime = Random.Range(MinFlowTime, MaxFlowTime);
                NewStream();
            }
        }
	}

    public void NewStream()
    {
        GameObject tile = _level.GetComponent<Level>()._tiles[Random.Range(0, Level.LevelWidth - 1)][Level.LevelHeight - 1];
        tile.GetComponent<Tile>().GroundHeight = 2;
        tile.GetComponent<Tile>().LavaHeight = 1;
        _lavasource = tile;
    }
}
