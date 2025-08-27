using DG.Tweening;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField]
    private float _bobDistance = 0.2f;

    int _speed = 1;

    float _originalYPos;

    private void Awake()
    {
        _originalYPos = transform.position.y;
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    // Update is called once per frame
    void OnEnable()
    {
        bob();
    }

    public void bob()
    {
        _speed *= -1;
        transform.DOMoveY(_originalYPos + _bobDistance * _speed, 1f).onComplete += () => bob();
    }
}
