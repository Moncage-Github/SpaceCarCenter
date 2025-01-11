using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeteorSpawnRate : ICSVData
{
    public MeteorType Name { get; private set; }
    public float SpawnRate { get; private set; }

    public void SetDataFromCSV(Dictionary<string, string> csvData)
    {
        Type type = GetType();

        foreach (var key in csvData.Keys)
        {
            if (key == "Name")
            {
                if (Enum.TryParse(csvData[key], out MeteorType meteorType))
                {
                    Name = meteorType;
                }
                continue;
            }

            else
            {
                SpawnRate = float.Parse(csvData[key]);
            }
        }
    }
}
