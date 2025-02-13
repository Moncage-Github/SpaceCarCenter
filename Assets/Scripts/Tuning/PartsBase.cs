using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartsBase : MonoBehaviour
{
    protected Rigidbody2D Rigidbody;
    protected SpriteRenderer Renderer;

    public void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Drop()
    {
        Renderer.sortingOrder = 0;
        Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        Rigidbody.transform.parent = null;
    }
    public virtual void Pickup()
    {
        Renderer.sortingOrder = 2;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Rigidbody.transform.parent = null;
    }

}
