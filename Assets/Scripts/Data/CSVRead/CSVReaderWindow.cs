using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR
public class CSVReaderWindow : EditorWindow
{
    private string _filePath = "Assets/DataTable/";
    private string _savePath = "Assets/Resources/GameData";

    private int _selectedClassIndex = 0;
    private string[] _classNames;
    private Type[] _dataTypes;

    [MenuItem("Util/Read Data Table")]
    private static void Init()
    {
        CSVReaderWindow window = (CSVReaderWindow)GetWindow(typeof(CSVReaderWindow));
        window.Show();

        window.minSize = new Vector2(400f, 200f);
    }

    private void OnEnable()
    {
        // ICSVData를 상속받은 모든 클래스 타입 찾기
        FindAllDataTypes();
    }

    private void FindAllDataTypes()
    {
        // 어셈블리에서 ICSVData를 상속받은 모든 타입을 검색
        _dataTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ICSVData).IsAssignableFrom(type) && typeof(ScriptableObject).IsAssignableFrom(type) && !type.IsAbstract)
            .ToArray();

        // 타입 이름 배열 생성
        _classNames = _dataTypes.Select(t => t.Name).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("Path");

        GUILayout.BeginHorizontal();
        GUILayout.Label("CSV File Path: ");
        _filePath = GUILayout.TextField(_filePath, GUILayout.Width(400));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Folder Path: ");
        _savePath = GUILayout.TextField(_savePath, GUILayout.Width(400));
        GUILayout.EndHorizontal();

        _selectedClassIndex = EditorGUILayout.Popup("Select Data Type", _selectedClassIndex, _classNames);

        GUILayout.Space(10.0f);
        if (GUILayout.Button("Read CSV"))
        {
            Type type = typeof(CSVToSO);
            MethodInfo genericMethod = type.GetMethod("ReadCSV", BindingFlags.Static | BindingFlags.Public);
            MethodInfo concreteMethod = genericMethod.MakeGenericMethod(_dataTypes[_selectedClassIndex]);

            object[] parameters = new object[] { _filePath, _savePath };
            concreteMethod.Invoke(null, parameters);
        }
        
    }
}
#endif