using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    static RoomManager _instance;
    static public RoomManager Instance => _instance;

    //Room Data of current room
    RoomData _currentRoomData;

    [SerializeField]
    Room _currentRoom;

    [SerializeField]
    CharacterHandler _player;

    public CharacterHandler Player => _player;

    ////UI Panel to confirm whether we want to go to next room
    //[SerializeField]
    //RoomConfirmationPanel _confirmationPanel;

    [SerializeField]
    AudioClip _clip;

    AudioSource _source;


    public static event Action<Enemy> OnEnemyDeath;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        if (!_instance)
            _instance = this;
        else
        {
            Destroy(gameObject);
            Debug.Log("More than one Room Manager in Scene");
        }
    }



    void destroyCurrentRoom()
    {
        if (_currentRoom)
            Destroy(_currentRoom.gameObject);
    }

    //public void attemptGoToNewRoom(RoomData newRoom)
    //{
    //    _confirmationPanel.gameObject.SetActive(true);
    //    _confirmationPanel.initPanel(newRoom);
    //}

    public void goToNewRoom(RoomData newRoom, bool rotate = true)
    {
        //Go to next room
        _currentRoomData = newRoom;

        if (_source && _clip)
            _source.PlayOneShot(_clip);

        FadingTransition.Instance.RunAfterFadeOut(() => onNewRoomSet(_currentRoomData.AllowRotation));
    }

    void onNewRoomSet(bool rotate = true)
    {
        //Unsubscribe
        //Destroy the current room
        destroyCurrentRoom();


        Vector3 rotation = new Vector3(0f, 90f * UnityEngine.Random.Range(0, 3), 0f);

        if (!rotate)
            rotation = Vector3.zero;

        //Create the next room
        _currentRoom = Instantiate(_currentRoomData.RoomPrefab, Vector3.zero, Quaternion.Euler(rotation));

        _player.gameObject.SetActive(false);

        //Set the player position to the new spawn pos
        _player.transform.position = _currentRoom.PlayerSpawnPos.position + Vector3.up;
        _player.ResetPositions();

        _player.gameObject.SetActive(true);

        StartCoroutine(createNewRoom());
    }

    //Register a new enemy into the room
    public void registerEnemy(Enemy enemy)
    {
        if (!_currentRoom)
            return;

        _currentRoom.addEnemy(enemy);
    }

    public void raiseEnemyDeath(Enemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }


    public IEnumerator createNewRoom()
    {
        yield return null;

        if (AstarPath.active)
            AstarPath.active.Scan();
    }

    //public void setPlayer(Transform player)
    //{
    //    _player = player;
    //}
}
