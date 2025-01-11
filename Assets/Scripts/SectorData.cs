using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SectorData", menuName = "Data/SectorData")]
public class SectorData : ScriptableObject
{
    public SerializableDictionary<string, SectorOption> SectorOptions;
}
