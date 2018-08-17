using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Collections;

public class InputManager : MonoBehaviour
{

  public Transform ground; // location of ground plane for units.
  public Transform floor; // the floor of the dungeon.

  public RectTransform dragContainer;

  Unit selectedUnit;

  Plane groundPlane;

  Camera cam;
  public List<Transform> hits = new List<Transform>();

  Managers m;

  DragOperation dragOp = null;

  void Start()
  {
    cam = Camera.main;
    groundPlane = new Plane(Vector3.forward, ground.transform.position);
    m = Managers.Get();
  }

  void Update()
  {
    Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D[] raycastHits = Physics2D.RaycastAll(mouseRay.origin, mouseRay.direction);
    hits.Clear();
    foreach(RaycastHit2D hit in raycastHits)
    {
      hits.Add(hit.transform);
    }
    hits.Reverse();

    if(dragOp != null)
    {
      dragOp.attack.ValidateDropTarget(dragOp);
      if(Input.GetMouseButtonUp(0))
      {
        if(dragOp.status == 1)
        {
          Debug.Log("[InputManager] DROP SUCCESS");
          dragOp.attack.DoAttack(dragOp.target);
        }else{
          Debug.Log("[InputManager] CANCEL");
        }
        Destroy(dragOp.icon.gameObject);
        Destroy(dragOp.iconConfirm.gameObject);
        dragOp = null;
      }else{
        updateDragIcon();
      }
    }else{
      if(EventSystem.current.IsPointerOverGameObject())
      {
        
      }else{

        if(Input.GetMouseButtonUp(0))
        {
          Unit bestChoice = null;
          bool hitFloor = false;
          foreach(Transform hit in hits)
          {
            if(hit == floor)
            {
              hitFloor = true;
            }
            Unit u = hit.GetComponentInParent<Unit>();
            if(u != null){

              if(u.faction == Faction.Hero)
              {
                if(selectedUnit != null && bestChoice == selectedUnit)
                {
                  //this is a better choice.
                  bestChoice = u;
                }else
                {
                  bestChoice = u;
                }

              }else{
                // enemy select
                if(bestChoice == null)
                {
                  bestChoice = u;
                }
              }
            }
          }

          if(bestChoice == null && hitFloor)
          {
            OnBackgroundUp();
          }else if(bestChoice != null){
            OnUnitSelect(bestChoice);
          }

        }
      }

    }
  }

  public void OnUnitDown(Unit u)
  {
  }

  public void OnUnitSelect(Unit u)
  {

    // if(EventSystem.current.IsPointerOverGameObject())
    // {
    //   return;
    // }

    // if(dragOp != null)
    // {
    //   return;
    // }


    if(u.faction == Faction.Hero)
    {
      if(u == selectedUnit)
      {
        deselect();
        return;
      }

      // deselect any existing unit
      if(selectedUnit != null)
      {
        deselect();
      }

      //select the unit.
      select(u);
    }
    if(u.faction == Faction.Enemy)
    {
      if(selectedUnit != null)
      {
        selectedUnit.WalkNextTo(u);
      }
    }
  }

  void select(Unit u)
  {
    selectedUnit = u;
    selectedUnit.fx.Spawn(Keys.Selection);
    m.ui.showUIForUnit(selectedUnit);
    m.afx.PlayFX(Keys.Selection);
  }

  void deselect()
  {
    selectedUnit.fx.Despawn(Keys.Selection);
    selectedUnit = null;
    m.ui.showUIForUnit(null);
  }

  public void OnBackgroundUp()
  {
    if(EventSystem.current.IsPointerOverGameObject())
    {
      return;
    }
    if(dragOp != null)
    {
      return;
    }

    if(selectedUnit != null)
    {
      Vector3 mouseOnGroundPlane = getPointOnGround();
      selectedUnit.WalkTo(mouseOnGroundPlane);
    }
  }

  public Vector3 getPointOnGround()
  { 
    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    float enter = 0.0f;
    if(groundPlane.Raycast(ray, out enter))
    {
      return ray.GetPoint(enter);
    }
    return Vector3.zero;
  }

  public void BeginDrag(DragAttack atk)
  {
    dragOp = new DragOperation();
    dragOp.attack = atk;
    dragOp.icon = Instantiate(atk.dragIcon);
    dragOp.icon.transform.SetParent(dragContainer, false);
    dragOp.iconConfirm = Instantiate(atk.dragIconConfirm);
    dragOp.iconConfirm.transform.SetParent(dragContainer, false);

    updateDragIcon();
  }

  void updateDragIcon()
  {
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(dragContainer, Input.mousePosition, null, out localPoint );
    RectTransform currentIcon = dragOp.icon;
    if(dragOp.status == 1)
    {
      currentIcon = dragOp.iconConfirm;
      dragOp.icon.gameObject.SetActive(false);
    }else{
      dragOp.iconConfirm.gameObject.SetActive(false);
    }
    currentIcon.gameObject.SetActive(true);
    currentIcon.anchoredPosition = localPoint;
  }

}
