using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioFXManager : MonoBehaviour
{
  public AudioSource source;
  bool audioOn = false;

  [System.Serializable]
  public class AudioData
  {
    public AudioClip clip;
    public string name;
    public float volume = 1.0f;
  }

  public List<AudioData> audioDatum = new List<AudioData>();
  public Dictionary<string, AudioData> dataMap = new Dictionary<string, AudioData>();

  void Start()
  {
    string audio = PlayerPrefs.GetString("audio", "True");
    if(audio == "True")
    {
      audioOn = true;
    }

    foreach(AudioData data in audioDatum)
    {
      dataMap[data.name] = data;
    }
    
  }

  public void PlayFX(string key)
  {
    if(audioOn)
    {
      AudioData data = null;
      if(dataMap.TryGetValue(key, out data))
      {
        // Debug.Log("[AudioData] Playing fx:" +key);
        source.PlayOneShot(data.clip, data.volume);
      }
    }
  }

}
