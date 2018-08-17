using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class FXUnit : MonoBehaviour
{
  // optional label
  public TMP_Text label;

  // if this is positive, fx will self destruct.
  public float destroyAfter = 0.0f;

  public void Init(Unit u)
  {
    Init(u.t);
  }

  public void InitLocal(Transform t)
  {
    Init(t);
    transform.SetParent(t, true);

  }

  public void Init(Transform t)
  {
    transform.position = t.position;
    if(destroyAfter > 0.0f)
    {
      Destroy(gameObject, destroyAfter);
    }
  }

  public void Init(FXData data, Transform t)
  {
    if(data.spawnMode == SpawnMode.Global)
    {
      Init(t);
    }else{
      InitLocal(t);
    }
  }

  public void Despawn()
  {
    Destroy(gameObject);
  }

  // for animation system
  public void PlayAudioFX(string key)
  {
    Managers.Get().afx.PlayFX(key);
  }

}
