using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class BaseMob : Unit
{

  protected override void Awake()
  {
    base.Awake();
    faction = Faction.Enemy;
    enemyFaction = Faction.Hero;
  }

}
