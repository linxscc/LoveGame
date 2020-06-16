using System;
using System.Collections.Generic;
using Com.Proto;
using Google.Protobuf.Collections;

namespace game.main
{

	/// <summary>
	/// 约会规则数据
	/// </summary>
	public class AppointmentRuleVo
	{
		public int Id;
		public List<int> ActiveCards; //解锁需要获得的卡牌
		public int ActiveFavor;     //解锁需要的好感度
		public List<AppointmentGateRulePB> GateInfos;  //每关需要的数据
		public string Name;         //约会名字
		public string Sweetness;//甜蜜度

		public AppointmentRuleVo(AppointmentRulePB appointmentRulePb)
		{
			Id = appointmentRulePb.Id;
			ActiveCards=new List<int>();
			foreach (var v in  appointmentRulePb.ActiveCards)
			{
				ActiveCards.Add(v);
			}

			ActiveFavor = appointmentRulePb.ActiveFavor;
			
			GateInfos=new List<AppointmentGateRulePB>();
			foreach (var v in appointmentRulePb.GateInfos)
			{
				GateInfos.Add(v);
			}

//			PicSmallId = appointmentRulePb.PicSmallId;
//			PicBigId = appointmentRulePb.PicBigId;
			Name = appointmentRulePb.Name;
			Sweetness = appointmentRulePb.Sweetness;
		}
		
		public string SmallPicPath
		{
//			get { return "Card/Image/SmallCard" + PicSmallId; }
			get { return "Head/" + ActiveCards[0]; }
		}

		public string EvoSmallPicPath
		{
//			get { return "Card/Image/SmallCard" + PicSmallId; }
			get { return "Head/EvolutionHead/" + ActiveCards[0]; }
		}
		
		public string StoryPicPath
		{
			get { return "Card/Image/" + ActiveCards[0]; }
		}

		public string EvoStoryPicPath
		{
			get { return "Card/Image/EvolutionCard/" + ActiveCards[0]; }
		}
		
	}

	public class AppointmentGateRuleVo
	{
		public int Gate;    //关卡
		public Dictionary<int, int> CosumesDic;  //解锁需要消耗的道具
		public string SceneId;    //对应的剧情
		public int Star;   //需要的卡牌星级
		public int Evo;    //卡牌是否需要进化0否1是
		public RepeatedField<AwardPB> AwardPbs;	//通关关卡获得的奖励

		public AppointmentGateRuleVo(AppointmentGateRulePB pb)
		{
			Gate = pb.Gate;
			if (CosumesDic == null)
			{
				CosumesDic=new Dictionary<int, int>();
				foreach (var v in pb.Cosumes)
				{
					CosumesDic.Add(v.Key,v.Value);
				}
			}

			SceneId = pb.SceneId;
			Star = pb.Star;
			Evo = pb.Evo;
			AwardPbs = pb.Awards;
		}
	}

    
}