using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipment : MonoBehaviour
{
    [SerializeField] protected Vehicle Vehicle;

    

    public void SetVehivle(Vehicle vehicle)
    {
        Vehicle = vehicle;
    }
}
