using UnityEngine;
using System.Collections;

public class DynamicHeight : MonoBehaviour {

	public bool rotated = false;
    public const int MaxHeight = 10;
    public const float DeltaHeight = 0.22f;

    int _height = 0;
    Vector3 _lowestposition;
    Vector3 _deltaposition;
    Color _originalcolor;
    const System.Int32 _whiteconstant = 0x01;
    Color _deltawhite = new Color(_whiteconstant, _whiteconstant, _whiteconstant);
    public float DarkenOriginalConstant = 0.2f;
    public float BrightnessDeltaConstant = 1.2f;

	void Start () {
        _lowestposition = gameObject.transform.position;
		if(!rotated) {
        	_deltaposition = gameObject.transform.rotation * new Vector3(0, -1, 0) * DeltaHeight;
		}
		else {
			_deltaposition = gameObject.transform.rotation * new Vector3(0, 0, 1) * DeltaHeight;
		}
        _originalcolor = gameObject.renderer.material.color - DarkenOriginalConstant * _deltawhite;
	}

    public int Height
    {
        get { return _height; }
        set
        {
            _height = Mathf.Clamp(value, 0, MaxHeight);
            if (value < 0 || value > MaxHeight) Debug.Log("DynamicHeight being set too high or low. Clamped.");
            gameObject.transform.position = _lowestposition + (float)Height * _deltaposition;
            gameObject.renderer.material.color = _originalcolor + (((float)Height / (float)MaxHeight) * BrightnessDeltaConstant) * _deltawhite;
        }
    }
	
	void Update () {
	
	}
}
