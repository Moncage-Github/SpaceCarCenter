using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class CollectionAssetManager : AddressableManager
{
    public static CollectionAssetManager Instance
    { get; private set; }

    // Asset
    private Dictionary<string, Sprite> _sprites = new();
    private Dictionary<string, TextAsset> _tables = new();
    private Dictionary<string, GameObject> _prefabs = new();

    // Data
    private Dictionary<MeteorType, MeteorData> _meteorDatas = new();
    private Dictionary<MeteorType, float> _meteorSpawnRate = new();
    private Dictionary<CollectableItemType, CollectableItemData> _collectableDatas = new();

    public IReadOnlyDictionary<MeteorType, MeteorData> MeteorDatas { get => _meteorDatas; }
    public IReadOnlyDictionary<MeteorType, float> MeteorSpawnRate { get => _meteorSpawnRate; }
    public IReadOnlyDictionary<CollectableItemType, CollectableItemData> CollectableDatas { get => _collectableDatas; }


    public UnityEvent OnComplteLoad;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public override void CompleteLoadAsset(AsyncOperationHandle<IList<Object>> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        
        var result = handle.Result;
        foreach (var item in result)
        {
            if (item is Texture2D)
            {
                Texture2D texture = (Texture2D)item;

                // Texture2D를 기반으로 Sprite 생성
                Sprite newSprite = Sprite.Create
                (
                    texture,
                    new Rect(0, 0, texture.width, texture.height), // 텍스처의 전체 영역
                    new Vector2(0.5f, 0.5f) // Pivot: 중심점 설정
                );
                    
                _sprites[item.name] = newSprite;
            }

            else if (item is TextAsset)
            {
                _tables[item.name] = (TextAsset)item;
            }
            
            else if(item is GameObject)
            {
                _prefabs[item.name] = (GameObject)item;
            }
        }

        SetMeteorData();
        SetCollectableData();

        OnComplteLoad.Invoke();
    }

    public List<T> CSVtoData<T>(string name) where T : ICSVData, new()
    {

        if(!_tables.ContainsKey(name))
        {
            Debug.LogError($"{name} is not Key");
            return null;
        }

        var table = _tables[name];
        return CSVUtil.CSVToData<T>(table.text);
    }

    private void SetMeteorData()
    {
        var meteorData = CSVtoData<MeteorData>("MeteorData");

        foreach (var data in meteorData)
        {
            _meteorDatas[data.Name] = data;
        }

        var meteorSpawnRate = CSVtoData<MeteorSpawnRate>("MeteorSpawnRate");
        foreach (var data in meteorSpawnRate)
        {
            _meteorSpawnRate[data.Name] = data.SpawnRate;
        }
    }

    private void SetCollectableData()
    {
        var meteorData = CSVtoData<CollectableItemData>("CollectableData");

        foreach (var data in meteorData)
        {
            _collectableDatas[data.Name] = data;
        }
    }

    public Sprite GetSpriteWithName(string name)
    {
        if (!_sprites.ContainsKey(name)) return null;
        return _sprites[name];
    }

    public GameObject GetPrefabWithName(string name)
    {
        if(!_prefabs.ContainsKey(name)) return null;
        return _prefabs[name];
    }
}
