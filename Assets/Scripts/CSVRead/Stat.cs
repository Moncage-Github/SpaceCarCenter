using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/TestStat")]
public class Stat : ScriptableObject
{
    [SerializeField] private string _name;

    [Header("Vehicle Stat")]
    [SerializeField] private float _maxHp;
    [SerializeField] private float _maxFuelAmount;

    [Space(10.0f)]

    [Header("Vehicle Performance")]
    [SerializeField] private float _driftFactor;
    [SerializeField] private float _acclerationForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _rotationForce;

    // Property
    public string Name => _name;
    public float MaxHp => _maxHp;
    public float MaxFuelAmount => _maxFuelAmount;
    public float DriftFactor => _driftFactor;
    public float AcclerationForce => _acclerationForce;
    public float MaxSpeed => _maxSpeed;
    public float RotationForce => _rotationForce;
}
