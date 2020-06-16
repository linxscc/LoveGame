using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

namespace AssetBundleTool
{
    public class AssetBundleHelper : Editor
    {
        private const string ModulePath = "Assets/BundleAssets/Module/";
        private const string UiSpriteAtlasPath = "Assets/BundleAssets/UISpriteAtlas/";
        private const string SingleFilePath = "Assets/BundleAssets/SingleFile/";
        private const string StoryPath = "Assets/BundleAssets/Story/";
        private const string AudioPath = "Assets/BundleAssets/Audio/";


        public static string CopyCoaxSleepAudioPath()
        {
            return GetOriginalBundlesPath() + "/AssetBundles/" + PackageManager.SystemTag + "/music/coaxsleepaudios";
        }

        public static string NeedDeleteCoaxSleepAudioPath()
        {
            return GetOuterStreamingAssetsPath() + "/AssetBundles/" +PackageManager.SystemTag + "/music/coaxsleepaudios";                   
        }

        public static string GetOutCoaxSleepAudioFolderPath()
        {
            return  Application.dataPath.Replace("Assets", "CoaxSleepAudioFolder");
        }
       
        
        public static string GetOuterStreamingAssetsPath()
        {
            return Application.dataPath.Replace("Assets", "StreamingAssets");
        }
        
        public static string GetOriginalBundlesPath()
        {
            return Application.dataPath.Replace("Assets", "OriginalBundles");
        }
        
//        [MenuItem("AssetBundle/Build_ALL_AssetBundles", false, 0)]
        static void BuildAssetBundleAndroid()
        {
            string outputDir = GetOriginalBundlesPath() + "/AssetBundles/Android";
            CheckAndCreateDir(outputDir);

            BuildTarget buildTarget = BuildTarget.iOS;
            if (EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.Android)
            {
                buildTarget = BuildTarget.Android;
            }
            
            var buildManifest = BuildPipeline.BuildAssetBundles(outputDir,
                BuildAssetBundleOptions.StrictMode | 
                BuildAssetBundleOptions.ChunkBasedCompression,
                buildTarget);
            
            Debug.Log("<color='#009999'>Android bundle数量："+buildManifest.GetAllAssetBundles().Length+"</color>");
            
            AssetDatabase.Refresh();
        }
        

        static void CheckAndCreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        
        
        
        
        /************************************生成BundleName****************************************************/
        
        
        
        [MenuItem("AssetBundle/生成BundleNames/GenerateAll", false, 11)]
        static void GenerateAll()
        {
            GenerateModule();
            GenerateUiSpriteAtlas();
            GenerateAudio();
            GenerateStory();
            GenerateSingleFile();
        }

