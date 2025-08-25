using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    int _lives=3;
    //Health list
    [SerializeField]
    private List<Health> _health = new List<Health>();
    RoomManager roomManager;

    public void Start()
    {
        for (int i = 0; i < _health.Count; i++)
        {
            _health[i].OnDeathEvent+=respawn;
        }
    }

    public void respawn()
    {
        RoomManager.Instance.goToNewRoom(RoomManager.Instance.CurrentRoomData, false, () => {
            _lives--;
            for (int i = 0; i < _health.Count; i++)
            {
                _health[i].revive();
            }
        });
    }
}
