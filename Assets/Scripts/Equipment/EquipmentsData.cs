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
    Centor,
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
    public EquipmenScriptable EquipmentScriptable;

    public int CurrentVehicleId;
    public EquipmentsData(EquipmenScriptable _equiptmentScriptable)
    {
        InitEquipmentData(_equiptmentScriptable);
    }

    private void InitEquipmentData(EquipmenScriptable _equiptmentScriptable)
    {
        EquipmentScriptable = _equiptmentScriptable;
    }

    //private void InitTruck()
    //{
    //    TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Top, 0));
    //    TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Left, 0));
    //    TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Right, 0));
    //    TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Centor, 0));
    //    TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Bottom, 0));
    //}

    //TODO:: vehicle 종류에 따른 처리
    public void SetEquip(EquipIndexNumber equipIndexNumber, int equipId, EquipmentState state, int vehicleId = 0)
    {
        Debug.Log(equipIndexNumber.ToString() + equipId.ToString() + vehicleId.ToString());

        if (state == EquipmentState.Equip)
        {
            var vehicle = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(vehicle => vehicle.VehicleId == vehicleId);

            VehicleEquipmentInfo equipInfo = vehicle.EquipmentPos.Find(position => position.EquipmentPositionType == equipIndexNumber);

            equipInfo.ItemId = equipId;
        }

        var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == equipId);

        equip.State = state;
    }

}
