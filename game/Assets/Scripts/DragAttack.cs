using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class DragAttack : Attack
{
  public RectTransform dragIcon;
  public RectTransform dragIconConfirm;

  public virtual void ValidateDropTarget(DragOperation drag)
  {
    foreach(Transform t in m.input.hits)
    {
      Unit u = t.GetComponentInParent<Unit>();
      if(u != null && u.faction == unit.faction)
      {
        drag.target = u;
        drag.status = 1;
        return;
      }
    }
    drag.target = null;
    drag.status = -1;
  }

  public override Unit GetValidTarget()
  {
    return unit;
  }

}
