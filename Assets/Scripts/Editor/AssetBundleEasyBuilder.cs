using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    static void BuildAssetBundlesCompressed(string folderName, BuildTarget target)
    {
        string assetBundleDirectory = $"AssetBundles/Standalone{folderName}";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, target);
    }
    static void BuildStandaloneWindows64()
    {
        BuildAssetBundlesCompressed("Windows", BuildTarget.StandaloneWindows64);
    }
    static void BuildStandaloneOSX()
    {
        BuildAssetBundlesCompressed("OSX", BuildTarget.StandaloneOSX);
    }
    static void BuildStandaloneLinux64()
    {
        BuildAssetBundlesCompressed("Linux", BuildTarget.StandaloneLinux64);
    }
    [MenuItem("Build AssetBundles/Build Bundles")]
    static void BuildAllAssetBundlesCompressed()
    {
        BuildStandaloneWindows64();
        BuildStandaloneOSX();
        BuildStandaloneLinux64();
    }
}