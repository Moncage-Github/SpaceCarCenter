using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Metadata;

public class ManagerEquipmentSlot : MonoBehaviour
{
    public static ManagerEquipmentSlot Instance;

    public VehicleUiManager VehicleUiManager;

    private void Awake()
    {
        // 기존 인스턴스가 있으면 새로 생성된 객체를 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        // 인스턴스 해제
        if (Instance == this)
        {
            Instance = null;
        }
    }


    [SerializeField] private GameObject _prefabEquipment;
    [SerializeField] private Transform _content;
    [SerializeField] private List<EquipmentUI> _slotPool;
    public List<EquipmentUI> SlotPool { get { return _slotPool; } } 

    // Start is called before the first frame update
    void Start()
    {

        Init();
    }

    public void Init()
    {
        _slotPool = new List<EquipmentUI>();

        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }

        VehicleInfo vehicleInfo = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(equip => equip.VehicleId == GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle);

        GameManager.Instance.EquipmentData.CurrentVehicle = vehicleInfo;

        foreach (var equipment in GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData)
        {
            
            GameObject slot = Instantiate(_prefabEquipment);
            EquipmentUI equipmentUi = slot.GetComponent<EquipmentUI>();

            _slotPool.Add(equipmentUi);

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

            EquipIndexInit(equipmentUi);
            
            slot.transform.SetParent(_content.transform);
            slot.transform.localScale = Vector3.one;
            Debug.Log("equipment 생성");
        }
    }

    private void EquipIndexInit(EquipmentUI slot)
    {

        List<GameObject> vehicleInfo = VehicleUiManager.Instance.VehicleInfo;
        //현재 차량의 자식 객체 가져오기
        foreach (Transform child in vehicleInfo[GameManager.Instance.EquipmentData.CurrentVehicleId].transform)
        {

            //자식들 중 EquipIndex들 가져오기
            EquipIndex childIndex = child.GetComponent<EquipIndex>();
            if (childIndex)
            {

                if(childIndex.CurrentEquipmentId == slot.EquipmentId)
                    childIndex.CurrentEquipment = slot;
            }
        }
    }

    public void SlotReset()
    {
        foreach (var slot in _slotPool)
        {
            if(slot.IsState != EquipmentState.Lock)
                slot.SetState(EquipmentState.None);
        }
        
    }
}
