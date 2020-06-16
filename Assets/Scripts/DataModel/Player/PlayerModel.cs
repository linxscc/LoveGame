using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using game.main;
using Google.Protobuf.Collections;
using UniRx.Triggers;
using UnityEngine;

namespace DataModel
{
    /// <summary>
    /// 玩家基本数据
    /// </summary>
    public class PlayerModel : Model
    {
        public PlayerVo PlayerVo;

        //基础应援会能力
        public int BaseSupportPower;

        public PlayerModel()
        {
            PlayerVo = new PlayerVo();
        }

        public List<BuyGemRulePB> BuyGemRules; //购买体力/金币/星源体力，规则的集合


        /// <summary>
        /// 分享规则
        /// </summary>
        public List<ShareRulePB> ShareRules;
        
        /// <summary>
        /// 购买金币上限
        /// </summary>
        public int BuyGoldUpperlimit;

        /// <summary>
        /// 购买体力上限
        /// </summary>
        public int BuyPowerUpperlimit;

        /// <summary>
        /// 购买星缘体力上限
        /// </summary>
        public int BuyEncouragePowerUpperlimit;

        public UserLoginRes UserLoginRes;

        //public Dictionary<string, string> PowerMaxPushTime;

       

        /// <summary>
        /// 初始化购买钻石集合
        /// </summary>
        /// <param name="res">规则</param>
        public void InitRule(UserRuleRes res)
        {
            //PowerMaxPushTime=new Dictionary<string, string>();
            if (BuyGemRules == null)
            {
                BuyGemRules = new List<BuyGemRulePB>();
                BuyGemRules = res.BuyGemRules.ToList();
            }

            BuyGoldUpperlimit = 0;
            BuyPowerUpperlimit = 0;
            BuyEncouragePowerUpperlimit = 0;
            for (int i = 0; i < BuyGemRules.Count; i++)
            {
                switch (BuyGemRules[i].BuyType)
                {
                    case BuyGemTypePB.BuyGold:
                        BuyGoldUpperlimit++;
                        break;
                    case BuyGemTypePB.BuyPower:
                        BuyPowerUpperlimit++;
                        break;
                    case BuyGemTypePB.BuyEncouragePower:
                        BuyEncouragePowerUpperlimit++;
                        break;
                }
            }
                                
            
           GuideManager.InitGuideRule(res.GuideRules);
           ShareRules = res.ShareRules.ToList();
 
        }


        public BuyGemRulePB GetBuyGemRule(BuyGemTypePB pb, int buyNum)
        {
            BuyGemRulePB temp = null;
            buyNum = buyNum + 1;
            for (int i = 0; i < BuyGemRules.Count; i++)
            {
                if (pb == BuyGemRules[i].BuyType && buyNum == BuyGemRules[i].Num)
                {
                    temp = BuyGemRules[i];
                    break;
                }
            }

            return temp;
        }


        public string GetBuyWindowPresonImageName(int bgPicId)
        {                    
            return "Background/PersonIcon/" + bgPicId;
        }


        public int _restoreEncourage;
        private int _restorePower;

