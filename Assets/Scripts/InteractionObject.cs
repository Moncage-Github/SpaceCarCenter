using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    public UnityEvent InteractionEvent;

    private InteractionOutline _outline;

    private bool _canInteraction;
    public bool CanInteraction
    {
        get => _canInteraction;
        set
        {
            _canInteraction = value;
            if (_outline != null)
            {
                _outline.Outline = value;
            }
        }
    }

    public void Interaction()
    {
        if (_canInteraction)
        {
            InteractionEvent.Invoke();
        }
    }

    public void Awake()
    {
        _outline = GetComponent<InteractionOutline>();
    }
}
