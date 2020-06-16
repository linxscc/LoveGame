using UnityEngine;

namespace game.main
{
    public class BundleWrapper
    {
        protected AssetBundle Bundle;

        protected Object Resource;

        protected string BundleName;
        
        public string[] GetAllAssetNames()
        {
            if (Bundle != null)
            {
                return Bundle.GetAllAssetNames();
            }

            return new[] {BundleName};
        }
        
        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (Bundle != null)
            {
                return Bundle.LoadAsset<T>(assetName);
            }
            
            return (T) Resource;
        }
        
        public Object[] LoadAllAssets()
        {
            if (Bundle != null)
            {
                return Bundle.LoadAllAssets();
            }
            return null;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <returns></returns>
        public AssetBundleRequest LoadAllAssetsAsync()
        {
            if (Bundle != null)
            {
                return Bundle.LoadAllAssetsAsync();
            }
            return null;
        }

        public T[] LoadAllAssets<T>() where T : Object
        {
            if (Bundle != null)
            {
                return Bundle.LoadAllAssets<T>();
            }

            return null;
        }

        public void Init(string bundleName, Object resource)
        {
            BundleName = bundleName;
            Resource = resource;
        }
        
        public void Init(string bundleName, AssetBundle bundle)
        {
            BundleName = bundleName;
            Bundle = bundle;
        }

        public AssetBundle GetBundle()
        {
            return Bundle;
        }

        public Object GetResource()
        {
            return Resource;
        }

        public void Unload(bool b)
        {
            if(Bundle != null)
            {
                Bundle.Unload(b);
            }
        }

        public bool IsEmpty()
        {
            return Bundle == null && Resource == null;
        }
    }
}