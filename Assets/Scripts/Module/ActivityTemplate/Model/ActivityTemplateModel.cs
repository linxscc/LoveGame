
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Newtonsoft.Json;


public class ActivityTemplateModel : Model
{


	
	public int Price;  //单价
	public int PlayMoreNum; //玩多次
	public int ConsumeItemId;  //消耗道具Id
	private int _maxActive;   //活跃上限
	public long EndTimeStamp; // 活动结束时间戳 
	
	
	private UserActivityHolidayInfoPB _userActivityHolidayInfo;  //活动用户信息
	public ActivityTemplateUIVo TemplateUiVo;// 活动模板UI数据
    
    public List<int> playerList;
    public int curPlayerIndex = 0;

    public int curPlayerId
    {
        get
        {
            if(curPlayerIndex >= 0 && curPlayerIndex < playerList.Count)
            {
                return playerList[curPlayerIndex];
            }
            return -1;
        }
    }

    private RepeatedField<ActiveHolidayAwardRulePB> _activeHolidayAwardRules;//活跃奖励规则
	private RepeatedField<ActivityHolidayAwardRulePB> _activityHolidayAwardRulePbs; //
	public Dictionary<int, RepeatedField<AwardPB>> AwardPools;//奖池数据
	private RepeatedField<ActivityDrawRulePB> _activityDrawRulePbs;  //抽卡规则

	public int CurActivityId;

    public RepeatedField<ActiveHolidayAwardRulePB> activeHolidayAwardRules
    {
        get
        {
            return _activeHolidayAwardRules;
        }
    }

    public ActivityTemplateModel()
	{
		InitUiData();
		InitRule();
		InitData();
	}

	private void InitRule()
	{
		CurActivityId = GlobalData.ActivityModel.ActivityDrawTemplateId();
		
		_activeHolidayAwardRules =GetActiveHolidayAwardRules(CurActivityId);		
		_activityHolidayAwardRulePbs =GlobalData.ActivityModel.GetActivityHolidayAwardRules(CurActivityId);
		_activityDrawRulePbs = GlobalData.ActivityModel.GetActivityDrawRules(CurActivityId);
		InitAwardPools();

	}
	
	/// <summary>
	/// 抽奖活动模板活跃奖励规则
	/// </summary>
	/// <returns></returns>
	private RepeatedField<ActiveHolidayAwardRulePB> GetActiveHolidayAwardRules(int activityId)
	{
		RepeatedField<ActiveHolidayAwardRulePB> pbs =new RepeatedField<ActiveHolidayAwardRulePB>();
		foreach (var t in GlobalData.ActivityModel.BaseActivityRule.ActiveHolidayAward)
		{
			if (t.ActivityId== activityId)
			{
				pbs.Add(t);
			}	
		}
		return  pbs;
	}
	
	
	/// <summary>
	/// 假抽奖活动模板所有的奖励
	/// </summary>
	/// <returns></returns>
	private RepeatedField<HolidayAwardPoolRulePB> GetHolidayAwardPoolRules()
	{               
		return GlobalData.ActivityModel.BaseActivityRule.HolidayAwardPoolRules;
	}
	
	
	/// <summary>
	/// 初始化奖池信息
	/// </summary>
	private void InitAwardPools()
	{
        AwardPools = new Dictionary<int, RepeatedField<AwardPB>>();
        playerList = new List<int>();
        var id = GlobalData.ActivityModel.GetCurActivityTemplate(ActivityTypePB.ActivityDrawTemplate)[0].ActivityId;
		foreach (var t in  GetHolidayAwardPoolRules())
		{
			if (t.ActivityId== id)
			{
				var awards = t.HolidayAwardRule;
				foreach (var data in awards)
				{
                    if (!AwardPools.ContainsKey(data.Player))
                    {
                        AwardPools.Add(data.Player, new RepeatedField<AwardPB>());
                        playerList.Add(data.Player);
                        //UnityEngine.Debug.LogWarning("award player:"+data.Player);
                    }
                    foreach (var awardUnit in data.Award) {
                        AwardPB awardPb = new AwardPB
                        {
                            Resource = awardUnit.DropItems.Resource,
                            ResourceId = awardUnit.DropItems.ResourceId,
                            Num = 0,
                        };
                        AwardPools[data.Player].Add(awardPb);
                    }
				}
				break;
			}
		}

        curPlayerIndex = 0;

        

    }

    public RepeatedField<AwardPB> GetCurAwardData()
    {
        if (curPlayerId == -1 || !AwardPools.ContainsKey(curPlayerId)) return null;
        return AwardPools[curPlayerId];
    }

    public void SetCurNextPlayer()
    {
        curPlayerIndex++;
        CheckCurPlayerIndex();
    }

    public void SetCurPrePlayer()
    {
        curPlayerIndex--;
        CheckCurPlayerIndex();
    }

    private void CheckCurPlayerIndex()
    {
        if (curPlayerIndex > playerList.Count - 1)
            curPlayerIndex = 0;
        if(curPlayerIndex < 0)
            curPlayerIndex = playerList.Count - 1;
    }

    private void InitData()
	{
		InitPriceAndPlayMoreNum();
		ConsumeItemId = _activityDrawRulePbs[0].ConsumeType.ResourceId;
		InitMaxActive();
		EndTimeStamp = GlobalData.ActivityModel.GetCurActivityTemplate(ActivityTypePB.ActivityDrawTemplate)[0].EndTime;		
	}


