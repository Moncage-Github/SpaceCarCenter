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
    //Pair, Equip ������ EquiptmentScriptable�� ����Ǿ�����.
    private static EquipmentsData _instance = null;
    public List<Pair<Equipment, EquipmentState>> EquipmentData = new List<Pair<Equipment, EquipmentState>>();

    //TODO:: vehicle�� ������ ���� ó���� �ʿ�
    //TODO:: Scriptable�� ���� EquipmentDataó�� �����ؾߵɰ� ����.
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

    //TODO:: vehicle ������ ���� ó��
    public void SetEquip(EquipIndexNumber equipIndexNumber, int equipId, EquipmentState state, int vehicleId = 0)
    {
        Debug.Log(equipIndexNumber.ToString() + equipId.ToString() + vehicleId.ToString());

        //vehicleId�� ���� ���� ����
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
