using System;
using UnityEngine;
using System.Collections;

public class FlowScript : MonoBehaviour
{
  private Action _flowCallback = () => Debug.Log("DEFAULT ACTION ACTION");

  public const float MaxFillLevel = 5.0f;

  /// <summary>
  /// The initial fill level
  /// Goes from 0-10
  /// </summary>
  public float FillLevel = 0;

  /// <summary>
  /// How much the fill increases per tick
  /// A tick is 0.2f as of 2014.04.18
  /// </summary>
  public float FillRate = 1f;

  /// <summary>
  /// Is the lava flowing at all?
  /// </summary>
  public bool IsFlowing = false;

  public int TickRate = 50;
  private int _currentTick = 0;

  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  void FixedUpdate()
  {
    _currentTick++;

    if (_currentTick % TickRate == 0)
    {
      Flow();
    }
  }
  
  public void Flow()
  {
    if (!IsFlowing) return;
    return; // REMOVE ME?

    FillUp();

    if (FillLevel >= MaxFillLevel) _flowCallback();
  }

  public void FillUp()
  {
    FillLevel = Math.Min(MaxFillLevel, FillLevel + FillRate);

    Render();

    Debug.Log(string.Format("Increased Fill Level to {0}", FillLevel));
  }

  public void RegisterFlowCallback(Action callback)
  {
    _flowCallback = callback;
  }

  private void Render()
  {
    
  }
}
