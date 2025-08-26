using UnityEngine;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    Inventory _inventory;

    [SerializeField]
    RoomData _originalRoom;

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

            //Reduce lives by 1
            _inventory.changeLivesBy(-1);

            if (_inventory.Lives <= 0)
            {
                _inventory.resetInventory();
                RoomManager.Instance.goToNewRoom(_originalRoom);
            }

            for (int i = 0; i < _health.Count; i++)
            {
                _health[i].revive();
            }
        });
    }
}
