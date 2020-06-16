using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.U2D;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace game.main
{
    public partial class AssetManager : MonoBehaviour
    {
        private const string ImagePrefix = "assets/bundleassets/image/";
        private const string ModulePrefix = "assets/bundleassets/module/";

        private const string AtlasExt = ".atlas";
        private const string PrefabExt = ".prefab";
        private const string BytesExt = ".bytes";
        private const string AudioExt = ".music";

        private static AssetManager _instance;

        private Dictionary<string, BundleItem> _prefabDict;

        private Dictionary<string, AssetItem> _loadedAssets;

        private Dictionary<string, AssetBundle> _cachedBundles;

        private string _platformBundlePath;
        public string PlatformBundlePath => _platformBundlePath;
        
        private Dictionary<string, Dictionary<string, bool>> _singleFileDict = new Dictionary<string, Dictionary<string, bool>>();

       

        /// <summary>
        /// 标记单个文件bundle的作用域
        /// </summary>
        /// <param name="bundleName">bundle加载地址</param>
        /// <param name="domainName">作用域名称，自定义名称注意不要和模块名相同</param>
        public void MarkSingleFileBundle(string bundleName, string domainName)
        {
            if (_singleFileDict.ContainsKey(domainName))
            {
                Dictionary<string,bool> dict = _singleFileDict[domainName];
                if (dict.ContainsKey(bundleName) == false)
                {
                    dict.Add(bundleName, true);
                }
            }
            else
            {
                Dictionary<string,bool> dict = new Dictionary<string, bool>();
                dict.Add(bundleName, true);
                _singleFileDict.Add(domainName, dict);
            }
        }

        public void UnloadSingleFileBundle(string domain)
        {
            if (_singleFileDict.ContainsKey(domain) == false)
                return;

            foreach (var bundle in _singleFileDict[domain])
            {
                UnloadBundle(bundle.Key);

            }
            _singleFileDict.Remove(domain);
        }

        

        /// <summary>
        /// 回主界面的时候清理依赖Bundle
        /// </summary>
        public void UnloadSingleFileBundleLater()
        {
            foreach (var bundle in _laterUnloadDict)
            {
                UnloadCacheBundle(bundle.Key);
            }
            
            _laterUnloadDict.Clear();
        }
        
        /// <summary>
        /// 常驻内存
        /// </summary>
        private List<string> _residentBundleList;

        private AssetBundleManifest _manifest;
        private Dictionary<string, bool> _laterUnloadDict;
        
        private AssetBundle manifestBundle;

        public static AssetManager Instance
        {
            get { return _instance; }
        }

        void Awake()
        {
            _instance = this;

            _prefabDict = new Dictionary<string, BundleItem>();
            _loadedAssets = new Dictionary<string, AssetItem>();
            _cachedBundles = new Dictionary<string, AssetBundle>();

            //配置常驻内存bundle
            _residentBundleList = new List<string>();
            _residentBundleList.Add("fonts/huawenyuanti.bytes");
            _residentBundleList.Add("fonts/lantinghei.bytes");
            _residentBundleList.Add("fonts/lantingtehei.bytes");
            _residentBundleList.Add("fonts/fzltzhjw.bytes");
            _residentBundleList.Add("head/playerhead/playerhead.bytes");
            _laterUnloadDict = new Dictionary<string, bool>();
            
            SpriteAtlasManager.atlasRequested += OnLoadSpriteAtlas;

#if UNITY_IOS
            _platformBundlePath = "AssetBundles/iOS/";
#else
            _platformBundlePath = "AssetBundles/Android/";
#endif
            
            LoadingProgress.Instance.SetBg();
        }

        private void OnLoadSpriteAtlas(string atlasTag, Action<SpriteAtlas> action)
        {
            SpriteAtlas atlas = LoadAtlas(atlasTag);
            action(atlas);
            Debug.Log("<color='#00CAFF'>OnLoadSpriteAtlas===>" + atlasTag + "</color>");
        }

        public static void Initialize()
        {
            GameObject go = new GameObject("AssetManager");
            go.AddComponent<AssetManager>();
            
#if USE_BUNDLE
             _instance.InitManifest();
#endif
        }

        public void InitManifest()
        {
#if UNITY_IOS
            string path = _platformBundlePath + "iOS";
#else
            string path = _platformBundlePath + "Android";
#endif
            Stopwatch sw = Stopwatch.StartNew();
            
            if(manifestBundle != null)
                manifestBundle.Unload(true);
            
            _prefabDict = new Dictionary<string, BundleItem>();
            
            string manifestPath = PathUtil.GetPath(path, AssetLoader.ExternalHotfixPath);
            manifestBundle = AssetBundle.LoadFromFile(manifestPath);

            _manifest = manifestBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");

            string[] bundles = _manifest.GetAllAssetBundles();

            foreach (var bundleName in bundles)
            {
                if (bundleName.IndexOf(PrefabExt, StringComparison.Ordinal) != -1)
                {
                    BundleItem item = new BundleItem();
                    item.Name = bundleName;
                    item.Type = BundleType.Module;
                    item.Dependencies = _manifest.GetAllDependencies(bundleName);
                    _prefabDict.Add(bundleName, item);
                }
            }

            Debug.LogWarning("manifestBundle======>处理时间：" + sw.ElapsedMilliseconds);
            
            LoadFont();
        }

        private void LoadFont()
        {
            LoadBundle(_residentBundleList[0]);
            LoadBundle(_residentBundleList[1]);
        }

        public void LoadModuleAtlas(string moduleName)
        {
            //自动加载模块对应图集，其他图集需要手动调用LoadAtlas
            LoadAtlas("UIAtlas_" + moduleName);
        }

        public void LoadModuleAtlasAsync(string moduleName,Action Finish)
        {
            Action<SpriteAtlas> FinishCallback = (atlas) => { Finish(); };
            //自动加载模块对应图集，其他图集需要手动调用LoadAtlas
            LoadAtlasAsync("UIAtlas_" + moduleName, FinishCallback);
        }

        private void LoadDependencies(string[] dependencies)
        {
            for (int i = 0; i < dependencies.Length; i++)
            {
                LoadBundle(dependencies[i]);
            }
        }

        private string GetModuleAssetShortName(string assetPath)
        {
            string shortName = assetPath.Replace(ModulePrefix, "");
            shortName = "module/" + shortName.Substring(0, shortName.LastIndexOf(".", StringComparison.Ordinal));
            return shortName;
        }

        public void UnloadModuleBundles(string moduleName, bool autoUnloadAtlas)
        {
            if (autoUnloadAtlas)
                UnloadModuleAtlas(moduleName);

            string prefix = "module/" + moduleName.ToLower()+"/";
            foreach (var prefab in _prefabDict)
            {
                if (prefab.Key.StartsWith(prefix))
                {
                    UnloadCacheBundle(prefab.Key);
                    string[] dependencies = prefab.Value.Dependencies;
                    if (dependencies != null)
                    {
                        foreach (var dep in dependencies)
                        {
                            //主界面资源不卸载
                            if (dep.StartsWith("gamemain/") || dep.StartsWith("module/gamemain/"))
                                continue;
                            
                            if (dep.StartsWith("module/"))
                            {
                                UnloadCacheBundle(dep);
                            }
                            else
                            {
                                AddToLaterUnload(dep);
                            }
                        }
                    }
                    
                }
            }
        }

        public void AddToLaterUnload(string bundleName)
        {
           if(_laterUnloadDict.ContainsKey(bundleName) || _residentBundleList.IndexOf(bundleName) != -1)
               return;

            _laterUnloadDict.Add(bundleName, true);
        }

        /// <summary>
        /// 卸载模块默认图集
        /// </summary>
        /// <param name="assetName"></param>
        public void UnloadModuleAtlas(string assetName)
        {
            string bundleName = ("UIAtlas_" + assetName).ToLower();
            UnloadAtlas(bundleName);
        }

        /// <summary>
        /// 卸载图集
        /// </summary>
        /// <param name="assetName"></param>
        public void UnloadAtlas(string assetName)
        {
            string bundleName = assetName.ToLower() + AtlasExt;
            BundleWrapper bundle = LoadBundle(bundleName);
            bundle.Unload(true);

            _cachedBundles.Remove(bundleName);

            Debug.Log("<color='#ff5566'>UnloadAtlas " + bundleName + "</color>");
        }

        private void UnloadCacheBundle(string bundleName)
        {
            if (_cachedBundles.ContainsKey(bundleName) == false || _residentBundleList.IndexOf(bundleName) != -1)
                return;

            AssetBundle bundle = _cachedBundles[bundleName];
            if (bundle==null)
            {
                Debug.LogError("the prefab is using error singleFile:"+bundleName);
                return;
            }
            string[] names = bundle.GetAllAssetNames();
            foreach (var assetPath in names)
            {
                string shortName = GetModuleAssetShortName(assetPath);
                _loadedAssets.Remove(shortName);
            }

            bundle.Unload(true);
            _cachedBundles.Remove(bundleName);

            Debug.Log("<color='#00ffff'>UnloadModuleBundles " + bundleName + "</color><color='#00ff88'>" +
                      "  Loaded Module asset count:" + names.Length + "</color>");
        }

        public T GetAsset<T>(string assetName) where T : UnityEngine.Object
        {
            string asset_Path = GetAssetPathByAssetName<T>(assetName);
            assetName = assetName.ToLower();

            UnityEngine.Object obj = null;

            bool useBundle = false;
#if USE_BUNDLE
            useBundle = true;
#endif
   
            if (_loadedAssets.ContainsKey(assetName) == false)
            {
                string assetKey = assetName + PrefabExt;
                AddAssetBundleNameDic(asset_Path, assetKey);
                if (_prefabDict.ContainsKey(assetKey) == false && useBundle)
                {
//                    Debug.LogError("No Assets Found -> " + assetName);
                }
                else
                {
                    BundleWrapper bundle = LoadBundle(assetKey);

#if UNITY_EDITOR && !USE_BUNDLE
                    string[] dps = AssetDatabase.GetAssetBundleDependencies(assetKey, true);
                    LoadDependencies(dps);
                    return (T) bundle.GetResource(); 
#endif
                    string[] allAssetNames = bundle.GetAllAssetNames();
                    foreach (var assetPath in allAssetNames)
                    {
                        AssetItem aItem = new AssetItem()
                        {
                            AssetBundle = bundle.GetBundle(),
                            AssetName = assetName,
                            AssetPath = assetPath
                        };
                        _loadedAssets.Add(assetName, aItem);
                    }

                    string[] dependencies = _manifest.GetAllDependencies(assetKey);
                    LoadDependencies(dependencies);


                    AssetItem item = _loadedAssets[assetName];
                    obj = item.AssetBundle.LoadAsset<T>(item.AssetPath);
                }
            }
            else
            {
                string assetKey = assetName + PrefabExt;
                string[] dependencies = _manifest.GetAllDependencies(assetKey);
                LoadDependencies(dependencies);
                AssetItem item = _loadedAssets[assetName];
                obj = item.AssetBundle.LoadAsset<T>(item.AssetPath);
            }

            return (T) obj;
        }

        /// <summary>
        /// 获取图集中的sprite
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public Sprite GetSpriteAtlas(string assetName)
        {
            string[] moduleNameParts = assetName.Split('_');
            string moduleFolderName = moduleNameParts[0] + "_" + moduleNameParts[1];

#if UNITY_EDITOR && !USE_BUNDLE
            string assetPath = "Assets/BundleAssets/UISpriteAtlas/" + moduleFolderName + ".spriteatlas";
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            if (atlas == null) return null;
            Sprite resource = atlas.GetSprite(assetName);
            return resource;
#else
            BundleWrapper bundle = LoadBundle(moduleFolderName.ToLower() + AtlasExt);
            bundle.LoadAllAssets();

            string[] names = bundle.GetAllAssetNames();

            SpriteAtlas atlas = bundle.LoadAsset<SpriteAtlas>(names[0]);
            return atlas.GetSprite(assetName);
#endif
        }

        /// <summary>
        /// 加载图集
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public SpriteAtlas LoadAtlas(string assetName)
        {
            string[] moduleNameParts = assetName.Split('_');
            string moduleFolderName = moduleNameParts[0] + "_" + moduleNameParts[1];

#if UNITY_EDITOR && !USE_BUNDLE
            string assetPath = "Assets/BundleAssets/UISpriteAtlas/" + moduleFolderName + ".spriteatlas";
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            return atlas;
#else

            BundleWrapper bundle = LoadBundle(moduleFolderName.ToLower() + AtlasExt);
            bundle.LoadAllAssets();
            string[] names = bundle.GetAllAssetNames();

            SpriteAtlas atlas = bundle.LoadAsset<SpriteAtlas>(names[0]);
            return atlas;
#endif
        }

        /// <summary>
        /// 异步加载图集
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public void LoadAtlasAsync(string assetName,Action<SpriteAtlas> finishCallBack)
        {
#if UNITY_EDITOR && !USE_BUNDLE
            string[] moduleNameParts = assetName.Split('_');
            string moduleFolderName = moduleNameParts[0] + "_" + moduleNameParts[1];
            string assetPath = "Assets/BundleAssets/UISpriteAtlas/" + moduleFolderName + ".spriteatlas";
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);

            ClientTimer.Instance.DelayCall(() =>   //同步外面某些方法会报错，先加个延迟
            {
                if (finishCallBack != null)
                {
                    finishCallBack(atlas);
                }
            }, 0.1f);
#else
            StartCoroutine(AsyncLoadCoroutine(assetName, finishCallBack));
#endif
        }

        IEnumerator AsyncLoadCoroutine(string assetName, Action<SpriteAtlas> callBack)
        {
            string[] moduleNameParts = assetName.Split('_');
            string moduleFolderName = moduleNameParts[0] + "_" + moduleNameParts[1];
            BundleWrapper bundle = LoadBundle(moduleFolderName.ToLower() + AtlasExt);
            //yield return null;
            AssetBundleRequest req = bundle.LoadAllAssetsAsync();
            if (req == null)
            {
                SpriteAtlas atlas1 = LoadAtlas(assetName);
                yield return null;
                callBack(atlas1);
                yield break;
            }

            while (req.isDone == false)
            {            
                yield return null;
            }
            string[] names = bundle.GetAllAssetNames();
            SpriteAtlas atlas = bundle.LoadAsset<SpriteAtlas>(names[0]);
            //yield return null;
            callBack(atlas);
        }

        public Sprite GetSprite(string assetName)
        {
            string assetPath = GetAssetPathByAssetName<Texture>(assetName);
            assetName = assetName.ToLower();
            string bundleName = assetName + BytesExt;
            AddAssetBundleNameDic(assetPath, bundleName);

            BundleWrapper bundle = LoadBundle<Sprite>(bundleName);
            if (bundle == null)
                return null;

            string[] names = bundle.GetAllAssetNames();
            Sprite sp = bundle.LoadAsset<Sprite>(names[0]);
            return sp;
        }

        public Texture GetTexture(string assetName)
        {
            string assetPath = GetAssetPathByAssetName<Texture>(assetName);
            if (String.IsNullOrEmpty(assetName))
            {
//                Debug.LogError("Null!"+assetName);
                return null;
            }
            
            assetName = assetName.ToLower();
            string bundleName = assetName + BytesExt;
            AddAssetBundleNameDic(assetPath, bundleName);
            BundleWrapper bundle = LoadBundle(bundleName);
            if (bundle.IsEmpty())
                return null;

            string[] names = bundle.GetAllAssetNames();
            Texture sp = bundle.LoadAsset<Texture>(names[0]);
            return sp;
        }

        public TextAsset GetTextAsset(string assetName)
        {
            string assetPath = GetAssetPathByAssetName<TextAsset>(assetName);
            assetName = assetName.ToLower();
            AddAssetBundleNameDic(assetPath, assetName + BytesExt);
            BundleWrapper bundle = LoadBundle(assetName + BytesExt);
            if (bundle.IsEmpty())
                return null;

            string[] names = bundle.GetAllAssetNames();
            TextAsset ta = bundle.LoadAsset<TextAsset>(names[0]);

            return ta;
        }

        public ExternalBehaviorTree GetBehaviorTreeAsset(string assetName)
        {
            string assetPath = GetAssetPathByAssetName<TextAsset>(assetName);
            assetName = assetName.ToLower();
            AddAssetBundleNameDic(assetPath, assetName + BytesExt);
            BundleWrapper bundle = LoadBundle(assetName + BytesExt);
            if (bundle.IsEmpty())
                return null;

            string[] names = bundle.GetAllAssetNames();
            ExternalBehaviorTree ta = bundle.LoadAsset<ExternalBehaviorTree>(names[0]);

            return ta;
        }

        public AudioClip GetAudioClip(string assetName)
        {
            string assetPath = GetAssetPathByAssetName<AudioClip>(assetName);
            assetName = assetName.ToLower();
            AddAssetBundleNameDic(assetPath, assetName + AudioExt);
            BundleWrapper bundle = LoadBundle(assetName + AudioExt);
            if (bundle.IsEmpty())
                return null;

            string[] names = bundle.GetAllAssetNames();
            AudioClip ac = bundle.LoadAsset<AudioClip>(names[0]);
            return ac;
        }

        private BundleWrapper LoadBundle(string bundleName)
        {
            BundleWrapper bundleWrapper = new BundleWrapper();

            AssetBundle bundle = null;
            if (_cachedBundles.ContainsKey(bundleName))
            {
                bundle = _cachedBundles[bundleName];
                bundleWrapper.Init(bundleName, bundle);
            }
            else
            {
#if UNITY_EDITOR && !USE_BUNDLE
                string assetPath = GetAssetPathByBundleName(bundleName);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    if (bundleName.StartsWith("/"))
                        bundleName = bundleName.Substring(1);
                    UnityEngine.Object resource = null;
                    resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                    if (resource == null)  //找不到時用bundleName查找
                    {
                        resource = GetResourceByBundleNameEditor(bundleName);
                    }
                    bundleWrapper.Init(bundleName, resource);
                }
                else
                {
                    if (bundleName.StartsWith("/"))
                        bundleName = bundleName.Substring(1);
                    UnityEngine.Object resource = GetResourceByBundleNameEditor(bundleName);
                    if (resource != null)
                    {
                        bundleWrapper.Init(bundleName, resource);
                    }

                    //string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                    //if (bundles.Length == 0)
                    //{
                    //    Debug.LogError("bundleName no found:" + bundleName);
                    //}
                    //else
                    //{
                    //    UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(bundles[0]);
                    //    bundleWrapper.Init(bundleName, resource);
                    //}
                }
#else
                string path = PathUtil.GetPath(_platformBundlePath + bundleName, AssetLoader.ExternalHotfixPath);

                bundle = AssetBundle.LoadFromFile(path);
                if(bundle != null)
                    _cachedBundles.Add(bundleName, bundle);
                
                bundleWrapper.Init(bundleName, bundle);
#endif
            }

#if UNITY_EDITOR && USE_BUNDLE

            if (bundle!=null)
            {
                var materials = bundle.LoadAllAssets<Material>();
                foreach (var material in materials)
                {
                    var shaderName = material.shader.name;

                    var newShader = Shader.Find(shaderName);
                    if (newShader != null)
                    {
                        material.shader = newShader;
                    }
                    else
                    {
                        Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + material.name);
                    }
                }
            }

