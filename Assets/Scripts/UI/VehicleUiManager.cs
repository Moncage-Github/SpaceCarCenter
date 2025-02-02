                using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class VehicleUiManager : MonoBehaviour
{
    //TODO:: ���� VegicleInfos�� �ٷ� �־ �ɰ� ����
    [SerializeField] private List<GameObject> _vehicleInfo = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        EquipIndexInit();
    }


    //������ �ٲ� �� �ٲ� �������� �������ִ� �Լ�
    public void InitVehicleUi(int VehicleId, int value)
    {
        Debug.Log(VehicleId);
        Debug.Log(value);

        GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle = VehicleId;

        _vehicleInfo[VehicleId].SetActive(true);
        if(VehicleId + value < 0)
        {
            _vehicleInfo[GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Count - 1].SetActive(false);
        }
        else if(VehicleId + value >= GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Count)
        {
            _vehicleInfo[0].SetActive(false);
        }
        else
        {
            _vehicleInfo[VehicleId + value].SetActive(false);
        }

        EquipIndexInit();

        //���� ������ �´� UI â���� �� ����
        ManagerEquipmentSlot.Instance.Init();
    }

    //���� ���� ID�� EquipmentPos�� �°� EquipIndex�� ��ġ���ִ� �Լ�
    public void EquipIndexInit()
    {
        int vehicleId = GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle;

        VehicleInfo vehicleInfo = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(equip => equip.VehicleId == vehicleId);


        //EquipIndex�� VehicleInfos�� ���缭 ��ġ���ش�. ������ X : 100, Y : 200
        foreach (Transform child in _vehicleInfo[vehicleId].transform)
        {
            if (child.GetComponent<EquipIndex>())
            {
                VehicleEquipmentInfo info = vehicleInfo.EquipmentPos.Find(info => info.EquipmentPositionType == child.GetComponent<EquipIndex>().EquipNumber);

                if (info != null)
                {
                    child.transform.localPosition = new Vector3(info.EquipmentPosition.x * 100, info.EquipmentPosition.y * 200, 0);

                    var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == info.ItemId);

                    if(equip != null)
                    {
                        child.transform.GetChild(0).GetComponent<RawImage>().texture = equip.Equipment.ImageLog.texture;
                    }
                    
                }
            }
        }

    }
}
