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
        // CSV 파일 Read후 리스트로 저장
        List<string> allLines = File.ReadAllLines(filePath).ToList();

        // 변수명 추출
        string[] readVariableNames = allLines[0].Split(',');
        allLines.RemoveAt(0);

        //생성할 타입에 추출한 변수가 모두 있는지 확인
        Type type = typeof(T);
        foreach(var readVariableName in readVariableNames)
        {
            //데이터 테이블에 없는 변수 있을시 리턴
            PropertyInfo propertyInfo = type.GetProperty(readVariableName);
            if (propertyInfo == null)
            {
                Debug.LogError($"There is no '{readVariableName}' property on '{type}'");
                return;
            }
        }

        savePath = Path.Combine(savePath, Path.GetFileNameWithoutExtension(filePath));

        //한줄씩 읽어 오브젝트 생성
        foreach (string line in allLines)
        {
            string[] splitData = line.Split(",");
            Dictionary<string, string> map = new Dictionary<string, string>();
            for(int i = 0; i < readVariableNames.Length; i++)
            {
                map[readVariableNames[i]] = splitData[i];
            }

            T newObj = ScriptableObject.CreateInstance<T>();
            foreach (var readVariableName in readVariableNames)
            {
                PropertyInfo propertyInfo = type.GetProperty(readVariableName);
                Type propertyType = propertyInfo.PropertyType;

                // 알맞은 타입으로 형 변환
                object convertedValue = Convert.ChangeType(map[readVariableName], propertyType);
                propertyInfo.SetValue(newObj, convertedValue);
            }

            Directory.CreateDirectory(savePath);

            //에셋 파일 생성
            string fileName = map["Name"] + ".asset";
            string path = Path.Combine(savePath, fileName);
            Debug.Log(path + "is Create");
            AssetDatabase.CreateAsset(newObj, path);
        }

        // 에셋 파일 저장
        AssetDatabase.SaveAssets();
    }
}
#endif