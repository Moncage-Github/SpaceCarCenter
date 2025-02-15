using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class VehicleUiManager : MonoBehaviour
{
    public static VehicleUiManager Instance { get; private set; }

    //TODO:: ���� VegicleInfos�� �ٷ� �־ �ɰ� ����
    [SerializeField] private List<GameObject> _vehicleInfo = new List<GameObject>();
    [SerializeField] private Transform _imageTransform;
    public List<GameObject> VehicleInfo { get { return _vehicleInfo; } }

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
            EquipIndex childIndex = child.GetComponent<EquipIndex>();
            if (childIndex)
            {
                VehicleEquipmentInfo info = vehicleInfo.EquipmentPos.Find(info => info.EquipmentPositionType == childIndex.EquipNumber);


                //TODO:: ��� �������� ���� ���Կ� ���� ó��
                if (childIndex.CurrentEquipmentId == 0)
                {
                    
                    child.transform.localPosition = new Vector3(info.EquipmentPosition.x * 100, info.EquipmentPosition.y * 175, 0);

                    Transform image = child.transform.GetChild(0);
                    image.GetComponent<Image>().enabled = false;

                    Image temp = child.transform.GetComponent<Image>();
                    Color color = temp.color;
                    color.a = 255.0f;
                    temp.color = color;

                    child.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(50.0f, 50.0f);
                    //return;
                }


                if (info != null)
                {
                    child.transform.localPosition = new Vector3(info.EquipmentPosition.x * 100, info.EquipmentPosition.y * 175, 0);


                    var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == info.ItemId);


                    if (equip != null)
                    {
                        Transform image = child.transform.GetChild(0);
                        image.GetComponent<Image>().enabled = true;
                        image.GetComponent<Image>().sprite = equip.Equipment.ImageLog;
                        image.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                        child.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);

                        Image temp = child.transform.GetComponent<Image>();
                        Color color = temp.color;
                        color.a = 0x000001 / 255.0f;
                        temp.color = color;

                        image.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);

                        //image.enabled = false;

                        SettingItemImage(equip.Equipment.EquipmentId, image);
                    }
                    
                }
            }
        }

    }


    private void SettingItemImage(int itemId, Transform image)
    {
        RectTransform imageRect = image.GetComponent<RectTransform>();
        

        switch (itemId)
        {
            case 10:
                imageRect.sizeDelta = new Vector2(image.GetComponent<RectTransform>().sizeDelta.x * 2f, image.GetComponent<RectTransform>().sizeDelta.y * 2f);

                imageRect.position = _imageTransform.GetComponent<RectTransform>().position;
                break;
            case 21:
                RectTransform standardImageRec = _imageTransform.GetComponent<RectTransform>();
                imageRect.position = new Vector3(standardImageRec.position.x, standardImageRec.position.y - standardImageRec.sizeDelta.y * 0.8f, standardImageRec.position.y);
                break;
            default:
                break;
        }
    }


    /*
     ������ ��� � ������� �����ϱ� ���� �ڵ�

    ----------------------------------------------
    None : 0
    �ν�Ʈ : 1, ���� : 2
    �� : 10, ü�� : 11
    �⺻ : 20, �׹� : 21, �帱 : 22, Ȯ�� :23
    �� : 30, �ۻ� : 31, ���� : 32, ������ : 33
    ----------------------------------------------

    ���� ������ ��ȣ
    ----------------------------------------------
    Enum���� ����
    ----------------------------------------------
     */

    public void OnReset()
    {
        foreach (Transform child in _vehicleInfo[GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle].transform)
        {
            EquipIndex childIndex = child.GetComponent<EquipIndex>();
            if (childIndex)
            {
                GameManager.Instance.EquipmentData.SetEquip(childIndex.EquipNumber, childIndex.CurrentEquipmentId, EquipmentState.None, GameManager.Instance.EquipmentData.CurrentVehicleId);

                ManagerEquipmentSlot.Instance.SlotReset();

                childIndex.CurrentEquipmentId = 0;
                childIndex.CurrentEquipment = null;
            }
        }

        EquipIndexInit();
    }
}
