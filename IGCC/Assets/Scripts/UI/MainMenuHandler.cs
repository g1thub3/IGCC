using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void OpenSettings()
    {
        GlobalCanvasManager.Instance.ToggleSettings(true);
    }
}
