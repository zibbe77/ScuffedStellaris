using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //private MaterialApplier materialApplier;

    private void Awake()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //materialApplier.ApplyOther();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //materialApplier.ApplyOriginal();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Empty
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Empty
    }
}
