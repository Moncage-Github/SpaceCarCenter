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
        Debug.Log($"{gameObject.name} - Awake 호출됨");
        Debug.Log(GameManager.Instance);
        Debug.Log(GameManager.Instance.EquipmentData);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos);
        Debug.Log(GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);
        Debug.Log("");
        //Index 이미지 초기에 설정 및 ID 값 설정
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
        // 마우스 우클릭인지 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("마우스 우클릭 발생!");

            //장착 정보에서 제거
            GameManager.Instance.EquipmentData.SetEquip(EquipNumber, CurrentEquipmentId, EquipmentState.None, GameManager.Instance.EquipmentData.CurrentVehicleId);

            //TODO:: EquipmentData에서 None으로 설정 : id 값을 이용
            Pair<Equipment, EquipmentState> prevEquipment = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == CurrentEquipmentId);

            prevEquipment.State = EquipmentState.None;

            //TODO:: EquipmentUI에서 Noned로 설정 : 매니저equipslot에 pool을 이용
            EquipmentUI equipmentUI = ManagerEquipmentSlot.Instance.SlotPool.Find(equip => equip.EquipmentId == CurrentEquipmentId);

            equipmentUI.SetState(EquipmentState.None);

            CurrentEquipmentId = 0;
            CurrentEquipment = null;

            VehicleUiManager.Instance.EquipIndexInit();
        }
    }
}
