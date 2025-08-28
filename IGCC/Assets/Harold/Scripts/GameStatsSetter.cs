using TMPro;
using UnityEngine;

public class GameStatsSetter : MonoBehaviour
{
    [SerializeField]
    TMP_Text _bananaCount;

    [SerializeField]
    TMP_Text _timer;

    private void Awake()
    {
        _bananaCount.text = PlayerPrefs.GetFloat("BananaCount").ToString();

        float time = PlayerPrefs.GetFloat("Timer");

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        _timer.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM("BGM_BicycleRide");
    }
}
