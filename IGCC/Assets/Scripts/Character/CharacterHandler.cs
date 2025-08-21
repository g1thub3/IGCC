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
    Monkey _currMonkey;
    private uint _charIndex;
    private bool _hasLoaded;

    InputAction _actionJump, _actionMove, _actionWhite, _actionGreen, _actionPink, _actionStack, _actionRemoveStack;

    private void Start()
    {
        _inputManager = GetComponent<PlayerInput>();
        _actionJump = _inputManager.actions["Jump"];
        _actionMove = _inputManager.actions["Move"];
        _actionWhite = _inputManager.actions["White"];
        _actionGreen = _inputManager.actions["Green"];
        _actionPink = _inputManager.actions["Pink"];
        _actionStack = _inputManager.actions["Stack"];
        _actionRemoveStack = _inputManager.actions["RemoveStack"];
        _hasLoaded = false;
        Switch(0);
    }

    private void Switch(uint newInd)
    {
        if (_charIndex == newInd && _hasLoaded) return;
        if (newInd < 0)
        {
            newInd = (uint)(_controllers.Count - 1);
        }
        if (newInd >= _controllers.Count)
        {
            newInd = 0;
        }
        if (_controllers[(int)newInd].enabled)
        {
            _charIndex = newInd;
            _currController = _controllers[(int)_charIndex];
            _currMonkey = _currController.GetComponent<Monkey>();
            _virtualCam.Follow = _currController.transform;
            _currMonkey.OnSwitch();
        }
        _hasLoaded = true;
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

    private void StackInput()
    {
        if (_actionStack.WasPressedThisFrame())
        {
            var monk = _currMonkey.GetInteractedMonkey();
            if (monk != null)
                _currMonkey.AddToStack(monk);
        }
        if (_actionRemoveStack.WasPressedThisFrame())
        {
            _currMonkey.RemoveFromStack();
        }
    }
    private void Update()
    {
        Jump();
        Move();
        SwapInput();
        StackInput();
    }
}
