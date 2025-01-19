using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Equipment", menuName = "Data/Equipment")]
public class Equipment : ScriptableObject
{
    [field : SerializeField] public string Name;
    [field : SerializeField] public string Explan;
    [field : SerializeField] public int EquipmentId;
    [field : SerializeField] public Sprite ImageLog;
    [field : SerializeField] public GameObject Prefab;
    [field : SerializeField] public EquipIndexNumber EquipIndexNumber;
}
