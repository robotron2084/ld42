using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Managers : MonoBehaviour
{

  public InputManager input;
  public GameManager game;
  public UIManager ui;
  public UnitManager units;
  public FXSpawner fx; //global fx
  public AudioFXManager afx;
  
  static Managers instance;

  void Awake()
  {
    instance = this;
  }

  public static Managers Get()
  {
    return instance;
  }
}
