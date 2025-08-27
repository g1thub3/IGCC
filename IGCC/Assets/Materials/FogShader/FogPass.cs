using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class FogPass : ScriptableRenderPass
{
    Material _mat;
    int fogId = Shader.PropertyToID("_Temp");
    RenderTargetIdentifier src, bw;
    public FogPass()
    {
        if (!_mat)
        {
            _mat = CoreUtils.CreateEngineMaterial("Custom Post-Processing/Fog");
        }
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        src = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.GetTemporaryRT(fogId, desc, FilterMode.Bilinear);
        bw = new RenderTargetIdentifier(fogId);
    }
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer commandBuffer = CommandBufferPool.Get("Custom/Fog Post-Processing");
        VolumeStack volumes = VolumeManager.instance.stack;
        FogPostProcess fogPP = volumes.GetComponent<FogPostProcess>();

        if (fogPP.IsActive())
        {
            _mat.SetFloat("_power", (float)fogPP._power.value);
            _mat.SetFloat("_distance", (float)fogPP._distance.value);
            _mat.SetFloat("_density", (float)fogPP._density.value);
            _mat.SetColor("_FogColour", fogPP._fogColour.value);
            commandBuffer.Blit(src, bw, _mat, 0);
            commandBuffer.Blit(bw, src);
        }

        context.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
        CommandBufferPool.Release(commandBuffer);
    }
    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        cmd.ReleaseTemporaryRT(fogId);
    }
}
