using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
  private int _x, _y;
  private GameObject _level;
  public int maxdepositallowed = 3;

  public void SetHighlightHeight()
  {
      getChild(ChildNames.Highlight).GetComponent<DynamicHeight>().Height = (int)(DynamicHeight.MaxHeight * 1.2f);
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
          int dheight = LavaHeight;
          getChild(ChildNames.Ground).GetComponent<DynamicHeight>().Height = value;
          getChild(ChildNames.Structure).GetComponent<DynamicHeight>().Height = value + 1;
          LavaHeight = dheight;
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
          if (value > DynamicHeight.MaxHeight / 2) value = DynamicHeight.MaxHeight / 2;
          getChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height = value + GroundHeight;
      }
  }

  public int TotalHeight()
  {
      return GroundHeight + LavaHeight;
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
          LavaHeight -= maxdepositallowed;
          return;
      }

      Tile bottom = _level.GetComponent<Level>()._tiles[_x][_y - 1].GetComponent<Tile>();
      int rightx = Mathf.Min(_x + 1, Level.LevelWidth - 1);
      int leftx = Mathf.Max(_x - 1, 0);
      Tile bottomright = _level.GetComponent<Level>()._tiles[rightx][_y - 1].GetComponent<Tile>();
      Tile bottomleft = _level.GetComponent<Level>()._tiles[leftx][_y - 1].GetComponent<Tile>();
      Tile right = _level.GetComponent<Level>()._tiles[rightx][_y].GetComponent<Tile>();
      Tile left = _level.GetComponent<Level>()._tiles[leftx][_y].GetComponent<Tile>();

      if (bottom.HasWall())
      {
          bottom.getChild(Tile.ChildNames.Structure).GetComponent<Structure>().Hurt(1);
      }

      int deposited = 0;
      while (LavaHeight > 0 && deposited < maxdepositallowed)
      {
          int depositedstart = deposited;
          if (TileOkay(bottom, -1))
          {
              LavaHeight -= 1;
              bottom.LavaHeight += 1;
              deposited++;
          }
          if (!(LavaHeight > 0 && deposited < maxdepositallowed)) break;
          if (TileOkay(bottomright, 1) && TileOkay(right, 1))
          {
              LavaHeight -= 1;
              bottomright.LavaHeight += 1;
              deposited++;
          }
          if (!(LavaHeight > 0 && deposited < maxdepositallowed)) break;
          if (TileOkay(bottomleft, 1) && TileOkay(left, 1))
          {
              LavaHeight -= 1;
              bottomleft.LavaHeight += 1;
              deposited++;
          }
          if (!(LavaHeight > 0 && deposited < maxdepositallowed)) break;
          if (TileOkay(right, 1))
          {
              LavaHeight -= 1;
              right.LavaHeight += 1;
              deposited++;
          }
          if (!(LavaHeight > 0 && deposited < maxdepositallowed)) break;
          if (TileOkay(left, 1))
          {
              LavaHeight -= 1;
              left.LavaHeight += 1;
              deposited++;
          }
          if (deposited == depositedstart) break;
      }
      float prob = 0.15f;
      if (deposited == 0) prob += 0.3f;
      if (Random.value < prob && LavaHeight > 0)
      {
          LavaHeight -= 1;
          GroundHeight += 2;
      }
   }

  private bool HasWall()
  {
      return getChild(ChildNames.Structure).GetComponent<Structure>().GetStructureType() == Structure.Type.Wall;
  }

  private bool TileOkay(Tile other, int cutoff)
  {
      int heightdiff = TotalHeight() - other.GroundHeight;
      return heightdiff > cutoff && other.TotalHeight() < DynamicHeight.MaxHeight && !other.HasWall();
  }

  private bool _highlighted = false;
  public bool highlighted
  {
      get { return _highlighted; }
      set
      {
          if (_highlighted != value)
          {
              _highlighted = value;
              if (_highlighted) getChild(ChildNames.Highlight).renderer.enabled = true;
              else getChild(ChildNames.Highlight).renderer.enabled = false;
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

  public GameObject getChild(string childName)
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
    public const string Highlight = "Highlight";
  }
}
