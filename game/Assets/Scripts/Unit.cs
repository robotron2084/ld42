using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Collections;

public class Unit : MonoBehaviour
{
    public bool logging = false;

    [System.Serializable]
    public class DamageEvent : UnityEvent<Damage>{}

    [System.NonSerialized][HideInInspector]
    public Animator anim;

    public Collider2D range;

    public CircleCollider2D body;
    public Transform facingContainer;

    public float hp;
    public float mana;
    public float manaRegen = 0.1f; //units per second.

    public float defense = 0.0f;
    public float spirit = 0.0f;
    public float intelligence = 0.0f;
    public float power = 0.0f;
    public float agility = 0.0f;
    public float dexterity = 0.0f;

    public float physicalResist = 1.0f;
    public float magicResist = 1.0f;
    public float psychicResist = 1.0f;

    public bool facing = true; // true is right.

    public bool stunned = false;

    //this is true when we want to consider a unit uninteresting to ai.
    [System.NonSerialized][HideInInspector]
    public bool uninteresting = false;

    [System.NonSerialized][HideInInspector]
    public float fullHP; // full hp

    [System.NonSerialized][HideInInspector]
    public float maxMana; 

    public Attack automaticAttack;

    public DamageEvent OnDamageTaken;

    public UnityEvent OnUpdateBegin;
    public UnityEvent OnUpdateEnd;

    [System.NonSerialized][HideInInspector]
    public UnitState state = UnitState.Alive;

    // this is mainly public to see in the editor 
    // for debugging.
    public UnitAction actionState = (UnitAction)0;

    [System.NonSerialized][HideInInspector]
    public Faction faction;

    [System.NonSerialized][HideInInspector]
    public Faction enemyFaction;

    public List<Unit> targets = new List<Unit>();

    [System.NonSerialized][HideInInspector]
    public Transform t;

    [System.NonSerialized][HideInInspector]
    public Managers m;

    [System.NonSerialized][HideInInspector]
    public FXSpawner fx;

    // Movement Speed
    public float speed = 2.0f;

    public List<Attack> attacks = new List<Attack>();

    IEnumerator moveRoutine = null;

    public bool deathCausesGameEnd = false;

    protected virtual void Awake()
    {
      t = GetComponent<Transform>();
      fx = GetComponent<FXSpawner>();
      anim = GetComponent<Animator>();
      fullHP = hp;
      maxMana = mana;
    }

    void Start()
    {
      m = Managers.Get();
      SetFacing(facing);
      fx.Spawn(Keys.Health);
      m.units.Add(this);
    }

    void Update()
    {
      if(stunned)
      {
        return;
      }
      if(state == UnitState.Zombie)
      {
        return;
      }
      OnUpdateBegin.Invoke();
      if(canAttackInCurrentState())
      {
        Log("Can Attack");
        // can we attack? Ie are we on cooldown?
        if(automaticAttack.CanAttack())
        {
          //yep we can attack.
          List<Unit> units = getTargets();
          Log(" auto can attack"+units.Count);
          if(units.Count > 0)
          {
            Unit firstUnit = units[0];
            FaceTowards(firstUnit);
            if(automaticAttack.IsInRange(firstUnit) && !firstUnit.uninteresting)
            {
              automaticAttack.DoAttack(firstUnit);
            }else{
              Log("Not in range");
            }
          }
        }else{
          Log("auto atk isn't viable");
        }
      }else
      {
        Log("atk isn't viable");
        
      }
      OnUpdateEnd.Invoke();

      mana += manaRegen * Time.deltaTime;
      mana = Math.Min(mana, maxMana);
    }

    // called via proxy
    public void OnAggroEnter2D(Collider2D col)
    {
      // Debug.Log("[Unit] OnAggroEnter2D:"+col, col.gameObject);
      Unit u = col.GetComponentInParent<Unit>();
      if(u != null && u.body == col && u.faction == enemyFaction)
      {
        targets.Add(u);
      }
    }

