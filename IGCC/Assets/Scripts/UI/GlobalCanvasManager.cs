using UnityEngine;

public class GlobalCanvasManager : SingletonMonobehaviour<GlobalCanvasManager>
{
    [SerializeField] SettingsMenu _settingsMenu;

    public void ToggleSettings(bool isOpen)
    {
        _settingsMenu.gameObject.SetActive(isOpen);
    }
}
