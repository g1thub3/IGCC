using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] PlayerInput _inputManager;
    [SerializeField] Transform _monkeyContainer;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Button _quitBtn;
    public static System.Action<int, bool> OnMonkeyToggled;
    private void ToggleMonkey(int index, bool isActive)
    {
        if (index < 0 || index >= _monkeyContainer.childCount) return;
        var entry = _monkeyContainer.GetChild(index);
        entry.Find("Active").gameObject.SetActive(isActive);
        entry.Find("Inactive").gameObject.SetActive(!isActive);
    }

    private void Start()
    {
        var sceneHandler = FindAnyObjectByType<SceneSwitcher>();
        if (sceneHandler != null)
            _quitBtn.onClick.AddListener(delegate
            {
                sceneHandler.goToScene("MainMenuScene");
            });
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
