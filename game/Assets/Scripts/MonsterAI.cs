using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class MonsterAI : MonoBehaviour
{

  Unit unit;

  Unit target;

  void Start()
  {
    unit = GetComponentInParent<Unit>();
    unit.OnUpdateBegin.AddListener(Tick);
    unit.AddState(UnitAction.Patrolling);
  }


  public void Tick()
  {
    if( unit.HasState(UnitAction.Patrolling))
    {
      patrol();
    }
    if( unit.HasState(UnitAction.Chasing))
    {
      if(target == null)
      {
        enterPatrol(UnitAction.Chasing);
      }else{
        //we can still chase it!
        unit.Log("[MonsterAI] chasing!");
        if(unit.automaticAttack.IsInRange(target))
        {
          // we found it!
          enterAttack(UnitAction.Chasing, target);
        }else{
          //chase it!
          unit.MoveTowards(target);
        }
      }
    }
    if(unit.HasState(UnitAction.Attacking))
    {
      if(target == null)
      {
        enterPatrol(UnitAction.Attacking);
        return;
      }
      if(unit.automaticAttack.IsInRange(target))
      {
        unit.Log("[MonsterAI] Attacking!");
      }else{
        enterChasing(UnitAction.Attacking, target);
      }
    }
  }

  void patrol()
  {
    List<Unit> units = unit.getTargets();
    if(units.Count > 0)
    {
      Unit firstUnit = units[0];
      if(!unit.automaticAttack.IsInRange(firstUnit))
      {
        unit.Log("[MonsterAI] Walking next to "+firstUnit + "of " + units.Count);
        enterChasing(UnitAction.Patrolling, firstUnit);
      }else{
        enterAttack(UnitAction.Patrolling, firstUnit);
      }
    }
  }

  void enterPatrol(UnitAction fromState)
  {
    unit.SwitchState(fromState, UnitAction.Patrolling);
    unit.anim.SetBool("Walk", false);
    target = null;
  }

  void enterChasing(UnitAction fromState, Unit u)
  {
    //we're in range!
    unit.SwitchState(fromState, UnitAction.Chasing);
    target = u;

  }

  void enterAttack(UnitAction fromState, Unit u)
  {
    unit.anim.SetBool("Walk", false);
    //we're in range!
    unit.SwitchState(fromState, UnitAction.Attacking);
    target = u;


  }


  

}
