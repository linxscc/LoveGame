using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using game.main;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace DataModel
{
	public class FavorabilityMainModel : Model
	{

		public List<DressUpUnlockRulePB> DressUpUnlockRuleLists; //换装服饰信息
        public List<UserLevelPB> UserLevels;   //玩家关卡信息
        public List<FavorabilityLevelRulePB> FavorabilityLevelRuleLists;	//好感度等级规则。
		public UserFavorabilityInfoPB UserFavorabilityInfoPb;	//玩家角色好感度信息 		
		public List<UserFavorabilityVo> UserFavorabilityList;	//用户好感度数据
		public UserFavorabilityVo CurrentRoleVo;	//当前选中角色,用户数据刷新的话记得要刷新该数据！       
        public List<SceneUnlockRulePB> SceneUnlockRulePbs;
        public List<FavorabilityItemPB> FavorabilityItemPBLists;

        private List<FavorabilityNpcInfo> _npcInfos;

        private void InitNpcInfo()
        {	       
	        string fileName = "NpcInfo";
	        string text =new AssetLoader().LoadTextSync(AssetLoader.GetLocalConfiguratioData("FavorabilityNpcInfo",fileName));
	        _npcInfos =  JsonConvert.DeserializeObject<List<FavorabilityNpcInfo>>(text);	      
        }

        public FavorabilityNpcInfo GetNpcInfo(PlayerPB playerPb)
        {
	        var npcId = (int) playerPb;
	        FavorabilityNpcInfo info = null;
	        foreach (var t in _npcInfos)
	        {
		        if (t.NpcId==npcId)
		        {
			        info= t;
			        break;
		        }
	        }
	        return info;
        }
        
        
        public void Init(FavorabilityRuleRes res)
        {
	        InitNpcInfo();
			if (FavorabilityLevelRuleLists==null)
			{
				FavorabilityLevelRuleLists=new List<FavorabilityLevelRulePB>();
				FavorabilityLevelRuleLists = res.FavorabilityLevelRules.ToList();
			
			}
            if (FavorabilityItemPBLists==null)
            {
                FavorabilityItemPBLists = new List<FavorabilityItemPB>();
                FavorabilityItemPBLists = res.FavorabilityItems.ToList();           
            }
            if (DressUpUnlockRuleLists==null)
			{
				
                DressUpUnlockRuleLists =new List<DressUpUnlockRulePB>();
                DressUpUnlockRuleLists = res.DressUpUnlockRules.ToList();                
                InitBgImageDic();
			}

			if (SceneUnlockRulePbs==null)
			{
				SceneUnlockRulePbs=new List<SceneUnlockRulePB>();				
				SceneUnlockRulePbs = res.SceneUnlockRules.ToList();

			}					
		}

     

        public void GetUserFavorabilityData(RepeatedField<UserFavorabilityPB> pbs)
		{
			
			if (UserFavorabilityList==null)
			{
				UserFavorabilityList=new List<UserFavorabilityVo>();
								
			}
			
			foreach (var v in pbs)
			{    			
			 //	Debug.LogError(v);
				var vo=new UserFavorabilityVo(v);
				
				UserFavorabilityList.Add(vo);
			}    
		}


        public void UpdateUserFavorability(UserFavorabilityPB vo)
        {
            for (int i = 0; i < UserFavorabilityList.Count; i++)
            {
                if (vo.Player == UserFavorabilityList[i].Player)
                {
                    UserFavorabilityList[i].UserId = vo.UserId;
                    UserFavorabilityList[i].Player = vo.Player;

                    UserFavorabilityList[i].Apparel = vo.Apparel;
                    UserFavorabilityList[i].Level = vo.Level;
	                UserFavorabilityList[i].Exp = GetCurExp(vo.Exp, vo.Level);	                
                    UserFavorabilityList[i].VoiceKeep = vo.VoiceKeep;
                    UserFavorabilityList[i].ShowExp = vo.Exp;
                    break;
                }
            }
        }

     




        public UserFavorabilityVo GetUserFavorabilityVo(int playerPb)
		{
			foreach (var vo in UserFavorabilityList)
			{
				if ((int)vo.Player==playerPb)
				{
					return vo;
				}
			}			
			Debug.LogError("No Such Player"+playerPb);
			return null;
        }

    
        public void UpdataCurrentRoleVo(UserFavorabilityPB vo)
        {	    
	        foreach (var v in UserFavorabilityList)
	        {
		        if (v.Player==vo.Player)
		        {
			        v.Level = vo.Level;
			        v.Apparel = vo.Apparel;
			        v.Exp = GetCurExp(vo.Exp, vo.Level);
			       
			        v.VoiceKeep = vo.VoiceKeep;
			        v.ShowExp = vo.Exp;
		        }         		        
	        }
	        	        
            CurrentRoleVo.UserId = vo.UserId;
            CurrentRoleVo.Player = vo.Player;
            CurrentRoleVo.Apparel = vo.Apparel;
            CurrentRoleVo.Level = vo.Level;
		    CurrentRoleVo.Exp = GetCurExp(vo.Exp,vo.Level);   	        	        
            CurrentRoleVo.VoiceKeep = vo.VoiceKeep;
            CurrentRoleVo.ShowExp = vo.Exp;
        }


        public string GetPlayerName(PlayerPB playerPb)
		{
			switch (playerPb)
			{
				case PlayerPB.ChiYu:

                    return I18NManager.Get("Common_Role4");
                case PlayerPB.YanJi:

                    return I18NManager.Get("Common_Role3");

                case PlayerPB.QinYuZhe:

                    return I18NManager.Get("Common_Role2");

                case PlayerPB.TangYiChen:

                    return I18NManager.Get("Common_Role1");
				default:
                    return I18NManager.Get("Common_Invalid");

            }
			
		}




       

    
		public FavorabilityLevelRulePB GetCurrentLevelRule(int curlevel)
		{
			foreach (var v in FavorabilityLevelRuleLists)
			{
				//要判断是否为当前升级所需经验
				if (v.Level==curlevel)
				{
					return v;
				}
			}

			Debug.LogError("no current level");
			return null;

		}


		public int GetCurrentLevelExpNeed(int curLevel)
		{
			for (int i = 0; i < FavorabilityLevelRuleLists.Count; i++)
			{
				if (FavorabilityLevelRuleLists[i].Level==curLevel)
				{
					if (i==0)
					{
						return FavorabilityLevelRuleLists[i].Exp;
					}
					else
					{
						return FavorabilityLevelRuleLists[i].Exp - FavorabilityLevelRuleLists[i - 1].Exp;
					}
				}				
			}			          
			return 0;
		}

		public int GetCurExp(int curExp,int level)
		{
			if (level>1)
			{
				return curExp - GetCurrentLevelRule(level - 1).Exp;
			}
			else
			{
				return curExp;
			}							
		}


       
        /// <summary>
        /// 得到好感度等级最后一个规则
        /// </summary>
        /// <returns></returns>
        public FavorabilityLevelRulePB GetLatsFavorabilityLevelRulePB()
        {
            var _lastIndex = FavorabilityLevelRuleLists.Count;	       
            return FavorabilityLevelRuleLists[_lastIndex - 1];
        }

		public int GetLastExp()
		{
			return FavorabilityLevelRuleLists[FavorabilityLevelRuleLists.Count - 2].Exp;
		}

        public SceneUnlockRulePB GetUnlockRulePb(int sceneid)
		{
			foreach (var v in SceneUnlockRulePbs)
			{
				if (v.SceneId==sceneid)
				{
					return v;
				}
			}
			
			Debug.LogError("NO SUCH SCENE"+sceneid);

			return null;
		}

		public DressUpUnlockRulePB IsUnlockCloth(int cardId)
		{
			foreach (var v in DressUpUnlockRuleLists)
			{
                //if (v.UnlockClaim.CardId==cardId)
                //{
                //	return v;
                //}

                if (v.ClothesGoal.ChangeNum == cardId)
                {
                    return v;
                }
			}
			
			Debug.LogError("No Such Cloth");
			return null;

		}

		public string GetPhoneEventType(int id)
		{
			switch (id)
			{
				case	1:
					return I18NManager.Get("Phone_Sms");				
				case	2:
					return I18NManager.Get("Phone_Call");			
				case	3:
					return I18NManager.Get("Phone_Friendscircle");			
				case	4:
					return I18NManager.Get("Phone_Weibo");	
				
				default:
					return I18NManager.Get("Common_Hint6");	
			}
						
		}


		private List<UserVisitingLevelPB> _myVisiting;
		public void OnMyVisitingHandler(MyVisitingRes res)
		{			
			_myVisiting =new  List<UserVisitingLevelPB>();
			_myVisiting = res.UserLevels.ToList();
		}

		public bool IsPassVisit(int levelId)
		{
			if (_myVisiting==null)
			{
				return false;
			}
			
			foreach (var t in _myVisiting)
			{
				if (t.LevelId==levelId && t.Star!=0)
				{
					return true;
				}
			}

			return false;
		}
		
		
		private Dictionary<int,List<string>> _bgImageDic =new Dictionary<int, List<string>>();


		private void InitBgImageDic()
		{
			foreach (var t in DressUpUnlockRuleLists)
			{
				if (t.ItemType== DressUpTypePB.TypeBackground)
				{
					List<string> temp =new List<string>{t.MornImage,t.NoonImage,t.AfternoonImage};
					_bgImageDic.Add(t.ItemId,temp);
				}
			}
			
		}

		public DressUpUnlockRulePB GetDressUpUnlockRulePb(int itemId)
		{
			foreach (var v in DressUpUnlockRuleLists)
			{
				if (v.ItemId==itemId)
				{
					return v;
				}
				
			}
			
			return null;
		}
		
		
		public string GetBgImageName(int backDrop)
		{
			int index = 0;
			var dt = DateUtil.GetTodayDt();
			float minute = dt.Hour * 60 + dt.Minute; //转成分钟计算 1h=60m
			if (0<minute && minute <=6*60f)   //0点~6点
			{
				index = 2;
			}
			else if(6 * 60f < minute && minute <= 16 * 60f)//6点~16点
			{
				index = 0; 
			}
			else if (16 * 60f < minute && minute <= 19 * 60f) //16点~19点
			{
				index = 1;
			}
			else if (19 * 60f < minute && minute <= 24 * 60f)//19点~24点
			{
				index = 2; 
			}
			
			return _bgImageDic[backDrop][index];
		}


		public UserFavorabilityVo GetUserFavorability(int roleId)
		{
			foreach (var t in UserFavorabilityList)
			{
				if ((int)t.Player==roleId)
				{
					return t;
				}
			}

			return null;
		}
    }


}



