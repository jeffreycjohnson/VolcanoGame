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

        // slices of the cone
        for (int y = 0; y < Height; y++)
        {
            float tiley = ((float)y * (volcanoheight / Height));
            float radius = volcanoradius - ((volcanoradius - volcanotopradius) / Height) * y;
            float scalar = 1 + 1.4f * (Height - y) / Height;
            // create each tile of the slice
            for (int x = 0; x < Width; x++)
            {
                GameObject tile = (GameObject)Instantiate(TileTemplate);
                tile.gameObject.transform.parent = gameObject.transform;
                float theta = (Mathf.PI * 2f / (float)Width) * x;
                tile.transform.position = new Vector3(Mathf.Cos(theta) * radius, tiley, Mathf.Sin(theta) * radius);
                tile.transform.localScale *= 1.75f;
                tile.transform.localScale *= scalar;
                //tile.transform.RotateAround(gameObject.transform.position - (gameObject.transform.position + new Vector3(0, 1, 0)), theta);
                //tile.transform.position += new Vector3(0, tiley, 0);
                //tile.GetComponent<Tile>().InitializeLevelData(x, y, gameObject);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(1, 0, 0)), volcanoheight / (volcanoradius - volcanotopradius) * 360, Space.Self);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 1, 0)), theta * 360 / (Mathf.PI * 2), Space.Self);
                _tiles[x][y] = tile;
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
