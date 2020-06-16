using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.main
{
    public class VisitVo
    {
        // public int NpcId;
        /// <summary>
        /// 当前已探班次数
        /// </summary>
        public int CurVisitTime
        {
            get
            {

                return _visitedTime;
            }
        }

        public int MaxVisitTime
        {
            get
            {
                return _weatherPB.VisitingNum;
            }
        }

        public string CurWeatherName
        {
            get
            {
                return _weatherPB.Name;
            }
        }
        public WeatherPB CurWeatherPB
        {
            get
            {
                return _weatherPB;
            }
        }


        /// <summary>
        /// 单个关卡最大探班次数
        /// </summary>
        public static int MaxSingleVisitTime
        {
            get
            {
                return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.VISITING_LEVEL_MAX_TIMES);
            }
        }

        /// <summary>
        /// 当前天气下祈福成功率
        /// </summary>
        public int CurSuccessRate
        {
            get
            {

                int blessTime = 0;
                if (BlessNumDir.ContainsKey(_weatherPB.WeatherId))
                {
                    blessTime = BlessNumDir[_weatherPB.WeatherId];
                }

                return _visitModel.GetSuccessRate(_weatherPB.WeatherId, blessTime + 1);
            }
        }

        public VISIT_WEATHER CurWeather
        {
            get
            {
                return (VISIT_WEATHER)_weatherPB.WeatherId;
            }
        }

        public PlayerPB NpcId;
        public string NpcName
        {
            get
            {
                return Util.GetPlayerName(NpcId);
            }
        }
        //   public VISIT_WEATHER Weather;
        //public int WeatherId;
        public Dictionary<int, int> BlessNumDir;

        private WeatherPB _weatherPB;

        private int _visitedTime;

        public int GetResetCost(int time)
        {
            return _visitModel.GetResetCost(time);
        }

        public int GetMaxVisitTimeByWeather(VISIT_WEATHER weather)
        {
            int weatherId = (int)weather;
            return _visitModel.GetWeatherRulesById(weatherId).VisitingNum;
        }


        VisitModel _visitModel;
        public VisitVo(UserWeatherPB userWeatherPB, WeatherPB weatherPB, int curVisitTime, VisitModel visitModel)
        {
            _visitModel = visitModel;
            NpcId = userWeatherPB.Player;
            BlessNumDir = new Dictionary<int, int>();
            foreach (var v in userWeatherPB.BlessNumMap)
            {
                BlessNumDir[v.Key] = v.Value;
            }

            //WeatherId = userWeatherPB.WeatherId;
            _weatherPB = weatherPB;
            //_visitedTime = curVisitTime;
            _visitedTime = userWeatherPB.ChallengeCount;
        }

        public void UpdateWeatherData(WeatherPB weatherPB)
        {
            _weatherPB = weatherPB;
        }

        public void UpdateUserWeatherData(UserWeatherPB userWeatherPB, WeatherPB weatherPB)
        {
            _weatherPB = weatherPB;
            foreach (var v in userWeatherPB.BlessNumMap)
            {
                BlessNumDir[v.Key] = v.Value;
            }
            _visitedTime = userWeatherPB.ChallengeCount;
        }

        /// <summary>
        /// 祈福消耗星钻数量
        /// </summary>
        public int BlessCost
        {
            get
            {
                int count = 1;
                foreach (var vk in BlessNumDir)
                {
                    count += vk.Value;
                }
                return _visitModel.GetBlessCost(count);
            }
        }

        public void UpdateVisitedData(int curVisitTime)
        {
            _visitedTime = curVisitTime;
        }
    }
}
