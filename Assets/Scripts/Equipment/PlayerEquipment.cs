using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    //TODO:: �ٸ� vehicle�� ��� ��� ó������
    [SerializeField] public Dictionary<EquipIndexNumber, GameObject> CurrentEquip = new Dictionary<EquipIndexNumber, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        TruckInit();

        foreach (var data in EquipmentsData.Instance.TruckEquipData)
        {
            if (data.EquipmentId == 0)
                continue;
            Pair<Equipment, bool> result = EquipmentsData.Instance.EquipmentData.Find(pair => pair.Equipment.EquipmentId == data.EquipmentId);
            
            CurrentEquip[data.EquipIndexNumber] = result.Equipment.Prefab;

            //TODO:: ���� ��ġ�� ���� ����� ���� ��ġ ���ؾ���.
            GameObject equipmentPrefab = Instantiate(result.Equipment.Prefab);
            equipmentPrefab.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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