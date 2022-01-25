using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CombineOutputFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CombineOutputSettings
    {
        public RenderPassEvent RenderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Material CombineAT = null;
        public Material CombineAM = null;
        public Material Blur = null;
        public Material Outline = null;
        public RenderTexture Arrow = null;
        public RenderTexture Trail = null;
        public RenderTexture Temp1 = null;
        public RenderTexture Temp2 = null;
    }
    public CombineOutputSettings settings = new CombineOutputSettings();

    class CombineOutputPass : ScriptableRenderPass 
    {
        public Material combineat, combineam, blur, outline;
        public RenderTexture arrow, trail, temp1, temp2;
        string profilerTag;
        RenderTargetIdentifier cameraColorTexture;
        int tempID;
        RenderTargetIdentifier tempRT;
        int tempID2;
        RenderTargetIdentifier tempRT2;
        public CombineOutputPass(string name) 
        {
            this.profilerTag = name;
        }
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            var width = cameraTextureDescriptor.width;
            var height = cameraTextureDescriptor.height;
            tempID = Shader.PropertyToID("tempRT");
            cmd.GetTemporaryRT(tempID, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            tempRT = new RenderTargetIdentifier(tempID);
            ConfigureTarget(tempRT);
            tempID2 = Shader.PropertyToID("tempRT2");
            cmd.GetTemporaryRT(tempID2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            tempRT2 = new RenderTargetIdentifier(tempID2);
            ConfigureTarget(tempRT2);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            cameraColorTexture = renderingData.cameraData.renderer.cameraColorTarget;
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            cmd.Blit(arrow, tempRT, outline, 0);
            cmd.Blit(tempRT, tempRT2, outline, 1);
            cmd.Blit(tempRT2, temp2, outline, 2);
            cmd.Blit(trail, tempRT, blur, 0);
            cmd.Blit(tempRT, temp1, blur, 1);
            cmd.Blit(temp1, tempRT, combineat, 0);
            cmd.Blit(tempRT, temp1);
            cmd.Blit(cameraColorTexture, tempRT, combineam, 2);
            cmd.Blit(tempRT, cameraColorTexture);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
    CombineOutputPass pass;

    public override void Create()
    {
        pass = new CombineOutputPass(name);
        pass.combineat = settings.CombineAT;
        pass.combineam = settings.CombineAM;
        pass.blur = settings.Blur;
        pass.outline = settings.Outline;
        pass.arrow = settings.Arrow;
        pass.trail = settings.Trail;
        pass.temp1 = settings.Temp1;
        pass.temp2 = settings.Temp2;
        pass.renderPassEvent = settings.RenderPassEvent;
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(renderingData.cameraData.camera.CompareTag("MainCamera"))
            renderer.EnqueuePass(pass);
    }
}
