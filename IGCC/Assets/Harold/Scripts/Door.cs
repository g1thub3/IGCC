using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Transform _door;

    [SerializeField]
    Vector3 _doorOriginalPos;

    [SerializeField]
    Vector3 _doorFinalPos;

    bool _doorOpened = false;

    private void Awake()
    {
        _doorOriginalPos = _door.localPosition;
    }

    public void openDoor()
    {
        if (!_doorOpened)
            StartCoroutine(openDoorCoroutine());

        _doorOpened = true;
    }

    public IEnumerator openDoorCoroutine()
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer<duration)
        {
            timer += Time.deltaTime;
            _door.transform.localPosition = Vector3.Lerp(_doorOriginalPos,_doorFinalPos,timer/duration);

            yield return null;
        }

        _door.transform.localPosition = _doorFinalPos;
    }
}
