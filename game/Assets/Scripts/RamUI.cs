using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class RamUI : MonoBehaviour
{
  public Animator bitPrefab;

  public Transform bitsContainer;

  public List<Animator> bits = new List<Animator>();

  public int bitsFlipped;

  public TMP_Text label;

  Unit unit;

  void Start()
  {
    Init(null);
  }

  public void Init(Unit u)
  {
    unit = u;
    if(unit == null)
    {
      gameObject.SetActive(false);
      return;
    }
    if(unit.maxMana <= 0.0f)
    {
      gameObject.SetActive(false);
      return;
    }else{
      gameObject.SetActive(true);
    }
    foreach(Animator bit in bits)
    {
      Destroy(bit.gameObject);
    }
    bits.Clear();

    for(int i = 0; i < unit.maxMana; i++)
    {
      Animator anim = (Animator)Instantiate(bitPrefab);
      anim.transform.SetParent(bitsContainer, false);
      anim.SetBool("Show", i < unit.mana);
      bits.Add(anim);
    }
    bitsFlipped = Mathf.RoundToInt(unit.mana);
    label.text = bitsFlipped.ToString();

  }

  void Update()
  {
    if(unit == null)
    {
      return;
    }

    int mana = Mathf.RoundToInt(unit.mana);
    for(int i = 0; i < bits.Count; i++)
    {
      bits[i].SetBool("Show", i < mana);
    }
    label.text = bitsFlipped.ToString();
  }
}
