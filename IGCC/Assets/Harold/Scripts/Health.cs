using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float _maxHealthPoints;
    [SerializeField] float _healthPoints;

    [SerializeField] GameObject _damageParticlesPrefab;

    SpriteAnimationController _spriteRenderer;

    AudioSource _sfxAudioSrc;
    [SerializeField] AudioClip _damageAudioClip;


    private bool _isDead = false;
    //keep track of no of times hit
    int _hitCounter = 0;
    public float HitCounter => _hitCounter;

    //Setter for hp
    public float HealthPoints
    {
        get { return _healthPoints; }
        set
        {
            if (value > _maxHealthPoints)
            {
                _healthPoints = _maxHealthPoints;
            }
            else if (value < 0f)
            {
                _healthPoints = 0f;
            }
            else
                _healthPoints = value;

            OnHealthChangeEvent?.Invoke(_healthPoints);
        }
    }
    public float MaxHealthPoints
    {
        get { return _maxHealthPoints; }
        set
        {
            _maxHealthPoints = value;
            OnHealthChangeEvent?.Invoke(_healthPoints);
        }
    }


    [SerializeField] float _iDuration = 0.5f;

    public event Action<float> OnHealthChangeEvent;

    public event Action<float> OnDamageEvent;

    public event System.Action OnDeathEvent;
    private Coroutine _invincibilityCoroutine;

    private void Awake()
    {
        OnHealthChangeEvent += checkIfDead;
        _sfxAudioSrc = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteAnimationController>();
    }

    public void OnEnable()
    {
        _invincibilityCoroutine = null; // Reset coroutine reference when done
    }

    private void OnDestroy()
    {
        OnHealthChangeEvent -= checkIfDead;
    }

    public void resetHitCounter()
    {
        _hitCounter = 0;
    }

    //Take damage
    public void takeDamage(float damage)
    {
        if (isInvincible()|| isDead())
            return;

        //Debug.Log(gameObject.name + " damage");

        //Take Damage and spawn particles
        HealthPoints -= damage;

        generateDamageParticles();

        //Set the sprite to flicker if there is a anim controller on it
        if (_spriteRenderer)
            _spriteRenderer.setToFlicker(_iDuration);

        if (_sfxAudioSrc && _damageAudioClip && !_isDead)
            _sfxAudioSrc.PlayOneShot(_damageAudioClip);

        //Increment hit counter
        _hitCounter++;

        if (_invincibilityCoroutine != null)
        {
            StopCoroutine(_invincibilityCoroutine);
        }
        _invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine());

        //invoke the event
        OnDamageEvent?.Invoke(damage);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        float timer = _iDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        _invincibilityCoroutine = null; // Reset coroutine reference when done
    }

    public void generateDamageParticles()
    {
        if (_damageParticlesPrefab != null)
            Instantiate(_damageParticlesPrefab, transform);
    }

    public bool isInvincible()
    {
        return _invincibilityCoroutine != null;
    }


    ////Set HP without triggering events
    //public void setHealthDirectly(float health)
    //{
    //    _healthPoints = health;
    //    OnHealthChangeEvent?.Invoke(_healthPoints);
    //}

    //public void setMaxHealthDirectly(float maxHealth)
    //{
    //    _maxHealthPoints = maxHealth;
    //    OnHealthChangeEvent?.Invoke(_healthPoints);
    //}

    public bool isDead()
    {
        return _healthPoints<=0f;
    }

    public void revive()
    {
        _isDead = false;
        _healthPoints = _maxHealthPoints;
        _spriteRenderer.setToOpaque();
    }

    void checkIfDead(float health)
    {
        if (health <= 0f && !_isDead)
        {
            OnDeathEvent?.Invoke();
            _isDead = true;
            //Destroy(gameObject);
        }
    }
}