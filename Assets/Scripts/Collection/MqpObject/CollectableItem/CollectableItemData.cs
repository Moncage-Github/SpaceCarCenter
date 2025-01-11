using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CollectableItemData : ICSVData
{
    public int ItemCode { get; private set; }
    public CollectableItemType Name { get; private set; }
    public int InventoryWeight { get; private set; }
    public float SpawnRate { get; private set; }


    public void SetDataFromCSV(Dictionary<string, string> csvData)
    {
        Type type = GetType();

        foreach (var key in csvData.Keys)
        {
            if (key == "Name")
            {
                if (Enum.TryParse(csvData[key], out CollectableItemType collectableType))
                {
                    Name = collectableType;
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
