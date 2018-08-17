using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Attack : MonoBehaviour
{

  [System.NonSerialized][HideInInspector]
  public  Unit unit;

  // The collider that defines the trigger range.
  public Collider2D col;

  public AttackFlavor attackFlavor;

  public float attackValue = 1.0f;

  // how long to wait between attacks.
  public float cooldownAmt = 0.5f;

  // the current amount of time waited.
  [System.NonSerialized][HideInInspector]
  public float cooldown = 0.0f;

  public bool automatic = true;

  public Sprite attackIcon;
  [Multiline]
  public string description;

  public string fxKey;

  protected Managers m;

  void Start()
  {
    m = Managers.Get();
    unit = GetComponentInParent<Unit>();
    unit.OnUpdateEnd.AddListener(Tick);
  }

  public virtual void DoAttack(Unit u)
  {
    cooldown = cooldownAmt;
    if(fxKey != "")
    {
      unit.fx.Spawn(fxKey);
    }

    bool hit = true;
    float baseDex = 90.0f;
    float dex = unit.dexterity;
    float agi = u.agility;
    float agi2 = agi * 0.25f;
    float modi = Mathf.Round( ((dex / agi2) - 1.0f) * 100.0f);
    float total = baseDex + modi;
    if((UnityEngine.Random.value * 100.0f) > total)
    {
      hit = false;
    }
    if(hit)
    {
      Damage dmg = new Damage(this);
      dmg.amount = unit.calculateDamage(dmg);
      u.DealDamage(dmg);
    }else{
      Damage dmg = new Damage(this);
      dmg.flavor = AttackFlavor.Miss;
      u.DealDamage(dmg);
    }

  }

  public virtual bool CanAttack()
  {
    return cooldown <= 0.0f; // float precision problems
  }

  public virtual Unit GetValidTarget()
  {
    List<Unit> units = unit.getTargets();
    foreach(Unit u in units)
    {
      if(IsInRange(u))
      {
        return u;
      }
    }
    return null;
  }

  public virtual bool IsInRange(Unit u)
  {
    return col.IsTouching(u.body);
  }

  // called each frame by the unit
  public virtual void Tick()
  {
    cooldown -= Time.deltaTime;
    Math.Max(cooldown, 0.0f);
  }

  public override string ToString()
  {
    return "[Attack:" + base.gameObject.name + " Unit:"+ unit+"]";
  }

}
