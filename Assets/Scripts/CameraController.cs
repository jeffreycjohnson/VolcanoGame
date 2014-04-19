using UnityEngine;

public class CameraController : MonoBehaviour {
	void Update () {
		if(Input.GetJoystickNames().Length > 0) {
			State.selectedGrid.x += Input.GetAxis("Horizontal") * Level.LevelWidth / (2 * Mathf.PI);
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
				State.selectedGrid.x : Level.LevelWidth + State.selectedGrid.x;
			State.selectedGrid.x = State.selectedGrid.x < Level.LevelWidth ?
				State.selectedGrid.x : State.selectedGrid.x - Level.LevelWidth;

			State.selectedGrid.y += Input.GetAxis("Vertical");
			State.selectedGrid.y = State.selectedGrid.y > 0?
				State.selectedGrid.y : State.selectedGrid.y + Level.LevelHeight;
			State.selectedGrid.y = State.selectedGrid.y < Level.LevelHeight ?
				State.selectedGrid.y : State.selectedGrid.y - Level.LevelHeight;
		}
		transform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Horizontal"));
	}
}
