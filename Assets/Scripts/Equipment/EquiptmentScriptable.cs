using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentScriptable", menuName = "Data/EquipmentScriptable")]
public class EquiptmentScriptable : ScriptableObject
{
    public List<Pair<Equipment, EquipmentState>> EquipmentData;
}
