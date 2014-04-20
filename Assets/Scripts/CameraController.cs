using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform level;
	private bool started = false;

	void Update () {
		if(!started) {
			State.selectedGrid = new Vector2(Level.LevelWidth * 0.772f, 0);
			started = true;
		}
		if(Input.GetJoystickNames().Length > 0) {
			level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].GetComponent<Tile>().highlighted = false;

			State.selectedGrid.x += Input.GetAxis("Horizontal") * Level.LevelWidth / 360;
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
				State.selectedGrid.x : Level.LevelWidth + State.selectedGrid.x;
			State.selectedGrid.x = State.selectedGrid.x < Level.LevelWidth ?
				State.selectedGrid.x : State.selectedGrid.x - Level.LevelWidth;

			State.selectedGrid.y += Input.GetAxis("Vertical") / 10;
			State.selectedGrid.y = State.selectedGrid.y > 0?
				State.selectedGrid.y : State.selectedGrid.y + Level.LevelHeight;
			State.selectedGrid.y = State.selectedGrid.y < Level.LevelHeight ?
				State.selectedGrid.y : State.selectedGrid.y - Level.LevelHeight;

			level.GetComponent<Level>()._tiles[(int)State.selectedGrid.x][(int)State.selectedGrid.y].GetComponent<Tile>().highlighted = true;
		}
		transform.RotateAround(Vector3.zero, Vector3.up, -Input.GetAxis("Horizontal"));
		transform.RotateAround(Vector3.zero, Vector3.up, -Input.acceleration.x * 3);
	}
}
