

//假期活动任务ItemVo

using Com.Proto;
using Google.Protobuf.Collections;
using UnityEngine;

public class ActivityTemplateTaskVo
{
	public bool IsDrop;
	public string Desc;	
	public int Max;     //上限
	public int CurNum;
	public HolidayModulePB HolidayModule;
	public string JumpTo;
	public RepeatedField<ActivityDropItemRulePB> DropItems;
	
	public ActivityTemplateTaskVo(ActivityHolidayAwardRulePB pb,int dropId,UserActivityHolidayInfoPB userPb)
	{
		Desc = pb.Desc;
		JumpTo = pb.JumpTo;
		HolidayModule = pb.HolidayModule;
		IsDrop = HolidayModule != HolidayModulePB.ActivityMall;
		DropItems = pb.DropItems;

		//获取上限
		foreach (var t in DropItems)
		{
			if (t.DropItems.ResourceId==dropId )
			{
			
				Max = t.Limit;
				break;
			}
		}

		//获取当前掉落的值
		foreach (var t in  userPb.DropProgressMap)
		{
			if (t.HolidayType==HolidayModule)
			{
				foreach (var item in t.DropItem)
				{
					if (dropId==item.ResourceId)
					{
						CurNum = item.Num;
						break;
					}
				}
				break;
			}
		}
	}
	

}
