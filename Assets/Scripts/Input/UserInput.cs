using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    private Vehicle _carMove;

    private CollectionInput _playerInput;


    private void Awake()
    {
        _playerInput = new();
        _carMove = GetComponent<Vehicle>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.PlayerAction.Move.started += OnMove;
        _playerInput.PlayerAction.Move.canceled += OnMove;

        _playerInput.PlayerAction.Turn.started += OnTurn;
        _playerInput.PlayerAction.Turn.canceled += OnTurn;
    }


    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.PlayerAction.Move.started -= OnMove;
        _playerInput.PlayerAction.Move.canceled -= OnMove;

        _playerInput.PlayerAction.Turn.started -= OnTurn;
        _playerInput.PlayerAction.Turn.canceled -= OnTurn;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<float>());
        _carMove.AccelerationInput = context.ReadValue<float>();
    }

    private void OnTurn(InputAction.CallbackContext context)
    {
        _carMove.SteeringInput = context.ReadValue<float>();
    }
}
