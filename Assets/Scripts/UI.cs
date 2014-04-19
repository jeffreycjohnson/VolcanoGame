using UnityEngine;

public class UI : MonoBehaviour {
	public Texture2D Building;
	public Texture2D Wall;
	public Texture2D Generator;
	public Texture2D Dig;

	private GameObject clicked;

	void OnGUI() {
		if(Input.GetJoystickNames().Length == 0) {
			if(GUI.Button(new Rect(10, 10, 100, 100), Building)) {
			}
			if(GUI.Button(new Rect(120, 10, 100, 100), Wall)) {
			}
			if(GUI.Button(new Rect(230, 10, 100, 100), Generator)) {
			}
			if(GUI.Button(new Rect(340, 10, 100, 100), Dig)) {
			}
		}
	}

	void Update() {
		if(Input.GetButtonDown("A")) {
		}
		if(Input.GetButtonDown("B")) {
		}
		if(Input.GetButtonDown("X")) {
		}
		if(Input.GetButtonDown("Y")) {
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
