using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipIndex : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] 
    private             EquipIndexNumber _equipNumber;
    public              EquipIndexNumber EquipNumber {  get { return _equipNumber; } }

    [SerializeField] 
    private int         _currentEquipmentId = 0;
    public int          CurrentEquipmentId {  get { return _currentEquipmentId; } set { _currentEquipmentId = value; } }
    public EquipmentUI  CurrentEquipment;

    private void Start()
    {
        
        Init();
    }
    private void Init()
    {
        Debug.Log($"{gameObject.name} - Awake ȣ���");
        Debug.Log(GameManager.Instance);
        Debug.Log(GameManager.Instance.EquipmentData);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
        Debug.Log("");
        //Index �̹��� �ʱ⿡ ���� �� ID �� ����
        var vehicle = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(vehicle => vehicle.VehicleId == GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
            

        var equip = vehicle.EquipmentPos.Find(vehicleEquipment => vehicleEquipment.EquipmentPositionType == _equipNumber);

        _currentEquipmentId = equip.ItemId;

        //var equipment = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equipment => equipment.Equipment.EquipmentId == _currentEquipmentId);

    }

    public void SetImage(Image Image, GameObject TempObject, int ExquipmentId)
    {
        Image.sprite = TempObject.GetComponent<Image>().sprite;
        _currentEquipmentId = ExquipmentId;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ���콺 ��Ŭ������ Ȯ��
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("���콺 ��Ŭ�� �߻�!");

            //���� �������� ����
            GameManager.Instance.EquipmentData.SetEquip(EquipNumber, CurrentEquipmentId, EquipmentState.None, GameManager.Instance.EquipmentData.CurrentVehicleId);

            //TODO:: EquipmentData���� None���� ���� : id ���� �̿�
            Pair<Equipment, EquipmentState> prevEquipment = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == CurrentEquipmentId);

            prevEquipment.State = EquipmentState.None;

            //TODO:: EquipmentUI���� Noned�� ���� : �Ŵ���equipslot�� pool�� �̿�
            EquipmentUI equipmentUI = ManagerEquipmentSlot.Instance.SlotPool.Find(equip => equip.EquipmentId == CurrentEquipmentId);

            equipmentUI.SetState(EquipmentState.None);

            CurrentEquipmentId = 0;
            CurrentEquipment = null;

            VehicleUiManager.Instance.EquipIndexInit();
        }
    }
}
