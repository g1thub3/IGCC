using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    Transform _playerSpawnPos;

    public Transform PlayerSpawnPos => _playerSpawnPos;

    //Room Enemy root
    [SerializeField]
    Transform _roomEnemyRoot;

    //Enemies
    List<Enemy> _enemies = new List<Enemy>();

    [SerializeField]
    List<GameObject> _doors;


    bool _roomActive = false;
    //Whenever an enemy dies
    public static event Action<Enemy> OnEnemyDeath;


    private void Start()
    {
        for (int i = 0; i < _doors.Count; i++)
        {
            _doors[i].SetActive(false);
        }

        if (!_roomEnemyRoot)
            return;

        _enemies = _roomEnemyRoot.GetComponentsInChildren<Enemy>().ToList();
    }

    public void startRoom()
    {
        //If the room is already active return
        if (_roomActive)
            return;

        _roomActive = true;

        //Enable the doors and block all exits
        for (int i = 0; i < _doors.Count; i++)
        {
            _doors[i].SetActive(true);
        }

        //Spawn the enemies in the room
        for (int i = 0; i < _enemies.Count; i++)
        {
            //Set their target to be the player
            _enemies[i].setPlayer(RoomManager.Instance.Player.transform);
        }

        //Subscribe remove enemy on this
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].onDeath += removeEnemy;
            //_enemies[i].gameObject.SetActive(true);
        }
    }

    public void removeEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);

        //Debug.Log("Removed: " + enemy.name);

        enemy.onDeath -= removeEnemy;
        OnEnemyDeath?.Invoke(enemy);

        RoomManager.Instance.raiseEnemyDeath(enemy);

        //If all enemies are dead open the doors
        if (_enemies.Count <= 0)
        {

            for (int i = 0; i < _doors.Count; i++)
            {
                _doors[i].SetActive(false);
            }
        }
    }

    public void addEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        //Debug.Log("Added: " + enemy.name);

        //Subscribe the event to the new enemy
        enemy.onDeath += removeEnemy;
        _enemies.Add(enemy);

        //Set their target to be the player
        enemy.setPlayer(RoomManager.Instance.Player.transform);

        enemy.transform.SetParent(_roomEnemyRoot, true);
    }

}

