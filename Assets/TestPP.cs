using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TestPP : MonoBehaviour
{
    // Start is called before the first frame update
    public PostProcessResources pp;
    void Start()
    {
        PostProcessLayer postProcessLayer = gameObject.AddComponent<PostProcessLayer>();
        postProcessLayer.volumeTrigger = gameObject.transform;
        postProcessLayer.volumeLayer = -1; // -2147483648
        PostProcessResources pprNew = ScriptableObject.CreateInstance<PostProcessResources>();
        pprNew.shaders = new PostProcessResources.Shaders
        {
            bloom = Shader.Find("Hidden/PostProcessing/Bloom"),
            multiScaleAO = Shader.Find("Hidden/PostProcessing/MultiScaleVO"),
            copy = Shader.Find("Hidden/PostProcessing/Copy"),
            copyStd = Shader.Find("Hidden/PostProcessing/CopyStd"),
            copyStdFromDoubleWide = Shader.Find("Hidden/PostProcessing/CopyStdFromDoubleWide"),
            copyStdFromTexArray = Shader.Find("Hidden/PostProcessing/CopyStdFromTexArray"),
            deferredFog = Shader.Find("Hidden/PostProcessing/DeferredFog"),
            depthOfField = Shader.Find("Hidden/PostProcessing/DepthOfField"),
            discardAlpha = Shader.Find("Hidden/PostProcessing/DiscardAlpha"),
            finalPass = Shader.Find("Hidden/PostProcessing/FinalPass"),
            grainBaker = Shader.Find("Hidden/PostProcessing/GrainBaker"),
            motionBlur = Shader.Find("Hidden/PostProcessing/MotionBlur"),
            lut2DBaker = Shader.Find("Hidden/PostProcessing/Lut2DBaker"),
            scalableAO = Shader.Find("Hidden/PostProcessing/ScalableAO"),
            screenSpaceReflections = Shader.Find("Hidden/PostProcessing/ScreenSpaceReflections"),
            subpixelMorphologicalAntialiasing = Shader.Find("Hidden/PostProcessing/SubpixelMorphologicalAntialiasing"),
            temporalAntialiasing = Shader.Find("Hidden/PostProcessing/TemporalAntialiasing"),
            texture2dLerp = Shader.Find("Hidden/PostProcessing/Texture2DLerp"),
            uber = Shader.Find("Hidden/PostProcessing/Uber"),
        };
        pprNew.computeShaders = new PostProcessResources.ComputeShaders
        {
            multiScaleAODownsample1 = pp.computeShaders.multiScaleAODownsample1,
            multiScaleAODownsample2 = pp.computeShaders.multiScaleAODownsample2,
            multiScaleAORender = pp.computeShaders.multiScaleAORender,
            multiScaleAOUpsample = pp.computeShaders.multiScaleAOUpsample,
        };

        pprNew.blueNoise64 = pp.blueNoise64;
        pprNew.blueNoise256 = pp.blueNoise256;

        postProcessLayer.Init(pprNew);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
