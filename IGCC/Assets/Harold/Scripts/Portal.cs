using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPortal : MonoBehaviour
{
    //Room to go to
    [SerializeField]
    PortalData _data;

    RoomData _roomStored;

    private void Awake()
    {
        _roomStored = _data.getRandomRoom();
    }

    private void OnTriggerEnter(Collider other)
    {
        RoomManager.Instance.goToNewRoom(_roomStored);
    }

}
