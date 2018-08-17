using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Collections;

public class JumpAttack : DragAttack
{

  public override void ValidateDropTarget(DragOperation drag)
  {
    if(EventSystem.current.IsPointerOverGameObject())
    {
      drag.status = -1;
      return;
    }

    if(m.input.hits.Contains(m.input.floor))
    {
      drag.status = 1;
      return;
    }
  }

  public override void DoAttack(Unit u)
  {
    cooldown = cooldownAmt;
    if(fxKey != "")
    {
      unit.fx.Spawn(fxKey);
    }
    StartCoroutine(doAttack(m.input.getPointOnGround()));
  }

  IEnumerator doAttack(Vector3 attackPos)
  {
    unit.uninteresting = true;
    unit.anim.SetTrigger("JumpAttack");
    yield return new WaitForSeconds(0.25f);
    
    //teleport the unit over.
    unit.t.position = attackPos;
    
    yield return new WaitForSeconds(0.75f);

    // todo: just the units on screen.
    List<Unit> units = m.units.GetAllUnitsOnScreen();
    foreach(Unit u in units)
    {
      if(u.faction == unit.enemyFaction)
      {
        Damage dmg = new Damage(this);
        dmg.amount = unit.calculateDamage(dmg);
        u.DealDamage(dmg);
        // u.Knockback(attackPos);

      }
    }
    unit.uninteresting = false;
  }


  public override Unit GetValidTarget()
  {
    return unit;
  }

}