#endif
            return bundleWrapper;
        }


        private UnityEngine.Object GetResourceByBundleNameEditor(string bundleName)
        {
            UnityEngine.Object resource = null;
#if UNITY_EDITOR && !USE_BUNDLE
            string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            if (bundles.Length == 0)
            {
                Debug.LogError("bundleName no found:" + bundleName);
            }
            else
            {
                resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(bundles[0]);
            }
#endif
            return resource;
        }


        private BundleWrapper LoadBundle<T>(string bundleName) where T : Object
        {
            BundleWrapper bundleWrapper = new BundleWrapper();

            AssetBundle bundle = null;
            if (_cachedBundles.ContainsKey(bundleName))
            {
                bundle = _cachedBundles[bundleName];
                bundleWrapper.Init(bundleName, bundle);
            }
            else
            {
#if UNITY_EDITOR && !USE_BUNDLE
                string assetPath = GetAssetPathByBundleName(bundleName);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    if (bundleName.StartsWith("/"))
                        bundleName = bundleName.Substring(1);
                    UnityEngine.Object resource = null;
                    resource = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (resource == null)  //找不到時用bundleName查找
                    {
                        resource = GetResourceByBundleNameEditor<T>(bundleName);
                    }
                    bundleWrapper.Init(bundleName, resource);
                }
                else
                {
                    if (bundleName.StartsWith("/"))
                        bundleName = bundleName.Substring(1);
                    UnityEngine.Object resource = GetResourceByBundleNameEditor<T>(bundleName);
                    if (resource != null)
                    {
                        bundleWrapper.Init(bundleName, resource);
                    }
                }

                ////                if (bundleName.StartsWith("/"))
                ////                    bundleName = bundleName.Substring(1);
                //string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                //if (bundles.Length == 0)
                //{
                //    Debug.LogError("bundleName no found:" + bundleName);
                //}
                //else
                //{
                //    UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<T>(bundles[0]);
                //    bundleWrapper.Init(bundleName, resource);
                //}
#else
                string path = PathUtil.GetPath(_platformBundlePath + bundleName, AssetLoader.ExternalHotfixPath);
                bundle = AssetBundle.LoadFromFile(path);
                if(bundle != null)
                    _cachedBundles.Add(bundleName, bundle);

                bundleWrapper.Init(bundleName, bundle);
#endif
            }
            

