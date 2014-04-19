using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
  private int _x, _y;
  private GameObject _level;

  private bool _hasLava;
  public bool HasLava
  {
    get { return _hasLava; }
    set
    {
      if (_hasLava != value)
      {
        _hasLava = value;

        if (_hasLava)
        {
          var lava = EnableChildObject(ChildNames.Lava);
          lava.GetComponent<FlowScript>().IsFlowing = true;

          Debug.Log(string.Format("Set To Flowing {0}", lava.GetComponent<FlowScript>().IsFlowing));
        }
        else
        {
          var lava = DisableChildObject(ChildNames.Lava);
          lava.GetComponent<FlowScript>().IsFlowing = false;
        }
      }
    }
  }

  private bool _hasRock;
  public bool HasRock
  {
    get { return _hasRock; }
    set
    {
      if (_hasRock != value)
      {
        _hasRock = value;

        if (_hasRock) EnableChildObject(ChildNames.Ground);
        else DisableChildObject(ChildNames.Ground);
      }
    }
  }

  private bool _hasStructure;
  public bool HasStructure
  {
    get { return _hasStructure; }
    set
    {
      if (_hasStructure != value)
      {
        _hasStructure = value;

        if (_hasStructure) EnableChildObject(ChildNames.Structure);
        else DisableChildObject(ChildNames.Structure);
      }
    }
  }

  /// <summary>
  /// How often the tiles update (in Fixed Updates)
  /// </summary>
  public int TickRate = 1;
  private int _currentTick = 0;

  private Color oldColor = Color.red;
  private bool _highlighted = false;
  public bool highlighted
  {
    get { return _highlighted; }
    set
    {
      if (_highlighted != value)
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
    // we initially want lava hidden
    DisableChildObject(ChildNames.Lava);
    // lava.GetComponent<FlowScript>().IsFlowing = false;

    // set up the callback
    RegisterFlowCallback();
  }

  private void RegisterFlowCallback()
  {
    GameObject lava = getChild(ChildNames.Lava);

    if (lava != null)
    {
      lava.GetComponent<FlowScript>().RegisterFlowCallback(OnFlow);
    }
  }

  // Update is called once per frame
  void Update()
  {
  }

  void FixedUpdate()
  {
    _currentTick++;

    if (_currentTick >= TickRate)
    {
      _currentTick = 0;

      TickUpdate();
    }
  }

  public void InitializeLevelData(int x, int y, GameObject level)
  {
    SetPositionInGrid(x, y);
    SetLevel(level);
  }

  private void SetPositionInGrid(int x, int y)
  {
    _x = x;
    _y = y;
  }

  private void SetLevel(GameObject level)
  {
    _level = level;
  }

  public void TickUpdate()
  {
  }

  public void OnFlow()
  {
    Level level = _level.GetComponent<Level>();
    if (level == null) return;

    int downY = _y - 1;
    if (!withinBounds(downY, 0, level.Height)) return;

    Tile downTile = level.GetTile(_x, downY).GetComponent<Tile>();
    if (downTile == null) return;

    if (!downTile.HasLava) Debug.Log(string.Format("Made lava at {0}, {1}", downTile._x, downTile._y));

    downTile.HasLava = true;
  }

  private GameObject EnableChildObject(string objectName)
  {
    GameObject gobj = getChild(objectName);

    if (gobj != null)
    {
      Debug.Log(string.Format("Enabled {0}", objectName));
      gobj.renderer.enabled = true;
    }

    return gobj;
  }

  private GameObject DisableChildObject(string objectName)
  {
    GameObject gobj = getChild(objectName);

    if (gobj != null)
    {
      gobj.renderer.enabled = false;
    }

    return gobj;
  }

  private GameObject getChild(string childName)
  {
    Transform transform = this.gameObject.transform.FindChild(childName);
    return transform == null ? null : transform.gameObject;
  }

  private bool withinBounds(int val, int low, int high)
  {
    return val >= low && val <= high;
  }

  internal static class ChildNames
  {
    public const string Lava = "Lava";
    public const string Ground = "Ground";
    public const string Structure = "Structure";
  }
}
