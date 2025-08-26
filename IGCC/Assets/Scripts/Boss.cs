using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Boss Properties")]
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _rotationTime = 1.0f;
    [SerializeField] float _positionTolerance = 0.25f;
    [SerializeField] float _baseYPos = 2.15f;
    [SerializeField] float _platformCooldown = 6.0f;
    private float _speedMult;
    private Vector3 _desiredPoint;
    private bool _locationReached;
    private float _rotationLerp;
    private Quaternion _desiredRotation;
    private bool _lockPosition;

    private bool _timerFinished;
    private float _timer;
    private float _platformTimer;

    private delegate void OnActionComplete();
    private OnActionComplete _onLocationReached;
    private OnActionComplete _onTimerFinished;
    private OnActionComplete _onSequenceCompleted;

    private Health _healthController;

    private System.Action[] _basicAttacks, _specialAttacks;
    private int _basicAttacksUsed;
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
    private void SelectNewAttack()
    {
        if (_basicAttacksUsed == 3)
        {
            _basicAttacksUsed = 0;
            _specialAttacks[Random.Range(0, 3)]();
        } else
        {
            _basicAttacksUsed++;
            _basicAttacks[Random.Range(0, 2)]();
        }
    }

    private void ResetPlatforms()
    {
        _platformTimer = 0.0f;
        TogglePlatforms(false);
    }

    private void Start()
    {
        _basicAttacks = new System.Action[2] { SwipeSequence, SmashSequence };
        _specialAttacks = new System.Action[3] { HiddenAttackSequence, ThrowSequence, SpawnEnemies };
        _basicAttacksUsed = 0;
        _healthController = GetComponent<Health>();
        _healthController.OnHealthChangeEvent += UpdateUI;
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
            SelectNewAttack();
        };
        _lockPosition = false;

        AudioManager.Instance.PlayBGM("BGM_RPGBattle");

        ResetPlatforms();
        _healthController.OnHealthChangeEvent += delegate
        {
            ResetPlatforms();
        };

        SelectNewAttack();
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
        if (_lockPosition) return;
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

    private void ToggleLock(bool isActive) => _lockPosition = isActive;
    private void RotateToPoint(Vector3 position)
    {
        Quaternion desiredRotation = Quaternion.LookRotation((position - transform.position).normalized);
        desiredRotation.x = desiredRotation.z = 0.0f;
        _desiredRotation = desiredRotation;
        _rotationLerp = 0.0f;
    }
    private void RotationLerp()
    {
        if (_rotationLerp < _rotationTime)
        {
            _rotationLerp += Time.deltaTime;
            if (!_lockPosition)
            {
                Quaternion desiredRotation = Quaternion.LookRotation((_desiredPoint - transform.position).normalized);
                desiredRotation.x = desiredRotation.z = 0.0f;
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, _rotationLerp / _rotationTime);
            } else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, _rotationLerp / _rotationTime);
            }
        }
    }

    private void Update()
    {
        Timer();
        MoveToLocation();
        RotationLerp();
        if (_platformTimer < _platformCooldown)
        {
            _platformTimer += Time.deltaTime;
            if (_platformTimer >= _platformCooldown)
            {
                TogglePlatforms(true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (_desiredPoint != null)
            Gizmos.DrawWireSphere(_desiredPoint, _positionTolerance);
    }

    private void TogglePlatforms(bool isActive = false)
    {
        _jumpPlatforms.SetActive(isActive);
    }
    private void ToggleHiddenAttackIndicator(bool isActive = false)
    {
        _hiddenAttackIndicator.SetActive(isActive);
    }
    private void ClearHiddenStructures()
    {
        for (int i = _hiddenStructureContainer.childCount- 1; i >= 0; i--)
        {
            Destroy(_hiddenStructureContainer.GetChild(i).gameObject);
        }
    }
    private void GenerateHiddenStructures()
    {
        for (int i = 0; i < _structureCount; i++)
        {
            Vector3 randPos = _hiddenStructureSpawnRange.transform.position + 
                new Vector3(
                    Random.Range(-_hiddenStructureSpawnRange.localScale.x * 0.5f, _hiddenStructureSpawnRange.localScale.x * 0.5f),
                0,
                Random.Range(-_hiddenStructureSpawnRange.localScale.z * 0.5f, _hiddenStructureSpawnRange.localScale.z * 0.5f));
            Quaternion randRot = Quaternion.Euler(0,Random.Range(0,360.0f),0);
            var structure = Instantiate(_hiddenStructure, randPos, randRot, _hiddenStructureContainer);
        }
    }
    private MovementController SelectMonkey() {
            bool anyAvailable = false;
            foreach (var controller in _charHandler.Controllers)
            {
                if (controller.enabled == true) {
                    anyAvailable = true;
                    break;
                }
            }
            int check = 0;
            while (anyAvailable && check < 1000)
            {
                var controller = _charHandler.Controllers[Random.Range(0, _charHandler.Controllers.Count)];
                if (controller.enabled == true)
                    return controller;
                check++;
            }
            return null;
        }


    // BOSS RELATED
    [Header("Boss Attacks")]
    [SerializeField] Transform _swipeIndicators, _swipeWaypoints;
    [SerializeField] Transform _waitOutOfViewPt;
    [SerializeField] GameObject _smashIndicator;
    [SerializeField] GameObject _jumpPlatforms;
    [SerializeField] Transform _hiddenStructureSpawnRange;
    [SerializeField] Transform _hiddenStructureContainer;
    [SerializeField] GameObject _hiddenStructure;
    [SerializeField] GameObject _hiddenAttackIndicator;
    [SerializeField] Transform _hiddenAttackWaypoints;
    [SerializeField] int _structureCount = 4;
    [SerializeField] GameObject _spikeIndicator;
    [SerializeField] Transform _spikePoints;
    [SerializeField] GameObject _spikes;
    [SerializeField] Transform _enemySpawnPoints;
    [SerializeField] GameObject _flyingEnemy;
    [SerializeField] Transform _enemyContainer;
    [SerializeField] GameObject _sweepAttack;

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

        _actionSequences.Add(ClearEvents);

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

        // Wait
        _actionSequences.Add(delegate
        {
            Wait(1.5f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        var target = SelectMonkey();

        // Move above target
        _actionSequences.Add(delegate
        {
            _smashIndicator.transform.position = new Vector3(target.transform.position.x,_smashIndicator.transform.position.y, target.transform.position.z);
            _smashIndicator.SetActive(true);
            SetDesiredPoint(new Vector3(_smashIndicator.transform.position.x, transform.position.y, _smashIndicator.transform.position.z), 3);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait
        _actionSequences.Add(delegate
        {
            Wait(3.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        // Move down on target
        _actionSequences.Add(delegate
        {
            _smashIndicator.SetActive(false);
            SetDesiredPoint(new Vector3(_smashIndicator.transform.position.x, _baseYPos, _smashIndicator.transform.position.z), 10);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait
        _actionSequences.Add(delegate
        {
            Wait(3.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(ClearEvents);

        // Begin sequence
        UseNextInSequence();
    }
    private void HiddenAttackSequence()
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
        
        // Wait a bit
        _actionSequences.Add(delegate
        {
            Wait(2.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(delegate
        {
            GenerateHiddenStructures();
            ToggleHiddenAttackIndicator(true);
            Wait(3.5f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        bool fromRight = Random.Range(0, 2) == 1;
        Transform pt1 = _hiddenAttackWaypoints.GetChild(fromRight ? 0 : 1);
        Transform pt2 = _hiddenAttackWaypoints.GetChild(fromRight ? 1 : 0);

        // Move to position
        _actionSequences.Add(delegate
        {
            SetDesiredPoint(pt1.position, 8);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Swipe
        _actionSequences.Add(delegate
        {
            _sweepAttack.SetActive(true);
            ToggleHiddenAttackIndicator(false);
            SetDesiredPoint(pt2.position, 10);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(delegate
        {
            _sweepAttack.SetActive(false);
            Wait(3.0f);
            _onTimerFinished = delegate
            {
                ClearHiddenStructures();
                UseNextInSequence();
            };
        });

        _actionSequences.Add(ClearEvents);
        UseNextInSequence();
    }
    private void ThrowSequence()
    {
        Transform chosenSpikePath = _spikePoints.GetChild(Random.Range(0,2));

        // Move to point
        _actionSequences.Add(delegate
        {
            _spikeIndicator.SetActive(true);
            SetDesiredPoint(chosenSpikePath.GetChild(0).position, 3);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait a bit
        _actionSequences.Add(delegate
        {
            Wait(6.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait a bit
        _actionSequences.Add(delegate
        {
            // Activate big sweeper attack
            _sweepAttack.SetActive(true);
            _spikeIndicator.SetActive(false);
            _spikes.SetActive(true);
            Wait(3.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(delegate
        {
            SetDesiredPoint(chosenSpikePath.GetChild(1).position, 0.5f);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait a bit
        _actionSequences.Add(delegate
        {
            Wait(1.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(delegate
        {
            _spikes.SetActive(false);
            _sweepAttack.SetActive(false);
            SetDesiredPoint(new Vector3(transform.position.x, _baseYPos, transform.position.z), 3);
            _onLocationReached = delegate
            {
                UseNextInSequence();
            };
        });

        // Wait a bit
        _actionSequences.Add(delegate
        {
            Wait(6.0f);
            _onTimerFinished = delegate
            {
                UseNextInSequence();
            };
        });

        _actionSequences.Add(ClearEvents);
        UseNextInSequence();
    }
    private void SpawnEnemies()
    {
        _actionSequences.Add(delegate
        {
            for (int i = _enemyContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(_enemyContainer.GetChild(i).gameObject);
            }
            for (int i = 0; i < _enemySpawnPoints.childCount; i++)
            {
                Instantiate(_flyingEnemy, _enemySpawnPoints.GetChild(i).position, _enemySpawnPoints.GetChild(i).rotation, _enemyContainer);
            }
        });
        UseNextInSequence();
    }
    // BOSS UI

    [Header("UI")]
    [SerializeField] float _uiWidth = 900;
    [SerializeField] RectTransform _uiHealthBar;

    private void UpdateUI(float newHealth)
    {
        _uiHealthBar.sizeDelta = new Vector2(_uiWidth * (newHealth / _healthController.MaxHealthPoints), _uiHealthBar.sizeDelta.y);
    }
}