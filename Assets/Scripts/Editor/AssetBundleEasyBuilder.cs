using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Build AssetBundles/Build Bundles")]
    static void BuildAllAssetBundlesCompressed()
    {
        string assetBundleDirectory = "AssetBundles/StandaloneWindows";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);

        assetBundleDirectory = "AssetBundles/StandaloneOSX";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneOSX);

        assetBundleDirectory = "AssetBundles/StandaloneLinux";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneLinux64);
    }
}