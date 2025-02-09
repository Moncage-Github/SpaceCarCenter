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

        GameManager.Instance.EquipmentData.CurrentVehicle = vehicleInfo;

        foreach (var equipment in GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData)
        {
            
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
                //TODO:: equipmentMaskCenter�� currentEquipmentId�� 0���� ����.
                //���� �Ѱ��� �̻��� ��(0)�� ��� ��
                if(childIndex.CurrentEquipmentId == slot.EquipmentId)
                    childIndex.CurrentEquipment = slot;
            }
        }
    }
}
