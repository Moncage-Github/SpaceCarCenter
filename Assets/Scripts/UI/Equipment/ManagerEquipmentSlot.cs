using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class ManagerEquipmentSlot : MonoBehaviour
{
    public static ManagerEquipmentSlot Instance;

    private void Awake()
    {
        Instance = this; // 현재 인스턴스를 저장
    }

    [SerializeField] private GameObject _prefabEquipment;
    [SerializeField] private Transform _content;

    // Start is called before the first frame update
    void Start()
    {

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }

        VehicleInfo vehicleInfo = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(equip => equip.VehicleId == GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);

        foreach (var equipment in GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData)
        {
            Debug.Log("equipment 생성");
            GameObject slot = Instantiate(_prefabEquipment);
            EquipmentUI equipmentUi = slot.GetComponent<EquipmentUI>();

            equipmentUi.SetEquipment(equipment.Equipment.Explan, equipment.Equipment.ImageLog, equipment.Equipment.EquipmentId);

            if (equipment.State == EquipmentState.Lock)
            {
                equipmentUi.IsState = equipment.State;
            }
            else
            {
                equipmentUi.IsState = EquipmentState.None;
                equipment.State = EquipmentState.None;
            }
                

            //이미 장착된 장비에 대한 처리. 여기가 문제인가
            VehicleEquipmentInfo equip = vehicleInfo.EquipmentPos.Find(equip => equip.ItemId == equipment.Equipment.EquipmentId);
            
            if(equip != null)
            {
                equipmentUi.IsState = EquipmentState.Equip;
                equipment.State = EquipmentState.Equip;

            }       

            
            slot.transform.SetParent(_content.transform);
            slot.transform.localScale = Vector3.one;
        }
    }
}
