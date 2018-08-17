using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class BaseHero : Unit
{

  [Multiline]
  public string description;
  public Transform portraitPrefab;

  protected override void Awake()
  {
    base.Awake();
    faction = Faction.Hero;
    enemyFaction = Faction.Enemy;
  }


}
