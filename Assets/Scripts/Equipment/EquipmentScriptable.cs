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

    [Header("���� ���õ� ���� ID")]
    public int CurrentSelectVehicle;

    [Space(10)]
    [Header("������ ���(�Ϲ�, ���� ��, ���)")]
    public List<Pair<Equipment, EquipmentState>> EquipmentData;

    [Space(10)]
    [Header("Ư�� ���� ID �� �ش� ������ ������ ���� ��ġ�� ������ ID")]
    //� ������ �����ߴ���, �� ������ ������ ���� ��ġ ����
    public List<VehicleInfo> VehicleInfos;

}
