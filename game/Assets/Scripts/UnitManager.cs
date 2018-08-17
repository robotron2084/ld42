using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class UnitManager : MonoBehaviour
{

  public List<Unit> onScreenUnits;
  public Bounds cameraBounds;

  public int unitsOnScreen;
  public Vector2 pointA;
  public Vector2 pointB;

  public float levellingHack = 1.0f;
  public float levellingMod = 1.15f;

  Collider2D[] results = new Collider2D[64];

  [System.NonSerialized][HideInInspector]
  public List<Unit> allUnits = new List<Unit>();

  [System.NonSerialized][HideInInspector]
  public List<Unit> heroUnits = new List<Unit>();

  // base fx for all units.
  public FXSpawner baseFX;

  public void Add(Unit u)
  {
    allUnits.Add(u);
    if(u.faction == Faction.Hero)
    {
      heroUnits.Add(u);
    }
  }

  public void Remove(Unit u)
  {
    allUnits.Remove(u);
    if(u.faction == Faction.Hero)
    {
      heroUnits.Remove(u);
    }

    if(u.faction == Faction.Hero)
    {
      //hero was killed! do we have any heros left?
      if(heroUnits.Count == 0)
      {
        Debug.Log("[UnitManager] GAME OVER -- lost");
        Managers.Get().game.GameOver(false);
      }
    }

    if(u.faction == Faction.Enemy)
    {
      if(u.deathCausesGameEnd)
      {
        Managers.Get().game.GameOver(true);
      }
      levellingHack *= levellingMod;
    }
  }

  public void OnGameOver()
  {
    foreach(Unit u in allUnits)
    {
      u.MakeZombie();
    }
  }

  void Update()
  {
    onScreenUnits = GetAllUnitsOnScreen();
  }

  public List<Unit> GetAllUnitsOnScreen()
  {
    Camera cam = Camera.main;
    cameraBounds = cam.OrthographicBounds();
    Vector2 pointA = cameraBounds.min;
    Vector2 pointB = cameraBounds.max;
    
    onScreenUnits.Clear();
    unitsOnScreen = Physics2D.OverlapAreaNonAlloc(pointA, pointB, results);
    for(int i = 0; i < unitsOnScreen; i++)
    {
      Unit u = results[i].GetComponentInParent<Unit>();
      if(u != null)
      {
        if(!onScreenUnits.Contains(u))
        {
          onScreenUnits.Add(u);
        }
      }
    }
    return onScreenUnits;
  }
}
