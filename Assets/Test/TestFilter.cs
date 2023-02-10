using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class TestFilter : MonoBehaviour
{
    public Material EffectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }

    
}