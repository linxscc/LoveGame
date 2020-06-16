using System;

namespace Assets.Scripts.Module.Download
{
    [Serializable]
    public struct BundleStruct
    {
        public long Size;

        public string Md5;
    }
}