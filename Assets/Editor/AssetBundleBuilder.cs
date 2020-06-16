using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Debug = UnityEngine.Debug;

public class AssetBundleBuilder
{
    private const string BundleRoot = "Assets/BundleAssets/";
    private const string I18NBundleRoot = "Assets/BundleI18N/";

    private const string ModulePath = "Assets/BundleAssets/Module/";
    private const string UiSpriteAtlasPath = "Assets/BundleAssets/UISpriteAtlas/";
    private const string SingleFilePath = "Assets/BundleAssets/SingleFile/";
    private const string StoryPath = "Assets/BundleAssets/Story/";
    private const string AudioPath = "Assets/BundleAssets/Audio/";
    private const string FontPath = "Assets/BundleAssets/SingleFile/Fonts";
    
    public const string MenuItem_CreateAllBundle = "AssetBundle/生成所有Bundles";


    public static string GetI18NBundlesPath()
    {
        return I18NBundleRoot + PackageManager.Language;
    }
    public static string GetI18NBundleOutputPath()
    {
        return GetOriginalBundlesPath() + "/" + PackageManager.Language + "/AssetBundles/" + PackageManager.SystemTag;
    }
    
    public static string GetOriginalBundlesPath()
    {
        return Application.dataPath.Replace("Assets", "OriginalBundles");
    }

    public static string GetBundleOutputPath()
    {
        return GetOriginalBundlesPath() + "/AssetBundles/" + PackageManager.SystemTag;
    }


    [MenuItem(MenuItem_CreateAllBundle, false, 12)]
    static void CreateAllBundle()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        List<AssetBundleBuild> bundleList = new List<AssetBundleBuild>();

        string[] files = Directory.GetFiles(BundleRoot, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                continue;

            string filePath = file.Replace("\\", "/");

            AssetBundleBuild bundle = new AssetBundleBuild();
            if (filePath.StartsWith(SingleFilePath))
            {
                bundle = CreateSingleFile(filePath);
            }
            else if (filePath.StartsWith(ModulePath))
            {
                bundle = CreateModule(filePath);
            }
            else if (filePath.StartsWith(AudioPath))
            {
                bundle = CreateAudio(filePath);
            }
            else if (filePath.StartsWith(UiSpriteAtlasPath))
            {
                if (filePath.EndsWith(".spriteatlas") == false)
                    continue;
                bundle = CreateUiAtlas(filePath);
            }
            else if (filePath.StartsWith(StoryPath))
            {
                bundle = CreateStory(filePath);
            }

            if (string.IsNullOrEmpty(bundle.assetBundleName))
                continue;

            bundleList.Add(bundle);
        }

        CheckAndCreateDir(GetBundleOutputPath());

        AssetBundleManifest manifest =
            BuildPipeline.BuildAssetBundles(GetBundleOutputPath(), bundleList.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression | 
                BuildAssetBundleOptions.DeterministicAssetBundle,
                EditorUserBuildSettings.activeBuildTarget);
        
        Debug.Log("<color='#00ff66'>Bundle数量：" + bundleList.Count + " ====== 耗时： " +
                  (float) stopwatch.ElapsedMilliseconds / 1000 + "</color>");
    }

//    [MenuItem("AssetBundle/生成 I18N Bundles")]
    static void CreateI18NBundle()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<AssetBundleBuild> bundleList = new List<AssetBundleBuild>();
        
        string path = GetI18NBundlesPath() + "/UISpriteAtlas";
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        
        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");

            if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                continue;
            if (filePath.EndsWith(".spriteatlas") == false)
                continue;

            AssetBundleBuild bundle = CreateUiAtlas(filePath);
            bundleList.Add(bundle);
        }
        
        CheckAndCreateDir(GetI18NBundleOutputPath());

        AssetBundleManifest manifest =
            BuildPipeline.BuildAssetBundles(GetI18NBundleOutputPath(), bundleList.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

        Debug.Log("<color='#00ff66'>Bundle数量：" + bundleList.Count + " ====== 耗时： " +
                  (float) stopwatch.ElapsedMilliseconds / 1000 + "</color>");
    }

