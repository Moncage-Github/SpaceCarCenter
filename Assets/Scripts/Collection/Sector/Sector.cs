using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SectorOption
{
    public MapObjectType SpawnOption;

    [Header("Meteor")]
    public uint MaxMeteorCount;
    public MeteorType SpawnMeteorType = MeteorType.None;

    [Header("Collectable")]
    public uint MaxCollectableCount;
    public CollectableItemType SpawnCollectableItemType = CollectableItemType.None;

    [Header("Enemy")]
    public uint MaxEnemyCount;
    public MeteorType EnemyType = MeteorType.None;

    public bool IsSpawnOptionSelected(MapObjectType option)
    {
        return (SpawnOption & option) == option;
    }

    public bool IsMeteorOptionSelected(MeteorType option)
    {
        if (IsSpawnOptionSelected(MapObjectType.METEOR)) return false;

        return (SpawnMeteorType & option) == option;
    }
}

public class Sector : MonoBehaviour
{
    public static List<MapObjectType> AllTMapObjectType = new List<MapObjectType>{MapObjectType.COLLECTABLE, MapObjectType.ENEMY, MapObjectType.METEOR};

    [SerializeField] private SectorOption _sectorOption;

    public Dictionary<MapObjectType, List<Vector2>> MapObjectPositions = new();

    private int _meteorSpawnCount;

    public Vector2 Center { get => transform.position; }

    private void SetMapObject()
    {
        foreach(MapObjectType type in AllTMapObjectType)
        {
            uint maxCount = 0;
            if (!_sectorOption.IsSpawnOptionSelected(type)) continue;
            MapObjectPositions[type] = new List<Vector2>();

            if (type == MapObjectType.COLLECTABLE)
                maxCount = _sectorOption.MaxCollectableCount;
            else if (type == MapObjectType.METEOR)
                maxCount = _sectorOption.MaxMeteorCount;
            else if (type == MapObjectType.ENEMY)
                maxCount = _sectorOption.MaxEnemyCount;


            for (int i = 0; i < maxCount; i++)
            {
                if (!GenerateMapObject(type, 10))
                    break;
            }
        }
    }

    private bool GenerateMapObject(MapObjectType type, float minDistance)
    {
        Vector2 randomPos = Vector2.zero;

        float width = transform.localScale.x - 2f;
        float height = transform.localScale.y - 2f;

        for (int i = 0; i < 100; i++)
        {
            randomPos.x = UnityEngine.Random.Range(-width / 2.0f, width / 2.0f);
            randomPos.y = UnityEngine.Random.Range(-height / 2.0f, height / 2.0f);

            if (IsValidPosition(randomPos, minDistance))
            {
                MapObjectPositions[type].Add(randomPos);
                return true;
            }
        }

        return false;
    }

    private bool IsValidPosition(Vector2 pos, float minDistance)
    {
        foreach (var lists in MapObjectPositions.Values)
        {
            foreach (var position in lists)
            {
                if (Vector2.Distance(pos, position) <= minDistance)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void CreateMeteor()
    {
        if (_sectorOption.SpawnMeteorType == MeteorType.None) return;

        GameObject prefab = CollectionAssetManager.Instance.GetPrefabWithName("Meteor");

        MeteorType spawnOption = _sectorOption.SpawnMeteorType;

        List<MeteorData> meteorData = new();
        var activeFlags = GetActiveFlags(spawnOption);

        var meteorspawnRates = CollectionAssetManager.Instance.MeteorSpawnRate;

        float totalSpawnRate = 0;

        foreach (var flag  in activeFlags)
        {
            totalSpawnRate += meteorspawnRates[flag];
        }

        List<Vector2> positions = MapObjectPositions[MapObjectType.METEOR];
        
        foreach(var position in positions)
        {
            float randomValue = UnityEngine.Random.Range(0, totalSpawnRate);
            float currentRate  = 0;
            Meteor meteor = null;

            foreach (var data in meteorspawnRates)
            {
                currentRate += data.Value;
                if (randomValue < currentRate)
                {
                    meteor = Instantiate(prefab).GetComponent<Meteor>();
                    meteor.Init(data.Key);
                    break;
                }
            }

            meteor.transform.parent = transform;
            meteor.transform.position = position + Center;
        }
    }

    private void CreateCollectableItem()
    {
        if(!_sectorOption.SpawnOption.HasFlag(MapObjectType.COLLECTABLE)) { return; }

        CollectableItemType spawnOption = _sectorOption.SpawnCollectableItemType;

        if (spawnOption == CollectableItemType.None) return;

        GameObject prefab = CollectionAssetManager.Instance.GetPrefabWithName("CollectableItem");

        List<Vector2> positions = MapObjectPositions[MapObjectType.COLLECTABLE];

        foreach (var position in positions)
        {
            CollectableItem item = null;

            item = Instantiate(prefab).GetComponent<CollectableItem>();
            item.Init();

            item.transform.parent = transform;
            item.transform.position = position + Center;
        }
    }

    private void CreateEnemy()
    {
        //CollectableItemType spawnOption = _sectorOption.SpawnCollectableItemType;

        //if (spawnOption == CollectableItemType.None) return;

        if (!_sectorOption.SpawnOption.HasFlag(MapObjectType.ENEMY)) { return; }


        GameObject prefab = CollectionAssetManager.Instance.GetPrefabWithName("Enemy");


        //List<MeteorData> meteorData = new();
        //var activeFlags = GetActiveFlags(spawnOption);

        //var meteorspawnRates = CollectionAssetManager.Instance.MeteorSpawnRate;

        //float totalSpawnRate = 0;

        //foreach (var flag in activeFlags)
        //{
        //    totalSpawnRate += meteorspawnRates[flag];
        //}

        List<Vector2> positions = MapObjectPositions[MapObjectType.ENEMY];

        foreach (var position in positions)
        {
            //float randomValue = UnityEngine.Random.Range(0, totalSpawnRate);
            //float currentRate  = 0;
            EnemyBase enemy = null;

            //foreach (var data in meteorspawnRates)
            //{
            //    currentRate += data.Value;
            //    if (randomValue < currentRate)
            //    {
            enemy = Instantiate(prefab).GetComponent<EnemyBase>();

            enemy.transform.position = position + Center;

            enemy.Init();
            //        break;
            //    }
            //}


        }
    }

    public void DrawSector()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 0.2f);
    }

    public void CreateMapObjects()
    {
        SetMapObject();

        CreateMeteor();

        CreateCollectableItem();

        CreateEnemy();
    }

    public List<T> GetActiveFlags<T>(T flags) where T : System.Enum
    {
        List<T> activeFlags = new List<T>();

        // 모든 enum 값을 순회
        foreach (T flag in System.Enum.GetValues(typeof(T)))
        {
            if (System.Convert.ToInt64(flag) == 0) // 기본값(0)은 제외
                continue;

            if (flags.HasFlag(flag))
            {
                activeFlags.Add(flag);
            }
        }

        return activeFlags;
    }
}
