using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExplanationUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _objectItem;
    [SerializeField] private Transform _canvasTransform;        //equipmentPanel
    [SerializeField] private Canvas _canvas;
    private GameObject _tempObject;
    private RectTransform _showImage;
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;
    private Vector2 _beginPosition;
    private Color32 _objectColor;

    private void Start()
    {
        _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        //_showImage = _objectItem.GetComponent<Image>();
        //_showImage = GameObject.Find("equipmentImage").GetComponent<Image>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //_beginPosition = _objectItem.transform.position;
        _tempObject = Instantiate(_objectItem, _canvasTransform);
        _showImage = _tempObject.GetComponent<RectTransform>();
        SetObjectSize(_showImage);
        
        Debug.Log("OnBeginDrag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _showImage.transform.position = eventData.position;
        Debug.Log("OnDrag");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        Debug.Log(eventData.position);
        Destroy(_tempObject);

        //현재 마우스 위치에 있는 UI 오브젝트 정보를 가져옵니다.
        List<RaycastResult> result = new List<RaycastResult>();

        //PointerEventData 설정
        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = eventData.position;

        //Ratcast를 사용해 UI 오브젝트 탐색
        _raycaster.Raycast(_pointerEventData, result);

        //레이캐스트 결과 처리
        foreach (RaycastResult raycastResult in result)
        {
            Debug.Log("Hit: " + raycastResult.gameObject.name); // 마우스 아래에 있는 UI 오브젝트의 이름 출력

            // UI 오브젝트의 이미지를 가져오는 경우
            Transform objectComponent = raycastResult.gameObject.transform;
            if (objectComponent != null)
            {
                if(objectComponent.name == "equipmentMaskTop" ||
                    objectComponent.name == "equipmentMaskLeft" ||
                    objectComponent.name == "equipmentMaskRight" ||
                    objectComponent.name == "equipmentMaskSenter" ||
                    objectComponent.name == "equipmentMaskBottom")
                {
                    RawImage imageComponent = objectComponent.GetChild(0).GetComponent<RawImage>();
                    imageComponent.texture = _tempObject.GetComponent<Image>().sprite.texture;
                }
                
            }
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //_objectColor = _objectItem.color;
        //_objectItem.color = _objectItem.color - new Color32(0, 0, 0, 50);
        Debug.Log("OnPointerDown");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //_objectItem.color = _objectColor;
        Debug.Log("OnPointerUp");
    }


    void SetObjectSize(RectTransform _showImage)
    {
        _showImage.anchorMin = new Vector2(0.5f, 0.5f);
        _showImage.anchorMax = new Vector2(0.5f, 0.5f);
        _showImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
        _showImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
    }
}
