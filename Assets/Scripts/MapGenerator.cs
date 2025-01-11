using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sector;

public class MapGenerator : MonoBehaviour
{
    public List<Sector> Sectors;

    [SerializeField] private Vector2 _mapSize;

    [SerializeField] private GameObject _vehicle;

    [Header("Prefabs")]
    [SerializeField] private GameObject _meteorPrefab;
    [SerializeField] private GameObject _collectablePrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _sectorPrefab;

    private void Start()
    {
        GenerateMap();
    }


    public void GenerateMap()
    {
        SetPlayerPosition();

        foreach (var sector in Sectors)
        {
            sector.SetMapObject();
        }

        CreateBoundary();

        CreateMapObject();
    }

    public void SetPlayerPosition()
    {
        Vector3 vehiclePos = new Vector3(0, -_mapSize.y / 2.0f + _vehicle.transform.localScale.y / 2.0f);
        Vector3 cameraPos = vehiclePos;
        cameraPos.z = -10.0f;

        _vehicle.transform.position = vehiclePos;

        Camera.main.GetComponent<CameraController>().Init(_mapSize);
        Camera.main.transform.position = cameraPos;
    }

    public void CreateBoundary()
    {
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(-_mapSize.x / 2, -_mapSize.y / 2);    // 좌하단
        points[1] = new Vector2(_mapSize.x / 2, -_mapSize.y / 2);    // 우하단
        points[2] = new Vector2(_mapSize.x / 2, _mapSize.y / 2);      // 우상단
        points[3] = new Vector2(-_mapSize.x / 2, _mapSize.y / 2);      // 좌상단
        points[4] = points[0];                                      // 시작점으로 되돌아오기

        edgeCollider.points = points;
    }

    private void CreateMapObject()
    {
        GameObject prefab = null;

        foreach(Sector sector in Sectors)
        {
            foreach (MapObjectData data in sector.MapObjectDatas)
            {
                switch (data.Type)
                {
                    case MapObjectType.COLLECTABLE:
                        prefab = _collectablePrefab;
                        break;
                    case MapObjectType.METEOR:
                        prefab = _meteorPrefab;
                        break;
                    case MapObjectType.ENEMY:
                        break;
                }

                MapObject mapObject = Instantiate(prefab).GetComponent<MapObject>();
                mapObject.transform.position = data.Position + sector.Center;

                mapObject.transform.parent = sector.transform;
                mapObject.Init();
            }
        }

    }
}