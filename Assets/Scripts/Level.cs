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
        float tiley = 0;
        float radius = volcanoradius;
        // slices of the cone
        for (int y = 0; y < Height; y++)
        {
            //float tiley = ((float)y * (volcanoheight / Height));
            
            //float radius = volcanoradius - ((volcanoradius - volcanotopradius) / Height) * y;
            
            
            //float scalar = 1 + 1.7f * (Height - y) / Height;
            //float scalar = 2 * radius * Mathf.Tan(Mathf.PI / (float)Width) * 1.28f;
            float scalar = 2 * radius * Mathf.Tan(Mathf.PI / (float)Width) * 1.2f;
            // create each tile of the slice
            for (int x = 0; x < Width; x++)
            {
                GameObject tile = (GameObject)Instantiate(TileTemplate);
                tile.gameObject.transform.parent = gameObject.transform;
                float theta = (Mathf.PI * 2f / (float)Width) * x;
                tile.transform.position = new Vector3(Mathf.Cos(theta) * radius, tiley, Mathf.Sin(theta) * radius);
                tile.transform.localScale *= scalar;
                //tile.transform.position += new Vector3(0, tiley, 0);
                //tile.GetComponent<Tile>().InitializeLevelData(x, y, gameObject);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 0, 1)),
                    -RadiansToDegrees(Mathf.Tan(volcanoheight) * (volcanoradius - volcanotopradius)), Space.Self);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 1, 0)),
                    RadiansToDegrees(theta), Space.World);
                _tiles[x][y] = tile;
            }
            //tiley += (volcanoheight / Height) * scalar * 0.42f;
            scalar *= 0.8f;
            tiley += (volcanoheight / Height) * scalar / 2;
            //radius = (volcanoradius - tiley * (volcanoradius / volcanoheight)) * 1.1f;
            radius = (volcanoradius - tiley * (volcanoradius / volcanoheight) * 0.675f);
        }
	}

	void Update ()
    {
	
	}

    float RadiansToDegrees(float rad)
    {
        return rad / (Mathf.PI * 2f) * 360;
    }

    public GameObject GetTile(int x, int y)
    {
        return _tiles[x][y];
    }
}
