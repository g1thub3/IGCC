using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] BoxCollider _enemyHitbox, _bodyHitbox;
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _rotationTime = 1.0f;
    [SerializeField] float _positionTolerance = 0.25f;
    [SerializeField] float _baseYPos = 2.15f;
    private float _speedMult;
    private Vector3 _desiredPoint;
    private bool _locationReached;
    private float _rotationLerp;

    private bool _timerFinished;
    private float _timer;

    private delegate void OnActionComplete();
    private OnActionComplete _onLocationReached;
    private OnActionComplete _onTimerFinished;
    private OnActionComplete _onSequenceCompleted;

    private void ClearEvents()
    {
        _onLocationReached = null;
        _onTimerFinished = null;
    }


    // DIVIDE FUNCTIONS INTO ACTIONS VS SEQUENCES

    // ACTIONS
    // FUNCTIONS TO DO
    // CAN YIELD

    private void SetDesiredPoint(Vector3 point, float mult = 1.0f)
    {
        // Set desired point
        _desiredPoint = point;
        _speedMult = mult;
        _rotationLerp = 0.0f;
        _locationReached = false;
        // Detect if location is reached

    }

    private void Wait(float t)
    {
        _timer = t;
        _timerFinished = false;
    }


    // SEQUENCES
    // A SEQUENCE OF ACTIONS TO BE DONE IN A MOVE

    private List<OnActionComplete> _actionSequences;

    private void UseNextInSequence()
    {
        _actionSequences.First().Invoke();
        _actionSequences.RemoveAt(0);
        if (_actionSequences.Count == 0 && _onSequenceCompleted != null)
        {
            _onSequenceCompleted.Invoke();
        }
    }

    private void Start()
    {
        _actionSequences = new List<OnActionComplete>();
        _desiredPoint = transform.position;
        _timerFinished = _locationReached = true;
        _charHandler = FindAnyObjectByType<CharacterHandler>();

        _speedMult = 1.0f;
        _onTimerFinished = delegate
        {
            SetDesiredPoint(transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));

        };
        _onLocationReached = delegate
        {
            Wait(3.0f);
        };
        Wait(2.0f);
        _rotationLerp = 0.0f;

        _swipePaths = new string[] {"TopSwipe", "BottomSwipe", "Zigzag", "ZigzagInv"};

        _onSequenceCompleted = delegate
        {
            ClearEvents();
            SwipeSequence();
        };
        SwipeSequence();

    }


    private void Timer()
    {
        if (!_timerFinished)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0.0f)
            {
                _timerFinished = true;
                if (_onTimerFinished != null)
                    _onTimerFinished.Invoke();
            }
        }
    }

    private void MoveToLocation()
    {
        if (!_locationReached)
        {
            Vector3 direction = (_desiredPoint - transform.position).normalized;
            transform.position += direction * _moveSpeed * _speedMult * Time.deltaTime;
            if ((transform.position - _desiredPoint).magnitude <= _positionTolerance)
            {
                _locationReached = true;
                if (_onLocationReached != null)
                {
                    _speedMult = 1.0f;
                    _onLocationReached.Invoke();
                }
            }
        }
    }
    private void RotationLerp()
    {
        if (_rotationLerp < _rotationTime)
        {
            _rotationLerp += Time.deltaTime;
            Quaternion desiredRotation = Quaternion.LookRotation((_desiredPoint - transform.position).normalized);
            desiredRotation.x = desiredRotation.z = 0.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, _rotationLerp / _rotationTime);
        }
    }

    private void Update()
    {
        Timer();
        MoveToLocation();
        RotationLerp();
    }

    // BOSS RELATED
    [SerializeField] Transform _swipeIndicators, _swipeWaypoints;
    [SerializeField] Transform _waitOutOfViewPt;
    [SerializeField] GameObject _smashIndicator;
    private CharacterHandler _charHandler;
    private string[] _swipePaths;
    private void SwipeSequence()
    {
        string chosenPath = _swipePaths[Random.Range(0,_swipePaths.Length)];
        int alt = Random.Range(0, 2) + 1;
        GameObject indicatorObject = _swipeIndicators.Find(chosenPath).gameObject;
        Transform swipeWaypoints = _swipeWaypoints.Find(chosenPath + alt.ToString());

        // Move to start of swipe
        _actionSequences.Add(delegate
        {
            indicatorObject.SetActive(true);
            SetDesiredPoint(swipeWaypoints.GetChild(0).position, 3);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });
        // Disable waypoints
        _actionSequences.Add(delegate
        {
            Wait(3);
            _onTimerFinished = delegate
            {
                indicatorObject.SetActive(false);
                UseNextInSequence();
            };
        });

        // Loop through waypoints
        for (int i = 1; i < swipeWaypoints.childCount; i++)
        {
            int chosen = i;
            _actionSequences.Add(delegate
            {
                SetDesiredPoint(swipeWaypoints.GetChild(chosen).position, 3);
                _onLocationReached = delegate
                {
                    UseNextInSequence();
                };
            });
        }

        _actionSequences.Add(delegate
        {
            ClearEvents();
        });

        // Begin sequence
        UseNextInSequence();
    }

    private void SmashSequence()
    {
        // Move out of view
        _actionSequences.Add(delegate
        {
            SetDesiredPoint(_waitOutOfViewPt.position, 3);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });



        _actionSequences.Add(delegate
        {
            ClearEvents();
        });

        // Begin sequence
        UseNextInSequence();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (_desiredPoint != null)
            Gizmos.DrawWireSphere(_desiredPoint, _positionTolerance);
    }
}