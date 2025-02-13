using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyPlayer : PlayerBase
{
    private LobbyPlayerInput _input;
    private float _moveValue;

    private Rigidbody2D _rigidbody;

    private bool _isGround;
    private bool _isDownPressed;
    private bool _isDownJump;

    private List<InteractionObject> _detectedObjects = new List<InteractionObject>();
    private InteractionObject _interactionObject;

    public void Awake()
    {
        _input = new LobbyPlayerInput();
    }

    void OnEnable()
    {
        _input.PlayerAction.Enable();
        _input.PlayerAction.Move.performed += OnMove;
        _input.PlayerAction.Move.canceled += OnMove;

        _input.PlayerAction.Jump.performed += OnJump;

        _input.PlayerAction.Down.performed += OnDown;
        _input.PlayerAction.Down.canceled += OnDown;

        _input.PlayerAction.Interaction.performed += OnInteraction;
    }

    void OnDisable()
    {
        _input.PlayerAction.Disable();
        _input.PlayerAction.Move.performed -= OnMove;

        _input.PlayerAction.Jump.performed -= OnJump;

        _input.PlayerAction.Down.performed -= OnDown;
        _input.PlayerAction.Down.canceled -= OnDown;

        _input.PlayerAction.Interaction.performed -= OnInteraction;
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;
        if (_interactionObject == null) return;

        _interactionObject.Interaction();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckInteraction();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        InteractionObject interactionObject = other.GetComponent<InteractionObject>();
        if (interactionObject == null) return;
        interactionObject.CanInteraction = true;
        //Debug.Log(other.name);
        // 트리거된 물체 추가
        if (!_detectedObjects.Contains(interactionObject))
        {
            _detectedObjects.Add(interactionObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        InteractionObject interactionObject = other.GetComponent<InteractionObject>();
        if (interactionObject == null) return;

        interactionObject.CanInteraction = false;

        // 트리거된 물체 제거
        if (_detectedObjects.Contains(interactionObject))
        {
            _detectedObjects.Remove(interactionObject);
        }
    }

    private void CheckInteraction()
    {
        if (_detectedObjects.Count == 0)
        {
            _interactionObject = null;
            return;
        }

        var closestObject = GetClosestObject();

        if (_interactionObject == null)
        {
            _interactionObject = closestObject;
        }

        else if (_interactionObject != closestObject)
        {
            _interactionObject.CanInteraction = false;
            _interactionObject = closestObject;
        }

        _interactionObject.CanInteraction = true;
    }

    public InteractionObject GetClosestObject()
    {
        InteractionObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        List<InteractionObject> reserveDelete = new();

        foreach (var obj in _detectedObjects)
        {
            if (obj == null)
            {
                reserveDelete.Add(obj);
                continue;
            }
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        foreach (var obj in reserveDelete)
        {
            _detectedObjects.Remove(obj);
        }
        reserveDelete.Clear();
        return closestObject;
    }
}
