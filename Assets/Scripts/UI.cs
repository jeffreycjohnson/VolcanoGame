using UnityEngine;

public class UI : MonoBehaviour {
	public Texture2D Building;
	public Texture2D Wall;
	public Texture2D Generator;
	public Texture2D Dig;
	public Transform level;
	
	public Texture2D A;
	public Texture2D B;
	public Texture2D X;
	public Texture2D Y;

	private GameObject clicked;

	void OnGUI() {
		GUI.skin.label.fontSize = 24;
		if(GUI.Button(new Rect(10, 10, 100, 100), Building)) {
			clicked.transform.FindChild("Structure").GetComponent<Structure>().buildBuilding();
		}
		if(GUI.Button(new Rect(10, 120, 100, 100), Wall)) {
			clicked.transform.FindChild("Structure").GetComponent<Structure>().buildWall();
		}
		if(GUI.Button(new Rect(10, 230, 100, 100), Generator)) {
			clicked.transform.FindChild("Structure").GetComponent<Structure>().buildGenerator();
		}
		if(GUI.Button(new Rect(10, 340, 100, 100), Dig)) {
		}
		if(Input.GetJoystickNames().Length == 0) {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
			GUI.Label(new Rect(55, 42, 20, 40), "1");
			GUI.Label(new Rect(55, 152, 20, 40), "2");
			GUI.Label(new Rect(55, 262, 20, 40), "3");
			GUI.Label(new Rect(55, 372, 20, 40), "4");
#endif
		}
		else {
			GUI.Label(new Rect(120, 35, 40, 40), A);
			GUI.Label(new Rect(120, 145, 40, 40),B);
			GUI.Label(new Rect(120, 255, 40, 40), X);
			GUI.Label(new Rect(120, 365, 40, 40), Y);
		}
		GUI.Label(new Rect(Screen.width - 200, 25, 200, 50), "Money: " + ((int)State.money).ToString());
	}

	void Update() {
		if(Input.GetButtonDown("A")) {
			level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].transform.FindChild("Structure").GetComponent<Structure>().buildBuilding();
		}
		if(Input.GetButtonDown("B")) {
			level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].transform.FindChild("Structure").GetComponent<Structure>().buildWall();
		}
		if(Input.GetButtonDown("X")) {
			level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].transform.FindChild("Structure").GetComponent<Structure>().buildGenerator();
		}
		if(Input.GetButtonDown("Y")) {
            if (level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].GetComponent<Tile>().LavaHeight == 0)
            {
                //level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].GetComponent<Tile>().LavaHeight -= 2;
                level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].GetComponent<Tile>().GroundHeight -= 3;
            }
		}
		if(Input.GetKeyDown("mouse 0")) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
				if(clicked != null) {
					clicked.GetComponent<Tile>().highlighted = false;
				}
				clicked = hit.collider.gameObject;
				clicked.GetComponent<Tile>().highlighted = true;
			}
		}
		for(int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			if(touch.phase == TouchPhase.Began) {
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit)) {
					if(clicked != null) {
						clicked.GetComponent<Tile>().highlighted = false;
					}
					clicked = hit.collider.gameObject;
					clicked.GetComponent<Tile>().highlighted = true;
				}
			}
		}
	}
}
