using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class HealAttack : DragAttack
{

  public float percent = 0.2f;

  public float manaRequired = 3.0f;

  public override void ValidateDropTarget(DragOperation drag)
  {
    foreach(Transform t in m.input.hits)
    {
      Unit u = t.GetComponentInParent<Unit>();
      if(u != null && u.faction == unit.faction)
      {
        drag.target = u;
        drag.status = 1;
        return;
      }
    }
    drag.target = null;
    drag.status = -1;
  }

  public override void DoAttack(Unit u)
  {
    cooldown = cooldownAmt;
    if(fxKey != "")
    {
      unit.fx.Spawn(fxKey);
    }
    Damage d = new Damage(this);
    d.amount = u.fullHP * percent;
    d.flavor = AttackFlavor.Heal;
    u.DoHeal(d);
    unit.mana -= manaRequired;
  }

  public override bool CanAttack()
  {
    return cooldown <= 0.0f && unit.mana > manaRequired; // float precision problems
  }


  public override Unit GetValidTarget()
  {
    return unit;
  }

}
