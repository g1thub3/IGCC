using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform _camTransform;

    void Awake()
    {
        _camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = _camTransform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}