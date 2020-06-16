using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using game.main;

namespace Assets.Scripts.Services
{
    public class JumpDataService : LocalService<Dictionary<string, string[]>>
    {
        protected override void OnExecute()
        {
            useAsync = true;
            resType = ResType.Text;
            resPath = AssetLoader.GetJumpDataPath();
        }
       
        protected override void ProcessData(object text)
        {
            _data = new Dictionary<string, string[]>();
            string str = (string) text;
            var strings = str.Split('\n');
            for (int i = 3; i < strings.Length; i++)
            {
                int index = strings[i].IndexOf(',');
                if(index == -1)
                    continue;

                string[] arr = strings[i].Split(',');
                    
                _data.Add(arr[0], arr);
            }
        }
    }
}