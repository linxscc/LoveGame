using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Recollection.Data;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;

public class RecollectionModel : Model
{
	private Dictionary<int, List<RecollectionCardDropVo>> _dropItemDict;

	public Dictionary<int, bool> OpenCardDict { get; private set; }

	public int ResetTimes;

	public int BuyEnergyNum;

	private CardMemoriesRuleRes _rules;
	
	public UserMemoriesMissionPB Mission;

	public RepeatedField<AwardPB> RewardList;
	
	public JumpData JumpData;


	public int ReselatMax;   //星缘回忆重置上线
	
	
	public List<RecollectionCardDropVo> GetCardByPropId(int id)
	{
		if(_dropItemDict == null)
			InitDropData();

		return _dropItemDict[id];
	}

   
   

    public void SetOpenCardDict(RepeatedField<DrawProbPB> resDrawProbs)
	{
		OpenCardDict = new Dictionary<int, bool>();
	
		foreach (var pb in resDrawProbs)
		{
			if (pb.Resource == ResourcePB.Card && OpenCardDict.ContainsKey(pb.ResourceId) == false)
				OpenCardDict.Add(pb.ResourceId, true);
		}
	}

	public int GetResetGem()
	{
		foreach (var pb in _rules.CardMemoriesConsumeRule)
		{
			if (pb.Type == MemoriesConsumeTypePB.MemoriesReselatNum && pb.Num == ResetTimes + 1)
			{
				return pb.Gem;
			}
		}

		return -1;
	}

	
	public int GetResetLastGem()
	{
		int temp = 0;
		foreach (var pb in _rules.CardMemoriesConsumeRule)
		{
			if (pb.Type== MemoriesConsumeTypePB.MemoriesReselatNum)
			{
				temp = pb.Gem;
			}
		}
		
		return temp;		
	}


	public int GetEnergyLastGem()
	{
		int temp = 0;
		foreach (var pb in _rules.CardMemoriesConsumeRule)
		{
			if (pb.Type== MemoriesConsumeTypePB.MemoriesBuyPower)
			{
				temp = pb.Gem;
			}
		}
		return temp;		
	}
	
	
	
	public int GetEnergyGem()
	{
		foreach (var pb in _rules.CardMemoriesConsumeRule)
		{
			if (pb.Type == MemoriesConsumeTypePB.MemoriesBuyPower && pb.Num == BuyEnergyNum + 1)
			{
				return pb.Gem;
			}
		}

		return -1;
	}

	private void InitDropData()
	{
		_dropItemDict = new Dictionary<int, List<RecollectionCardDropVo>>();
		Dictionary<int, CardPB> dict = GlobalData.CardModel.OpenBaseCardDict;
		
		foreach (var pb in dict)
		{
			int propId = pb.Value.MemoriesItem;
                         
			if (_dropItemDict.ContainsKey(propId) == false)
			{
				_dropItemDict.Add(propId, new List<RecollectionCardDropVo>());
			}
		
		   RecollectionCardDropVo vo = new RecollectionCardDropVo(pb.Value, GlobalData.CardModel.GetUserCardById(pb.Value.CardId));
		   _dropItemDict[propId].Add(vo);					
		}	
	}

	public void Init(CardMemoriesInfoRes res)
	{
		RewardList = res.Award;

		BuyEnergyNum = res.UserCardMemoriesInfo.BuyPowerNum;
		ResetTimes = res.UserCardMemoriesInfo.ResetMemoriesNum;		
		Mission = res.UserMemmoriesMission;
	}

	public void InitRule(CardMemoriesRuleRes res)
	{
		_rules = res;
		foreach (var t in _rules.CardMemoriesConsumeRule)
		{
			if (t.Type== MemoriesConsumeTypePB.MemoriesReselatNum)
			{
				ReselatMax++;
			}
		}
	}

	public CardMemoriesMissionPB GetCurrentMission()
	{
		foreach (var missionPb in _rules.CardMemoriesMission)
		{
			if(missionPb.MissionId == Mission.MissionId)
			{
				return missionPb;
			}
		}
		return null;
	}

	public void UpdateInfo(UserCardMemoriesInfoPB pb)
	{
		BuyEnergyNum = pb.BuyPowerNum;
		ResetTimes = pb.ResetMemoriesNum;
		Debug.LogError("重置回忆体力购买"+pb.BuyPowerNum);
	}
}
