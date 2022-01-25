using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FadeArrowFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class FadeArrowSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Material Combine = null;
        public Material Fade = null;
        public RenderTexture texture = null;
    }
    public FadeArrowSettings settings = new FadeArrowSettings();

    class FadeArrowPass : ScriptableRenderPass 
    {
        public Material combine, fade;
        public RenderTexture tex;
        string profilerTag;
        RenderTargetIdentifier cameraColorTexture;
        int tempID;
        RenderTargetIdentifier tempRT;
        public FadeArrowPass(string name) 
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
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            cameraColorTexture = renderingData.cameraData.renderer.cameraColorTarget;
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            combine.SetTexture("_OldTex", tex);
            cmd.Blit(cameraColorTexture, tempRT, combine, 1);
            cmd.Blit(tempRT, tex, fade);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
    FadeArrowPass pass;

    public override void Create()
    {
        pass = new FadeArrowPass(name);
        pass.combine = settings.Combine;
        pass.fade = settings.Fade;
        pass.tex = settings.texture;
        pass.renderPassEvent = settings.renderPassEvent;
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if(renderingData.cameraData.camera.CompareTag("ArrowCam"))
            renderer.EnqueuePass(pass);
    }
}
