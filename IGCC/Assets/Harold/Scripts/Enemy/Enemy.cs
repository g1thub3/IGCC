using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;

    [SerializeField]
    float _speed;
    public float Speed => _speed;

    //Enemy's target
    [SerializeField]
    CharacterHandler _playerRef;
    public CharacterHandler PlayerRef => _playerRef;

    public event Action<Enemy> onDeath;

    public event Action OnTargetChangedEvent;

    public virtual bool attemptToAttack() { return true; }

    public abstract void onStateChanged();

    public virtual void die()
    {
        onDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void setPlayer(CharacterHandler target)
    {
        _playerRef = target;
        OnTargetChangedEvent?.Invoke();
    }
}

//Enum of enemy types

public enum EnemyType
{
    Skeleton,
    Alien,
    Killer,
    Angel,
    Bear,
    Tower,
    Summoner,
    Vampire,
    Minotaur,
    BlackDemon
}
