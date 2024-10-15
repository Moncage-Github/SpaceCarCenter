using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExplanationUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _objectImage;
    private Vector2 _beginPosition;
    private Color32 _objectColor;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //_beginPosition = eventData.position;
        _beginPosition = transform.position;
        _objectImage.color = _objectImage.color - new Color32(0, 0, 0, 50);
        Debug.Log("OnBeginDrag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        Debug.Log("OnDrag");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        Debug.Log(eventData.position);

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _objectColor = _objectImage.color;
        _objectImage.color = _objectImage.color - new Color32(0, 0, 0, 50);
        Debug.Log("OnPointerDown");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        _objectImage.color = _objectColor;
        Debug.Log("OnPointerUp");
    }

}
