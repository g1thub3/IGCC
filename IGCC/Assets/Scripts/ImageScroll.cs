using UnityEngine;

public class ImageScroll : MonoBehaviour
{
    [SerializeField] float _loopLength = 3.0f;
    private Vector2 _finalPosition;
    private Transition trans;
    private RectTransform _rectTrans;
    void Start()
    {
        trans = new Transition(_loopLength);
        _rectTrans = GetComponent<RectTransform>();
        _finalPosition = _rectTrans.sizeDelta * 0.5f * -1;
    }

    // Update is called once per frame
    void Update()
    {
        trans.Progress();
        if (trans.Progression >= 1.0f)
            trans.t = 0;
        _rectTrans.anchoredPosition = Vector2.Lerp(Vector2.zero, _finalPosition, trans.Progression);
    }
}
