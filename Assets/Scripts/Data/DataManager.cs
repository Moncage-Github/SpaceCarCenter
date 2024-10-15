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

        //Debug.Log("Start Load Vehicle Data");
        var vehicleDatas = Resources.LoadAll<VehicleData>("GameData/VehicleData");
        foreach (var vehicle in vehicleDatas)
        {
            _vehicleDatas[vehicle.Name] = vehicle;
        }
        //Debug.Log("Finish Load Vehicle Data: " + _vehicleDatas.Count);

    }

    public VehicleData GetVehicleData(string name)
    {
        if (_vehicleDatas == null)
        {
            LoadVehicleData();
        }

        if (!_vehicleDatas.TryGetValue(name, out VehicleData vehicleStat))
        {
            Debug.LogError($"Vehicle {name} not Found!");
            return null;
        }

        return vehicleStat;
    }

    public List<VehicleData> GetVehicleDataList()
    {
        if (_vehicleDatas == null)
        {
            LoadVehicleData ();
        }

        var list = new List<VehicleData>(_vehicleDatas.Values);
        return list;
    }
}
