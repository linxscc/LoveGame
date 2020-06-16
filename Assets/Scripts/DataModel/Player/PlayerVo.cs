using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using game.main;
using Google.Protobuf.Collections;
using Debug = UnityEngine.Debug;

namespace DataModel
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    public class PlayerVo
    {
        private int _level;

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                SdkHelper.StatisticsAgent.SetLevel(_level);
            }
        }

        //是否是游客登录
        public bool IsGuset => GalaAccountManager.Instance.IsTour();

        public int PreLevel;
        public int Exp;
        public long Gold;
        public int Gem;
        public string LogoId;
        public string AccountId;
        public string ChannelAccountId;
        public string Token;

        /// <summary>
        /// 用户新手引导步骤
        /// </summary>
        public int Index;

        public int UserId;
        public string UserName;

        public int NpcId;

        /// <summary>
        /// 玩家体力，用于主线战斗
        /// </summary>
        public int Energy;

        /// <summary>
        /// 应援活动体力改为探班体力
        /// </summary>
        public int EncourageEnergy;


        /// <summary>
        /// 星缘回忆体力
        /// </summary>
        public int RecollectionEnergy;

        public long EncourageEnergyTime;

        public long RecollectionEnergyTime;

        public long EnergyTime;

        private int _currentLevelExp = -1;


        public RepeatedField<FirstRechargePB> FirstRecharges;

        public UserMonthCardPB UserMonthCard; //用户月卡状况表

        public MapField<string, int> Birthday;


        /// <summary>
        /// 购买金币次数
        /// </summary>
        public int GoldNum;

        /// <summary>
        /// 购买体力次数
        /// </summary>
        public int PowerNum;

        /// <summary>
        /// 购买星源体力次数
        /// </summary>
        public int EncourageNum;

        /// <summary>
        /// 用户创建时间
        /// </summary>
        public long CreateTime;

        /// <summary>
        /// 额外信息
        /// </summary>
        public UserExtraInfoPB ExtInfo;

        public bool HasGetFreeGemGift = false;

        public bool IsAdult; //身份认证
        public bool Addication; //是否实名认证


        /// <summary>
        ///用户额外信息(头像，头像框) 
        /// </summary>
        public UserOtherPB UserOther;



        /// <summary>
        /// 用户额外奖励信息（实名认证奖励）
        /// </summary>
        public List<int> ExtraAwardInfo=>ExtInfo.ExtraRecord.ToList();
        
        /// <summary>
        /// 是否领取过分享奖励
        /// </summary>
        /// <param 分享类型="shareType"></param>
        /// <returns></returns>
        public bool IsGetShareAward(ShareTypePB shareType)
        {
            if (ExtInfo == null || ExtInfo.ShareAwards == null)
            {
                return false;
            }

            //检查时间是否是今天的

//            if (shareType == ShareTypePB.ShareClothes)
//                return true;

            int id = (int) shareType + 1;

            if (ExtInfo.ShareAwards.Contains(id))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 用户主界面服饰
        /// Key：DressUpType  0是服装 1 是背景
        /// Value: 道具Id
        /// </summary>
        public MapField<int, int> Apparel;

        public void InitData(UserLoginRes userLoginRes, UserDepartmentPB userDepartmentPb)
        {
            CreateTime = userLoginRes.User.CreateTime;
            Level = userDepartmentPb.Level;
            Exp = userDepartmentPb.Exp;
            Gold = userLoginRes.UserMoney.Gold;
            Gem = userLoginRes.UserMoney.Gem;
            LogoId = userLoginRes.User.Logo;
            AccountId = userLoginRes.User.AccountId;
            ChannelAccountId = userLoginRes.User.ChannelAccountId;
            Index = userLoginRes.User.Index;
            UserId = userLoginRes.User.UserId;
            UserName = userLoginRes.User.UserName;

            Apparel = userLoginRes.User.Apparel;

            Birthday = userLoginRes.User.Birthday;
            //HasGetFreeGemGift = false;


            NpcId = (Apparel[0] / 100) % 10;

            Energy = userLoginRes.UserPower.Energy;
            EnergyTime = userLoginRes.UserPower.EnergyTime;

            EncourageEnergy = userLoginRes.UserPower.EncourageEnergy;
            EncourageEnergyTime = userLoginRes.UserPower.EncourageEnergyTime;

            RecollectionEnergy = userLoginRes.UserPower.MemoriesEnergy;
            RecollectionEnergyTime = userLoginRes.UserPower.MemoriesEnergyTime;


            //获取购买金币次数
            GoldNum = userLoginRes.UserBuyGemInfo.GoldNum;


            //获取购买体力次数
            PowerNum = userLoginRes.UserBuyGemInfo.PowerNum;


            //购买应援体力次数 
            EncourageNum = userLoginRes.UserBuyGemInfo.EncourageNum;

            FirstRecharges = userLoginRes.FirstRecharge;
            UserMonthCard = userLoginRes.UserMonthCard;

            ExtInfo = userLoginRes.UserExtraInfo;
            //            var pushDic = new Dictionary<string, string>();
            //            pushDic.Add((ClientTimer.Instance.GetCurrentTimeStamp()+86400000).ToString(),I18NManager.Get("Push_HintOldPlayer"));
            //
        
            UserOther = userLoginRes.User.UserOther;
             

            SdkHelper.PushAgent.InitPushData();
            SdkHelper.PushAgent.Refeash();
        }

        public bool IsOnVip
        {
            get
            {
                bool temp = false;
                var userMonthCard = UserMonthCard;
                if (userMonthCard != null)
                {
                    var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
                    var endTime = userMonthCard.EndTime;
                    if (curTimeStamp < endTime)
                    {
                        temp = true;
                    }
                }

                return temp;
            }
        }

        public void UpDataBGID(int temp)
        {
            NpcId = ((temp / 100) % 10);
        }

        public int MaxEnergy
        {
            get
            {
                int max = GlobalData.DepartmentRule.PowerRules[Level].MaxEnergy;
                if (IsOnVip)
                {
                    max += GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_POWER_NUM);
                }

                return max;
            }
        }

        public int MaxEncourageEnergy
        {
            get { return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_ENCOURAGE_ACT_POWER_MAX_SIZE); }
        }

        public int OldCurrentLevelExp()
        {
            return _currentLevelExp;
        }

        public int CurrentLevelExp
        {
            get
            {
                if (Level == 0)
                {
                    _currentLevelExp = Exp;
                }
                else
                {
                    DepartmentRulePB prevRule = MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support, Level - 1);
                    _currentLevelExp = Exp - prevRule.Exp;
                }

                return _currentLevelExp;
            }
        }

        public int NeedExp
        {
            get
            {
                DepartmentRulePB rulePrev = MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support, Level - 1);
                DepartmentRulePB rule = MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support, Level);
                if (rulePrev == null)
                {
                    return rule.Exp;
                }

                return rule.Exp - rulePrev.Exp;
            }
        }
    }
}