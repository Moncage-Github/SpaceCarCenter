using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MapObject
{
    [SerializeField] private int _itemCode;
    [SerializeField] private string _itemName;

    public int ItemCode { get => _itemCode; }

    public override void Init()
    {
        ObjectType = MapObjectType.COLLECTABLE;
    }
}
