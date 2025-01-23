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

    public VehicleInfo(int first, VehicleEquipmentInfo second)
    {
        VehicleId = first;
        EquipmentPos.Add(second);
    }
}

[System.Serializable]
public class VehicleEquipmentInfo
{
    public int VehicleId;
    public Vector3 EquipmentPos;

    public VehicleEquipmentInfo(int first, Vector3 second)
    {
        VehicleId = first;
        EquipmentPos = second;
    }
}

[CreateAssetMenu(fileName = "EquipmentScriptable", menuName = "Data/EquipmentScriptable")]
public class EquiptmentScriptable : ScriptableObject
{
    public List<Pair<Equipment, EquipmentState>> EquipmentData;

    //어떤 차량을 선택했는지, 그 차량의 아이템 장착 위치 정보
    public List<VehicleInfo> VehicleInfos;

}