    // called via proxy
    public void OnAggroExit2D(Collider2D col)
    {
      // Debug.Log("[Unit OnTriggerExit2D] Trigger:"+col, col.gameObject);
      Unit u = col.GetComponentInParent<Unit>();
      if(u != null && u.faction == enemyFaction)
      {
        targets.Remove(u);
      }
    }

    public virtual List<Unit> getTargets()
    {
      //todo: sort targets by distance.
      return targets;
    }

    public virtual void DoHeal(Damage dmg)
    {
      Debug.Log("[DamageEvent] healing " + gameObject.name +  " for " + dmg.amount + " current HP : " + hp);
      hp += dmg.amount;
      hp = Math.Min(hp, fullHP);
      OnDamageTaken.Invoke(dmg);
      FXUnit unit = fx.Spawn(Keys.Heal);
      Util.RandomizePosition(unit.transform, 0.5f, 0.1f);

      FXUnit uiUnit = fx.Spawn(Keys.DamageUI);
      uiUnit.label.text = "<color=green>+" + Mathf.Round(dmg.amount * m.units.levellingHack) + "</color>";
      Util.RandomizePosition(uiUnit.transform, 0.5f, 0.1f);
      m.afx.PlayFX(Keys.Heal);
    }

    public virtual void DealDamage(Damage dmg)
    {
      if(state == UnitState.Dead || state == UnitState.Zombie)
      {
        return;
      }
      // Debug.Log("[Unit] " + this.gameObject + " taking " + dmg.amount + " from " + dmg.source, gameObject);

      hp -= dmg.amount;
      hp = Math.Max(hp, 0);
      OnDamageTaken.Invoke(dmg);
      if(hp == 0)
      {
        // Debug.Log("[Unit] DEAD!");
        state = UnitState.Dead;
        m.units.Remove(this);
        actionState = (UnitAction)0;
        fx.Spawn(Keys.Death);
        Destroy(gameObject);
      }else{
        displayDamageFX(dmg);
      }

    }

    void displayDamageFX(Damage dmg)
    {
      m.afx.PlayFX(Keys.Damage);
      if(dmg.flavor != AttackFlavor.Miss)
      {
        FXUnit unit = fx.Spawn(Keys.Damage);
        Util.RandomizePosition(unit.transform, 0.5f, 0.1f);
      }

      FXUnit uiUnit = fx.Spawn(Keys.DamageUI);
      if(dmg.flavor != AttackFlavor.Miss)
      {
        uiUnit.label.text = "<color=white>-" + Mathf.Round(dmg.amount * m.units.levellingHack)  + "</color>";
      }else{
        uiUnit.label.text = "<color=white>MISS</color>";
      }
      Util.RandomizePosition(uiUnit.transform, 0.5f, 0.1f);
    }

    void OnMouseDown()
    {
      // m.input.OnUnitDown(this);
    }

    void OnMouseUp()
    {
      // m.input.OnUnitUp(this);
    }

    public void WalkTo(Vector3 pos)
    {
      // Debug.Log("[Unit] moving:"+pos);
      if(moveRoutine != null)
      {
        StopCoroutine(moveRoutine);
      }

      moveRoutine = walkTo(pos);
      StartCoroutine(moveRoutine);
    }

    public void WalkNextTo(Unit u)
    {
      // move selected unit towards the enemy.
      Vector3 direction = u.t.position - t.position;
      Ray ray = new Ray(t.position, direction);
      float distance = Vector3.Distance(u.t.position, t.position);
      // remove the radius of our units...
      distance -= body.radius;
      distance -= u.body.radius * Math.Abs(u.body.transform.lossyScale.x);
      Vector3 pos = ray.GetPoint(distance);
      WalkTo(pos);
    }

