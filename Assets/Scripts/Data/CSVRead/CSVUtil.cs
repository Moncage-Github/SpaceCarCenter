using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
public class CSVUtil
{
    public static List<T> CSVToData<T>(string asset) where T :ICSVData, new()
    {
        List<T> list = new List<T>();

        string[] allLines = asset.Split("\r\n");

        string[] readVariableNames = allLines[0].Split(',');

        for (int i = 1; i < allLines.Length - 1; i++)
        {
            T newObj = new T();

            string[] splitData = allLines[i].Split(",");
            Dictionary<string, string> map = new Dictionary<string, string>();
            for (int j = 0; j < readVariableNames.Length; j++)
            {
                map[readVariableNames[j]] = splitData[j];
            }

            try
            {
                newObj.SetDataFromCSV(map);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }

            list.Add(newObj);
        }

        return list;
    }
}