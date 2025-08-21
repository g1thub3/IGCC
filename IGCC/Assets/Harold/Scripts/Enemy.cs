using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyType _enemyType;
    public EnemyType EnemyType => _enemyType;

    //Enemy's target

    Transform _playerRef;
    public Transform PlayerRef => _playerRef;

    public event Action<Enemy> onDeath;

    public event Action OnTargetChangedEvent;

    public virtual bool attemptToAttack() { return true; }

    public abstract void onStateChanged();

    public virtual void die()
    {
        onDeath?.Invoke(this);
    }

    public void setPlayer(Transform target)
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
