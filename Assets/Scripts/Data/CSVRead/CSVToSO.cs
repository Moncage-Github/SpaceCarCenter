using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class CSVToSO
{
    public static void ReadCSV<T>(string filePath, string savePath) where T : ScriptableObject, ICSVData
    {
        // CSV ���� Read�� ����Ʈ�� ����
        List<string> allLines = File.ReadAllLines(filePath).ToList();

        // ������ ����
        string[] readVariableNames = allLines[0].Split(',');
        allLines.RemoveAt(0);

        //������ Ÿ�Կ� ������ ������ ��� �ִ��� Ȯ��
        Type type = typeof(T);
        foreach(var readVariableName in readVariableNames)
        {
            //������ ���̺� ���� ���� ������ ����
            PropertyInfo propertyInfo = type.GetProperty(readVariableName);
            if (propertyInfo == null)
            {
                Debug.LogError($"There is no '{readVariableName}' property on '{type}'");
                return;
            }
        }

        savePath = Path.Combine(savePath, Path.GetFileNameWithoutExtension(filePath));
        Directory.CreateDirectory(savePath);

        //���پ� �о� ������Ʈ ����
        foreach (string line in allLines)
        {
            string[] splitData = line.Split(",");
            Dictionary<string, string> map = new Dictionary<string, string>();
            for(int i = 0; i < readVariableNames.Length; i++)
            {
                map[readVariableNames[i]] = splitData[i];
            }

            T newObj = ScriptableObject.CreateInstance<T>();
            newObj.SetDataFromCSV(map);

            //���� ���� ����
            string fileName = map["Name"] + ".asset";
            string path = Path.Combine(savePath, fileName);
            Debug.Log(path + "is Create");
            AssetDatabase.CreateAsset(newObj, path);
        }

        // ���� ���� ����
        AssetDatabase.SaveAssets();
    }
}
#endif