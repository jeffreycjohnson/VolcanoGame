using System;
using UnityEngine;
using System.Collections;

public class FlowScript : MonoBehaviour
{
  /// <summary>
  /// The initial fill level
  /// Goes from 0-10
  /// </summary>
  public float FillLevel = 0;

  /// <summary>
  /// How much the fill increases per second
  /// </summary>
  public float FillRate = 1;

  // Use this for initialization
  void Start()
  {
    InvokeRepeating("FillUp", 0, 1.0f);
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Flow()
  {
    
  }

  public void FillUp()
  {
    FillLevel = Math.Min(10, FillLevel + FillRate);
  }
}
