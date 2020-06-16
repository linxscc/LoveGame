#region 模块信息

// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             AssetLoader.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/3/7 16:6:26
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************

#endregion

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using Debug = UnityEngine.Debug;

namespace game.main
{
    /// <summary>
    /// 外部资源加载器,负责加载音频和文本文件
    /// 只能加载单个资源，例如一张图片，一个音频
    /// </summary>
    public class AssetLoader
    {
        public static readonly string ExternalHotfixPath = Application.persistentDataPath + "/Hotfix";
        public static readonly string ExternalOldHotfixPath = Application.persistentDataPath + "/OldHotfix";
        public static readonly string ExternalDownloadPath = Application.persistentDataPath + "/Download";
        public static readonly string CachePath = Application.persistentDataPath + "/DataCache/";
        public static readonly string CoaxSleepAudioFolderPath = Application.persistentDataPath + "/CoaxSleepAudioFolder";

        private Action<Sprite, AssetLoader> _onComplete;
        private Action<string, AssetLoader> _onTextComplete;

        public string FilePath { get; private set; }
        public object Data { get; private set; }

        private static Dictionary<string, AssetBundle> _audioBundleDict = new Dictionary<string, AssetBundle>();

        public AssetLoader SetData(object data)
        {
            this.Data = data;
            return this;
        }
        
