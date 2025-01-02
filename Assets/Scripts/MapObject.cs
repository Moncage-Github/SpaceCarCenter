using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObject : MonoBehaviour
{
    public MapObjectType ObjectType { get; protected set; }

    public abstract void Init();
}
