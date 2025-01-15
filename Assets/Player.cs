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

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private void Awake()
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
    }

    void OnDisable()
    {
        _input.PlayerAction.Disable();
        _input.PlayerAction.Move.performed -= OnMove;

        _input.PlayerAction.Jump.performed -= OnJump;

        _input.PlayerAction.Down.performed -= OnDown;
        _input.PlayerAction.Down.canceled -= OnDown;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 입력값을 읽어서 moveValue에 저장
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
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        }
    }
    private void Move(float value)
    {
        Vector3 position = transform.localPosition;
        position.x += value * _speed * Time.deltaTime;

        transform.localPosition = position;
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
    }

    private IEnumerator DownJump(Collider2D collider)
    {
        collider.enabled = false;
        _isDownJump = true;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_jumpForce / 2.0f);

        yield return new WaitForSeconds(0.2f);

        collider.enabled = true;
    }
}
