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
        // ���� �ν��Ͻ��� ������ ���� ������ ��ü�� �ı�
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        // �ν��Ͻ� ����
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
                

            //�̹� ������ ��� ���� ó��. ���Ⱑ �����ΰ�
            VehicleEquipmentInfo equip = vehicleInfo.EquipmentPos.Find(equip => equip.ItemId == equipment.Equipment.EquipmentId);
            
            if(equip != null)
            {
                equipmentUi.IsState = EquipmentState.Equip;
                equipment.State = EquipmentState.Equip;

            }

            EquipIndexInit(equipmentUi);
            
            slot.transform.SetParent(_content.transform);
            slot.transform.localScale = Vector3.one;
            Debug.Log("equipment ����");
        }
    }

    private void EquipIndexInit(EquipmentUI slot)
    {

        List<GameObject> vehicleInfo = VehicleUiManager.Instance.VehicleInfo;
        //���� ������ �ڽ� ��ü ��������
        foreach (Transform child in vehicleInfo[GameManager.Instance.EquipmentData.CurrentVehicleId].transform)
        {

            //�ڽĵ� �� EquipIndex�� ��������
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
