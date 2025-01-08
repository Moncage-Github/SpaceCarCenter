using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum MapObjectType
{
    None = 0,
    COLLECTABLE = 1 << 0,
    METEOR = 1 << 1,
    ENEMY = 1 << 2,
}

public abstract class MapObject : MonoBehaviour
{
    public MapObjectType ObjectType { get; protected set; }

    public abstract void Init();
}
