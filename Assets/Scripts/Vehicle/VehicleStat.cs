using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStat
{
    public VehicleData Data { get; private set; }

    public event Action<float> OnHpChange;
    public event Action OnHpZero;
    private float _currentHp;
    public float CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = Mathf.Clamp(value, 0, Data.MaxHp);
            OnHpChange?.Invoke(_currentHp / Data.MaxHp);

            if (_currentHp <= 0)
            {
                _currentHp = 0;
                OnHpZero?.Invoke();
            }
        }
    }

    // �ְ�ӵ����� 1�ʰ� �Ҹ��ϴ� ���� ��
    readonly public float FUEL_USE_AMOUNT = 1.0f;
    public event Action<float> OnFuelChange;
    private float _currentFuelAmount;
    public float CurrentFuelAmount
    {
        get => _currentFuelAmount;
        set
        {
            //Debug.Log("Current Feul Percentage : " +  value / MaxFuelAmount * 100 + "%");
            _currentFuelAmount = Mathf.Clamp(value, 0, Data.MaxFuelAmount);
            _currentFuelAmount = value;
            OnFuelChange?.Invoke(value / Data.MaxFuelAmount);
        }
    }

    public VehicleStat(VehicleData data)
    {
        Data = data;
        CurrentFuelAmount = Data.MaxFuelAmount;
        CurrentHp = Data.MaxHp;
    }

    public bool IsFuelEmpty()
    {
        return !(CurrentFuelAmount > 0);
    }
}
