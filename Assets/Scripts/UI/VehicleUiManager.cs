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

    //TODO:: 왠지 VegicleInfos를 바로 넣어도 될거 같음
    [SerializeField] private List<GameObject> _vehicleInfo = new List<GameObject>();
    [SerializeField] private Transform _imageTransform;
    public List<GameObject> VehicleInfo { get { return _vehicleInfo; } }

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

    // Start is called before the first frame update
    void Start()
    {
        EquipIndexInit();
    }


    //차량을 바꿀 때 바뀐 차량으로 설정해주는 함수
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

        //현재 차량에 맞는 UI 창으로 재 생성
        ManagerEquipmentSlot.Instance.Init();
    }

    //현재 차량 ID에 EquipmentPos에 맞게 EquipIndex를 배치해주는 함수
    public void EquipIndexInit()
    {
        int vehicleId = GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle;

        VehicleInfo vehicleInfo = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Find(equip => equip.VehicleId == vehicleId);


        //EquipIndex를 VehicleInfos에 맞춰서 배치해준다. 배율은 X : 100, Y : 200
        foreach (Transform child in _vehicleInfo[vehicleId].transform)
        {
            EquipIndex childIndex = child.GetComponent<EquipIndex>();
            if (childIndex)
            {
                VehicleEquipmentInfo info = vehicleInfo.EquipmentPos.Find(info => info.EquipmentPositionType == childIndex.EquipNumber);


                //TODO:: 장비가 장착되지 않은 슬롯에 대한 처리
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
     장착된 장비가 어떤 장비인지 구분하기 위한 코드

    ----------------------------------------------
    None : 0
    부스트 : 1, 조향 : 2
    방어막 : 10, 체젠 : 11
    기본 : 20, 그물 : 21, 드릴 : 22, 확장 :23
    총 : 30, 작살 : 31, 대포 : 32, 레이저 : 33
    ----------------------------------------------

    장착 슬롯의 번호
    ----------------------------------------------
    Enum으로 구분
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
