using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{

  void Start()
  {
    Managers.Get().ui.gameObject.SetActive(true);
  }

  public void GameOver(bool hasWon)
  {
    Managers m = Managers.Get();
    m.ui.OnGameOver(hasWon);
    m.units.OnGameOver();
  }

  public void ReturnToMenu()
  {
    SceneManager.LoadScene("Menu");
  }
}
