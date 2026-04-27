using UnityEngine;

public class clearRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;

    void Start()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }
}
