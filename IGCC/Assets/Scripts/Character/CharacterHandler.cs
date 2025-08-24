using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterHandler : MonoBehaviour
{
    PlayerInput _inputManager;
    public CinemachineCamera virtualCam;
    [SerializeField] List<MovementController> _controllers;
    MovementController _currController;
    Monkey _currMonkey;
    public Monkey CurrMonkey => _currMonkey;

    private uint _charIndex;
    private bool _hasLoaded;
    private List<Vector3> _ogPositions;

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
        _ogPositions = new List<Vector3>();
        for (int i = 0; i < _controllers.Count; i++)
        {
            _ogPositions.Add(_controllers[i].transform.localPosition);
        }
        Switch(0);
    }

    public void ResetPositions()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].GetComponent<Monkey>().RevertStack();
        }
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].transform.localPosition = _ogPositions[i];
        }
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
            if (_currMonkey != null)
                _currMonkey.OnDeSwitch();
            _charIndex = newInd;
            _currController = _controllers[(int)_charIndex];
            _currMonkey = _currController.GetComponent<Monkey>();
            virtualCam.Follow = _currController.transform;
            _currMonkey.OnSwitch();
        }
        _hasLoaded = true;
    }

    private void Move()
    {
        Vector2 moveInput = _actionMove.ReadValue<Vector2>();
        _currController.MoveInput(moveInput);
        if (_actionMove.IsPressed() && moveInput.x != 0)
        {
            _currController.SetDirection(moveInput.x > 0);
        }
    }
    private void Jump()
    {
        if (_actionJump.WasPressedThisFrame())
        {
            _currController.JumpInput();
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
        if (_actionRemoveStack.WasPressedThisFrame())
        {
            _currMonkey.RemoveFromStack();
        }
        if (_actionStack.WasPressedThisFrame())
        {
            var monk = _currMonkey.GetInteractedMonkey();
            if (monk != null){
                if (monk is GreenMonkey)
                {
                    var greenmonk = monk as GreenMonkey;
                    if (greenmonk.IsCarrying)
                        return;
                }
                _currMonkey.AddToStack(monk);
            }
        }
    }
    private void Update()
    {
        Jump();
        Move();
        SwapInput();
        StackInput();
        _currMonkey.Controls(_inputManager);
    }
}
