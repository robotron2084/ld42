using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class MeleeAttack : Attack
{
  public override void DoAttack(Unit u)
  {
    base.DoAttack(u);
    // Debug.Log("[MeleeAttack] doing attack");

  }


}
