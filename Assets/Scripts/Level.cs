using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

    public GameObject TileTemplate;
    public GameObject FlowController;
    public int LevelWidth = 25;
    public int LevelHeight = 15;
    public float volcanoheight = 25;
    public float volcanoradius = 20;
    public float volcanotopradius = 5;
    public float MoneyPerUpdate = 4f;
    public GameObject[][] _tiles { get; private set; }

	void Start ()
    {
        State.level = this;
        _tiles = new GameObject[LevelWidth][];
        for (int x = 0; x < LevelWidth; x++)
        {
            _tiles[x] = new GameObject[LevelHeight];
        }
        float tiley = 0;
        float radius = volcanoradius;
        // slices of the cone
        for (int y = 0; y < LevelHeight; y++)
        {
            float scalar = 2 * radius * Mathf.Tan(Mathf.PI / (float)LevelWidth) * 1.2f;
            // create each tile of the slice
            for (int x = 0; x < LevelWidth; x++)
            {
                GameObject tile = (GameObject)Instantiate(TileTemplate);
                tile.name = (x.ToString() + "," + y);
                tile.GetComponent<Tile>().InitializeLevelData(x, y, gameObject);
                tile.gameObject.transform.parent = gameObject.transform;
                _tiles[x][y] = tile;

                float theta = (Mathf.PI * 2f / (float)LevelWidth) * x;
                tile.transform.position = new Vector3(Mathf.Cos(theta) * radius, tiley, Mathf.Sin(theta) * radius);
                tile.transform.localScale *= scalar;
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 0, 1)),
                    RadiansToDegrees(Mathf.Tan(volcanoheight) * (volcanoradius - volcanotopradius)), Space.Self);
                tile.transform.Rotate(tile.transform.position - (tile.transform.position + new Vector3(0, 1, 0)),
                    RadiansToDegrees(theta), Space.World);
            }
            scalar *= 1.01f; // squishes them together
            tiley += (volcanoheight / LevelHeight) * scalar / 2;
            radius = (volcanoradius - tiley * (volcanoradius / volcanoheight) * 0.47f);
        }

        StartCoroutine(RandomizeGround());
	}

    IEnumerator RandomizeGround()
    {
        yield return new WaitForFixedUpdate();
        float seed = Random.value;
        for (int i = 0; i < LevelWidth; i++)
        {
            for (int j = 0; j < LevelHeight; j++)
            {
                //_tiles[i][j].GetComponent<Tile>().GroundHeight = Random.Range(0, (int)(DynamicHeight.MaxHeight * 0.7f));
                //_tiles[i][j].GetComponent<Tile>().GroundHeight = Random.Range((int)(DynamicHeight.MaxHeight * 0.2f), (int)(DynamicHeight.MaxHeight * 0.6f));
                _tiles[i][j].GetComponent<Tile>().GroundHeight = (int)(0f * DynamicHeight.MaxHeight) +
                    (int)(Mathf.PerlinNoise(((float)j / LevelWidth + seed) * 7f, ((float)i / LevelHeight + seed) * 7f) * (float)DynamicHeight.MaxHeight * 0.9f);
                _tiles[i][j].GetComponent<Tile>().LavaHeight = 0;
                _tiles[i][j].GetComponent<Tile>().SetHighlightHeight();
            }
        }
        //_tiles[0][1].GetComponent<Tile>().getChild(Tile.ChildNames.Structure).GetComponent<Structure>().buildBuilding(true);
        //_tiles[Width / 2][1].GetComponent<Tile>().getChild(Tile.ChildNames.Structure).GetComponent<Structure>().buildGenerator(true);
        GameObject controller = (GameObject)Instantiate(FlowController);
        controller.GetComponent<FlowController>().SetLevel(gameObject);
        controller.GetComponent<FlowController>().NewStream();

        _tiles[19][0].transform.FindChild("Structure").GetComponent<Structure>().buildBase();
    }

	void Update ()
    {
        State.money += MoneyPerUpdate * Time.deltaTime;
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
