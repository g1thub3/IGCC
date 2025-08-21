using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadingTransition : MonoBehaviour
{
    [SerializeField]
    Image _image;

    CanvasGroup _group;

    static FadingTransition _instance;
    public static FadingTransition Instance => _instance;

    public event System.Action OnFadeFinishEvent;


    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();

        if (!_instance)
            _instance = this;
        else
        {
            Destroy(gameObject);
            Debug.Log("more than one fading transition in scene");
        }

        onSceneOpen();
    }

    public void onSceneOpen()
    {
        _group.alpha = 1.0f;
        fadeIn(4f);
    }

    public void fadeIn(float duration = 0.5f)
    {
        _group.DOFade(0, duration).onComplete += () => { onFadeComplete(); };
        //StartCoroutine(fadeCanvas(duration, _group.alpha, 0));
    }

    public void fadeOut(float duration = 0.5f)
    {
        _group.DOFade(1, duration).onComplete += () => { onFadeComplete(); };
        //StartCoroutine(fadeCanvas(duration, _group.alpha, 1));
    }

    void onFadeComplete()
    {
        OnFadeFinishEvent?.Invoke();
    }

    private void OnDisable()
    {
        _group.DOKill();
        transform.DOKill();
    }



    public void RunAfterFadeOut(System.Action callback)
    {
        System.Action handler = null;

        handler = () =>
        {
            OnFadeFinishEvent -= handler; // Unsubscribe after it's done!
            callback?.Invoke();
            DOVirtual.DelayedCall(0.5f, () => fadeIn());
        };

        OnFadeFinishEvent += handler;

        _image.color = Color.black;
        fadeOut();
    }

    public void FadeOutAndRun(System.Action callback, Color color)
    {
        System.Action handler = null;

        handler = () =>
        {
            OnFadeFinishEvent -= handler; // Unsubscribe after it's done!
            callback?.Invoke();
        };

        _image.color = color;

        OnFadeFinishEvent += handler;
        fadeOut();
    }
}
