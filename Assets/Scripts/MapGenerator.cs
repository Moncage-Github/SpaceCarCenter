using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapObjectType
{
    COLLECTABLE,
    METEOR,
    ENEMY,
    INVALID
}

[Serializable]
public class MapInfo
{
    [SerializeField] private Vector2 _mapSize;
    public Vector2 MapSize { get => _mapSize; private set => _mapSize = value; }

    public Dictionary<MapObjectType ,List<Vector2>> ObjectInfos = new();

    const float MIN_DISTANCE = 5.0f;

    public static MapInfo CreateMapInfo(Vector2 mapSize, int meteorCount, int objectCount)
    {
        MapInfo info = new()
        {
            MapSize = mapSize
        };

        info.ObjectInfos.Add(MapObjectType.METEOR, new List<Vector2>());
        for (int i = 0; i < meteorCount; i++)
        {
            info.GenerateMeteor();
        }

        return info;
    }

    private bool GenerateMeteor()
    {
        Vector2 randomPos = Vector2.zero;

        for (int i = 0; i < 100; i++)
        {
            randomPos.x = UnityEngine.Random.Range(-_mapSize.x / 2.0f, _mapSize.x / 2.0f);
            randomPos.y = UnityEngine.Random.Range(-_mapSize.y / 2.0f, _mapSize.y / 2.0f);

            if (IsPositionValid(randomPos, MIN_DISTANCE))
            {
                ObjectInfos[MapObjectType.METEOR].Add(randomPos);
                return true;
            }
        }

        return false;
    }

    private bool IsPositionValid(Vector2 newPosition, float minDistance)
    {
        foreach (Vector2 position in ObjectInfos[MapObjectType.METEOR])
        { 
            if (Vector2.Distance(newPosition, position) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2 _mapSize;
    [SerializeField] int _meteorCount;

    private MapInfo _mapInfo;
    [SerializeField] private GameObject _meteorPrefab;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        _mapInfo = MapInfo.CreateMapInfo(_mapSize, _meteorCount, 0);

        CreateBoundary();

        CreateMapObject();
    }

    public void CreateBoundary()
    {
        Vector2 mapSize = _mapInfo.MapSize;
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(-mapSize.x / 2, -mapSize.y / 2);    // 좌하단
        points[1] = new Vector2(mapSize.x / 2, -mapSize.y / 2);    // 우하단
        points[2] = new Vector2(mapSize.x / 2, mapSize.y / 2);      // 우상단
        points[3] = new Vector2(-mapSize.x / 2, mapSize.y / 2);      // 좌상단
        points[4] = points[0];                                      // 시작점으로 되돌아오기

        edgeCollider.points = points;
    }

    private void CreateMapObject()
    {
        foreach(MapObjectType type in _mapInfo.ObjectInfos.Keys)
        {
            GameObject prefab = null;
            switch (type)
            {
                case MapObjectType.COLLECTABLE:
                    break;
                case MapObjectType.METEOR:
                    prefab = _meteorPrefab;
                    break;
                case MapObjectType.ENEMY:
                    break;
                case MapObjectType.INVALID:
                    break;
            }

            foreach (Vector2 pos in _mapInfo.ObjectInfos[type])
            {
                MapObject obj = Instantiate(prefab, pos, Quaternion.identity).GetComponent<MapObject>();
                obj.Init();
            }
        }
    }
}

