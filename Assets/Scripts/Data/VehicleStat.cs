using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

[CreateAssetMenu(fileName = "VehicleStat", menuName = "Data/VehicleStat")]
public class VehicleStat : ScriptableObject
{
    [field: Header("Vehicle Stat")]
    [field: SerializeField] public float MaxHp { get; private set; }
    [field: SerializeField] public float MaxFuelAmount { get; private set; }
    
    [field: Space(1.0f)]

    [field: Header("Vehicle Performance")]
    [field: SerializeField] public float AcclerationForce { get; private set; }
    [field: SerializeField] public float MaxSpeed { get; private set; }
    [field: SerializeField] public float RotationForce { get; private set; }

    readonly public float DRIFT_FACTOR = 0.95f;

    private int _curentLevel = 1;

    public event Action<float> OnHpChange;
    public event Action OnHpZero;
    private float _currentHp;
    public float CurrentHp 
    {
        get => _currentHp;
        set 
        { 
            _currentHp = Mathf.Clamp(0, MaxHp, value);
            OnHpChange?.Invoke(value / MaxHp);

            if(_currentHp <= 0)
            {
                _currentHp = 0;
                OnHpZero?.Invoke();
            }
        } 
    }

    // 최고속도에서 1초간 소모하는 연료 량
    readonly public float FUEL_USE_AMOUNT = 1.0f;
    public event Action<float> OnFuelChange;
    private float _currentFuelAmount;
    public float CurrentFuelAmount 
    {
        get => _currentFuelAmount;
        set
        {
            //Debug.Log("Current Feul Percentage : " +  value / MaxFuelAmount * 100 + "%");
            _currentFuelAmount = Mathf.Clamp(0, MaxFuelAmount, value);
            _currentFuelAmount = value;
            OnFuelChange?.Invoke(value / MaxFuelAmount);
        }
    }

    public bool IsFuelEmpty()
    {
        return CurrentFuelAmount <= 0;
    }
}
