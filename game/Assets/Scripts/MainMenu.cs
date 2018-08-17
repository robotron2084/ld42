using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject creditsUI;

  void Start()
  {
    ShowCredits(false);
  }
  
  public void PlayGame()
  {
    SceneManager.LoadScene("Level1");
  }

  public void ShowCredits(bool show)
  {
    creditsUI.SetActive(show);
  }
}