//    [MenuItem("AssetBundle/生成 Module Bundles")]
    static void CreateMoudleBundle()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        List<AssetBundleBuild> bundleList = new List<AssetBundleBuild>();

        string[] files = Directory.GetFiles(UiSpriteAtlasPath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");

            if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                continue;
            if (filePath.EndsWith(".spriteatlas") == false)
                continue;

            AssetBundleBuild bundle = CreateUiAtlas(filePath);
            bundleList.Add(bundle);
        }

        files = Directory.GetFiles(FontPath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");

            if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                continue;

            AssetBundleBuild bundle = CreateSingleFile(filePath);
            bundleList.Add(bundle);
        }

        files = Directory.GetFiles(ModulePath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                continue;
            AssetBundleBuild bundle = CreateModule(filePath);
            bundleList.Add(bundle);
        }

        CheckAndCreateDir(GetBundleOutputPath());

        AssetBundleManifest manifest =
            BuildPipeline.BuildAssetBundles(GetBundleOutputPath(), bundleList.ToArray(),
                BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        Debug.Log("<color='#00ff66'>Bundle数量：" + bundleList.Count + " ====== 耗时： " +
                  (float) stopwatch.ElapsedMilliseconds / 1000 + "</color>");
    }

    static void CheckAndCreateDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    private static AssetBundleBuild CreateStory(string file)
    {
        string path = file.Replace("\\", "/");
        string bundleName =
            path.Substring(path.IndexOf(StoryPath, StringComparison.Ordinal) + StoryPath.Length);
        bundleName = bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));
        bundleName = "story/" + bundleName;
        
        return CreateBundle(bundleName, path, "bytes");
    }

    private static AssetBundleBuild CreateUiAtlas(string file)
    {
        string path = file.Replace("\\", "/");
        string bundleName =
            path.Substring(0, path.LastIndexOf(".", StringComparison.Ordinal));
        bundleName = bundleName.Replace(UiSpriteAtlasPath, "");

        SpriteAtlas sa = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
        string variant = "atlas";

        return CreateBundle(bundleName, path, variant);
    }

    private static AssetBundleBuild CreateAudio(string file)
    {
        string path = file.Replace("\\", "/");
        string bundleName =
            path.Substring(path.IndexOf(AudioPath, StringComparison.Ordinal) + AudioPath.Length);
        bundleName = "music/" + bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));

        return CreateBundle(bundleName, path, "music");
    }

    private static AssetBundleBuild CreateModule(string file)
    {
        string path = file.Replace("\\", "/");
        string bundleName =
            path.Substring(path.IndexOf(ModulePath, StringComparison.Ordinal) + ModulePath.Length);
        bundleName = "module/" + bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));

        return CreateBundle(bundleName, path, "prefab");
    }

    private static AssetBundleBuild CreateBundle(string name, string path, string variant)
    {
        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetBundleName = name;
        assetBundleBuild.assetBundleVariant = variant;
        assetBundleBuild.assetNames = new string[] {path};
        return assetBundleBuild;
    }

    static AssetBundleBuild CreateSingleFile(string file)
    {
        string path = file.Replace("\\", "/");

        string bundleName =
            path.Substring(path.IndexOf(SingleFilePath, StringComparison.Ordinal) + SingleFilePath.Length);

        if (bundleName.EndsWith(".json") == false)
            bundleName = bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));

        if (path.Contains("Live2d/Animation"))
        {
            bundleName = bundleName.Replace(".", "_");
        }

        return CreateBundle(bundleName, path, "bytes");
    }
    
    //    [MenuItem("Assets/===CreateBundle===", false, 10)]
    static void CreateSelectedBundle()
    {
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        string path = AssetDatabase.GetAssetOrScenePath(arr[0]);

        FileInfo fileInfo = new FileInfo(path);

        List<AssetBundleBuild> bundleList = new List<AssetBundleBuild>();

        int count = 0;

        if (fileInfo.Attributes == FileAttributes.Directory)
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                if (file.LastIndexOf(".meta", StringComparison.Ordinal) != -1)
                    continue;

                string filePath = file.Replace("\\", "/");

                AssetBundleBuild bundle = new AssetBundleBuild();
                if (filePath.StartsWith(SingleFilePath))
                {
                    bundle = CreateSingleFile(filePath);
                }
                else if (filePath.StartsWith(ModulePath))
                {
                    bundle = CreateModule(filePath);
                }
                else if (filePath.StartsWith(AudioPath))
                {
                    bundle = CreateAudio(filePath);
                }
                else if (filePath.StartsWith(UiSpriteAtlasPath))
                {
                    if (filePath.EndsWith(".spriteatlas") == false)
                        continue;
                    bundle = CreateUiAtlas(filePath);
                }
                else if (filePath.StartsWith(StoryPath))
                {
                    bundle = CreateStory(filePath);
                }

                if (string.IsNullOrEmpty(bundle.assetBundleName))
                    continue;

                bundleList.Add(bundle);
            }
        }

        CheckAndCreateDir(GetBundleOutputPath());


//        AssetBundleManifest manifest =
//            BuildPipeline.BuildAssetBundles(GetBundleOutputPath(), bundleList.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }
}