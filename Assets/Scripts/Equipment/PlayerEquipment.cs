using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    //TODO:: 다른 vehicle일 경우 어떻게 처리할지
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

            //TODO:: 장착 위치에 따른 장비의 생성 위치 정해야함.
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
