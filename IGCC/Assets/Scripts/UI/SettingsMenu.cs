using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void CloseSettings()
    {
        GlobalCanvasManager.Instance.ToggleSettings(false);
    }
}
