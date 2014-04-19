using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public GameObject TileTemplate;
    public int Width = 20;
    public int Height = 12;
    public float volcanoheight = 25;
    public float volcanoradius = 14;
    public float volcanotopradius = 5;
    GameObject[][] _tiles;

    public static int LevelWidth;
    public static int LevelHeight;

	void Start ()
    {
        LevelWidth = Width;
        LevelHeight = Height;
        _tiles = new GameObject[Width][];
        for (int x = 0; x < Width; x++)
        {
            _tiles[x] = new GameObject[Height];
        }

        Vector3 origin = gameObject.transform.position;
        //float radius = volcanoradius;
        //float dradius = -(radius* 0.5f) / Height;
        // slices of the cone
        for (int y = 0; y < Height; y++)
        {
            float tiley = origin.y + ((float)y * (volcanoheight / Height));
            float radius = volcanoradius - ((volcanoradius - volcanotopradius) / Height) * y;
            // create each tile of the slice
            for (int x = 0; x < Width; x++)
            {
                float theta = (Mathf.PI * 2f / (float)Width) * x;
                GameObject tile = (GameObject)Instantiate(TileTemplate);
                tile.gameObject.transform.parent = gameObject.transform;
                tile.transform.position = new Vector3(Mathf.Cos(theta) * radius, tiley, Mathf.Sin(theta) * radius);
                //tile.GetComponent<Tile>().InitializeLevelData(x, y, gameObject);
                // TODO: set angle, and maybe scale?
            }
        }
	}
	
	void Update ()
    {
	
	}

    public GameObject GetTile(int x, int y)
    {
        return _tiles[x][y];
    }
}
