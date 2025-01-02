using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapBoundary : MonoBehaviour
{
    [SerializeField] MapInfo _mapInfo;

    void Start()
    {
        CreateBoundary();
    }



    private void CreateBoundary()
    {
        Vector2 mapSize = _mapInfo.MapSize;
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        Vector2[] points = new Vector2[5];
        points[0] = new Vector2(-mapSize.x / 2, -mapSize.y / 2);  // 좌하단
        points[1] = new Vector2(-mapSize.x / 2, -mapSize.y / 2);   // 우하단
        points[2] = new Vector2(mapSize.x / 2, mapSize.y / 2);    // 우상단
        points[3] = new Vector2(mapSize.x / 2, mapSize.y / 2);   // 좌상단
        points[4] = points[0];                                      // 시작점으로 되돌아오기

        edgeCollider.points = points;
    }
}
