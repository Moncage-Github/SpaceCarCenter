using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private VehicleUiManager _vehicleUiManager;
    private int _vehicleSize;

    private void Start()
    {
        //Vehicle의 count는 EquipmentScriptable에 VehicleInfos List 갯수로 정해진다.
        _vehicleSize = GameManager.Instance.EquipmentData.EquipmentScriptable.VehicleInfos.Count;
    }
    public void NextVehicleButton()
    {
        int nextVehicleId = (GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle + 1) % _vehicleSize;
        _vehicleUiManager.InitVehicleUi(nextVehicleId, 1);
        Debug.Log("Next");
    }

    public void PrevVehicleButton()
    {
        int temp = (GameManager.Instance.EquipmentData.EquipmentScriptable.CurrentSelectVehicle - 1) % _vehicleSize;
        int nextVehicleId = temp < 0 ? _vehicleSize - 1 : temp;

        _vehicleUiManager.InitVehicleUi(nextVehicleId, -1);
        Debug.Log("Prev");

    }

    public void StartGameButton()
    {
        GameManager.Instance.LoadCollectionScene();
    }
}
