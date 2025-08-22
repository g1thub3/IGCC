using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] PlayerInput _inputManager;
    [SerializeField] Transform _monkeyContainer;
    [SerializeField] GameObject _pauseMenu;
    public static System.Action<int, bool> OnMonkeyToggled;
    private void ToggleMonkey(int index, bool isActive)
    {
        if (index < 0 || index >= _monkeyContainer.childCount) return;
        var entry = _monkeyContainer.GetChild(index);
        entry.Find("Active").gameObject.SetActive(isActive);
        entry.Find("Inactive").gameObject.SetActive(!isActive);
    }

    private void OnEnable()
    {
        OnMonkeyToggled += ToggleMonkey;
    }
    private void OnDisable()
    {
        OnMonkeyToggled -= ToggleMonkey;
    }


    private void Update()
    {
        if (_inputManager.actions["Pause"].WasPressedThisFrame())
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        }
    }
}
