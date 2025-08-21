using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    CharacterController _controller;

    [Header("Assets")]
    [SerializeField] InputActionAsset _inputManager;

    [Header("Properties")]
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 2.0f;
    [SerializeField] float _characterMass = 1.0f;
    [SerializeField] float _moveSpeed = 2.0f;

    InputAction _actionJump, _actionMove;


    Vector3 _yVelocity;
    Vector3 _xVelocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _yVelocity = Vector3.zero;
        _xVelocity = Vector3.zero;
        _actionJump = _inputManager["Jump"];
        _actionMove = _inputManager["Move"];
    }

    private void Jump()
    {
        if (_actionJump.IsPressed() &&  _controller.isGrounded)
        {
            _yVelocity.y = Mathf.Sqrt(-2 * _jumpHeight * _gravity) * Time.deltaTime;
        }
        if (_controller.isGrounded && _yVelocity.y < 0)
        {
            _yVelocity.y = -2;
        }
    }

    private void ApplyGravity()
    {
        _yVelocity.y += _gravity * _characterMass * Time.deltaTime;
    }

    private void Run()
    {
        Vector2 moveInput = _actionMove.ReadValue<Vector2>();
        _xVelocity = (Vector3.right * moveInput.x + Vector3.forward * moveInput.y) * _moveSpeed * Time.deltaTime;
    }

    private void Move()
    {
        Vector3 finalVel = _yVelocity + _xVelocity;
        _controller.Move(finalVel);
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        ApplyGravity();
        Jump();
        Move();
    }
}
