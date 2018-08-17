using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class UIAttackView : MonoBehaviour
{
  public Image image;
  public TMP_Text label;

  public Image cooldownImage;

  public Image border;

  public Color disabledColor;

  Attack atk;
  Managers m;

  public void Init(Attack a)
  { 
    atk = a;
    image.sprite = atk.attackIcon;
    label.text = atk.description;
    m = Managers.Get();
  }

  void Update()
  {
    Unit u = atk.GetValidTarget();
    if(u != null)
    {
      cooldownImage.fillAmount = atk.cooldown / atk.cooldownAmt;
    }else{
      cooldownImage.fillAmount = 1.0f;
    }

    border.enabled = atk.CanAttack();
  }

  public void OnButtonDown()
  {
    if(!atk.CanAttack())
    {
      return;
    }
    if(atk is DragAttack)
    {
      m.input.BeginDrag((DragAttack)atk);
    }
    // Unit u = atk.GetValidTarget();
    // if(u != null && atk.CanAttack())
    // {
    //   atk.DoAttack(u);
    // }
  }


}
