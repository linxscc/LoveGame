using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Download;
using game.main;

namespace Assets.Scripts.Services
{
    public class BundleConfigService : LocalService<Dictionary<string, BundleStruct>>
    {
        protected override void OnExecute()
        {
            resPath = AssetLoader.GetBundleConfigPath();
            
            useAsync = false;
            resType = ResType.Binary;
        }

        protected override void ProcessData(object data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _data = data as Dictionary<string, BundleStruct>;
            
            UnityEngine.Debug.Log(resPath + "BundleConfigService解析时间：" + sw.ElapsedMilliseconds/1000.0f + "s");
            sw.Stop();
            
        }
    }
}