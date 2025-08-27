using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Fog Post-Processing", typeof(UniversalRenderPipeline))]
public class FogPostProcess : VolumeComponent, IPostProcessComponent
{
    public FloatParameter _power = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public FloatParameter _distance = new FloatParameter(10.0f);
    public FloatParameter _density = new FloatParameter(10.0f);
    public ColorParameter _fogColour = new ColorParameter(Color.white);

    public bool IsActive()
    {
        return (_power.value > 0.0f) && active;
    }
    public bool IsTileCompatible() => true;
}
