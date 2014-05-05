using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform level;
	public float Friction = .4f;
	public float MaxSpeed = 20f;
	public float Velocity = 0f;
	private float _rotatetospeed = 0f;
	private bool _started = false;

	void Update ()
    {
		if(Velocity - Friction > 0)
        {
			Velocity -= Friction * Time.deltaTime * 60;
			Velocity = System.Math.Min(Velocity, MaxSpeed);
		}
		else if(Velocity + Friction < 0)
        {
            Velocity += Friction * Time.deltaTime * 60;
			Velocity = System.Math.Max(Velocity, -MaxSpeed);
		}
		if(!_started)
        {
            State.selectedGrid = new Vector2(State.level.LevelWidth * 0.78f, 0);
			transform.RotateAround(Vector3.zero, Vector3.up, -3.75f);
			State.selected = State.level.GetComponent<Level>().GetTile((int)State.selectedGrid.x, (int)State.selectedGrid.y);
			State.selected.GetComponent<Tile>().highlighted = true;
			_started = true;
		}
		if(System.Math.Abs(Input.GetAxis("Horizontal")) > 0.01f || System.Math.Abs(Input.GetAxis("Vertical")) > 0.01f || System.Math.Abs(Velocity) > Friction)
        {
			_rotatetospeed = 0f;
			State.selected.GetComponent<Tile>().highlighted = false;

			State.selectedGrid.x += (Input.GetAxis("Horizontal") - Velocity) * State.level.LevelWidth / 360;
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
                State.selectedGrid.x : State.level.LevelWidth + State.selectedGrid.x;
            State.selectedGrid.x = State.selectedGrid.x < State.level.LevelWidth ?
                State.selectedGrid.x : State.selectedGrid.x - State.level.LevelWidth;

			State.selectedGrid.y += Input.GetAxis("Vertical") / 10;
			State.selectedGrid.y = State.selectedGrid.y > 0?
                State.selectedGrid.y : State.selectedGrid.y + State.level.LevelHeight;
            State.selectedGrid.y = State.selectedGrid.y < State.level.LevelHeight ?
                State.selectedGrid.y : State.selectedGrid.y - State.level.LevelHeight;
			
			State.selected = level.GetComponent<Level>().GetTile((int)State.selectedGrid.x, (int)State.selectedGrid.y);

			State.selected.GetComponent<Tile>().highlighted = true;

			transform.RotateAround(Vector3.zero, Vector3.up, -Input.GetAxis("Horizontal"));
			transform.RotateAround(Vector3.zero, Vector3.up, Velocity);
		}
		else if(State.selected != null)
        {
			Velocity = 0f;
			State.selectedGrid.y = State.selected.GetComponent<Tile>().Y;

            State.selectedGrid.x += _rotatetospeed * State.level.LevelWidth / 360;
			State.selectedGrid.x = State.selectedGrid.x > 0 ?
                State.selectedGrid.x : State.level.LevelWidth + State.selectedGrid.x;
            State.selectedGrid.x = State.selectedGrid.x < State.level.LevelWidth ?
                State.selectedGrid.x : State.selectedGrid.x - State.level.LevelWidth;

			_rotatetospeed = State.selected.GetComponent<Tile>().X + 0.5f - State.selectedGrid.x;
            if (_rotatetospeed > State.level.LevelWidth / 2)
            {
                _rotatetospeed -= State.level.LevelWidth;
			}
            else if (_rotatetospeed < -State.level.LevelWidth / 2)
            {
                _rotatetospeed += State.level.LevelWidth;
			}
			transform.RotateAround(Vector3.zero, Vector3.up, -_rotatetospeed);
		}
		foreach(Touch touch in Input.touches)
        {
			if(touch.phase == TouchPhase.Moved)
            {
				Velocity = touch.deltaPosition.x * Time.deltaTime * 60;
			}
		}
        if(Input.GetKey("mouse 0"))
        {
            Velocity = Input.GetAxis("Mouse X") * Time.deltaTime * 120;
        }
	}
}
