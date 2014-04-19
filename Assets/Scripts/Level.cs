using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public GameObject TileTemplate;
    public int width = 20;
    public int height = 20;
    GameObject[][] _tiles;

	// Use this for initialization
	void Start ()
    {
        _tiles = new GameObject[width][];
        for (int i = 0; i < width; i++)
        {
            _tiles[i] = new GameObject[height];
        }
        //GameObject tile = (GameObject)Instantiate(TileTemplate);
        //tile.gameObject.transform.parent = gameObject.transform;
        //tile.GetComponent<Tile>().SetLevel(gameObject); // TODO: give tile a reference of the level.
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    GameObject GetTile(int x, int y)
    {
        return _tiles[x][y];
    }
}
