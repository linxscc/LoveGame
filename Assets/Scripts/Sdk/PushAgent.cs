using System;
using System.Collections.Generic;
using Assets.Scripts.Module.Framework.Utils;
using DataModel;
using GalaSDKBase;
using UnityEngine;

namespace Assets.Scripts.Module.Sdk
{

    public enum PushType
    {
        PowerMax,//体力达到上限时
        PowerReceiverTime0,//每日体力可领取时
        PowerReceiverTime1,//每日体力可领取时
        PowerReceiverTime2,//每日体力可领取时
        PowerReceiverTime3,//每日体力可领取时
        PowerReceiverTime4,//每日体力可领取时
        PowerReceiverTime5,//每日体力可领取时
        DrawGem,//存在免费星卡抽取时
        DrawGold,//存在免费金币抽取时
        Supporter, //应援活动
        MonthCard,// 月卡 存在未领取的月卡奖励时
        OldPlayer,//老玩家回归
        CreateMsg1,   //创建号1天短信
        CreateMsg2,   //创建号2天短信
        CreateMsg3,   //创建号3天短信
        CreateMsg4,   //创建号4天短信
        CreateMsg5,   //创建号5天短信
        CreateMsg6,   //创建号6天短信
        CreateMsg7,   //创建号7天短信
        CreateMsg8,   //创建号8天短信
        CreateMsg9,   //创建号9天短信
        CreateMsg10,   //创建号10天短信
        CreateMsg11,   //创建号11天短信
        CreateMsg12,   //创建号12天短信
        CreateMsg13,   //创建号13天短信
        CreateMsg14,   //创建号14天短信
        CreateMsg15,   //创建号15天短信
        CreateMsg16,   //创建号16天短信
        CreateMsg17,   //创建号17天短信
        CreateMsg18,   //创建号18天短信
        CreateMsg19,   //创建号19天短信
        CreateMsg20,   //创建号20天短信
        CreateMsg21,   //创建号21天短信
        CreateMsg22,   //创建号22天短信
        CreateMsg23,   //创建号23天短信
        CreateMsg24,   //创建号24天短信
        CreateMsg25,   //创建号25天短信
        CreateMsg26,   //创建号26天短信
        CreateMsg27,   //创建号27天短信
        CreateMsg28,   //创建号28天短信
        CreateMsg29,   //创建号29天短信
        CreateMsg30,   //创建号30天短信
    };

    public class PushAgent
    {
        //private Dictionary<string, string> PushDic;
        //private string _supporterActTime;
        //private string _goldDrawTime;
        //private string _gemDrawTime;
        //private string _powerMaxTime;

        private Dictionary<PushType, string> _pushRecordDic;

        public PushAgent()
        {
            //PushDic=new Dictionary<string, string>();

            _pushRecordDic = new Dictionary<PushType, string>();
        }

        public void InitPushData()
        {
            _pushRecordDic.Clear();
//            Debug.LogError("InitPushData " +GlobalData.PlayerModel.PlayerVo.UserName + PushType.PowerMax);
            //if (PlayerPrefs.HasKey(GlobalData.PlayerModel.PlayerVo.UserName + PushType.PowerMax))
            //{
                var powerMaxTime = PlayerPrefs.GetString(GlobalData.PlayerModel.PlayerVo.UserName + PushType.PowerMax);
                _pushRecordDic[PushType.PowerMax]= powerMaxTime;
           // }
            //if (PlayerPrefs.HasKey(GlobalData.PlayerModel.PlayerVo.UserName + PushType.DrawGold))
            //{
                var goldDrawTime = PlayerPrefs.GetString(GlobalData.PlayerModel.PlayerVo.UserName + PushType.DrawGold);
                _pushRecordDic[PushType.DrawGold] = powerMaxTime;
            //}
            //if (PlayerPrefs.HasKey(GlobalData.PlayerModel.PlayerVo.UserName + PushType.DrawGem))
            //{
            var gemDrawTime = PlayerPrefs.GetString(GlobalData.PlayerModel.PlayerVo.UserName + PushType.DrawGem);
   
            _pushRecordDic[PushType.DrawGem] = powerMaxTime;
            // }
            // if (PlayerPrefs.HasKey(GlobalData.PlayerModel.PlayerVo.UserName + PushType.Supporter))
            // {
            var supporterActTime = PlayerPrefs.GetString(GlobalData.PlayerModel.PlayerVo.UserName + PushType.Supporter);
   
            _pushRecordDic[PushType.Supporter] = powerMaxTime;
            //  }
        }

