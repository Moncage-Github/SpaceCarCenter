using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipIndex : MonoBehaviour
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

    public void SetImage(RawImage rawImage, GameObject TempObject, int ExquipmentId)
    {
        rawImage.texture = TempObject.GetComponent<Image>().sprite.texture;
        _currentEquipmentId = ExquipmentId;
    }
}
