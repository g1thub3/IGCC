using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    CharacterController _controller;

    [Header("Properties")]
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 2.0f;
    [SerializeField] float _characterMass = 1.0f;
    [SerializeField] float _moveSpeed = 2.0f;
    [SerializeField] float _jumpTolerance = 0.25f;
    [SerializeField] float _groundRay = 1.25f;

    Vector2 _currInput;
    Vector3 _yVelocity;
    Vector3 _xVelocity;
    Transition _jumpToleranceTimer;

    private bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, -transform.up, _groundRay, LayerMask.GetMask("Ground"));
        return isGrounded;
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _yVelocity = Vector3.zero;
        _xVelocity = Vector3.zero;
        _currInput = Vector2.zero;
        _jumpToleranceTimer = new Transition(_jumpTolerance);
    }

    public void Jump()
    {
        if (IsGrounded() && _jumpToleranceTimer.Progression == 0)
        {
            _jumpToleranceTimer.Progression = 1.0f;
            _yVelocity.y = Mathf.Sqrt(-2 * _jumpHeight * _gravity) * Time.deltaTime;
        }
    }

    private void YForces()
    {
        _yVelocity.y += _gravity * _characterMass * Time.deltaTime;
        if (IsGrounded() && _yVelocity.y < 0)
        {
            _yVelocity.y = -2;
        }
    }

    public void MoveInput(Vector2 moveInput)
    {
        _currInput = moveInput;
    }
    private void Run()
    {
        _xVelocity = (Vector3.right * _currInput.x + Vector3.forward * _currInput.y) * _moveSpeed * Time.deltaTime;
    }

    private void Move()
    {
        Vector3 finalVel = _yVelocity + _xVelocity;
        _controller.Move(finalVel);
    }

    private void Update()
    {
        if (_jumpToleranceTimer.Progression > 0)
        {
            _jumpToleranceTimer.Revert();
        }
    }
    private void FixedUpdate()
    {
        Run();
        YForces();
        Move();
        MoveInput(Vector2.zero);
    }
}
