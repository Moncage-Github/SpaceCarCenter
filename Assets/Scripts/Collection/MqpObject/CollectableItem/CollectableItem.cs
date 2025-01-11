using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum CollectableItemType
{
    None = 0,
    Scrap = 1,
    Iron = 2,
    Copper = 4,
    Gold = 8,
    Vibranium = 16,
    BlackIron = 32,
    Diamond = 64,
    Glass = 128,
}

public class CollectableItem : MapObject
{
    [SerializeField] private int _itemCode;
    [SerializeField] private string _itemName;
    [SerializeField] private Rigidbody2D _rigidbody;

    public int ItemCode { get => _itemCode; }

    public void Init()
    {
        ObjectType = MapObjectType.COLLECTABLE;
    }

    public void MoveInRandomDirection()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float moveSpeed = Random.Range(0.3f, 1.0f);

        _rigidbody.velocity = randomDirection * moveSpeed;
    }
}
