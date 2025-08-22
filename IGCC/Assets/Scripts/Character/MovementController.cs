using UnityEngine;

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
    [SerializeField] float _castRange = 0.8f;
    [SerializeField] float _maxXZVel = 100;
    [SerializeField] float _maxYVel = 100;

    Vector2 _currInput;
    Vector3 _yVelocity;
    Vector3 _xVelocity;
    Vector3 _extVelocity;
    Transition _jumpToleranceTimer;
    public bool isRight;

    public void AddVelocity(Vector3 force)
    {
        _extVelocity += force;
    }
    private bool IsGrounded()
    {
        Vector3 charOrigin = transform.position - new Vector3(0, _controller.height * 0.35f, 0);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 currOrigin = charOrigin + new Vector3(_controller.radius * i * _castRange,0, _controller.radius * j * _castRange);
                bool isGrounded = Physics.Raycast(currOrigin, -transform.up, _groundRay, LayerMask.GetMask("Ground"));
                if (isGrounded)
                    return true;
            }
        }
        //bool isGrounded = Physics.CapsuleCast(origin, origin + new Vector3(0, -_groundRay, 0), 
        //    _controller.radius - 0.1f, Vector3.down, _groundRay, LayerMask.NameToLayer("Ground"));
        //bool isGrounded = Physics.Raycast(origin, -transform.up, _groundRay, LayerMask.GetMask("Ground"));
        //return isGrounded;
        return false;
    }

    private bool PlayerContact()
    {
        //bool isGrounded = Physics.Raycast(transform.position, -transform.up, _groundRay, LayerMask.GetMask("Player"));
        //return isGrounded;
        Vector3 charOrigin = transform.position - new Vector3(0, _controller.height * 0.35f, 0);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 currOrigin = charOrigin + new Vector3(_controller.radius * i * _castRange, 0, _controller.radius * j * _castRange);
                bool isGrounded = Physics.Raycast(currOrigin, -transform.up, _groundRay, LayerMask.GetMask("Player"));
                if (isGrounded)
                    return true;
            }
        }
        return false;
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _yVelocity = Vector3.zero;
        _xVelocity = Vector3.zero;
        _currInput = Vector2.zero;
        _extVelocity = Vector3.zero;
        _jumpToleranceTimer = new Transition(_jumpTolerance);
        isRight = true;
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
        if (PlayerContact() && _yVelocity.y < 0)
        {
            _yVelocity.y *= -1;
            _yVelocity.y *= 0.75f;
            return;
        }
        if (IsGrounded() && _yVelocity.y < 0)
        {
            _yVelocity.y = -0.15f;
        }
    }

    public void MoveInput(Vector2 moveInput) => _currInput = moveInput;
    public void SetDirection(bool dir) => isRight = dir;

    private void Run()
    {
        _xVelocity = (Vector3.right * _currInput.x + Vector3.forward * _currInput.y) * _moveSpeed * Time.deltaTime;
    }

    private void Move()
    {
        Vector3 finalVel = _yVelocity + _xVelocity + _extVelocity;
        finalVel.x = Mathf.Clamp(finalVel.x, -_maxXZVel, _maxXZVel);
        finalVel.y = Mathf.Clamp(finalVel.y, -_maxYVel, _maxYVel);
        finalVel.z = Mathf.Clamp(finalVel.z, -_maxXZVel, _maxXZVel);
        _controller.Move(finalVel);
    }

    private void Update()
    {
        if (_jumpToleranceTimer.Progression > 0)
        {
            _jumpToleranceTimer.Revert();
        }
        _extVelocity *= 0.6f * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        Run();
        YForces();
        Move();
        MoveInput(Vector2.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (_controller != null){
            Vector3 charOrigin = transform.position - new Vector3(0, _controller.height * 0.35f, 0);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Vector3 currOrigin = charOrigin + new Vector3(_controller.radius * i * _castRange, 0, _controller.radius * j * _castRange);
                    Gizmos.DrawLine(currOrigin, currOrigin + new Vector3(0, -_groundRay, 0));
                }
            }
        }
    }
}
