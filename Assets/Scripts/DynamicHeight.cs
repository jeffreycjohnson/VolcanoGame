using UnityEngine;
using System.Collections;

public class DynamicHeight : MonoBehaviour {

	public bool rotated = false;
    public const int MaxHeight = 12;
    public const float DeltaHeight = 0.24f;

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
		if (!rotated)
        {
        	_deltaposition = gameObject.transform.rotation * new Vector3(0, -1, 0) * DeltaHeight;
		}
		else
        {
			_deltaposition = gameObject.transform.rotation * new Vector3(0, 0, 1) * DeltaHeight;
		}
        //float d = (gameObject.transform.localScale.x - 3.77f - 1.5f) * 0.01f;
        //float d = gameObject.transform.localScale.x / (6.06f - 1.48f) + (1 - (1.48f / (6.06f - 1.48f)));
        //_deltaposition += new Vector3(d, d, d);
        //_deltaposition *= d;
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
