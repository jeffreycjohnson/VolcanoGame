using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public GameObject TileTemplate;
    public GameObject FlowController;
    public int Width = 25;
    public int Height = 15;
    public float volcanoheight = 25;
    public float volcanoradius = 20;
    public float volcanotopradius = 5;
    public GameObject[][] _tiles;

    public static int LevelWidth = 25; // note: change these too!
    public static int LevelHeight = 20;
    public float MoneyPerUpdate = 0.05f;

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
            float scalar = 2 * radius * Mathf.Tan(Mathf.PI / (float)Width) * 1.2f;
            // create each tile of the slice
            for (int x = 0; x < Width; x++)
            {
                GameObject tile = (GameObject)Instantiate(TileTemplate);
                tile.name = (x.ToString() + "," + y);
                tile.GetComponent<Tile>().InitializeLevelData(x, y, gameObject);
                tile.gameObject.transform.parent = gameObject.transform;
                _tiles[x][y] = tile;

                float theta = (Mathf.PI * 2f / (float)Width) * x;
                tile.transform.position = new Vector3(Mathf.Cos(theta) * radius, tiley, Mathf.Sin(theta) * radius);
                tile.transform.localScale *= scalar;
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 0, 1)),
                    RadiansToDegrees(Mathf.Tan(volcanoheight) * (volcanoradius - volcanotopradius)), Space.Self);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 1, 0)),
                    RadiansToDegrees(theta), Space.World);
            }
            scalar *= 1.01f; // squishes them together
            tiley += (volcanoheight / Height) * scalar / 2;
            radius = (volcanoradius - tiley * (volcanoradius / volcanoheight) * 0.47f);
        }

        StartCoroutine(RandomizeGround());
	}

    IEnumerator RandomizeGround()
    {
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                //_tiles[i][j].GetComponent<Tile>().GroundHeight = Random.Range(0, (int)(DynamicHeight.MaxHeight * 0.7f));
                //_tiles[i][j].GetComponent<Tile>().GroundHeight = Random.Range((int)(DynamicHeight.MaxHeight * 0.2f), (int)(DynamicHeight.MaxHeight * 0.6f));
                _tiles[i][j].GetComponent<Tile>().GroundHeight = (int)(0f * DynamicHeight.MaxHeight) + 
                    (int)(Mathf.PerlinNoise((float)j / Width * 7f, (float)i / Height * 7f) * (float)DynamicHeight.MaxHeight * 0.9f);
                _tiles[i][j].GetComponent<Tile>().LavaHeight = 0;
                _tiles[i][j].GetComponent<Tile>().SetHighlightHeight();
            }
        }
        _tiles[0][1].GetComponent<Tile>().getChild(Tile.ChildNames.Structure).GetComponent<Structure>().buildBuilding(true);
        _tiles[Width / 2][1].GetComponent<Tile>().getChild(Tile.ChildNames.Structure).GetComponent<Structure>().buildGenerator(true);
        GameObject controller = (GameObject)Instantiate(FlowController);
        controller.GetComponent<FlowController>().SetLevel(gameObject);
        controller.GetComponent<FlowController>().NewStream();
    }

	void Update ()
    {
        State.money += MoneyPerUpdate;
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
