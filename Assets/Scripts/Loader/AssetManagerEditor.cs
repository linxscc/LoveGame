using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEditor;
using UnityEngine;

namespace game.main
{
    public partial class AssetManager
    {
        public void AddAssetBundleNameDic(string assetPath, string bundleName)
        {
#if UNITY_EDITOR && !USE_BUNDLE
            if (!_bundle2AssetDic.ContainsKey(bundleName))
                _bundle2AssetDic.Add(bundleName, assetPath);
#endif
        }
       
        
#if UNITY_EDITOR && !USE_BUNDLE
        //bundleName转assetPath的映射表
        private Dictionary<string, string> _bundle2AssetDic = new Dictionary<string, string>();

        private string[] textureExNames = new string[] { "png", "jpg" };
        private string[] spriteExNames = new string[] { "png", "jpg" };
        private string[] materialExNames = new string[] { "mat" };
        private string[] animtorExNames = new string[] { "controller" };
        private string[] textExNames = new string[] { "json", "bytes", "txt", "lua", "xml" };
        private string[] audioExNames = new string[] { "mp3", "wmv" };
        private string[] prefabExNames = new string[] { "prefab" };
        private string[] assetExNames = new string[] { "asset" };
        
        public string GetAssetPathByBundleName(string bundleName)
        {
            if (_bundle2AssetDic.ContainsKey(bundleName))
                return _bundle2AssetDic[bundleName];
            return null;
        }

        /// <summary>
        /// 加载的assetName转成Asset下路径
        /// </summary>
        private string GetAssetPathByAssetName<T>(string assetName) where T : Object
        {
            string assetPath = "";

            string[] exNames = null;
            if (typeof(T) == typeof(Texture))
            {
                exNames = textureExNames;
            }
            else if (typeof(T) == typeof(Sprite))
            {
                exNames = spriteExNames;
            }
            else if (typeof(T) == typeof(Material))
            {
                exNames = materialExNames;
            }
            else if (typeof(T) == typeof(RuntimeAnimatorController))
            {
                exNames = animtorExNames;
            }
            else if (typeof(T) == typeof(TextAsset))
            {
                exNames = textExNames;
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                int rootIndex = assetName.IndexOf(PathUtil.PlatfromBundlePath());
                if (rootIndex != -1)
                {
                    assetName = assetName.Remove(0, rootIndex);
                    assetName = assetName.Replace(PathUtil.PlatfromBundlePath(), "");
                }

                int exIndex = assetName.LastIndexOf(AudioExt);
                if (exIndex != -1)
                {
                    assetName = assetName.Substring(0, exIndex);
                }
                else
                {
                    exIndex = assetName.LastIndexOf(BytesExt);
                    if (exIndex != -1)
                    {
                        assetName = assetName.Substring(0, exIndex);
                    }
                }
                exNames = audioExNames;
            }
            else if (typeof(T) == typeof(ExternalBehaviorTree))
            {
                exNames = assetExNames;
            }
            else if (typeof(T) == typeof(GameObject))
            {
                exNames = prefabExNames;
            }
            else
            {
                exNames = assetExNames;
            }


            if (assetName.StartsWith("story/"))
            {
                assetPath = "Assets/BundleAssets/" + assetName;
            }
            else if (assetName.StartsWith("music/"))
            {
                assetName = "Audio" + assetName.Remove(0, "music".Length);
                assetPath = "Assets/BundleAssets/" + assetName;
            }
            else if (assetName.StartsWith("module/"))
            {
                assetName = "Module" + assetName.Remove(0, "module".Length);
                assetPath = "Assets/BundleAssets/" + assetName;
            }
            else if (assetName.StartsWith("Module/"))
            {
                assetPath = "Assets/BundleAssets/" + assetName;
            }
            else
            {
                if (assetName.Contains("Live2d/Animation"))
                {
                    string filepath = "";
                    string filename = "";
                    Framework.Utils.FileUtil.SpliteFilePathAndName(assetName, out filepath, out filename);
                    if (!filename.Contains("texture_"))
                    {
                        filename = filename.Replace("_", ".");
                    }
                    filepath = filepath.Replace("_", ".");
                    assetName = filepath + '/' + filename;
                }
                assetPath = "Assets/BundleAssets/SingleFile/" + assetName;
            }

            if (exNames != null && exNames.Length > 0)
            {
                string guid;
                for (int i = 0; i < exNames.Length; ++i)
                {
                    guid = AssetDatabase.AssetPathToGUID(assetPath + '.' + exNames[i]);
                    if (!string.IsNullOrEmpty(guid) && !guid.Equals("-1"))
                    {
                        assetPath += '.' + exNames[i];
                    }
                }
            }

            return assetPath;
        }
#else
        private string GetAssetPathByAssetName<T>(string assetName) where T : Object
        {
            return "";
        }
#endif
    }
}