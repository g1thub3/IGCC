using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBehaviour : Enemy
{
    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    float _moveSpeed;

    [SerializeField]
    bool _canChase=true;

    [SerializeField]
    float _attackRange = 1f;

    Vector3 _moveDirection = Vector3.zero;

    //All the components relating to the enemy's controls and animations
    EnemySight _sight;


    //State of the enemy
    State _currentState = State.IDLE;

    //Reference to the HP value of the enemy
    Health _healthController;

    //Ref to attack controller
    MeleeAttackController _attackController;

    SpriteAnimationController _animator;

    //Path handler of enemy
    EnemyPathHandler _pathHandler;

    ////Movement controller of enemy
    //MovementController _movementController;

    WaypointController _waypointController;

    //Rigidbody _rb;

    float _stateTimeElapsed = 0f;


    // Start is called before the first frame update
    void Awake()
    {
        //Get the parts making up the enemy
        _sight = GetComponent<EnemySight>();

        _animator = GetComponent<SpriteAnimationController>();

        //_rb = GetComponent<Rigidbody>();
        _attackController = GetComponent<MeleeAttackController>();

        _pathHandler = GetComponent<EnemyPathHandler>();

        //_movementController = GetComponent<MovementController>();

        _waypointController = GetComponent<WaypointController>();

        //Get a reference to the HP of the enemy
        _healthController = GetComponent<Health>();
        if (_healthController != null)
        {
            //subscribe the die method to the _health component's onDeath method
            _healthController.OnDeathEvent += die;
        }
        _moveDirection = Vector3.one;
        _moveDirection.y = 0;

        //On target set
        OnTargetChangedEvent += onTargetSet;
    }

    public void Start()
    {
        _attackController.OnAttackFinishEvent += () => { changeState(State.IDLE); };


        onTargetSet();
    }


    public void onTargetSet()
    {
        _sight.setTarget(PlayerRef);
        if (PlayerRef.CurrMonkey)
            _pathHandler.setTarget(PlayerRef.CurrMonkey.transform);
    }

    // Update is called once per frame
    void Update()
    {

        if (_healthController.isDead())
            return;

        if (_sight == null)
        {
            Debug.LogWarning("There is a missing ref in Enemy Brain " + gameObject.name);
            return;
        }

        if (_sight.Target == null)
            return;

        //Debug.Log("Basic enemy state running");

        _pathHandler.setTarget(_sight.Target);

        //Debug.Log(_currentState.ToString());


        switch (_currentState)
        {
            case State.ATTACK:
                //If the attack animation has finished
                //if (_animator.GetBool("IsAttacking"))
                //{
                //    changeState(State.IDLE);
                //}
                //else
                _attackController.setAttackDirection((_sight.Target.transform.position - transform.position).normalized);
                _attackController.doAttack();


                break;

            case State.IDLE:


                _moveSpeed = Mathf.Lerp(_moveSpeed, 0f, 5 * Time.deltaTime);
                _moveSpeed = 0f;
                _moveDirection = (_sight.Target.transform.position - transform.position).normalized;
                CheckForPlayer();

                //Go back to patrol after a certain amount of time
                if (_stateTimeElapsed > 3f)
                {
                    _moveDirection = Random.insideUnitSphere.normalized;
                    _moveDirection.y = 0;

                    _pathHandler.setPathEnabled(false);
                    _waypointController.enabled = true;

                    changeState(State.PATROL);
                }

                //_movementController.move(Vector3.zero);


                break;
            case State.CHASE:

                //Set the path handler to true
                _pathHandler.setPathEnabled(true);

                // Debug.Log("Chasing Player");

                _moveSpeed = Speed;
                //_moveDirection = (_sight.PlayerRef.transform.position - transform.position).normalized;
                _moveDirection = _pathHandler.getDir();

                //If we cant see player anymore go back to idle
                if (!_sight.canSee())
                {
                    changeState(State.IDLE);
                }
                else
                    attemptToAttack();


                break;
            case State.PATROL:
                _moveSpeed = Speed;

                _waypointController.setSpeed(_moveSpeed);
                CheckForPlayer();

                rotateToTargetGradual();
                //Move during patrol
                //_movementController.move(_moveSpeed * _moveDirection.normalized);

                break;


            default:
                CheckForPlayer();
                break;

        }

        //Set the direction of the animation
        _animator.setFloat("Speed", _moveSpeed);
        //Set speed of path
        _pathHandler.setSpeed(_moveSpeed);

        //Movement direction and rotation handling
        _moveDirection.y = 0f;

        //Handle state time elapsed
        _stateTimeElapsed += Time.deltaTime;

        //Debug.Log(_stateTimeElapsed);


    }

    public void rotateToTargetGradual()
    {

        if (_moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void changeState(State newState)
    {
        _currentState = newState;
        //Debug.Log("StateTimeCHanged" + newState);
        onStateChanged();
    }

    public override void onStateChanged()
    {
        _stateTimeElapsed = 0f;
    }

    public void CheckForPlayer()
    {
        if (!_canChase)
            return;

        if (_sight.canSee())
        {
            _waypointController.enabled = false;
            //Set the path handler to true
            _pathHandler.setPathEnabled(true);
            changeState(State.CHASE);
            //_animator.SetBool("IsWalking", true);
            //If attack successful return
            if (attemptToAttack())
            {
                return;
            }

            //Set movement direction to looking direction
            _moveDirection = _moveDirection.normalized;
        }
    }

    public override bool attemptToAttack()
    {
        //float distToTarget = Vector2.Distance(_sight.PlayerRef.transform.position, transform.position);

        ////If the distance to target is less than the attack range of the enemy
        if (_sight.inRange(_attackRange))
        {
            //Debug.Log("Can attack player");
            //Set the trigger of attack

            //set the move speed to 0
            _moveSpeed = 0;

            //_pathHandler.setPathEnabled(false);
            changeState(State.ATTACK);
            return true;
        }

        return false;
    }

    public override void die()
    {
        //_itemToDropOnDeath.createItem();
        base.die();
    }

}
