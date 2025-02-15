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

    public void Start()
    {
        CollectionAssetManager.Instance.StartLoadAsset();

    }

    public void GenerateMap()
    {
        SetPlayerPosition();

        CreateBoundary();

        CreateMapObject();
    }

    public void SetPlayerPosition()
    {
        Vector3 vehiclePos = new Vector3(0, -_mapSize.y / 2.0f + _vehicle.transform.localScale.y / 2.0f + 5.0f);
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
        foreach(Sector sector in Sectors)
        {
            sector.CreateMapObjects();
        }

    }
}