        public static string GetIndexFilePath(string path)
        {
            string suffix = "IndexFiles/" + path + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        
        public static string GetBundleConfigPath()
        {
            string suffix = "IndexFiles/bundle.config";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetPhoneDataPath(string dataId)
        {
            string suffix = "PhoneData/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetDiaryTemplateDataPath(string dataId)
        {
            string suffix = "DiaryTemplate/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetVisitLevelMapDataPath(string dataId)
        {
            string suffix = "visit/visitmaps/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        public static string GetMusicRhythmDataPath(string dataId)
        {
            string suffix = "MusicRhythm/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }


        public static string GetActivityTemplateDataPath(string name)
        {
            string suffix = "ActivityTemplate/"+name+".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
            
        }

        public static string GetActivityPopWindowDataPath(string name)
        {
            string suffix = "ActivityPopWindowData/"+name+".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        
     

        /// <summary>
        /// 获取本地Json配置信息
        /// </summary>
        /// <param name="folderName">文件夹名</param>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        public static string GetLocalConfiguratioData(string folderName, string name)
        {
            string suffix = folderName+"/"+name+".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        
        
        #region 剧情路径

        public static string GetStoryDataPath(string dataId)
        {
            string suffix = "Data/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetStoryTelphoneDataPath(string dataId)
        {
            string suffix = "Telphone/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetStorySmsDataPath(string dataId)
        {
            string suffix = "Sms/" + dataId + ".json";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetStoryBgImage(string bgImageId)
        {
            string suffix = "story/background/" + bgImageId.ToLower();
            return suffix;
        }

        public static string GetStoryRoleImageById(string id)
        {
            string suffix = "story/roles/" + id.ToLower();
            return suffix;
        }

        public static string GetHeadImageById(string id)
        {
            string suffix = "story/head/" + id;
            return suffix;
        }

        public static string GetDubbingById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "story/dubbing/" + id.ToLower() + ".bytes";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        #endregion

        public static string GetCacheVersionPath()
        {
            string suffix = "ProtoCache/cacheVersion.txt";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        
        public static string GetJumpDataPath()
        {
            string suffix = "Config/jump.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetItemDescDataPath()
        {
            string suffix = "Config/ItemDesc.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetSpecialItemDescDataPath()
        {
            string suffix = "Config/SpecialItemDesc.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetLanguageDataPath(I18NManager.LanguageType type)
        {
            string fileName = "zh-CN";
//            switch (type)
//            {
//                case I18NManager.LanguageType.ChineseSimplified:
//                    fileName = "zh-CN";
//                    break;
//                case I18NManager.LanguageType.ChineseTraditional:
//                    fileName = "zh-TW";
//                    break;
//                case I18NManager.LanguageType.English:
//                    fileName = "en-US";
//                    break;
//            }

            string suffix = "Languages/" + fileName + ".txt";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        /// <summary>
        /// 获取打包的接口数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetProtoCachePath(string id)
        {
            string suffix = "ProtoCache/" + id;
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        
        public static string GetLevelDataPath()
        {
            string suffix = "Config/LevelData.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetExpresssionDataPath()
        {
            string suffix = "Config/expression.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        public static string GetPhoneUnlockDataPath()
        {
            string suffix = "Config/PhoneUnlock.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetDrawCardDialogDataPath()
        {
            string suffix = "Config/drawcarddialog.csv";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetErrorCodePath()
        {
            string suffix = "Config/errorCode.txt";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }


        public static string GetLuaPath(string luaFileName)
        {
            string suffix = "LuaFiles/" + luaFileName + ".lua.txt";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }
        public static string GetLuaProtoPath(string protoFileName)
        {
            string suffix = "LuaFiles/Proto/" + protoFileName;
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }


        public static string GetBackgrounMusicById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "music/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        /// <summary>
        /// 获取外部哄睡音频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetCoaxSleepMusicById(string id)
        {
            string suffix =  PathUtil.PlatfromBundlePath() +"music/coaxsleepaudios/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, CoaxSleepAudioFolderPath);
        }
        
        
        public static string GetSoundEffectById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "music/soundeffect/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }


        public static string GetPhoneDialogById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "music/phonedialogs/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetMainPanleDialogById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "music/mainplay/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetDrawCardDialogById(string id)
        {
            string suffix = PathUtil.PlatfromBundlePath() + "music/drawcarddialogs/" + id.ToLower() + ".music";
            return PathUtil.GetPath(suffix, ExternalHotfixPath);
        }

        public static string GetLive2dModelJsonById(string id)
        {
            return "Live2d/Animation/" + id + "/" + id + ".model.json";
        }

        public static string GetLive2dDirById(string id)
        {
            return "Live2d/Animation/" + id;
        }

        public AssetLoader LoadAudio(string filePath, Action<AudioClip, AssetLoader> onComplete)
        {
#if UNITY_EDITOR && !USE_BUNDLE
            AudioClip audioClip = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<AudioClip>(filePath);
            if (audioClip != null)
            {
                onComplete(audioClip, this);
                return this;
            }
            else
            {
                return LoadAudioByBundle(filePath, onComplete);
            }
#else
            return LoadAudioByBundle(filePath, onComplete);
#endif
        }

        private AssetLoader LoadAudioByBundle(string filePath, Action<AudioClip, AssetLoader> onComplete)
        {
            FilePath = filePath;
            AssetBundle bundle;
            if (_audioBundleDict.ContainsKey(filePath))
            {
                bundle = _audioBundleDict[filePath];
            }
            else
            {
                bundle = AssetBundle.LoadFromFile(filePath);
                if (bundle != null)
                    _audioBundleDict.Add(filePath, bundle);
            }

            AudioClip audioClip = null;
            if (bundle != null)
            {
                string[] names = bundle.GetAllAssetNames();
                audioClip = bundle.LoadAsset<AudioClip>(names[0]);
            }

            onComplete(audioClip, this);
            return this;
        }

        public string LoadTextSync(string filePath)
        {
#if UNITY_IOS
            filePath = "file://" + filePath;
#elif UNITY_ANDROID && !UNITY_EDITOR
            if (!filePath.Contains("jar:file://"))
            {
                try
                {
                    string text = File.ReadAllText(filePath);
                    return text;
                }
                catch (Exception e)
                {
                    Debug.LogWarning("LoadTextSync Error:" + e.Message + " Path:"+filePath);
                    return "";
                }
            }
#endif
            WWW www = new WWW(filePath);
            while (!www.isDone)
            {
            }

            if (string.IsNullOrEmpty(www.error))
            {
                return www.text;
            }
            else
            {
                Debug.LogError("AssetLoader LoadTextSync Error:" + filePath + "\n" + www.error);
                return "";
            }
        }

        public byte[] LoadBytes(string filePath, bool showError = true)
        {
#if UNITY_IOS
            filePath = "file://" + filePath;
#elif UNITY_ANDROID && !UNITY_EDITOR
            if (!filePath.Contains("jar:file://"))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    return bytes;
                }
                catch (Exception e)
                {
                    Debug.LogWarning("LoadBytes Error:" + e.Message + " Path:"+filePath);
                    return null;
                }
            }
#endif
            WWW www = new WWW(filePath);
            while (!www.isDone)
            {
            }

            if (string.IsNullOrEmpty(www.error))
            {
                if (showError == false)
                {
                    string str = Encoding.UTF8.GetString(www.bytes);
                    Debug.LogWarning("LoadBytes NormalLoader>>>>>>>>" + str);
                }

                return www.bytes;
            }
            else
            {
                Debug.LogError("AssetLoader LoadError:" + filePath);
                Debug.LogError("AssetLoader LoadError www.error=>" + www.error);
                return null;
            }
        }

        public AssetLoader LoadText(string filePath, Action<string, AssetLoader> onComplete)
        {
#if UNITY_IOS
            filePath = "file://" + filePath;
#endif
            _onTextComplete = onComplete;
            MonoObject.Instance.Coroutine(LoadString(filePath));
            return this;
        }

        private IEnumerator LoadString(string filePath)
        {
#if !UNITY_IOS
            if (!filePath.Contains("jar:file://"))
            {
                Debug.LogWarning("LoadString ReadAllText>>>>>>>>" + filePath);
                string text = File.ReadAllText(filePath);
                yield return null;
                if (_onTextComplete != null)
                {
                    _onTextComplete(text, this);
                    _onTextComplete = null;
                }
                yield break;
            }
#endif


            WWW www = new WWW(filePath);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                if (_onTextComplete != null)
                {
                    _onTextComplete(www.text, this);
                    _onTextComplete = null;
                }
            }
            else
            {
                Debug.LogError("AssetLoader LoadError:" + filePath);
            }
        }

        public static void UnloadBundle(string filePath)
        {
            if (IsIgnoreAudio(filePath))
                return;

            if (_audioBundleDict.ContainsKey(filePath))
            {
                _audioBundleDict[filePath].Unload(true);
                _audioBundleDict.Remove(filePath);
                Debug.Log("AssetLoader <color='#00ff00'>UnloadBundle</color>=>" + filePath);
            }
        }

        public static void UnloadAllAudio()
        {
            List<string> list = new List<string>();
            foreach (var bundle in _audioBundleDict)
            {
                if (IsIgnoreAudio(bundle.Key))
                    continue;

                bundle.Value.Unload(true);
                list.Add(bundle.Key);
            }

            foreach (var key in list)
            {
                _audioBundleDict.Remove(key);
            }
        }

        private static bool IsIgnoreAudio(string bundleName)
        {
            if (bundleName.Contains(AudioManager.DefaultBgMusicName.ToLower()) ||
                bundleName.Contains(AudioManager.DefaultButtonEffectName.ToLower()) ||
                bundleName.Contains(AudioManager.DefaultBackButtonEffectName.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}