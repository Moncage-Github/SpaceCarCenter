using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CSVImporter : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        HashSet<string> importedPath = new HashSet<string>();
        HashSet<string> deletePath = new HashSet<string>();

        foreach (string assetPath in importedAssets)
            importedPath.Add(assetPath);
        foreach (string assetPath in movedAssets)
            importedPath.Add(assetPath);

        foreach (string path in importedPath)
        {
            if (Path.GetExtension(path) != ".csv") return;

            //ProcessCSVFile(path);
        }

        foreach (string asset in deletedAssets)
        {
            if (Path.GetExtension(asset) == ".csv")
            {
                //Debug.Log("A");
            }
        }
    }

    private static Type FindTypeByName(string className)
    {
        // 현재 로드된 모든 어셈블리 탐색
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(className);
            if (type != null)
                return type;
        }

        return null; // 타입을 찾지 못했을 경우
    }

    private static void ProcessCSVFile(string csvPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(csvPath);
        string savePath = Path.Combine("Assets\\Scripts\\Data");

        Type targetType = FindTypeByName("VehicleData");
        if(targetType == null)
        {
            Debug.LogError($"Class Not Found!");
            return;
        }

        // ReadCSV<T> 메서드 찾기
        MethodInfo method = typeof(CSVToSO).GetMethod("ReadCSV", BindingFlags.Public | BindingFlags.Static);
        if (method == null)
        {
            Debug.LogError("ReadCSV method not found.");
            return;
        }

        // 제네릭 타입 설정
        MethodInfo genericMethod = method.MakeGenericMethod(targetType);

        // ReadCSV<T> 실행
        try
        {
            genericMethod.Invoke(null, new object[] { csvPath, savePath });
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error executing ReadCSV<{fileName}>: {ex.Message}");
        }
    }
}