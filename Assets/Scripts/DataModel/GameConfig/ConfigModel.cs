using System;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using UnityEngine;

namespace DataModel
{
    /// <summary>
    /// 游戏配置数据
    /// </summary>
    public class ConfigModel
    {
        private Dictionary<string, int> _gameConfig;
        private Dictionary<string, long> _gameTimeConfig;
        
        public void InitBaseData(GameConfigRes res)
        {
            
           
            if (_gameConfig == null)
            {
                _gameConfig = new Dictionary<string, int>();
            }
            else
            {
                _gameConfig.Clear();
            }

            if (_gameTimeConfig==null)
            {
                _gameTimeConfig = new Dictionary<string, long>();
            }
            else
            {
                _gameTimeConfig.Clear(); 
            }
            
           
            foreach (var gpb in res.GameConfigs)
            {
                _gameConfig[gpb.ConfigKey] = gpb.ConfigVal;
            }

            foreach (var gpb in res.GameTimeConfigs)
            {
                _gameTimeConfig[gpb.ConfigKey] = gpb.ConfigVal;
            }
        }

        public int  GetConfigByKey(string key)
        {
            if (!_gameConfig.ContainsKey(key))
            {
                Debug.LogError("Don't have ConfigData for key :"+ key);
                return -1;
            }
            return _gameConfig[key];
        }


        public long GetGameTimeConfigByKey(string key)
        {
            if (!_gameTimeConfig.ContainsKey(key))
            {
                Debug.LogError("Don't have GameTimeConfig for key :"+ key);
                return -1; 
            }

            return _gameTimeConfig[key];
        }
    }
}