        public void InitData()
        {
           
            List<DepartmentVo> arr = GlobalData.DepartmentData.MyDepartments;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].UserDepartmentPb.DepartmentType == DepartmentTypePB.Support)
                {
                    PlayerVo.InitData(UserLoginRes, arr[i].UserDepartmentPb);
                    break;
                }
            }

            _restorePower= GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_INFO_POWER_ONE_TIME);
            
            _restoreEncourage =
                GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_ENCOURAGE_ACT_POWER_ONE_TIME);
            ClientTimer.Instance.AddCountDown("PlayerEnergyHandler", long.MaxValue, 5, PlayerEnergyHandler, null);
            //这里要加体力上限的推送
            if (PlayerVo.Energy<PlayerVo.MaxEnergy)
            {
                var tipsTime = PlayerVo.EnergyTime + _restorePower * 60 * 1000 * (PlayerVo.MaxEnergy - PlayerVo.Energy);
                Debug.LogError(DateUtil.GetDataTime(tipsTime));
                //PowerMaxPushTime.Add(tipsTime.ToString(),I18NManager.Get("Push_HintPowerMax"+Random.Range(0,3)));
                SdkHelper.PushAgent.PushPowerMax(tipsTime);
            }
            
         
            GuideManager.InitData(UserLoginRes.UserGuide);//userLoginRes.UserGuide
        }

        private void PlayerEnergyHandler(int interval)
        {
            long timestamp = ClientTimer.Instance.GetCurrentTimeStamp();

            //每_restoreEncourage分钟增加1点体力
            if (timestamp - PlayerVo.EnergyTime >= _restorePower * 60 * 1000 && PlayerVo.Energy < PlayerVo.MaxEnergy)
            {
                PlayerVo.Energy += 1;
                PlayerVo.EnergyTime = timestamp;

                EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
            }

            if (timestamp - PlayerVo.EncourageEnergyTime >= _restoreEncourage * 60 * 1000 &&
                PlayerVo.EncourageEnergy < PlayerVo.MaxEncourageEnergy)
            {
                PlayerVo.EncourageEnergy += 1;
                PlayerVo.EncourageEnergyTime = timestamp;

                EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
            }

            int restore = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_MEMORIES_POWER_ONE_TIME);
            if (PlayerVo.RecollectionEnergy < 90 &&
                timestamp - PlayerVo.RecollectionEnergyTime >= restore * 60 * 1000)
            {
                PlayerVo.RecollectionEnergy += 1;
                PlayerVo.RecollectionEnergyTime = timestamp;

                EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
            }
        }

        /// <summary>
        /// 更新购买次数
        /// </summary>
        public void UpDataBuyNum(UserBuyGemInfoPB pB)
        {
            PlayerVo.GoldNum = pB.GoldNum;
            PlayerVo.PowerNum = pB.PowerNum;
            PlayerVo.EncourageNum = pB.EncourageNum;
        }


        public void UpdateUserMoney(UserMoneyPB pb)
        {
            PlayerVo.Gold = pb.Gold;
            PlayerVo.Gem = pb.Gem;
            EventDispatcher.TriggerEvent(EventConst.UpdateUserMoney);
        }

        /// <summary>
        /// 增减玩家金币
        /// </summary>
        /// <param name="goldAdd">负值是减少</param>
        public void UpdateUserGold(int goldAdd)
        {
            PlayerVo.Gold += goldAdd;
            EventDispatcher.TriggerEvent(EventConst.UpdateUserMoney);
        }

        /// <summary>
        /// 增减玩家钻石
        /// </summary>
        /// <param name="gemAdd">负值是减少</param>
        public void UpdateUserGem(int gemAdd)
        {
            PlayerVo.Gem += gemAdd;
            EventDispatcher.TriggerEvent(EventConst.UpdateUserMoney);
        }

        public bool AddExp(int exp)
        {
            if (IsMaxLevel(PlayerVo.Level))
            {
                return false;
            }
            
            
            PlayerVo.Exp += exp;
            int currentLevel = PlayerVo.Level;
            int afterLevel = GetLevelByExp(PlayerVo.Exp);

            //var levelupNum = afterLevel - currentLevel;

            if (afterLevel > currentLevel)
            {
                PlayerVo.PreLevel = currentLevel;
                PlayerVo.Level = afterLevel;
                //AddPower(GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.UPGRADE_DEPARTMENT_POWER_NUM)*levelupNum);
                GlobalData.DepartmentData.UpdateDepartmentMainValue();
                EventDispatcher.TriggerEvent(EventConst.UserLevelUp);
                AddPower(GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.UPGRADE_DEPARTMENT_POWER_NUM)*(afterLevel - currentLevel));
                return true;
            }

            return false;
        }

        private int GetLevelByExp(int exp)
        {
            var rules = GlobalData.DepartmentRule.DepartmentRules;

            for (int i = 0; i < rules.Count; i++)
            {
                if (rules[i].DepartmentType == DepartmentTypePB.Support && exp < rules[i].Exp)
                {
                    return rules[i].Level;
                }
            }

            return 0;
        }

        private bool IsMaxLevel(int level)
        {
            var rules = GlobalData.DepartmentRule.DepartmentRules;
            for (int i = 0; i < rules.Count; i++)
            {
                if (rules[i].DepartmentType == DepartmentTypePB.Support && level < rules[i].Level)
                {
                    return false;
                }
                
            }

            return true;
        }
        

        public void UpdateUserPower(UserPowerPB pb)
        {
            PlayerVo.EncourageEnergy = pb.EncourageEnergy;
            PlayerVo.Energy = pb.Energy;
            PlayerVo.EnergyTime = pb.EnergyTime;
            PlayerVo.RecollectionEnergy = pb.MemoriesEnergy;

            if (PlayerVo.Energy<PlayerVo.MaxEnergy)
            {
                //PowerMaxPushTime.Clear();
                var tipsTime = PlayerVo.EnergyTime + _restorePower * 60 * 1000 * (PlayerVo.MaxEnergy - PlayerVo.Energy);
                Debug.LogError(DateUtil.GetDataTime(tipsTime));
                //PowerMaxPushTime.Add(tipsTime.ToString(),I18NManager.Get("Push_HintPowerMax"+Random.Range(0,3)));
                SdkHelper.PushAgent.PushPowerMax(tipsTime);
            }

            EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
        }

        public void AddPower(int powerAdd)
        {
            PlayerVo.Energy += powerAdd;
            EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
        }

        /// <summary>
        /// 星缘回忆体力（记忆碎片）
        /// </summary>
        /// <param name="powerAdd"></param>
        public void AddRecollectionEnergy(int powerAdd)
        {
            PlayerVo.RecollectionEnergy += powerAdd;
            EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
        }

        public void SetUserName(string userName)
        {
            PlayerVo.UserName = userName;
        }

        public void UpdataUserGameName(UserPB pB, UserExtraInfoPB extraInfoPB)
        {
            PlayerVo.ExtInfo = extraInfoPB;
            PlayerVo.UserName = pB.UserName;
            EventDispatcher.TriggerEvent(EventConst.UpDataUserName, PlayerVo);
        }

        public void UpdataUserExtra(UserExtraInfoPB extraInfoPB)
        {
            PlayerVo.ExtInfo = extraInfoPB;
        }

        /// <summary>
        /// 更新首冲数据
        /// </summary>
        /// <param name="repeated"></param>
        public void UpdataFirstRecharges( RepeatedField<FirstRechargePB> repeated)
        {
            PlayerVo.FirstRecharges = repeated;
        }

        public void SetUser(UserPB user)
        {
            PlayerVo.UserId = user.UserId;
            PlayerVo.AccountId = user.AccountId;
            PlayerVo.Index = user.Index;
            PlayerVo.LogoId = user.Logo;
            PlayerVo.CreateTime = user.CreateTime;
            PlayerVo.UserName = user.UserName;
            PlayerVo.UserOther = user.UserOther;
            PlayerVo.ChannelAccountId = user.ChannelAccountId;
            
            PlayerVo.Apparel = user.Apparel;
        }
    }
}