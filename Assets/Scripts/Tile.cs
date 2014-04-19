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
    
  }

  public void RemoveLava()
  {
    
  }
}