#if UNITY_EDITOR && USE_BUNDLE

            var materials = bundle.LoadAllAssets<Material>();
            foreach (var material in materials)
            {
                var shaderName = material.shader.name;

                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    material.shader = newShader;
                }
                else
                {
                    Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + material.name);
                }
            }
#endif
            return bundleWrapper;
        }

        private UnityEngine.Object GetResourceByBundleNameEditor<T>(string bundleName) where T : UnityEngine.Object
        {
            UnityEngine.Object resource = null;
#if UNITY_EDITOR && !USE_BUNDLE
            string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
            if (bundles.Length == 0)
            {
                Debug.LogError("bundleName no found:" + bundleName);
            }
            else
            {
                resource = AssetDatabase.LoadAssetAtPath<T>(bundles[0]);
            }
#endif
            return resource;
        }


        public void UnloadBundle(string assetName)
        {
#if USE_BUNDLE
            assetName = assetName.ToLower();
            string bundleName = assetName + BytesExt;
            if (_cachedBundles.ContainsKey(bundleName))
            {
                var bundles = _cachedBundles[bundleName];
                if (bundles==null)
                {
                   Debug.LogError(bundles+" "+bundleName);
                }
                else
                {
                    bundles.Unload(true);
                    _cachedBundles.Remove(bundleName);  
                }             
                Debug.Log("<color='#00ffee'>UnloadModuleBundles </color>"  + bundleName);
            }
#endif
        }

        public void LogMessage()
        {
            Debug.Log("<color='#aaffee'>CachedBundles:" + _cachedBundles.Count + "</color>");
           
#if UNITY_EDITOR
            GetBundleTotalMemory();
#else
            if (AppConfig.Instance.isTestMode)
            {
                GetBundleTotalMemory();
            }
#endif
        }

        public int BundleCount => _cachedBundles.Count;

        public long GetBundleTotalMemory()
        {
            if (!Debug.isDebugBuild)
                return -1;
            
            long size = 0;
            foreach (var bundle in _cachedBundles)
            {
                Object[] objs = bundle.Value.LoadAllAssets();
                foreach (var obj in objs)
                {
                    size += Profiler.GetRuntimeMemorySizeLong(obj);
                }
            }
            
            Debug.Log("<color='#aaffee'>CachedBundles GetRuntimeMemorySizeLong:" + Util.GetSizeUnit(size) + "</color>");

            return size;
        }
    }

    
}