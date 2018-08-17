using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ShowViewButton : MonoBehaviour
{
  public Transform viewToShow;

  public void ShowView()
  {
    Managers m = Managers.Get();
    Transform view = Instantiate(viewToShow);
    view.SetParent(m.ui.transform, false);
  }
}
