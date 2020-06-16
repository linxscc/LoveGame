using System;
using Assets.Scripts.Framework.GalaSports.Service;
using QFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Module.Effect
{
    public class EffectManager
    {
        private static BackgroundBlurEffect _backgroundBlurEffect;

        public static BackgroundBlurEffect CreateImageBlurEffect()
        {
            if (_backgroundBlurEffect != null)
                return _backgroundBlurEffect;
            
            GameObject prefab = ResourceManager.Load<GameObject>("Module/Prefabs/BackgroundEffect/RenderTextureCamera");
            GameObject go = Object.Instantiate(prefab);
            
            _backgroundBlurEffect = go.GetComponent<BackgroundBlurEffect>();
            return _backgroundBlurEffect;
        }

        public static void DestroyBackgroundEffect()
        {
            if (_backgroundBlurEffect != null)
            {
                Object.DestroyImmediate(_backgroundBlurEffect.gameObject);
            }
        }
    }


}