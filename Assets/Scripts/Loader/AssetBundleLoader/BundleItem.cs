using System;
using UnityEngine;

namespace game.main
{
    [Serializable]
    public class BundleItem
    {
        public string Name;

        public BundleType Type;

        public string[] Dependencies;

        /// <summary>
        /// 其他语言的bundle
        /// </summary>
        private BundleItem[] _languageBundleItems;
        
        public AssetBundle AssetBundle;

        public string BundlePath;
    }

    public enum BundleType
    {
        Module,
        Image,
        Atlas,
        Audio
    }
}