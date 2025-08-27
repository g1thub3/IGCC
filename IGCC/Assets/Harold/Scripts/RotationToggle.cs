using UnityEngine;
using System.Collections;

public class RotationToggle : MonoBehaviour
{
    [SerializeField]
    Transform _bridge;

    [SerializeField]
    Vector3 _originalRot;

    [SerializeField]
    Vector3 _finalRot;

    bool _doorOpened = false;

    private void Awake()
    {
        _originalRot = _bridge.localEulerAngles;
    }

    //Toggle it
    public void lowerBridge()
    {
        if (!_doorOpened)
            StartCoroutine(lowerBridgeCoroutine());

        _doorOpened = true;
    }

    public IEnumerator lowerBridgeCoroutine()
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            _bridge.transform.localRotation = Quaternion.Euler(Vector3.Lerp(_originalRot, _finalRot, timer / duration));

            yield return null;
        }

        _bridge.transform.localRotation = Quaternion.Euler(_finalRot);
    }
}
