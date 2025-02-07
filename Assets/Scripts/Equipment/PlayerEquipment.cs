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
            //��� ������� �Ѿ��
            if (data.ItemId == 0)
                continue;

            //��� �����Ǿ� �ִٸ� �ش� ����� ���� ã��
            Pair<Equipment, EquipmentState> result = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(pair => pair.Equipment.EquipmentId == data.ItemId);

            //���� ��� ������ �ش� ��� ������ �����ϱ�
            CurrentEquip[data.EquipmentPositionType] = result.Equipment.Prefab;

            //�� ������ ��� ��ġ�� ��� ����
            Vector3 position = new Vector3(data.EquipmentPosition.x, data.EquipmentPosition.y * 2, 0);
            GameObject equipmentPrefab = Instantiate(result.Equipment.Prefab, position, Quaternion.identity);

            //�ش� ��� Player�� �ڽ� ����� ����
            equipmentPrefab.transform.parent = transform;
            //��� �ʱ�ȭ
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
