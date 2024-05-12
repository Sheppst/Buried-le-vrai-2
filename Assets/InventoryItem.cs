using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;
  //Drag n Drop
  public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
      
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
      
        transform.SetParent(parentAfterDrag);
    }
    
}
