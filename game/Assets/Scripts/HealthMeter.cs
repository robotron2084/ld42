using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HealthMeter : MonoBehaviour
{

  Unit u;

  public SpriteRenderer hpLeft;

  void Start()
  {
    u = GetComponentInParent<Unit>();
    u.OnDamageTaken.AddListener(onDamage);
    refresh();
  }

  void onDamage(Damage dmg)
  {
    // Debug.Log("[HealthMeter] DamageTaken!");
    refresh();
  }

  void refresh()
  {
    hpLeft.size = new Vector2( (float)u.hp / (float)u.fullHP , hpLeft.size.y);
  }
}
