using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private LobbyPlayerInput _input;
    private float _moveValue;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    private bool _isGround;
    private bool _isDownPressed;
    private bool _isDownJump;
    [SerializeField] private bool _canInteractionOnlyClosestObject;

    [SerializeField] protected float Speed;
    [SerializeField] protected float JumpForce;

    private List<InteractionObject> _detectedObjects = new List<InteractionObject>();
    private InteractionObject _interactionObject;
    virtual public void Awake()
    {
        _input = new LobbyPlayerInput();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
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

        _input.PlayerAction.LeftClick.performed += LeftClick;
        _input.PlayerAction.RightClick.performed += RightClick;
    }


    void OnDisable()
    {
        _input.PlayerAction.Disable();
        _input.PlayerAction.Move.performed -= OnMove;

        _input.PlayerAction.Jump.performed -= OnJump;

        _input.PlayerAction.Down.performed -= OnDown;
        _input.PlayerAction.Down.canceled -= OnDown;

        _input.PlayerAction.Interaction.performed -= OnInteraction;

        _input.PlayerAction.LeftClick.performed -= LeftClick;
        _input.PlayerAction.RightClick.performed -= RightClick;
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;
        if (_interactionObject == null) return;

        _interactionObject.Interaction();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        // �Է°��� �о moveValue�� ����
        _moveValue = context.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void OnDown(InputAction.CallbackContext context)
    {
        _isDownPressed = context.performed;
    }

    private void Jump()
    {
        if (!_isGround || _isDownJump) return;

        if (_isDownPressed)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(_rigidbody.position, new Vector2(transform.lossyScale.x, 0.2f),0 , Vector3.down, transform.localScale.y / 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null) return; 
            if(rayHit.collider.GetComponent<PlatformEffector2D>() != null)
            {
                StartCoroutine(DownJump(rayHit.collider));
            }
        }
        else
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
        }
    }
    private void Move(float value)
    {
        Vector3 position = transform.localPosition;
        position.x += value * Speed * Time.deltaTime;

        transform.localPosition = position;
    }

    virtual public void LeftClick(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        int layerMask = LayerMask.GetMask("Platform");

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        if(hit.collider == null) return;
        
        InteractionObject interaction = hit.collider.gameObject.GetComponent<InteractionObject>();
        if(interaction == null || interaction.CanInteraction == false) return;

        interaction.Interaction();
        Debug.Log($"Interaction {interaction.name}");

        return;
    }

    public virtual void RightClick(InputAction.CallbackContext context)
    {

    }

    private bool CheckIsGround()
    {
        if(_rigidbody.velocity.y > 0) return false;

        RaycastHit2D rayHit = Physics2D.BoxCast(_rigidbody.position, new Vector2(transform.lossyScale.x, 0.2f),0 , Vector3.down, transform.localScale.y / 2, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null) return false;
        _isDownJump = false;
        return true;
    }


    private void FixedUpdate()
    {
        Move(_moveValue);

        _isGround = CheckIsGround();

        CheckInteraction();
    }

    private IEnumerator DownJump(Collider2D collider)
    {
        collider.enabled = false;
        _isDownJump = true;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -JumpForce / 2.0f);

        yield return new WaitForSeconds(0.2f);

        collider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        InteractionObject interactionObject = other.GetComponent<InteractionObject>();
        if (interactionObject == null) return;
        interactionObject.CanInteraction = true;
        //Debug.Log(other.name);
        // Ʈ���ŵ� ��ü �߰�
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

        // Ʈ���ŵ� ��ü ����
        if (_detectedObjects.Contains(interactionObject))
        {
            _detectedObjects.Remove(interactionObject);
        }
    }

    private void CheckInteraction()
    {
        if(_detectedObjects.Count == 0)
        {
            _interactionObject = null;
            return;
        }

        var closestObject = GetClosestObject();

        if(_interactionObject == null)
        {
            _interactionObject = closestObject;
        }

        else if (_interactionObject != closestObject)
        {
            if(_canInteractionOnlyClosestObject) 
                _interactionObject.CanInteraction = false;
            _interactionObject = closestObject;
        }

        if (_canInteractionOnlyClosestObject)
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
