using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    Inventory _inventory;

    [SerializeField]
    RoomData _originalRoom;

    [SerializeField]
    GameObject _respawnCanvas;

    [SerializeField]
    Button _respawnButton;

    //Health list
    [SerializeField]
    private List<Health> _health = new List<Health>();


    public void Start()
    {
        for (int i = 0; i < _health.Count; i++)
        {
            //Debug.Log("Subscribed Event");
            _health[i].OnDeathEvent+=respawn;
        }

        _respawnButton.onClick.AddListener(restart);

    }

    public void restart()
    {
        _inventory.resetInventory();
        _inventory.gameObject.SetActive(true);
        RoomManager.Instance.goToNewRoom(_originalRoom,false, ()=> { _respawnCanvas.SetActive(false); });
    }

    public void respawn()
    {
        RoomManager.Instance.goToNewRoom(RoomManager.Instance.CurrentRoomData, false, () => {

            //Debug.Log("Going to new room");
            //Reduce lives by 1
            _inventory.changeLivesBy(-1);

            if (_inventory.Lives <= 0)
            {
                _respawnCanvas.SetActive(true);
                _inventory.gameObject.SetActive(false);
                //_inventory.resetInventory();
                //RoomManager.Instance.goToNewRoom(_originalRoom);
            }

            for (int i = 0; i < _health.Count; i++)
            {
                _health[i].revive();
            }
        });
    }
}
