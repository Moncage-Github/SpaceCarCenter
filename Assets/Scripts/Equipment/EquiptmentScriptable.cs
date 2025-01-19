using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*-------------------------------------
        ���� ���� ID ��

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
    public List<Vector3> EquipmentPos;

    public VehicleInfo(int first, Vector3 second)
    {
        VehicleId = first;
        EquipmentPos.Add(second);
    }
}

[CreateAssetMenu(fileName = "EquipmentScriptable", menuName = "Data/EquipmentScriptable")]
public class EquiptmentScriptable : ScriptableObject
{
    public List<Pair<Equipment, EquipmentState>> EquipmentData;

    //� ������ �����ߴ���, �� ������ ������ ���� ��ġ ����
    public List<VehicleInfo> VehicleInfos;
}
