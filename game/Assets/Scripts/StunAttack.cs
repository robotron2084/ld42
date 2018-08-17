using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Collections;

public class StunAttack : DragAttack
{

  public float duration = 1.0f;

  public override void ValidateDropTarget(DragOperation drag)
  {
    drag.status = EventSystem.current.IsPointerOverGameObject() ? -1 : 1;
  }

  public override void DoAttack(Unit u)
  {
    cooldown = cooldownAmt;
    if(fxKey != "")
    {
      unit.fx.Spawn(fxKey);
    }
    StartCoroutine(doStun());
  }

  IEnumerator doStun()
  {
    m.fx.Spawn(Keys.Stun);

    foreach(Unit u in m.units.allUnits)
    {
      if(u.faction == unit.enemyFaction)
      {
        u.fx.Spawn(Keys.Stun); //this is persistent
        u.stunned = true;
      }
    }

    yield return new WaitForSeconds(duration);

    foreach(Unit u in m.units.allUnits)
    {
      if(u.faction == unit.enemyFaction)
      {
        u.fx.Despawn(Keys.Stun);
        u.stunned = false;
      }
    }

  }


  public override Unit GetValidTarget()
  {
    return unit;
  }

}
