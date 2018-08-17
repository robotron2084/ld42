using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioButton : MonoBehaviour
{
  public AudioSource source;
  bool playing = false;

  void Start()
  {
    string audio = PlayerPrefs.GetString("audio", "True");
    if(audio == "True")
    {
      source.Play();
      playing = true;
    }
  }

  public void ToggleAudio()
  {
    playing = !playing;
    PlayerPrefs.SetString("audio", playing.ToString());
    PlayerPrefs.Save();
    if(playing){
      source.Play();
    }else{
      source.Pause();
    }
  }
}
