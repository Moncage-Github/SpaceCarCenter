                using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class VehicleUiManager : MonoBehaviour
{
    //TODO:: 왠지 VegicleInfos를 바로 넣어도 될거 같음
    [SerializeField] private List<GameObject> _vehicleInfo = new List<GameObject>();
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
            if (child.GetComponent<EquipIndex>())
            {
                VehicleEquipmentInfo info = vehicleInfo.EquipmentPos.Find(info => info.EquipmentPositionType == child.GetComponent<EquipIndex>().EquipNumber);

                if (info != null)
                {
                    child.transform.localPosition = new Vector3(info.EquipmentPosition.x * 100, info.EquipmentPosition.y * 150, 0);

                    //TODO:: 마스크를 제거해야될거 같음.

                    var equip = GameManager.Instance.EquipmentData.EquipmentScriptable.EquipmentData.Find(equip => equip.Equipment.EquipmentId == info.ItemId);


                    //TODO:: 크기 바뀌는 것도 포함해 함수화, 마스크의 alpha 값 1로 설정
                    if(equip != null)
                    {
                        Transform rawImage = child.transform.GetChild(0);
                        rawImage.GetComponent<RawImage>().texture = equip.Equipment.ImageLog.texture;

                        child.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);

                        Image image = child.transform.GetComponent<Image>();
                        Color color = image.color;
                        color.a = 0x000001 / 255.0f;
                        image.color = color;

                        rawImage.GetComponent<RectTransform>().sizeDelta = new Vector3(equip.Equipment.ImageLog.rect.width * 0.07f, equip.Equipment.ImageLog.rect.height * 0.07f);
                    }
                    
                }
            }
        }

    }
}
