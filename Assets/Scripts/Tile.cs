using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
  private int _x, _y;
  private GameObject _level;

  void Start()
  {

  }

  void Update()
  {

  }

  public bool HasLava
  {
      get
      {
          return LavaHeight > 0;
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

  public void PatrickFlowIn(int amount)
  {
      if (GroundHeight + LavaHeight < DynamicHeight.MaxHeight)
      {
          gameObject.GetComponent<Tile>().LavaHeight += amount;
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
      int rightx = Mathf.Min(_x + 1, Level.LevelWidth - 1);
      int leftx = Mathf.Max(_x - 1, 0);
      Tile bottomright = _level.GetComponent<Level>()._tiles[rightx][_y - 1].GetComponent<Tile>();
      Tile bottomleft = _level.GetComponent<Level>()._tiles[leftx][_y - 1].GetComponent<Tile>();
      if (GroundHeight + LavaHeight >= bottom.GroundHeight + bottom.LavaHeight
          && bottom.GroundHeight + bottom.LavaHeight < DynamicHeight.MaxHeight)
      {
          int deposited = 0;
          while (LavaHeight > 0 && deposited < 2)
          {
              LavaHeight -= 1;
              bottom.LavaHeight += 1;
              deposited++;
          }
      }
      else if (GroundHeight + LavaHeight >= bottomright.GroundHeight + bottomright.LavaHeight
          && bottomright.GroundHeight + bottomright.LavaHeight < DynamicHeight.MaxHeight)
      {
          int deposited = 0;
          while (LavaHeight > 0 && deposited < 2)
          {
              LavaHeight -= 1;
              bottomright.LavaHeight += 1;
              deposited++;
          }
      }
      else if (GroundHeight + LavaHeight >= bottomleft.GroundHeight + bottomleft.LavaHeight
          && bottomleft.GroundHeight + bottomleft.LavaHeight < DynamicHeight.MaxHeight)
      {
          int deposited = 0;
          while (LavaHeight > 0 && deposited < 2)
          {
            LavaHeight -= 1;
            bottomleft.LavaHeight += 1;
            deposited++;
          }
      }

      if (Random.value < 0.2 && LavaHeight > 0)
      {
          LavaHeight -= 1;
          GroundHeight += 1;
      }
   }

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
