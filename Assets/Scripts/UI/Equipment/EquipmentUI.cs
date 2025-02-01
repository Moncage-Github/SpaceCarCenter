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

    //SetExplain, SetImage�� ����
    [SerializeField] private Text _explainText;
    [SerializeField] private Image _equipmentImage;
    private int _equipmentId;
    [SerializeField] private EquipmentState _isState;
    public EquipmentState IsState { get { return _isState; } set { _isState = value; } }

    //���� ������ ���� ����
    //TODO:: ���� ��ȯ �� �ٲپ��־����.

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

        //���� ���콺 ��ġ�� �ִ� UI ������Ʈ ������ �����ɴϴ�.
        List<RaycastResult> result = new List<RaycastResult>();

        if(IsState == EquipmentState.Equip)
        {
            Debug.Log("������ ��� �̹� �����Ǿ� �ֽ��ϴ�.");
            return;
        }

        //PointerEventData ����
        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = eventData.position;

        //Ratcast�� ����� UI ������Ʈ Ž��
        _raycaster.Raycast(_pointerEventData, result);

        //����ĳ��Ʈ ��� ó��
        foreach (RaycastResult raycastResult in result)
        {
            //Debug.Log("Hit: " + raycastResult.gameObject.name); // ���콺 �Ʒ��� �ִ� UI ������Ʈ�� �̸� ���

            // UI ������Ʈ�� �̹����� �������� ���
            Transform objectComponent = raycastResult.gameObject.transform;
            EquipIndex equipIndex = objectComponent.GetComponent<EquipIndex>();

            //raycast�� EquipIndex�� ������ ������ - EquipSlot�̸�
            if (equipIndex != null)
            {
                //equipSlot�� �������� None�� �ƴϸ� - �������� ��� ������
                if (equipIndex.CurrentEquipmentId != 0)
                {
                    //���� �������� None���� ó���Ѵ� - �������� ���ش�.
                    GameManager.Instance.EquipmentData.SetEquip(equipIndex.EquipNumber, equipIndex.CurrentEquipmentId, EquipmentState.None, GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
                    equipIndex.CurrentEquipment.SetState(EquipmentState.None);

                    //������ ������ ��� None���� ����
                    Pair<Equipment, EquipmentState> prevEquipment = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == equipIndex.CurrentEquipmentId);

                    prevEquipment.State = EquipmentState.None;

                    //TODO:: vehicle ������ ���� ���� �����̴� ��򰡿� �����س���
                    //���� �Ź� ã�� �ִµ� ��ȿ��


                }

                SetState(EquipmentState.Equip);
                equipIndex.CurrentEquipment = this;

                equipIndex.SetImage(objectComponent.GetChild(0).GetComponent<RawImage>(), _tempObject, _equipmentId);

                //TODO::����
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
