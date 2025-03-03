using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 장착된 장비가 어떤 장비인지 구분하기 위한 코드

----------------------------------------------
None : 0
부스트 : 1, 조향 : 2
방어막 : 10, 체젠 : 11
기본 : 20, 그물 : 21, 드릴 : 22, 확장 :23
총 : 30, 작살 : 31, 대포 : 32, 레이저 : 33
----------------------------------------------

장착 슬롯의 번호
----------------------------------------------
Enum으로 구분
----------------------------------------------
 */

public enum EquipIndexNumber
{
    None,
    Top,
    Left,
    Right,
    Center,
    Bottom
    
}


public enum EquipmentState
{
    None,
    Lock,
    Equip
}


/*
 시작 버튼 클릭 시 현재 장착된 장비 정보를 저장
 */


public class EquipmentsData
{
    public EquipmentScriptable EquipmentScriptable;

    public int CurrentVehicleId;
    public VehicleInfo CurrentVehicle;
    public EquipmentsData(EquipmentScriptable _equiptmentScriptable)
    {
        InitEquipmentData(_equiptmentScriptable);
    }

    private void InitEquipmentData(EquipmentScriptable _equiptmentScriptable)
    {
        EquipmentScriptable = _equiptmentScriptable;
    }

    public void SetEquip(EquipIndexNumber equipIndexNumber, int equipId, EquipmentState state, int vehicleId = 0)
    {
        if(equipId == 0)
        {
            Debug.Log(equipIndexNumber.ToString() + ("는 비어있음"));
            return;
        }

        Debug.Log(equipIndexNumber.ToString() + equipId.ToString() + vehicleId.ToString());

        var vehicle = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(vehicle => vehicle.VehicleId == vehicleId);

        VehicleEquipmentInfo equipInfo = vehicle.EquipmentPos.Find(position => position.EquipmentPositionType == equipIndexNumber);

        if (state == EquipmentState.Equip)
        {
            equipInfo.ItemId = equipId;
        }
        else if (state == EquipmentState.None)
        {
            equipInfo.ItemId = 0;
        }

        var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == equipId);

        equip.State = state;
    }

}
