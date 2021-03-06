﻿using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private GameObject _level { get; set; }
    private int _maxdepositallowed = 3;

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
            if (GroundHeight + LavaHeight < DynamicHeight.MaxHeight)
            {
                if (value > DynamicHeight.MaxHeight / 2) value = DynamicHeight.MaxHeight / 2;
                getChild(ChildNames.Lava).GetComponent<DynamicHeight>().Height = value + GroundHeight;
            }
        }
    }

    public int TotalHeight {
        get
        {
            return GroundHeight + LavaHeight;
        }
    }

    private bool HasWall
    {
        get
        {
            return getChild(ChildNames.Structure).GetComponent<Structure>().type == Structure.Type.Wall;
        }
    }

    private bool HasBase
    {
        get
        {
            return getChild(ChildNames.Structure).GetComponent<Structure>().type == Structure.Type.Base;
        }
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

    private bool TileOkay(Tile other, int cutoff)
    {
        int heightdiff = TotalHeight - other.GroundHeight;
        return heightdiff > cutoff && other.TotalHeight < DynamicHeight.MaxHeight && !other.HasWall && !other.HasBase;
    }

    public void InitializeLevelData(int x, int y, GameObject level)
    {
        X = x;
        Y = y;
        _level = level;
    }

    // flow 1 unit out. standard "update".
    // TODO: return true or false as to whether it was able? should that be done in flow in? determine this.
    public void TrickleDown()
    {
        if (LavaHeight == 0) return;

        if (Y == 0)
        {
            LavaHeight -= _maxdepositallowed;
            return;
        }

        Tile bottom = _level.GetComponent<Level>()._tiles[X][Y - 1].GetComponent<Tile>();
        int rightx = Mathf.Min(X + 1, State.level.LevelWidth - 1);
        int leftx = Mathf.Max(X - 1, 0);
        Tile bottomright = _level.GetComponent<Level>()._tiles[rightx][Y - 1].GetComponent<Tile>();
        Tile bottomleft = _level.GetComponent<Level>()._tiles[leftx][Y - 1].GetComponent<Tile>();
        Tile right = _level.GetComponent<Level>()._tiles[rightx][Y].GetComponent<Tile>();
        Tile left = _level.GetComponent<Level>()._tiles[leftx][Y].GetComponent<Tile>();

        if (bottom.HasWall || bottom.HasBase)
        {
            bottom.getChild(Tile.ChildNames.Structure).GetComponent<Structure>().Hurt(1);
        }

        int deposited = 0;
        while (LavaHeight > 0 && deposited < _maxdepositallowed)
        {
            int depositedstart = deposited;
            if (TileOkay(bottom, -1))
            {
                LavaHeight -= 1;
                bottom.LavaHeight += 1;
                deposited++;
            }
            if (!(LavaHeight > 0 && deposited < _maxdepositallowed)) break;
            if (TileOkay(bottomright, 1) && TileOkay(right, 1))
            {
                LavaHeight -= 1;
                bottomright.LavaHeight += 1;
                deposited++;
            }
            if (!(LavaHeight > 0 && deposited < _maxdepositallowed)) break;
            if (TileOkay(bottomleft, 1) && TileOkay(left, 1))
            {
                LavaHeight -= 1;
                bottomleft.LavaHeight += 1;
                deposited++;
            }
            if (!(LavaHeight > 0 && deposited < _maxdepositallowed)) break;
            if (TileOkay(right, 1))
            {
                LavaHeight -= 1;
                right.LavaHeight += 1;
                deposited++;
            }
            if (!(LavaHeight > 0 && deposited < _maxdepositallowed)) break;
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

    public GameObject getChild(string childName)
    {
        Transform transform = this.gameObject.transform.FindChild(childName);
        return transform == null ? null : transform.gameObject;
    }

    internal static class ChildNames
    {
        public const string Lava = "Lava";
        public const string Ground = "Rock";
        public const string Structure = "Structure";
        public const string Highlight = "Highlight";
    }
}