    IEnumerator walkTo(Vector3 pos)
    {
      anim.SetBool("Walk", true);
      Vector3 startPos = t.position;
      SetFacing(pos.x > startPos.x);
      float length = Vector3.Distance(t.position, pos);
      float start = Time.time;
      float time = 0.0f;
      if(pos == startPos)
      {
        // Debug.Log("[DamageEvent] already there");
        yield break;
      }
      actionState |= UnitAction.Walking;
      while(time < 1.0f)
      {
        float dist = (Time.time - start) * speed;
        time = dist / length;
        transform.position = Vector3.Lerp(startPos, pos, time);
        yield return null;
      }
      transform.position = pos;
      anim.SetBool("Walk", false);
      actionState &= ~UnitAction.Walking;

    }

    public void MoveTowards(Unit u)
    {
      Vector3 direction = u.t.position - t.position;
      SetFacing(direction.x > 0.0f);
      direction *= Time.deltaTime * speed * 0.5f; // HACK: for the sake of the jam, move towards is 50% of speed.
      t.position += direction;
      anim.SetBool("Walk", true);

    }

    bool canAttackInCurrentState()
    {
      //we're not walking, then we can attack.
      return (actionState & (UnitAction.Walking | UnitAction.Patrolling | UnitAction.Chasing)) == (UnitAction)0;
    }

    public bool HasState(UnitAction state)
    {
      return (actionState & state) == state;
    }

    public void AddState(UnitAction state)
    {
      actionState |= state;
    }

    public void RemoveState(UnitAction state)
    {
      actionState &= ~state;
    }

    public void SwitchState(UnitAction currentState, UnitAction nextState)
    {
      RemoveState(currentState);
      AddState(nextState);
    }

    public void Log(string msg)
    {
      if(logging)
      {
        Debug.Log("[" + gameObject.name + "] "+msg);
      }
    }


    public float calculateDamage(Damage dmg)
    {
      float randVal = UnityEngine.Random.Range(0.85f, 1.15f);
      float critVal = UnityEngine.Random.value < 0.05f ? 1.5f : 1.0f;
      float charAttackVal = dmg.source.unit.getAttackVal(dmg.source.attackFlavor);
      float attackVal = dmg.source.attackValue;
      float defenseVal = getDefenseVal(dmg.source.attackFlavor);
      float resistVal = getResistVal(dmg.source.attackFlavor);
      float damage = (charAttackVal * randVal * critVal * attackVal) - (defenseVal * resistVal);
      damage = Math.Max(1.0f, Mathf.Ceil(damage));
      return damage;
    }

    float getDefenseVal(AttackFlavor attackFlavor)
    {
      switch(attackFlavor)
      {
        case AttackFlavor.Physical:
          return defense;
        case AttackFlavor.Magical:
          return intelligence;
        case AttackFlavor.Psychic:
          return spirit;
      }
      return 0.0f;
    }

    float getResistVal(AttackFlavor attackFlavor)
    {
      switch(attackFlavor)
      {
        case AttackFlavor.Physical:
          return physicalResist;
        case AttackFlavor.Magical:
          return magicResist;
        case AttackFlavor.Psychic:
          return psychicResist;
      }
      return 0.0f;
    }

    float getAttackVal(AttackFlavor attackFlavor)
    {
      switch(attackFlavor)
      {
        case AttackFlavor.Physical:
          return power;
        case AttackFlavor.Magical:
          return spirit;
        case AttackFlavor.Psychic:
          return intelligence;
      }
      return 0.0f;
    }

    public void SetFacing(bool right)
    {
      facing = right;
      Vector3 localScale = facingContainer.localScale;
      localScale.x = facing ? 1.0f : -1.0f;
      facingContainer.localScale = localScale;
    }

    public void FaceTowards(Unit u)
    {
      Log("[DamageEvent] facing towards:"+u);
      SetFacing(u.t.position.x > t.position.x);
    }

    public void MakeZombie()
    {
      state = UnitState.Zombie;
    }

    // for animation system
    public void PlayAudioFX(string key)
    {
      m.afx.PlayFX(key);
    }

}
