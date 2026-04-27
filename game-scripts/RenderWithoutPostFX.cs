using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[System.Serializable]
public class RenderWithoutPostFX : CustomPass
{
    public LayerMask layerMask;

    protected override void Execute(CustomPassContext ctx)
    {
        var sortingSettings = new SortingSettings(ctx.hdCamera.camera);
        var filteringSettings = new FilteringSettings(RenderQueueRange.all, layerMask);

        ShaderTagId[] shaderTags = new ShaderTagId[]
        {
            new ShaderTagId("SRPDefaultUnlit"),
            new ShaderTagId("Forward")
        };

        foreach (var shaderTag in shaderTags)
        {
            var drawingSettings = new DrawingSettings(shaderTag, sortingSettings);
            drawingSettings.perObjectData = PerObjectData.None;
            ctx.renderContext.DrawRenderers(ctx.cullingResults, ref drawingSettings, ref filteringSettings);
        }
    }
}
