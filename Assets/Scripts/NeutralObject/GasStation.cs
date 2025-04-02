using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasStation : NeutralObject
{
    [SerializeField] private float _fuelAmount;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnInteraction()
    {
        Debug.Log("중립 몹과 상호작용");
        //상호작용시 동작할 스크립트 _enemyState에 넣기
        _enemyState = new GasStationCharging(vehicle, _fuelAmount);
        IsInteraction = false;

        base.OnInteraction();

        return;
    }
}


public class GasStationCharging : IEnemyState
{
    private Vehicle _vehicle;
    private float _fuelAmount;

    public GasStationCharging(Vehicle Vehicle, float FuelAmount)
    {
        _vehicle = Vehicle;
        _fuelAmount = FuelAmount;
    }

    public void Update(EnemyBase enemy)
    {
        _vehicle.GetFuel(_fuelAmount);
        Debug.Log("연료 충전");
        //TODO:: 뭔가 이상
    }
}
