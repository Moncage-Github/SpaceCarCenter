using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public enum ETuningPartsType
{
    Roof,
    Bumper,
    Trunk,
    Frontwindow,
    Door,
    Engine,
    Wheel,
    Custom,
    Exhaust,
}

public class TuningSlot : InteractionObject
{
    [SerializeField] private ETuningPartsType _type;

    [SerializeField] private TuningParts _parts;

    private Collider2D _collider;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _parts = transform.parent.gameObject.GetComponentInChildren<TuningParts>();
        if (_parts == null) return;

        TryCompositionParts(_parts);
        _parts.Init();
    }

    public bool IsEmpty() => _parts == null;

    public bool TryCompositionParts(TuningParts parts)
    {
        if(parts.Type != _type)
        {
            Debug.Log("Mismatched Part Types");
            return false;
        }
        _collider.enabled = false;
        parts.transform.SetParent(transform.parent, true);
        parts.transform.localPosition = Vector3.zero; 
        parts.CompositeToSlot(this);
        return true;
    }

    public void DecompositionParts()
    {
        _parts = null;
        _collider.enabled = true; 
    }
}
