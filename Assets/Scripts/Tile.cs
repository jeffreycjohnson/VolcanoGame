using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
  private bool _hasLava;
  public bool HasLava
  {
    get { return _hasLava; }
    set
    {
      if (_hasLava != value)
      {
        _hasLava = value;

        if (_hasLava) AddLava();
        else RemoveLava();
      }
    }
  }

	private Color oldColor = Color.red;
	private bool _highlighted = false;
	public bool highlighted
	{
		get { return _highlighted; }
		set{
			if(_highlighted != value)
			{
				_highlighted = value;

				Color tmp = transform.FindChild("Rock").renderer.material.color;
				transform.FindChild("Rock").renderer.material.color = oldColor;
				oldColor = tmp;
			}
		}
	}

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void AddLava()
  {
    Transform lavaTransform = this.gameObject.transform.FindChild("Lava");
    GameObject lava = null;

    if (lavaTransform == null) return;

    lava = lavaTransform.gameObject;
  }

  public void RemoveLava()
  {
    
  }
}
