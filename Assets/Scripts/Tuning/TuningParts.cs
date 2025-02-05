using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class TuningParts : InteractionObject
{
    public enum State { Dropped, PickUped, Composed };

    public ETuningPartsType Type;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    private TuningSlot _slot;

    //States
    public State CurState;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Drop()
    {
        CurState = State.Dropped;
        _renderer.sortingOrder = -1;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.transform.parent = null;
    }

    public void Pickup()
    {
        CurState = State.PickUped;
        _renderer.sortingOrder = 1;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody.transform.parent = null;
    }

    public void DeCompositeFromSlot()
    {
        GetSlot().DecompositionParts();
        SetSlot(null);
        Pickup();
    }

    public void CompositeToSlot(TuningSlot slot)
    {
        CurState = State.Composed;
        SetSlot(slot);
    }

    private void SetSlot(TuningSlot slot)
    {
        _slot = slot;
    }
    public TuningSlot GetSlot() { return _slot; } 
}
