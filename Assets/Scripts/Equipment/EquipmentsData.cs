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
    //Pair, Equip 구조는 EquiptmentScriptable에 저장되어있음.
    private static EquipmentsData _instance = null;
    public List<Pair<Equipment, EquipmentState>> EquipmentData = new List<Pair<Equipment, EquipmentState>>();

    //TODO:: vehicle의 종류에 따른 처리가 필요
    //TODO:: Scriptable로 만들어서 EquipmentData처럼 관리해야될거 같다.
    public List<Equip<EquipIndexNumber, int>> TruckEquipData = new List<Equip<EquipIndexNumber, int>>();


    public EquipmentsData(EquiptmentScriptable _equiptmentScriptable)
    {
        InitTruck();
        InitEquipmentData(_equiptmentScriptable);
    }

    private void InitEquipmentData(EquiptmentScriptable _equiptmentScriptable)
    {
        EquipmentData = _equiptmentScriptable.EquipmentData;
    }

    public static EquipmentsData Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }
            return _instance;
        }
    }

    private void InitTruck()
    {
        TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Top, 0));
        TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Left, 0));
        TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Right, 0));
        TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Centor, 0));
        TruckEquipData.Add(new Equip<EquipIndexNumber, int>(EquipIndexNumber.Bottom, 0));
    }

    //TODO:: vehicle 종류에 따른 처리
    public void SetEquip(EquipIndexNumber equipIndexNumber, int equipId, EquipmentState state, int vehicleId = 0)
    {
        Debug.Log(equipIndexNumber.ToString() + equipId.ToString() + vehicleId.ToString());

        //vehicleId로 차량 종류 구분
        switch (vehicleId)
        {
            case 0:
                var result = TruckEquipData.Find(equip => equip.EquipIndexNumber == equipIndexNumber);
                var equip = EquipmentData.Find(equip => equip.Equipment.EquipmentId == equipId);

                equip.Equipment.EquipIndexNumber = result.EquipIndexNumber;
                result.EquipmentId = equipId;
                equip.State = state;
                break;
            default:
                break;
        }
    }
    
}
