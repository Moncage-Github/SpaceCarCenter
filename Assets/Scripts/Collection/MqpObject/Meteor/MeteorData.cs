using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

[System.Serializable]
public class MeteorData : ICSVData
{
    public MeteorType Name { get; private set; }
    public float Gravity { get; private set; }
    public float Hp { get; private set; }
    public int ItemDropMax { get; private set; }
    public int ItemDropMin { get; private set; }

    public void SetDataFromCSV(Dictionary<string, string> csvData)
    {
        Type type = GetType();

        foreach (var key in csvData.Keys)
        {
            if(key == "Name")
            {
                if (Enum.TryParse(csvData[key], out MeteorType meteorType))
                {
                    Name = meteorType;
                }
                continue;
            }

            PropertyInfo propertyInfo = type.GetProperty(key);
            Type propertyType = propertyInfo.PropertyType;

            // 알맞은 타입으로 형 변환
            object convertedValue = Convert.ChangeType(csvData[key], propertyType);
            propertyInfo.SetValue(this, convertedValue);
        }
    }
}