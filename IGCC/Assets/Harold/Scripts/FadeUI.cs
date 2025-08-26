using DG.Tweening;
using System;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    [Header("Fade Settings")]
    public float _fadeInDuration = 0.5f;
    public float _fadeOutDuration = 0.5f;

    private CanvasGroup _canvasGroup;
    private Tween _currentTween;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDisable()
    {
        _currentTween?.Kill();
    }

    private void OnEnable()
    {
        //Start fully transparent
        _canvasGroup.alpha = 0f;

        //Kill any existing tween
        _currentTween?.Kill();

        //Fade in
        _currentTween = _canvasGroup.DOFade(1f, _fadeInDuration)
            .SetUpdate(true); // SetUpdate(true) allows tween during timescale = 0
    }

    public void fadeOutAndDisable()
    {
        //Kill any existing tween
        _currentTween?.Kill();

        //Fade out, then disable
        _currentTween = _canvasGroup.DOFade(0f, _fadeOutDuration)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void fadeOutAndDisable(Action action)
    {
        //Kill any existing tween
        _currentTween?.Kill();

        //Fade out, then disable
        _currentTween = _canvasGroup.DOFade(0f, _fadeOutDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                action?.Invoke();
            });
    }
}