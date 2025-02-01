using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _objectItem;
    [SerializeField] private Transform _canvasTransform;        //equipmentPanel
    [SerializeField] private Canvas _canvas;
    private GameObject _tempObject;
    private RectTransform _showImage;
    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    //SetExplain, SetImage를 위해
    [SerializeField] private Text _explainText;
    [SerializeField] private Image _equipmentImage;
    private int _equipmentId;
    [SerializeField] private EquipmentState _isState;
    public EquipmentState IsState { get { return _isState; } set { _isState = value; } }

    //현재 차량에 대한 정보
    //TODO:: 차량 전환 시 바꾸어주어야함.

    private void Start()
    {
        _canvas = FindObjectOfType<LobbyManager>().Canvas;
        _canvasTransform = _canvas.transform;

        _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        //_showImage = _objectItem.GetComponent<Image>();
        //_showImage = GameObject.Find("equipmentImage").GetComponent<Image>();
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(_isState == EquipmentState.Lock)
        {
            return;
        }
        //_beginPosition = _objectItem.transform.position;
        _tempObject = Instantiate(_objectItem, _canvasTransform);
        _showImage = _tempObject.GetComponent<RectTransform>();
        SetObjectSize(_showImage);
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _showImage.transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        Destroy(_tempObject);

        //현재 마우스 위치에 있는 UI 오브젝트 정보를 가져옵니다.
        List<RaycastResult> result = new List<RaycastResult>();

        if(IsState == EquipmentState.Equip)
        {
            Debug.Log("선택한 장비가 이미 장착되어 있습니다.");
            return;
        }

        //PointerEventData 설정
        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = eventData.position;

        //Ratcast를 사용해 UI 오브젝트 탐색
        _raycaster.Raycast(_pointerEventData, result);

        //레이캐스트 결과 처리
        foreach (RaycastResult raycastResult in result)
        {
            //Debug.Log("Hit: " + raycastResult.gameObject.name); // 마우스 아래에 있는 UI 오브젝트의 이름 출력

            // UI 오브젝트의 이미지를 가져오는 경우
            Transform objectComponent = raycastResult.gameObject.transform;
            EquipIndex equipIndex = objectComponent.GetComponent<EquipIndex>();

            //raycast중 EquipIndex를 가지고 있으면 - EquipSlot이면
            if (equipIndex != null)
            {
                //equipSlot에 아이템이 None이 아니면 - 아이템이 들어 있으면
                if (equipIndex.CurrentEquipmentId != 0)
                {
                    //기존 아이템을 None으로 처리한다 - 아이템을 빼준다.
                    GameManager.Instance.EquipmentData.SetEquip(equipIndex.EquipNumber, equipIndex.CurrentEquipmentId, EquipmentState.None, GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
                    equipIndex.CurrentEquipment.SetState(EquipmentState.None);

                    //이전에 장착된 장비를 None으로 변경
                    Pair<Equipment, EquipmentState> prevEquipment = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == equipIndex.CurrentEquipmentId);

                    prevEquipment.State = EquipmentState.None;

                    //TODO:: vehicle 정보는 현재 차량 정보이니 어딘가에 저장해놓기
                    //현재 매번 찾고 있는데 비효율


                }

                SetState(EquipmentState.Equip);
                equipIndex.CurrentEquipment = this;

                equipIndex.SetImage(objectComponent.GetChild(0).GetComponent<RawImage>(), _tempObject, _equipmentId);

                //TODO::ㅅㅂ
                GameManager.Instance.EquipmentData.SetEquip(equipIndex.EquipNumber, _equipmentId, IsState, GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
            }
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        //_objectColor = _objectItem.color;
        //_objectItem.color = _objectItem.color - new Color32(0, 0, 0, 50);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        //_objectItem.color = _objectColor;
    }


    void SetObjectSize(RectTransform _showImage)
    {
        _showImage.anchorMin = new Vector2(0.5f, 0.5f);
        _showImage.anchorMax = new Vector2(0.5f, 0.5f);
        _showImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
        _showImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
    }

    public void SetEquipment(string explain, Sprite image, int exquipmentId)
    {
        _explainText.text = explain;
        _equipmentImage.sprite = image;
        _equipmentId = exquipmentId;
    }

    public void SetState(EquipmentState state)
    {
        switch (state)
        {
            case EquipmentState.None:
                IsState = EquipmentState.None;

                break;

            case EquipmentState.Lock:
                IsState = EquipmentState.Lock;

                break;

            case EquipmentState.Equip:
                IsState = EquipmentState.Equip;

                break;
            
            default:
                break;
        }
    }

}
