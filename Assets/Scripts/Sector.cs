using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SectorOption
{
    public MapObjectType SpawnOption;

    [Header("Meteor")]
    public uint MaxMeteorCount;
    public MeteorType MeteorType = MeteorType.None;

    [Header("Collectable")]
    public uint MaxCollectableCount;
    public MeteorType CollectableType = MeteorType.None;

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

        return (MeteorType & option) == option;
    }
}

public class Sector : MonoBehaviour
{
    public struct MapObjectData
    {
        public MapObjectData(MapObjectType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public MapObjectType Type;
        public Vector2 Position;
    }

    public static List<MapObjectType> AllTMapObjectType = new List<MapObjectType>{MapObjectType.COLLECTABLE, MapObjectType.ENEMY, MapObjectType.METEOR};

    public SectorOption SectorOption;

    public List<MapObjectData> MapObjectDatas = new();

    private int _meteorSpawnCount;

    public Vector2 Center { get => transform.position; }

    public void SetMapObject()
    {
        foreach(MapObjectType type in AllTMapObjectType)
        {
            uint maxCount = 0;
            if (!SectorOption.IsSpawnOptionSelected(type)) continue;

            if (type == MapObjectType.COLLECTABLE)
                maxCount = SectorOption.MaxCollectableCount;
            else if (type == MapObjectType.METEOR)
                maxCount = SectorOption.MaxMeteorCount;
            else if (type == MapObjectType.ENEMY)
                maxCount = 0;


            for (int i = 0; i < maxCount; i++)
            {
                if (!GenerateMapObject(type, 5))
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
                MapObjectDatas.Add(new MapObjectData(type, randomPos));
                return true;
            }
        }

        return false;
    }

    private bool IsValidPosition(Vector2 pos, float minDistance)
    {
        foreach (var data in MapObjectDatas)
        {
            if(Vector2.Distance(pos, data.Position) <= minDistance)
            {
                return false;
            }
        }

        return true;
    }

    public void DrawSector()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 0.2f);
    }
}
