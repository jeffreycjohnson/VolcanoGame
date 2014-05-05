using UnityEngine;

public class UI : MonoBehaviour
{
    public Texture2D Building;
    public Texture2D Wall;
    public Texture2D Generator;
    public Texture2D Dig;

    public Texture2D A;
    public Texture2D B;
    public Texture2D X;
    public Texture2D Y;

    public Texture2D K1;
    public Texture2D K2;
    public Texture2D K3;
    public Texture2D K4;

    public int DigCost = 50;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnGUI()
    {
        if (State.defeated)
        {
            GUI.skin.label.fontSize = 64;
            GUI.Label(new Rect(Screen.width / 2 - 128, Screen.height / 2 - 32, Screen.width / 2, 64), "You Lose");
            return;
        }

        GUI.skin.label.fontSize = 24;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        int size = 100 + Screen.height / 30;
#else
		int size = 100 + Screen.height / 10;
#endif
        int pad = 15 + Screen.height / 100;

        if (GUI.Button(new Rect(pad, 1 * pad + size * 0, size, size), Building))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildBuilding();
        }
        if (GUI.Button(new Rect(pad, 2 * pad + size * 1, size, size), Wall))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildWall();
        }
        if (GUI.Button(new Rect(pad, 3 * pad + size * 2, size, size), Generator))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildGenerator();
        }
        if (GUI.Button(new Rect(pad, 4 * pad + size * 3, size, size), Dig))
        {
            if (State.selected.GetComponent<Tile>().LavaHeight == 0 && State.money > DigCost)
            {
                State.money -= DigCost;
                State.selected.GetComponent<Tile>().GroundHeight -= 3;
            }
        }
        if (Input.GetJoystickNames().Length == 0)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
            GUI.Label(new Rect(pad + 5, 2 + pad * 1 + size * 0, size - 4, size - 4), K1);
            GUI.Label(new Rect(pad + 5, 2 + pad * 2 + size * 1, size - 4, size - 4), K2);
            GUI.Label(new Rect(pad + 5, 2 + pad * 3 + size * 2, size - 4, size - 4), K3);
            GUI.Label(new Rect(pad + 5, 2 + pad * 4 + size * 3, size - 4, size - 4), K4);
#endif
        }
        else
        {
            GUI.Label(new Rect(2 * pad + size, 25 + pad * 1 + size * 0, 40, 40), A);
            GUI.Label(new Rect(2 * pad + size, 25 + pad * 2 + size * 1, 40, 40), B);
            GUI.Label(new Rect(2 * pad + size, 25 + pad * 3 + size * 2, 40, 40), X);
            GUI.Label(new Rect(2 * pad + size, 25 + pad * 4 + size * 3, 40, 40), Y);
        }
        GUI.Label(new Rect(Screen.width - 200, 25, 200, 50), "Money: " + ((int)State.money).ToString());
    }

    void Update()
    {
        if (State.defeated)
        {
            return;
        }
        if (Input.GetButtonDown("K1"))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildBuilding();
        }
        if (Input.GetButtonDown("K2"))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildWall();
        }
        if (Input.GetButtonDown("K3"))
        {
            State.selected.transform.FindChild("Structure").GetComponent<Structure>().buildGenerator();
        }
        if (Input.GetButtonDown("K4"))
        {
            if (State.selected.GetComponent<Tile>().LavaHeight == 0 && State.money > DigCost)
            {
                State.money -= DigCost;
                State.selected.GetComponent<Tile>().GroundHeight -= 3;
            }
        }
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown("mouse 0"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                State.selected.GetComponent<Tile>().highlighted = false;
                State.selected = hit.collider.transform.parent.gameObject;
                State.selected.GetComponent<Tile>().highlighted = true;
            }
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                {
                    State.selected.GetComponent<Tile>().highlighted = false;
                    State.selected = hit.collider.transform.parent.gameObject;
                    State.selected.GetComponent<Tile>().highlighted = true;
                    GetComponent<CameraController>().Velocity = 0f;
                }
            }
        }
    }
}
