using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Room/RoomData")]
public class RoomData : ScriptableObject
{
    [SerializeField]
    Sprite _roomImage;

    [SerializeField]
    public Sprite RoomImage => _roomImage;

    [SerializeField]
    string _roomName;
    public string RoomName => _roomName;

    [TextArea]
    [SerializeField]
    string _roomDesc;
    public string RoomDesc => _roomDesc;

    [SerializeField]
    Room _roomPrefab;

    [SerializeField]
    bool _allowRotation = true;
    public bool AllowRotation => _allowRotation;

    public Room RoomPrefab => _roomPrefab;

    //public void moveToNewRoom(Transform player)
    //{
    //    Room newRoom = Instantiate(_roomPrefab);

    //    //Go to the player spawn position of the new room
    //    player.transform.position = newRoom.PlayerSpawnPos.position;
    //}
}
