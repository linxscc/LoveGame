using BehaviorDesigner.Runtime;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Service
{
    public class ResourceManager
    {
        /// <summary>
        /// 统一资源加载入口
        /// </summary>
        /// <param name="bundleName">Bundle的名称</param>
        /// <param name="domain">跟随模块卸载</param>
        /// <param name="unloadLater">是否在回到主界面才卸载</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string bundleName, string domain = null, bool unloadLater = false) where T : Object
        {
            T obj;
            if (typeof(T) == typeof(Texture))
            {
                obj = AssetManager.Instance.GetTexture(bundleName) as T ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(Sprite))
            {
                obj = AssetManager.Instance.GetSprite(bundleName) as T ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(Material))
            {
                obj = AssetManager.Instance.GetAsset<T>("module/"+bundleName) ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(RuntimeAnimatorController))
            {
                obj = AssetManager.Instance.GetAsset<T>("module/" + bundleName) ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(TextAsset))
            {
                obj = AssetManager.Instance.GetTextAsset(bundleName) as T ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                obj = AssetManager.Instance.GetAudioClip(bundleName) as T ?? Resources.Load<T>(bundleName);
            }
            else if (typeof(T) == typeof(ExternalBehaviorTree))
            {
                obj = AssetManager.Instance.GetBehaviorTreeAsset(bundleName) as T ?? Resources.Load<T>(bundleName);
            }
            else
            {
                obj = AssetManager.Instance.GetAsset<T>(bundleName) ?? Resources.Load<T>(bundleName);
            }

            if (unloadLater)
            {
                AssetManager.Instance.AddToLaterUnload(bundleName);
            }
            else if (!string.IsNullOrEmpty(domain))
            {
                AssetManager.Instance.MarkSingleFileBundle(bundleName, domain);
            }
            return obj;
        }

        #region LuaCall

        public Texture LoadTexture(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<Texture>(bundleName, domain, unloadLater);
        }
        
        public Material LoadMaterial(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<Material>(bundleName, domain, unloadLater);
        }
        
        public GameObject LoadGameObject(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<GameObject>(bundleName, domain, unloadLater);
        }
        
        public TextAsset LoadTextAsset(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<TextAsset>(bundleName, domain, unloadLater);
        }
        
        public AudioClip LoadAudioClip(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<AudioClip>(bundleName, domain, unloadLater);
        }
        
        public ExternalBehaviorTree LoadExternalBehaviorTree(string bundleName, string domain = null, bool unloadLater = false)
        {
            return Load<ExternalBehaviorTree>(bundleName, domain, unloadLater);
        }

        #endregion
       
    }
}
