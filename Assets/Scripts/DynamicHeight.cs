using UnityEngine;
using System.Collections;

public class DynamicHeight : MonoBehaviour {

	public bool rotated = false;
    public const int MaxHeight = 12;
    public const float DeltaHeight = 0.24f;

    int _height = 0;
    Vector3 _lowestposition;
    Vector3 _deltaposition;
    public Color _originalcolor = Color.grey;
    public Color _topcolor = Color.white;
    public float FadeAmount = 2f;

	void Start () {
        _lowestposition = gameObject.transform.position;
		if (!rotated)
        {
        	_deltaposition = gameObject.transform.rotation * new Vector3(0, -1, 0) * DeltaHeight;
		}
		else
        {
			_deltaposition = gameObject.transform.rotation * new Vector3(0, 0, 1) * DeltaHeight;
		}
	}

    public int Height
    {
        get { return _height; }
        set
        {
            _height = Mathf.Clamp(value, 0, MaxHeight);
            if (value < 0 || value > MaxHeight) Debug.Log("DynamicHeight being set too high or low. Clamped.");
            gameObject.transform.position = _lowestposition + (float)Height * _deltaposition;
            gameObject.renderer.material.color = _originalcolor + (float)Height / (float)MaxHeight * ((_topcolor - _originalcolor) * FadeAmount);
        }
    }
	
	void Update () {
	
	}
}
