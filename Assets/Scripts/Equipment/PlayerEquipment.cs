using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    //TODO:: 다른 vehicle일 경우 어떻게 처리할지
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

            //TODO:: 장착 위치에 따른 장비의 생성 위치 정해야함.
            //vehicleId가 0인걸 찾아서 각 위치에 장착
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
