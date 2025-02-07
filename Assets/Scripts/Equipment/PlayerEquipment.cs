using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] public Dictionary<EquipIndexNumber, GameObject> CurrentEquip = new Dictionary<EquipIndexNumber, GameObject>();

    private Vehicle _vehicle;

    // Start is called before the first frame update
    void Start()
    {
        _vehicle = GetComponent<Vehicle>();

        VehicleInit(GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);

        foreach (var data in GameManager.Instance.EquipmentData.CurrentVehicle.EquipmentPos)
        {
            //장비가 비었으면 넘어가기
            if (data.ItemId == 0)
                continue;

            //장비가 장착되어 있다면 해당 장비의 정보 찾기
            Pair<Equipment, EquipmentState> result = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(pair => pair.Equipment.EquipmentId == data.ItemId);

            //현재 장비 정보에 해당 장비 프리펩 저장하기
            CurrentEquip[data.EquipmentPositionType] = result.Equipment.Prefab;

            //각 부위별 장비 위치에 장비 생성
            Vector3 position = new Vector3(data.EquipmentPosition.x, data.EquipmentPosition.y * 2, 0);
            GameObject equipmentPrefab = Instantiate(result.Equipment.Prefab, position, Quaternion.identity);

            //해당 장비를 Player의 자식 관계로 설정
            equipmentPrefab.transform.parent = transform;
            //장비 초기화
            equipmentPrefab.GetComponent<BaseEquipment>().SetVehivle(_vehicle);
        }
    }


    void VehicleInit(int vehicleId)
    {
        switch (vehicleId)
        {
            case 0:
                CurrentEquip.Add(EquipIndexNumber.Top, null);
                CurrentEquip.Add(EquipIndexNumber.Left, null);
                CurrentEquip.Add(EquipIndexNumber.Right, null);
                CurrentEquip.Add(EquipIndexNumber.Centor, null);
                CurrentEquip.Add(EquipIndexNumber.Bottom, null);
                break;
            default:
                break;
        }
        
    }
}
