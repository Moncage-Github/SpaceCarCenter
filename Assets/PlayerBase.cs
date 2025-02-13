using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour
{
    private float _moveValue;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;

    private bool _isGround;
    private bool _isDownPressed;
    private bool _isDownJump;

    [SerializeField] protected float Speed;
    [SerializeField] protected float JumpForce;

    private bool _canMove = true;
    public bool CanMove { get => _canMove; protected set => _canMove = value; }

    protected void OnMove(InputAction.CallbackContext context)
    {
        // 입력값을 읽어서 moveValue에 저장
        _moveValue = context.ReadValue<float>();
    }

    protected void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    protected void OnDown(InputAction.CallbackContext context)
    {
        _isDownPressed = context.performed;
    }

    private void Jump()
    {
        if (!CanMove) return;
        if (!_isGround || _isDownJump) return;

        if (_isDownPressed)
        {
            RaycastHit2D rayHit = Physics2D.BoxCast(_rigidbody.position, new Vector2(transform.lossyScale.x, 0.2f),0 , Vector3.down, transform.localScale.y / 2, LayerMask.GetMask("Platform"));
            if (rayHit.collider == null) return; 
            if(rayHit.collider.GetComponent<PlatformEffector2D>() != null)
            {
                StartCoroutine(DownJump());
            }
        }
        else
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
        }
    }
    public virtual void Move(float value)
    {
        if (!CanMove) return;
        Vector3 position = transform.localPosition;
        position.x += value * Speed * Time.deltaTime;

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


    protected virtual void FixedUpdate()
    {
        Move(_moveValue);

        _isGround = CheckIsGround();

    }

    private IEnumerator DownJump()
    {
        _collider.enabled = false;
        _isDownJump = true;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -JumpForce / 2.0f);

        yield return new WaitForSeconds(0.2f);

        _collider.enabled = true;
    }

}
