using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    //TODO:: �ٸ� vehicle�� ��� ��� ó������
    [SerializeField] public Dictionary<EquipIndexNumber, GameObject> CurrentEquip = new Dictionary<EquipIndexNumber, GameObject>();

    private Vehicle _vehicle;

    // Start is called before the first frame update
    void Start()
    {
        _vehicle = GetComponent<Vehicle>();

        TruckInit();

        foreach (var data in GameManager.Instance.EquipmentData.TruckEquipData)
        {
            if (data.EquipmentId == 0)
                continue;
            Pair<Equipment, EquipmentState> result = GameManager.Instance.EquipmentData.EquipmentData.Find(pair => pair.Equipment.EquipmentId == data.EquipmentId);
            
            CurrentEquip[data.EquipIndexNumber] = result.Equipment.Prefab;

            //TODO:: ���� ��ġ�� ���� ����� ���� ��ġ ���ؾ���.
            //vehicleId�� 0�ΰ� ã�Ƽ� �� ��ġ�� ����
            GameObject equipmentPrefab = Instantiate(result.Equipment.Prefab);
            equipmentPrefab.transform.parent = transform;
            equipmentPrefab.GetComponent<BaseEquipment>().SetVehivle(_vehicle);
        }
    }


    void TruckInit()
    {
        CurrentEquip.Add(EquipIndexNumber.Top, null);
        CurrentEquip.Add(EquipIndexNumber.Left, null);
        CurrentEquip.Add(EquipIndexNumber.Right, null);
        CurrentEquip.Add(EquipIndexNumber.Centor, null);
        CurrentEquip.Add(EquipIndexNumber.Bottom, null);
    }
}
