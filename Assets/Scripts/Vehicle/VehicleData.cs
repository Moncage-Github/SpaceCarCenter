using System;
using System.Collections;
using System.Collections.Generic;
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

    readonly public float DRIFT_FACTOR = 0.95f;

    //private int _curentLevel = 1;
}
