using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameInit : MonoBehaviour
{
  void Start()
  {
    Physics2D.queriesHitTriggers = false;
  }
}
