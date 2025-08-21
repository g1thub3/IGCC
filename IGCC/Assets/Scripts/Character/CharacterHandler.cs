using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHandler : MonoBehaviour
{
    PlayerInput _inputManager;
    [SerializeField] CinemachineCamera _virtualCam;
    [SerializeField] List<MovementController> _controllers;
    MovementController _currController;
    private uint _charIndex;

    InputAction _actionJump, _actionMove, _actionWhite, _actionGreen, _actionPink;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInput>();
        _actionJump = _inputManager.actions["Jump"];
        _actionMove = _inputManager.actions["Move"];
        _actionWhite = _inputManager.actions["White"];
        _actionGreen = _inputManager.actions["Green"];
        _actionPink = _inputManager.actions["Pink"];
        Switch(0);
    }

    private void Switch(uint newInd)
    {
        _charIndex = newInd;
        if (newInd < 0)
        {
            _charIndex = (uint)(_controllers.Count - 1);
        }
        if (newInd >= _controllers.Count) {
            _charIndex = 0;
        }
        _currController = _controllers[(int)_charIndex];
        _virtualCam.Follow = _currController.transform;
    }

    private void Move()
    {
        Vector2 moveInput = _actionMove.ReadValue<Vector2>();
        _currController.MoveInput(moveInput);
    }
    private void Jump()
    {
        if (_actionJump.WasPressedThisFrame())
        {
            _currController.Jump();
        }
    }
    private void SwapInput()
    {
        if (_actionWhite.WasPressedThisFrame())
            Switch(0);
        if (_actionGreen.WasPressedThisFrame())
            Switch(1);
        if (_actionPink.WasPressedThisFrame())
            Switch(2);
    }
    private void Update()
    {
        Jump();
        Move();
        SwapInput();
    }
}
