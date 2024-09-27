using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private CarMove _carMove;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _turnAction;


    private void Awake()
    {
        _carMove = GetComponent<CarMove>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _turnAction = _playerInput.actions["Turn"];
    }

    private void OnEnable()
    {
        _moveAction.started += OnMove;
        _moveAction.canceled += OnMove;

        _turnAction.started += OnTurn;
        _turnAction.canceled += OnTurn;
    }


    private void OnDisable()
    {
        _moveAction.started -= OnMove;
        _moveAction.canceled -= OnMove;

        _turnAction.started -= OnTurn;
        _turnAction.canceled -= OnTurn;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _carMove.AccelerationInput = context.ReadValue<float>();
    }

    private void OnTurn(InputAction.CallbackContext context)
    {
        _carMove.SteeringInput = context.ReadValue<float>();
    }
}
