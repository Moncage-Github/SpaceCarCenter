using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class TuningParts : MonoBehaviour
{
    public enum State { Dropped, PickUped, Composed, ScrewComposed };
    public bool NeedsScrewTightening;

    private readonly int _maxScrewTightenCount = 4;
    private int _curScrewTightenCount = 0;

    public ETuningPartsType Type;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    private TuningSlot _slot;

    [SerializeField] private GameObject _screw;

    //States
    public State CurState;

    // Start is called before the first frame update
    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        if(NeedsScrewTightening)
        {
            CurState = State.ScrewComposed;
            _curScrewTightenCount = _maxScrewTightenCount;
        }
        else 
        {
            CurState = State.Composed;
        }
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

    public void TryTightenScrew()
    {
        CurState = State.ScrewComposed;

        _curScrewTightenCount++;
        if(_curScrewTightenCount >= _maxScrewTightenCount)
        {
            _curScrewTightenCount = _maxScrewTightenCount;
            Debug.Log("Screw Tighten Finish");
        }
    }

    public void UnSrcew()
    {
        _curScrewTightenCount--;
        if(_curScrewTightenCount <= 0)
        {
            CurState = State.Composed;
            GameObject obj = Instantiate(_screw);
            obj.transform.position = transform.position;
            obj.transform.parent = transform.parent;
        }
    }


    private void SetSlot(TuningSlot slot)
    {
        _slot = slot;
    }
    public TuningSlot GetSlot() { return _slot; } 
}
