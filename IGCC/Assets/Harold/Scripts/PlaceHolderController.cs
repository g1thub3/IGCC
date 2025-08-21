using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceHolderController : MonoBehaviour
{
    CharacterController _controller;

    PlayerInput _playerInput;
    InputAction _moveInput;

    Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _moveInput = _playerInput.actions["Move"];

        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVal = _moveInput.ReadValue<Vector2>();

        _controller.Move((transform.forward * inputVal.y + transform.right * inputVal.x)*Time.deltaTime*10f);
    }
}