        public void Refeash()
        {
            PushPowerReceiverTime(false);
            PushMonthCard(false);
            PushOldPlayer(true);
        }

        private void PushOperate(PushType pushType, long timeStamp,bool isNowPush=false)
        {
            _pushRecordDic[pushType] = timeStamp.ToString();
            string prefsKey = GlobalData.PlayerModel.PlayerVo.UserName + pushType;
            Debug.Log("PushOperate prefsKey " + prefsKey);
            PlayerPrefs.SetString(prefsKey, timeStamp.ToString());
            if (isNowPush)
            {
                PushNew();
            }
        }

        public void PushNew()
        {
            Dictionary<string, string> pushDic = new Dictionary<string, string>();
            foreach (var kv in _pushRecordDic)
            {
                if(kv.Value=="")
                {
                    continue;
                }
                string stamp = kv.Value;
                long stampn=0;
                if (!long.TryParse(stamp,out stampn))
                {
                    continue;
                }
    
                if(stampn< ClientTimer.Instance.GetCurrentTimeStamp())
                {
                    continue;
                }

                pushDic[kv.Value.ToString()] = GetPushContent(kv.Key);
            }
            GalaSDKBaseFunction.UpdateNotification(pushDic);
        }

        private string GetPushContent(PushType pushType)
        {
            string content = "";
            switch (pushType)
            {
                case PushType.PowerMax:
                    content = I18NManager.Get("Push_HintPowerMax" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.OldPlayer:
                    content = I18NManager.Get("Push_HintOldPlayer" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.Supporter:
                    content = I18NManager.Get("Push_HintGetEncAct" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.MonthCard:
                    content = I18NManager.Get("Push_HintMonthCardGem" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.DrawGem:
                case PushType.DrawGold:
                    content = I18NManager.Get("Push_HintFreeDraw" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.PowerReceiverTime0:
                case PushType.PowerReceiverTime1:
                case PushType.PowerReceiverTime2:
                case PushType.PowerReceiverTime3:
                case PushType.PowerReceiverTime4:
                case PushType.PowerReceiverTime5:    
                    content = I18NManager.Get("Push_HintGetPower" + UnityEngine.Random.Range(0, 3));
                    break;
                case PushType.CreateMsg1:
                case PushType.CreateMsg2:
                case PushType.CreateMsg3:
                case PushType.CreateMsg4:
                case PushType.CreateMsg5:
                case PushType.CreateMsg6:
                case PushType.CreateMsg7:
                case PushType.CreateMsg8:
                case PushType.CreateMsg9:
                case PushType.CreateMsg10:
                case PushType.CreateMsg11:
                case PushType.CreateMsg12:
                case PushType.CreateMsg13:
                case PushType.CreateMsg14:
                case PushType.CreateMsg15:
                case PushType.CreateMsg16:
                case PushType.CreateMsg17:
                case PushType.CreateMsg18:
                case PushType.CreateMsg19:
                case PushType.CreateMsg20:
                case PushType.CreateMsg21:
                case PushType.CreateMsg22:
                case PushType.CreateMsg23:
                case PushType.CreateMsg24:
                case PushType.CreateMsg25:
                case PushType.CreateMsg26:
                case PushType.CreateMsg27:
                case PushType.CreateMsg28:
                case PushType.CreateMsg29:
                case PushType.CreateMsg30:
                    if (GlobalData.PlayerModel != null && GlobalData.PlayerModel.PlayerVo != null)
                    {
                        int role_id = GlobalData.PlayerModel.PlayerVo.NpcId;
                        var vo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo(role_id);
                        if (vo != null)
                        {
                            string name = GlobalData.FavorabilityMainModel.GetPlayerName(vo.Player);
                            content = string.Format(I18NManager.Get("Push_CreateMsg"), name);
                            //Debug.LogWarning("pushType:" + pushType + " content:" + content);
                        }
                    }
                    break;
                default:
                    content = I18NManager.Get("Push_HintOldPlayer" + UnityEngine.Random.Range(0, 3));
                    break;
            }
            return content;
        }


        public void PushPowerReceiverTime(bool isPushNow)
        {
            var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
            var point0 = new DateTime(serverDateTime.Year, serverDateTime.Month, serverDateTime.Day, 11, 0, 0);
            var point1 = new DateTime(serverDateTime.Year, serverDateTime.Month, serverDateTime.Day, 16, 0, 0);
            var point2 = new DateTime(serverDateTime.Year, serverDateTime.Month, serverDateTime.Day, 20, 0, 0);
            var point3 = point0.AddDays(1);
            var point4 = point1.AddDays(1); ;
            var point5 = point2.AddDays(1); ;
            var timestamp0 = DateUtil.GetTimeStamp(point0);
            var timestamp1 = DateUtil.GetTimeStamp(point1);
            var timestamp2 = DateUtil.GetTimeStamp(point2);
            var timestamp3 = DateUtil.GetTimeStamp(point3);
            var timestamp4 = DateUtil.GetTimeStamp(point4);
            var timestamp5 = DateUtil.GetTimeStamp(point5);

            PushOperate(PushType.PowerReceiverTime0, timestamp0, false);
            PushOperate(PushType.PowerReceiverTime1, timestamp1, false);
            PushOperate(PushType.PowerReceiverTime2, timestamp2, false);
            PushOperate(PushType.PowerReceiverTime3, timestamp3, false);
            PushOperate(PushType.PowerReceiverTime4, timestamp4,false);
            PushOperate(PushType.PowerReceiverTime5, timestamp5,isPushNow);

        }

        public void PushMonthCard(bool isPushNow)
        {
            if (GlobalData.PlayerModel.PlayerVo.UserMonthCard != null)
            {
                if ((ClientTimer.Instance.GetCurrentTimeStamp() - GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime) >= 0)
                {
                    var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());

                    var point0 = new DateTime(serverDateTime.Year, serverDateTime.Month, serverDateTime.Day, 6, 0, 0);
                    var point1 = point0.AddDays(1);
                    var timestamp = DateUtil.GetTimeStamp(point1);
                    PushOperate(PushType.MonthCard, timestamp, isPushNow);
                }
            }
        }

        public void PushOldPlayer(bool isPushNow)
        {
            var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
            var serverDateTime1=   serverDateTime.AddDays(1);
            long stamp = DateUtil.GetTimeStamp(serverDateTime1);
            PushOperate(PushType.OldPlayer, stamp, isPushNow);
        }

        public void PushPowerMax(long timeStamp)
        {
            PushOperate(PushType.PowerMax, timeStamp, true);
        }


        public void PushFreeGoldDraw(long timestamp)
        {
            PushOperate(PushType.DrawGold, timestamp,false);
        }

        public void PushFreeGemDraw(long timestamp)
        {
            PushOperate(PushType.DrawGem, timestamp, false);
        }

        public void PushSupporterAct(long timeStamp)
        {
            PushOperate(PushType.Supporter, timeStamp,true);
        }


        public void PushCreate30DaysNotice()
        {
            long curTime = ClientTimer.Instance.GetCurrentTimeStamp();
            long createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;
            DateTime create_dt = DateUtil.GetDataTime(createTime);
            create_dt = new DateTime(create_dt.Year, create_dt.Month, create_dt.Day);
            DateTime day31 = create_dt.AddDays(31);
            long stamp31 = DateUtil.GetTimeStamp(day31);
            if (curTime > stamp31) return;

            //Debug.LogWarning("createTime:"+createTime + " stamp30:"+ stamp31);
            for(int i = 0; i < 30; ++i)
            {
                PushType pType = PushType.CreateMsg1 + i;
                pushCreateDaysNotice(pType, create_dt, curTime, i+1, (i == 29));
            }

        }

        private void pushCreateDaysNotice(PushType pType, DateTime createDt, long curTime, int day, bool isNowPush = false)
        {
            DateTime dt = createDt.AddDays(day);
            dt = dt.AddHours(12);
            dt = dt.AddMinutes(10);
            long stamp = DateUtil.GetTimeStamp(dt);
            if (curTime > stamp) return;
            //Debug.LogWarning("pushCreateDaysNotice:"+pType+"  stamp: "+ stamp + " isNowPush:"+isNowPush);
            PushOperate(pType, stamp, isNowPush);
        }


        
    }
}