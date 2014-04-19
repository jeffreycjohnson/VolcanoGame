using UnityEngine;
using System.Collections;

public class DynamicHeight : MonoBehaviour {

    public const int MaxHeight = 3;
    public const float DeltaHeight = 0.4f;

    int _height = 0;
    Vector3 _lowestposition;
    Vector3 _deltaposition;

	void Start () {
        _lowestposition = gameObject.transform.position;
        _deltaposition = gameObject.transform.rotation * new Vector3(0, -1, 0) * DeltaHeight;
	}

    public int Height
    {
        get { return _height; }
        set
        {
            _height = Mathf.Clamp(value, 0, MaxHeight);
            if (value < 0 || value > MaxHeight) Debug.Log("DynamicHeight being set too high or low. Clamped.");
            gameObject.transform.position = _lowestposition + (float)Height * _deltaposition;
        }
    }
	
	void Update () {
	
	}
}
