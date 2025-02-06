using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenEquipment : BaseEquipment
{
    [SerializeField] private float _healthRegen;
    // Start is called before the first frame update
    void Start()
    {
        Vehicle.RegenValue = _healthRegen;
        Vehicle.GetRegen += Vehicle.HealthRegen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