	/// <summary>
	/// 初始化消耗的单价和多玩的次数
	/// </summary>
	private void InitPriceAndPlayMoreNum()
	{				
		foreach (var t in _activityDrawRulePbs)
		{
			if (t.ActivityDrawType==1)
			{
				Price = t.ConsumeType.Num;				
			}
			else if(t.ActivityDrawType==2)
			{
				PlayMoreNum = t.ConsumeType.Num / Price;
			}
		}
	}


	/// <summary>
	/// 初始化活动上限
	/// </summary>
	private void InitMaxActive()
	{
		var list = _activeHolidayAwardRules;
		var count = list.Count;
		var offset = list[count - 1].Weight - list[count - 2].Weight;
		_maxActive= list[count - 1].Weight + offset;	
	}


	/// <summary>
	/// 初始化UI数据
	/// </summary>
	private void InitUiData()
	{
		string fileName = "activitytemplate1";
		string text =new AssetLoader().LoadTextSync(AssetLoader.GetLocalConfiguratioData("ActivityTemplate",fileName));
		TemplateUiVo = JsonConvert.DeserializeObject<ActivityTemplateUIVo>(text);	
	}

	
	
	public void InitUserActivityHolidayInfo(GetUserActivityHolidayInfoRes res)
	{
		if (res.UserActivityHolidayInfoPB.ActivityId== CurActivityId)
		{
			_userActivityHolidayInfo = res.UserActivityHolidayInfoPB;
			GlobalData.PropModel.UpdateProps(new[] {res.UserItem});
		}
		
	}

	/// <summary>
	///更新用户假期活动活跃Info 
	/// </summary>
	/// <param name="res"></param>
	public void UpdateUserActivityHolidayInfo(UserActivityHolidayInfoPB pb)
	{
		if (pb.ActivityId== CurActivityId)
		{
			_userActivityHolidayInfo = pb;			
		}		
	}
	
	/// <summary>
	/// 获取用户假期活动活跃Info
	/// </summary>
	/// <returns></returns>
	public UserActivityHolidayInfoPB GetUserActivityHolidayInfo()
	{		
		return _userActivityHolidayInfo;
	}
	
	
	/// <summary>
	/// 获取国庆节活动任务数据
	/// </summary>
	/// <returns></returns>
	public List<ActivityTemplateTaskVo> GetActivityTaskVo()
	{
		List<ActivityTemplateTaskVo> vos =new List<ActivityTemplateTaskVo>();
		var baseRules = _activityHolidayAwardRulePbs;
		
		for (int i = 0; i < _activityHolidayAwardRulePbs.Count; i++)
		{
			if (i==0)
			{
				ActivityTemplateTaskVo vo =new ActivityTemplateTaskVo(baseRules[i],ConsumeItemId,_userActivityHolidayInfo);
				vos.Add(vo);
			}
			else
			{
				if (baseRules[i].HolidayModule!=baseRules[i-1].HolidayModule)
				{
					ActivityTemplateTaskVo vo =new ActivityTemplateTaskVo(baseRules[i],ConsumeItemId,_userActivityHolidayInfo);
					vos.Add(vo);
				}
			}
		}	
		return vos;
	}
	
	
	/// <summary>
	/// 进度条的值
	/// </summary>
	/// <returns></returns>
	public float Progress()
	{
		float progress =0;
		var drawCount = _userActivityHolidayInfo.DrawCount;	  
		if(drawCount!=0)
		{
			float fixedDis = 162.5f;
			var list = _activeHolidayAwardRules;
			int index=-1;
			for (int i = 0; i < list.Count; i++)
			{
				if (drawCount<=list[i].Weight)
				{
					index = i;
					break;
				}
			}
			if (index==-1)
			{
				var maxActive = _maxActive;
				if (drawCount>=maxActive)
				{
					progress = fixedDis * 6; 
				}
				else
				{
					var lastIndex = list.Count - 1;
					var offset = maxActive - list[lastIndex].Weight;				
					var num = drawCount - list[lastIndex].Weight;
					progress = (list.Count * fixedDis) + fixedDis / offset * num; 
				}
			 
			}
			else
			{
				if (index==0)
				{
					progress = fixedDis / list[index].Weight*drawCount;
				}
				else
				{
					var offset = list[index].Weight - list[index - 1].Weight;
					var num = drawCount - list[index - 1].Weight;
					progress = (index * fixedDis) + fixedDis / offset * num;
				}
			}		  
		}
		return progress;
	}
	
	


	/// <summary>
	///活动道具Num
	/// </summary>
	/// <returns></returns> 
	public int ActivityItemNum()
	{
        //var id= GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.HOLIDAY_ACTIVITY_ITEM_CONSUME);
        var id = ConsumeItemId;
        if (GlobalData.PropModel.GetUserProp(id)==null)
		{
			return 0;
		}	 
		return GlobalData.PropModel.GetUserProp(id).Num;
	}
	
	
	
	public RepeatedField<AwardPB> GetActiveAward(int weight)
	{
		foreach (var t in _activeHolidayAwardRules)
		{
			if (t.Weight==weight)
			{
				return t.Award;
			}
		}
		return null;
	} 
	
	
	public int Weight(int index)
	{
		return _activeHolidayAwardRules[index].Weight;
	}
	
	
}
