                using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class VehicleUiManager : MonoBehaviour
{
    public static VehicleUiManager Instance { get; private set; }

    //TODO:: ���� VegicleInfos�� �ٷ� �־ �ɰ� ����
    [SerializeField] private List<GameObject> _vehicleInfo = new List<GameObject>();
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

                if (info != null)
                {
                    child.transform.localPosition = new Vector3(info.EquipmentPosition.x * 100, info.EquipmentPosition.y * 175, 0);


                    var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == info.ItemId);


                    if (equip != null)
                    {
                        Transform rawImage = child.transform.GetChild(0);
                        rawImage.GetComponent<Image>().sprite = equip.Equipment.ImageLog;

                        child.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);

                        Image image = child.transform.GetComponent<Image>();
                        Color color = image.color;
                        color.a = 0x000001 / 255.0f;
                        image.color = color;

                        rawImage.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);

                        image.enabled = false;

                        //TODO:: ������ ���� �ʿ��� �ɼ� ����
                    }
                    
                }
            }
        }

    }


}
