using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

// For Aggro Radius. Bad name!
public class UnitColliderProxy : MonoBehaviour
{

  Unit u;

  void Start()
  {
    u = GetComponentInParent<Unit>();
  }

  void OnTriggerEnter2D(Collider2D col)
  {
    u.OnAggroEnter2D(col);
  }

  void OnTriggerExit2D(Collider2D col)
  {
    u.OnAggroExit2D(col);
  }

}
