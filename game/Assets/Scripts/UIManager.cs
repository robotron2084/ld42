using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{

  Unit unit;

  public Transform unitUIContainer;
  public UIAttackView attackViewPrefab;

  public Transform portraitView;
  public Transform portraitContainer;
  [System.NonSerialized][HideInInspector]
  public Transform portrait; // instance
  public TMP_Text unitDescription;

  public Transform gameOverWin;
  public Transform gameOverLose;


  public RamUI ramUI;

  List<UIAttackView> uiAttackViews = new List<UIAttackView>();

  void Start()
  {
    clearViews();
    gameOverWin.gameObject.SetActive(false);
    gameOverLose.gameObject.SetActive(false);
  }

  public void showUIForUnit(Unit u)
  {
    unit = u;
    clearViews();
    ramUI.Init(u);

    if(unit != null)
    {
      BaseHero hero = (BaseHero)u;
      portraitView.gameObject.SetActive(true);
      portrait = Instantiate(hero.portraitPrefab);
      portrait.SetParent(portraitContainer, false);
      unitDescription.text = hero.description;

      foreach(Attack atk in unit.attacks)
      {
        UIAttackView view = (UIAttackView)Instantiate(attackViewPrefab);
        view.Init(atk);
        view.transform.SetParent(unitUIContainer, false);
        uiAttackViews.Add(view);
      }
    }
  }

  void clearViews()
  {
    if(portrait != null)
    {
      Destroy(portrait.gameObject);
    }
    portraitView.gameObject.SetActive(false);
    foreach(UIAttackView v in uiAttackViews)
    {
      Destroy(v.gameObject);
    }
    uiAttackViews.Clear();
  }

  public void OnGameOver(bool hasWon)
  {
    StartCoroutine(onGameOver(hasWon));
  }

  IEnumerator onGameOver(bool hasWon)
  {
    yield return new WaitForSeconds(1.0f);
    if(hasWon)
    {
      gameOverWin.gameObject.SetActive(true);
    }else{
      gameOverLose.gameObject.SetActive(true);
    }
  }

}
