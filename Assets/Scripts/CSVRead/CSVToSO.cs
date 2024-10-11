using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Analytics.ValueProperty;

#if UNITY_EDITOR
public class CSVToSO
{
    [MenuItem("Util/ReadData")]
    private static void ReadCSV()
    {
        string filePath = "Assets/DataTable/";
        string fileName = "Vehiclestat.csv";
        string path = Path.Combine(filePath, fileName);

        List<string> allLines = File.ReadAllLines(path).ToList();
        string[] readVariableNames = allLines[0].Split(',');
        allLines.RemoveAt(0);


        Type type = typeof(VehicleStat);
        foreach(var readVariableName in readVariableNames)
        {
            PropertyInfo propertyInfo = type.GetProperty(readVariableName);
            if (propertyInfo == null)
            {
                Debug.LogError($"There is no '{readVariableName}' property on '{type}'");
                return;
            }
        }

        foreach (string line in allLines)
        {
            string[] splitData = line.Split(",");
            Dictionary<string, string> map = new Dictionary<string, string>();
            for(int i = 0; i < readVariableNames.Length; i++)
            {
                map[readVariableNames[i]] = splitData[i];
            }

            VehicleStat newObj = ScriptableObject.CreateInstance<VehicleStat>();
            foreach (var readVariableName in readVariableNames)
            {
                PropertyInfo propertyInfo = type.GetProperty(readVariableName);
                Type propertyType = propertyInfo.PropertyType;

                object convertedValue = Convert.ChangeType(map[readVariableName], propertyType);
                propertyInfo.SetValue(newObj, convertedValue);
            }

            AssetDatabase.CreateAsset(newObj, $"Assets/Data/{map["Name"]}.asset");
        }
        AssetDatabase.SaveAssets();
    }
}
#endif