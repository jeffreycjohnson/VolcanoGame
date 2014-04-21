using UnityEngine;
using System.Collections;

public class State {
	public static float money = 1000;
	public enum Action {
		Dig = 0,
		Building = 1,
		Wall = 2,
		Generator = 3
	};
	public static Action action = Action.Dig;
	public static Vector2 selectedGrid = new Vector2(0, 0);
}
