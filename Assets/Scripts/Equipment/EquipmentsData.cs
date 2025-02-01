using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 ������ ��� � ������� �����ϱ� ���� �ڵ�

----------------------------------------------
None : 0
�ν�Ʈ : 1, ���� : 2
�� : 10, ü�� : 11
�⺻ : 20, �׹� : 21, �帱 : 22, Ȯ�� :23
�� : 30, �ۻ� : 31, ���� : 32, ������ : 33
----------------------------------------------

���� ������ ��ȣ
----------------------------------------------
Enum���� ����
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
 ���� ��ư Ŭ�� �� ���� ������ ��� ������ ����
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

    //TODO:: vehicle ������ ���� ó��
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
