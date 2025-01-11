using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDivider
{
    [Header("Rectangle Parameters")]
    private float _minWidth = 5f; // 최소 직사각형 너비
    private float _minHeight = 3f; // 최소 직사각형 높이

    private Vector2 _mapSize;

    private List<Rect> _rects = new List<Rect>();

    public List<Rect> Divide(Vector2 mapSize, float minWidth, float minHeight)
    {
        _rects.Clear();

        _mapSize = mapSize;

        _minWidth = minWidth;
        _minHeight = minHeight;

        DivideRectangle(0, 0, mapSize.x, mapSize.y);

        return _rects;
    }

    private void DivideRectangle(float x, float y, float w, float h)
    {
        // 최소 크기에 도달하면 더 이상 분할하지 않고 직사각형을 생성
        if (w < _minWidth * 2 && h < _minHeight * 2)
        {
            CreateRectangle(x, y, w, h);
            return;
        }

        // 가로 또는 세로 방향으로 무작위로 분할
        if (w > h)
        {
            float split = Random.Range(_minWidth, w - _minWidth);
            DivideRectangle(x, y, split, h); // 왼쪽 부분
            DivideRectangle(x + split, y, w - split, h); // 오른쪽 부분
        }
        else
        {
            float split = Random.Range(_minHeight, h - _minHeight);
            DivideRectangle(x, y, w, split); // 아래쪽 부분
            DivideRectangle(x, y + split, w, h - split); // 위쪽 부분
        }
    }

    private void CreateRectangle(float x, float y, float w, float h)
    {
        Rect rect = new Rect(x - _mapSize.x / 2, y - _mapSize.y / 2, w, h);  
        _rects.Add(rect);  
    }
}