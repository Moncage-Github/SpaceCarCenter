using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, VehicleData> _vehicleDatas = null;

    private DataManager()
    {

    }
    
    private void LoadVehicleData()
    {
        _vehicleDatas = new Dictionary<string, VehicleData>();

        var vehicleStats = Resources.LoadAll<VehicleData>("GameData/VehicleData");
        foreach (var vehicle in vehicleStats)
        {
            _vehicleDatas[vehicle.Name] = vehicle;
        }
    }

    public VehicleData GetVehicleStat(string name)
    {
        if (_vehicleDatas == null)
        {
            LoadVehicleData();
        }

        if (!_vehicleDatas.TryGetValue(name, out VehicleData vehicleStat))
        {
            Debug.LogWarning($"Vehicle {name} not Found!");
            return null;
        }

        return vehicleStat;
    }

}
