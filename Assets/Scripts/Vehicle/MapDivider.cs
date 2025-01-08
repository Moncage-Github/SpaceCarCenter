using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDivider
{
    [Header("Rectangle Parameters")]
    private float _minWidth = 5f; // �ּ� ���簢�� �ʺ�
    private float _minHeight = 3f; // �ּ� ���簢�� ����

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
        // �ּ� ũ�⿡ �����ϸ� �� �̻� �������� �ʰ� ���簢���� ����
        if (w < _minWidth * 2 && h < _minHeight * 2)
        {
            CreateRectangle(x, y, w, h);
            return;
        }

        // ���� �Ǵ� ���� �������� �������� ����
        if (w > h)
        {
            float split = Random.Range(_minWidth, w - _minWidth);
            DivideRectangle(x, y, split, h); // ���� �κ�
            DivideRectangle(x + split, y, w - split, h); // ������ �κ�
        }
        else
        {
            float split = Random.Range(_minHeight, h - _minHeight);
            DivideRectangle(x, y, w, split); // �Ʒ��� �κ�
            DivideRectangle(x, y + split, w, h - split); // ���� �κ�
        }
    }

    private void CreateRectangle(float x, float y, float w, float h)
    {
        Rect rect = new Rect(x - _mapSize.x / 2, y - _mapSize.y / 2, w, h);  
        _rects.Add(rect);  
    }
}