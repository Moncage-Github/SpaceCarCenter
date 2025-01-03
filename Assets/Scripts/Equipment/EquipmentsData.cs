using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 Equipment;
    public T2 IsLock;

    public Pair(T1 first, T2 second)
    {
        Equipment = first;
        IsLock = second;
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


/*
 ���� ��ư Ŭ�� �� ���� ������ ��� ������ ����
 */


public class EquipmentsData : MonoBehaviour
{
    

    private static EquipmentsData _instance = null;
    public List<Pair<Equipment, bool>> EquipmentData;

    //TODO:: vehicle�� ������ ���� ó���� �ʿ�
    public List<Equip<EquipIndexNumber, int>> TruckEquipData;


    [SerializeField] public Canvas Canvas;
    [SerializeField] public Transform CanvasTransform;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            //EquipmentData = new List<Pair<Equipment, bool>>();
        }

        InitTruck();
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
    public void SetEquip(EquipIndexNumber equipIndexNumber, int equipId, int vehicleId = 0)
    {
        Debug.Log(equipIndexNumber.ToString() + equipId.ToString() + vehicleId.ToString());

        switch (vehicleId)
        {
            case 0:
                var result = TruckEquipData.Find(equip => equip.EquipIndexNumber == equipIndexNumber);
                result.EquipmentId = equipId;
                break;
            default:
                break;
        }
    }

}