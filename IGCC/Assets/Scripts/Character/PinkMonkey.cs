using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PinkMonkey : Monkey
{
    [SerializeField] List<Material> _hiddenMaterials;
    private float _desiredVisibility;
    private readonly float _changeRate = 5.0f;
    private Volume _globalVolume;
    private UniversalAdditionalCameraData camData;
    protected new void Start()
    {
        base.Start();
        index = 2;
        _desiredVisibility = 0;
        foreach (Material mat in _hiddenMaterials)
        {
            mat.SetFloat("_Visibility", _desiredVisibility);
        }
        _globalVolume = FindAnyObjectByType<Volume>();
        _globalVolume.weight = 0;
        camData = Camera.main.GetUniversalAdditionalCameraData();
        for (int i = 0; i < camData.cameraStack.Count; i++)
        {
            camData.cameraStack[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (Material mat in _hiddenMaterials)
        {
            float vis = mat.GetFloat("_Visibility");
            if (vis == _desiredVisibility)
                continue;
            bool isLess = vis < _desiredVisibility;
            mat.SetFloat("_Visibility", Mathf.Clamp(vis + ((isLess ? _changeRate : -_changeRate) * Time.deltaTime), isLess ? vis : _desiredVisibility, isLess ? _desiredVisibility : vis));
        }

        float currWeight = _globalVolume.weight;
        bool isWeightLess = currWeight < _desiredVisibility;
        _globalVolume.weight = Mathf.Clamp(currWeight + ((isWeightLess ? _changeRate : -_changeRate) * Time.deltaTime), isWeightLess ? currWeight : _desiredVisibility, isWeightLess ? _desiredVisibility : currWeight);
    }

    public override void OnDeSwitch()
    {
        base.OnSwitch();
        _desiredVisibility = 0;
        for (int i = 0; i < camData.cameraStack.Count; i++)
        {
            camData.cameraStack[i].gameObject.SetActive(false);
        }
    }

    public override void OnSwitch()
    {
        base.OnSwitch();
        _desiredVisibility = 1;
        for (int i = 0; i < camData.cameraStack.Count; i++)
        {
            camData.cameraStack[i].gameObject.SetActive(true);
        }
    }
}
