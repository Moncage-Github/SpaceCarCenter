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
        points[0] = new Vector2(-mapSize.x / 2, -mapSize.y / 2);  // ���ϴ�
        points[1] = new Vector2(-mapSize.x / 2, -mapSize.y / 2);   // ���ϴ�
        points[2] = new Vector2(mapSize.x / 2, mapSize.y / 2);    // ����
        points[3] = new Vector2(mapSize.x / 2, mapSize.y / 2);   // �»��
        points[4] = points[0];                                      // ���������� �ǵ��ƿ���

        edgeCollider.points = points;
    }
}