        [MenuItem("AssetBundle/生成BundleNames/Module")]
        static void GenerateModule()
        {
            DirectoryInfo di = new DirectoryInfo(ModulePath);
            FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories)
                .Where(s => s.Name.IndexOf(".meta", StringComparison.Ordinal) == -1).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].FullName.Replace("\\", "/");
                string bundleName =
                    path.Substring(path.IndexOf(ModulePath, StringComparison.Ordinal) + ModulePath.Length);
                bundleName = "module/" + bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));
                GenerateSingleBundle(files[i].FullName, bundleName, "prefab");
                
                EditorUtility.DisplayProgressBar("AssetBundle", "create bundle names Module " + i +"/"+ files.Length, (float)i / files.Length);
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("AssetBundle/生成BundleNames/Audio")]
        static void GenerateAudio()
        {
            DirectoryInfo di = new DirectoryInfo(AudioPath);
            FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories)
                .Where(s => s.Name.IndexOf(".meta", StringComparison.Ordinal) == -1).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].FullName.Replace("\\", "/");
                string bundleName =
                    path.Substring(path.IndexOf(AudioPath, StringComparison.Ordinal) + AudioPath.Length);
                bundleName = "music/" + bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));
                GenerateSingleBundle(files[i].FullName, bundleName, "music");
                
                EditorUtility.DisplayProgressBar("AssetBundle", "create bundle names Audio " + i +"/"+ files.Length, (float)i / files.Length);
            }
            
            EditorUtility.ClearProgressBar();
        }


        [MenuItem("AssetBundle/生成BundleNames/SingleFile")]
        static void GenerateSingleFile()
        {
            DirectoryInfo di = new DirectoryInfo(SingleFilePath);
            FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories)
                .Where(s => s.Name.IndexOf(".meta", StringComparison.Ordinal) == -1).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].FullName.Replace("\\", "/");

                string bundleName =
                    path.Substring(path.IndexOf(SingleFilePath, StringComparison.Ordinal) + SingleFilePath.Length);

                if (bundleName.EndsWith(".json") == false)
                    bundleName = bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));

                if (path.Contains("Live2d/Animation"))
                {
                    bundleName = bundleName.Replace(".", "_");
                }

                GenerateSingleBundle(files[i].FullName, bundleName, "bytes");
                
                EditorUtility.DisplayProgressBar("AssetBundle", "create bundle names SingleFile " + i +"/"+ files.Length, (float)i / files.Length);
            }
            
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("AssetBundle/生成BundleNames/Story")]
        static void GenerateStory()
        {
            DirectoryInfo di = new DirectoryInfo(StoryPath);
            FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories)
                .Where(s => s.Name.IndexOf(".meta", StringComparison.Ordinal) == -1).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].FullName.Replace("\\", "/");
                string bundleName =
                    path.Substring(path.IndexOf(StoryPath, StringComparison.Ordinal) + StoryPath.Length);
                bundleName = bundleName.Substring(0, bundleName.LastIndexOf(".", StringComparison.Ordinal));
                bundleName = "story/" + bundleName;
                GenerateSingleBundle(files[i].FullName, bundleName, "bytes");
                
                EditorUtility.DisplayProgressBar("AssetBundle", "create bundle names Story "  + i +"/"+ files.Length, (float)i / files.Length);
            }
            
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("AssetBundle/生成BundleNames/UISpriteAtlas")]
        static void GenerateUiSpriteAtlas()
        {
            DirectoryInfo di = new DirectoryInfo(UiSpriteAtlasPath);
            FileInfo[] files = di.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                    continue;

                string bundleName =
                    files[i].Name.Substring(0, files[i].Name.LastIndexOf(".", StringComparison.Ordinal));
                SpriteAtlas sa = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(UiSpriteAtlasPath + files[i].Name);
                string variant = "atlas";

                if (sa.isVariant)
                {
                    variant = "atlas_small";
                    bundleName = bundleName.Replace("_Small", "");
                }

                GenerateSingleBundle(files[i].FullName, bundleName, variant);
                
                EditorUtility.DisplayProgressBar("AssetBundle", "create bundle names UiSpriteAtlas" + i +"/"+ files.Length, (float)i / files.Length);
            }
            
            EditorUtility.ClearProgressBar();
        }

        private static void GenerateSingleBundle(string path, string bundleName, string variant)
        {
            AssetImporter ai =
                AssetImporter.GetAtPath(path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)));

//            if(ai.assetBundleName == bundleName.ToLower() && ai.assetBundleVariant == variant.ToLower())
//                return;
            
            ai.SetAssetBundleNameAndVariant(bundleName, variant);
        }

        private static void GeneratePackageBundle(string path, string bundleName, string variant)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories)
                    .Where(s => s.Name.IndexOf(".meta", StringComparison.Ordinal) == -1).ToArray();

                for (int i = 0; i < files.Length; i++)
                {
                    AssetImporter ai = AssetImporter.GetAtPath(files[i].FullName
                        .Substring(files[i].FullName.IndexOf("Assets", StringComparison.Ordinal)));

                    ai.SetAssetBundleNameAndVariant(bundleName, variant);
                }

                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
        /// 工程中只要设置了AssetBundleName的，都会进行打包
        /// </summary>
        [MenuItem("AssetBundle/生成BundleNames/清理")]
        static void ClearAssetBundlesName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            string[] oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                EditorUtility.DisplayProgressBar("AssetBundle", "Read Asset BundlesName " + i + "/" + length,
                    (float) i / length);
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
            {
                EditorUtility.DisplayProgressBar("AssetBundle",
                    "ClearAssetBundlesName " + j + "/" + oldAssetBundleNames.Length,
                    (float) j / oldAssetBundleNames.Length);

                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
            }
            
//            AssetDatabase

            EditorUtility.ClearProgressBar();
        }
    }
}