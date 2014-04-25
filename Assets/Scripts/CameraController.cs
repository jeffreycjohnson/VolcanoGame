using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform level;
	public float friction = .4f;
	public float maxSpeed = 20f;
	public float velocity = 0f;
	private float rotateToSpeed = 0f;
	private bool started = false;

	void Update () {
		if(velocity - friction > 0) {
			velocity -= friction;
			velocity = System.Math.Min(velocity, maxSpeed);
		}
		else if(velocity + friction < 0) {
			velocity += friction;
			velocity = System.Math.Max(velocity, -maxSpeed);
		}
		if(!started) {
			State.selectedGrid = new Vector2(Level.LevelWidth * 0.78f, 0);
			transform.RotateAround(Vector3.zero, Vector3.up, -3.75f);
			State.selected = level.GetComponent<Level>().GetTile((int)State.selectedGrid.x, (int)State.selectedGrid.y);
			State.selected.GetComponent<Tile>().highlighted = true;
			started = true;
		}
		if(System.Math.Abs(Input.GetAxis("Horizontal")) > 0.01f || System.Math.Abs(Input.GetAxis("Vertical")) > 0.01f || System.Math.Abs(velocity) > friction) {
			rotateToSpeed = 0f;
			State.selected.GetComponent<Tile>().highlighted = false;

			State.selectedGrid.x += (Input.GetAxis("Horizontal") - velocity) * Level.LevelWidth / 360;
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
				State.selectedGrid.x : Level.LevelWidth + State.selectedGrid.x;
			State.selectedGrid.x = State.selectedGrid.x < Level.LevelWidth ?
				State.selectedGrid.x : State.selectedGrid.x - Level.LevelWidth;

			State.selectedGrid.y += Input.GetAxis("Vertical") / 10;
			State.selectedGrid.y = State.selectedGrid.y > 0?
				State.selectedGrid.y : State.selectedGrid.y + Level.LevelHeight;
			State.selectedGrid.y = State.selectedGrid.y < Level.LevelHeight ?
				State.selectedGrid.y : State.selectedGrid.y - Level.LevelHeight;
			
			State.selected = level.GetComponent<Level>().GetTile((int)State.selectedGrid.x, (int)State.selectedGrid.y);

			State.selected.GetComponent<Tile>().highlighted = true;

			transform.RotateAround(Vector3.zero, Vector3.up, -Input.GetAxis("Horizontal"));
			transform.RotateAround(Vector3.zero, Vector3.up, velocity);
		}
		else if(State.selected != null) {
			velocity = 0f;
			State.selectedGrid.y = State.selected.GetComponent<Tile>()._y;

			State.selectedGrid.x += rotateToSpeed * Level.LevelWidth / 360;
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
				State.selectedGrid.x : Level.LevelWidth + State.selectedGrid.x;
			State.selectedGrid.x = State.selectedGrid.x < Level.LevelWidth ?
				State.selectedGrid.x : State.selectedGrid.x - Level.LevelWidth;

			rotateToSpeed = State.selected.GetComponent<Tile>()._x + 0.5f - State.selectedGrid.x;
			if(rotateToSpeed > Level.LevelWidth / 2) {
				rotateToSpeed -= Level.LevelWidth;
			}
			else if(rotateToSpeed < -Level.LevelWidth / 2) {
				rotateToSpeed += Level.LevelWidth;
			}
			transform.RotateAround(Vector3.zero, Vector3.up, -rotateToSpeed);
		}
		foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Moved) {
				velocity = touch.deltaPosition.x * Time.deltaTime * 60;
			}
		}
	}
}
