using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

[CreateAssetMenu(fileName = "VehicleStat", menuName = "Data/VehicleStat")]
public class VehicleData : ScriptableObject, ICSVData
{
    [field: SerializeField] public string Name { get; private set; }

    [field: Header("Vehicle Stat")]
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float MaxFuelAmount { get; private set; }
    
    [field: Space(1.0f)]

    [field: Header("Vehicle Performance")]
    [field: SerializeField] public float AcclerationForce { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float RotationForce { get; private set; }
    [field: SerializeField] public float DriftFactor { get; private set; }

    public void SetDataFromCSV(Dictionary<string, string> csvData)
    {
        Type type = typeof(VehicleData);

        foreach (var key in csvData.Keys)
        {
            PropertyInfo propertyInfo = type.GetProperty(key);
            Type propertyType = propertyInfo.PropertyType;

            // 알맞은 타입으로 형 변환
            object convertedValue = Convert.ChangeType(csvData[key], propertyType);
            propertyInfo.SetValue(this, convertedValue);
        }
    }

    //private int _curentLevel = 1;
}
