using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*-------------------------------------
        차량 정보 ID 값

 ------------------------------------*/

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 Equipment;
    public T2 State;

    public Pair(T1 first, T2 second)
    {
        Equipment = first;
        State = second;
    }
}

[System.Serializable]
public class Equip<T1, T2>
{
    public T1 EquipIndexNumber;
    public T2 EquipmentId;

    public Equip(T1 first, T2 second)
    {
        EquipIndexNumber = first;
        EquipmentId = second;
    }
}

[System.Serializable]
public class VehicleInfo
{
    public int VehicleId;
    public List<VehicleEquipmentInfo> EquipmentPos;

}

[System.Serializable]
public class VehicleEquipmentInfo
{
    public int ItemId;
    public EquipIndexNumber EquipmectPositionType;
    public Vector3 EquipmentPosition;

}

[CreateAssetMenu(fileName = "EquipmentScriptable", menuName = "Data/EquipmentScriptable")]
public class EquipmenScriptable : ScriptableObject
{

    [Header("현재 선택된 차량 ID")]
    public int CurrentSelectVehicle;

    [Space(10)]
    [Header("아이템 목록(일반, 장착 중, 잠금)")]
    public List<Pair<Equipment, EquipmentState>> EquipmentData;

    [Space(10)]
    [Header("특정 차량 ID 및 해당 차량의 아이템 장착 위치와 아이템 ID")]
    //어떤 차량을 선택했는지, 그 차량의 아이템 장착 위치 정보
    public List<VehicleInfo> VehicleInfos;

}
