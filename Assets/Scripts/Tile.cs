using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
  private int _x, _y;
  private GameObject _level;

  private bool _dripped;

  private bool _hasLava;
  /*public bool HasLava
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
  }*/

  public bool HasLava
  {
      get
      {
          return LavaHeight > 0;
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

  public int GroundHeight
  {
      get
      {
          return getChild(ChildNames.Ground).GetComponent<DynamicHeight>().Height;
      }
      set
      {
          getChild(ChildNames.Ground).GetComponent<DynamicHeight>().Height = value;
      }
  }

  public int LavaHeight
  {
      get
      {
          return transform.FindChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height - GroundHeight;
      }
      set
      {
          getChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height = value + GroundHeight;
      }
  }
    /*
  public void SetGroundHeight(int h)
  {
      transform.FindChild(ChildNames.Ground).GetComponent<DynamicHeight>().Height = h;
  }

  public void SetLavaHeight(int h)
  {
      transform.FindChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height = GetGroundHeight() + h;
  }

  public int GetGroundHeight()
  {
      return transform.FindChild(ChildNames.Ground).GetComponent<DynamicHeight>().Height;
  }

  public int GetLavaHeight()
  {
      return transform.FindChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height - GetGroundHeight();
  }

  public int GetTotalHeight()
  {
      return GetGroundHeight() + GetLavaHeight();
  }*/

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
    //DisableChildObject(ChildNames.Lava);
    // lava.GetComponent<FlowScript>().IsFlowing = false;

    // set up the callback
    //RegisterFlowCallback();
  }
/*
  private void RegisterFlowCallback()
  {
    GameObject lava = getChild(ChildNames.Lava);

    if (lava != null)
    {
      lava.GetComponent<FlowScript>().RegisterFlowCallback(OnFlow);
    }
  }*/

  // Update is called once per frame
  void Update()
  {
  }

  /*void FixedUpdate()
  {
    _currentTick++;

    if (_currentTick >= TickRate)
    {
      _currentTick = 0;

      TickUpdate();
    }
  }*/

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

  public void PatrickFlowIn()
  {
      if (GroundHeight + LavaHeight < DynamicHeight.MaxHeight)
      {
          gameObject.GetComponent<Tile>().LavaHeight += 1;
      }
      else
      {
          // if our block is already full, gg
      }
  }

  // flow 1 unit out. standard "update".
    // TODO: return true or false as to whether it was able? should that be done in flow in? determine this.
  public void TrickleDown()
  {
      if (LavaHeight == 0) return;

      if (_y == 0)
      {
          LavaHeight -= 1;
          return;
      }

      Tile bottom = _level.GetComponent<Level>()._tiles[_x][_y - 1].GetComponent<Tile>();
      Tile bottomright = _level.GetComponent<Level>()._tiles[(_x + 1) % Level.LevelWidth][_y - 1].GetComponent<Tile>();
      Tile bottomleft = _level.GetComponent<Level>()._tiles[(_x - 1) % Level.LevelWidth][_y - 1].GetComponent<Tile>();
      //Tile right = _level.GetComponent<Level>()._tiles[(_x + 1) % Level.LevelWidth][_y].GetComponent<Tile>();
      //Tile left = _level.GetComponent<Level>()._tiles[(_x - 1) % Level.LevelWidth][_y].GetComponent<Tile>();
      if (GroundHeight + LavaHeight >= bottom.GroundHeight + bottom.LavaHeight
          && bottom.GroundHeight + bottom.LavaHeight < DynamicHeight.MaxHeight)
      {
          LavaHeight -= 1;
          bottom.LavaHeight += 1;
      }
      else if (GroundHeight + LavaHeight >= bottomright.GroundHeight + bottomright.LavaHeight
          && bottomright.GroundHeight + bottomright.LavaHeight < DynamicHeight.MaxHeight)
      {
          LavaHeight -= 1;
          bottomright.LavaHeight += 1;
      }
      else if (GroundHeight + LavaHeight >= bottomleft.GroundHeight + bottomleft.LavaHeight
          && bottomleft.GroundHeight + bottomleft.LavaHeight < DynamicHeight.MaxHeight)
      {
          LavaHeight -= 1;
          bottomleft.LavaHeight += 1;
      }
      /*else if (GroundHeight + LavaHeight >= right.GroundHeight + right.LavaHeight
          && right.GroundHeight + right.LavaHeight < DynamicHeight.MaxHeight)
      {
          LavaHeight -= 1;
          right.LavaHeight += 1;
      }
      else if (GroundHeight + LavaHeight >= left.GroundHeight + left.LavaHeight
          && left.GroundHeight + left.LavaHeight < DynamicHeight.MaxHeight)
      {
          LavaHeight -= 1;
          left.LavaHeight += 1;
      }*/
   }

      /*if (!HasLava) return;

      // Case: if in the bottom row, just empty out.
      if (_y == 0)
      {
          SetLavaHeight(GetLavaHeight() - 1);
          if (GetLavaHeight() == 0) HasLava = false;
      }
      else
      {
          GameObject below = _level.GetComponent<Level>()._tiles[_x][_y - 1];
          int leftx = _x - 1;
          if (leftx == 0) leftx = Level.LevelWidth - 1;
          int rightx = _x + 1;
          if (rightx == Level.LevelWidth) rightx = 0;
          GameObject belowleft = _level.GetComponent<Level>()._tiles[leftx][_y - 1];
          GameObject belowright = _level.GetComponent<Level>()._tiles[rightx][_y - 1];
          // todo: refactor these cases into order of priority: bottom, bottom left, bottom right, left, right, etc?
          // Case: if our total height is greater than the space below, and it's also not at the max height, flow straight down.
          if (GetTotalHeight() > below.GetComponent<Tile>().GetTotalHeight())
          {
              if (!below.GetComponent<Tile>().HasLava)
              {
                  below.GetComponent<Tile>().HasLava = true;
                  below.GetComponent<Tile>().SetLavaHeight(1);
              }
              else below.GetComponent<Tile>().SetLavaHeight(below.GetComponent<Tile>().GetLavaHeight() + 1);

              //below.GetComponent<Tile>().SetLavaHeight(below.GetComponent<Tile>().GetLavaHeight() + 1);
              //if (!below.GetComponent<Tile>().HasLava) below.GetComponent<Tile>().HasLava = true;

              SetLavaHeight(GetLavaHeight() - 1);
              if (GetLavaHeight() == 0) HasLava = false;
          }
          // Case: we are not able to flow into the space below, so let's try below and to the left and right.
          else if (GetTotalHeight() > belowleft.GetComponent<Tile>().GetTotalHeight())
          {
              belowleft.GetComponent<Tile>().SetLavaHeight(belowleft.GetComponent<Tile>().GetLavaHeight() + 1);
              if (!belowleft.GetComponent<Tile>().HasLava) belowleft.GetComponent<Tile>().HasLava = true;

              SetLavaHeight(GetLavaHeight() - 1);
              if (GetLavaHeight() == 0) HasLava = false;
          }
          else if (GetTotalHeight() > belowright.GetComponent<Tile>().GetTotalHeight())
          {
              belowright.GetComponent<Tile>().SetLavaHeight(belowright.GetComponent<Tile>().GetLavaHeight() + 1);
              if (!belowright.GetComponent<Tile>().HasLava) belowright.GetComponent<Tile>().HasLava = true;

              SetLavaHeight(GetLavaHeight() - 1);
              if (GetLavaHeight() == 0) HasLava = false;
          }
      }*/
    /*
  public void OnFlow()
  {
    if (_dripped) return;

    Level level = _level.GetComponent<Level>();
    if (level == null) return;

    int downY = _y - 1;
    if (!withinBounds(downY, 0, level.Height)) return;

    Tile downTile = level.GetTile(_x, downY).GetComponent<Tile>();
    if (downTile != null)
    {
      Debug.Log(string.Format("Made lava at {0}, {1}", downTile._x, downTile._y));

      downTile.HasLava = true;
    }

    _dripped = true;

    // 10% chance of branching out
    if (Random.Range(0f, 1f) < 0.1f)
    {
      int leftX = (_x - 1)%level.Width;
      int rightX = (_x + 1)%level.Height;

      Tile downLeftTile = level.GetTile(leftX, downY).GetComponent<Tile>();
      Tile downRightTile = level.GetTile(rightX, downY).GetComponent<Tile>();

      if (downLeftTile != null)
      {
        Debug.Log(string.Format("Made lava at {0}, {1}", downLeftTile._x, downLeftTile._y));

        downLeftTile.HasLava = true;
      }

      if (downRightTile != null)
      {
        Debug.Log(string.Format("Made lava at {0}, {1}", downRightTile._x, downRightTile._y));

        downRightTile.HasLava = true;
      }
    }
  }*/

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
    public const string Ground = "Rock";
    public const string Structure = "Structure";
  }
}
