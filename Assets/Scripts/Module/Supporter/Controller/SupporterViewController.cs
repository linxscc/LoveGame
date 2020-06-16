using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Supporter.Data;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using Utils;

public class SupporterViewController : Controller {
	
	public SupporterFansView View;
	
	private SupporterModel _model;
	private SupporterAwardWindow _supporterAwardWindow;

	public override void Init()
	{
		EventDispatcher.AddEventListener(EventConst.UpdataSupporterFansViewName, UpdataSupporterFansViewName); 
	}

	private void UpdataSupporterFansViewName()
	{
		View.UpdataSupporterFansViewName();
	}
	
	public override void Start()
	{
		_model = GetData<SupporterModel>();
		_model.InitData(GlobalData.DepartmentData);
		if (_model.FansList.Count >= 1)
		{
			_model.FansList.Sort((vo1, vo2) =>
			{
				if (vo1.Num > vo2.Num) return -1;
				if(vo1.Num<vo2.Num)
				{
					return 1;
				}
				return 0;
			});
		}
		View.SetData(_model);
//		View.SetFansData(_model.FansList.Count>=1?_model.FansList[0]:null,
//			_model.FansList.Count>=2?_model.FansList[1]:null);
	}

	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEvent(EventConst.UpdataSupporterFansViewName);
	}

	/// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
	        case MessageConst.CMD_SUPPOTER_UPGRADE:
		        UpgradeSupporter((SupporterVo)message.Body);
		        break;	
			case MessageConst.CMD_SUPPOTER_RECEIVEGIFT:
				ReceiveAward((DepartmentTypePB)message.Body);
				break;
        }
    }

	private void UpgradeSupporter(SupporterVo vo)
	{
		LoadingOverlay.Instance.Show();
		//Debug.LogError(vo.CostNum);
		byte[] buffer = NetWorkManager.GetByteData(new UpgradeDepartmentsReq
		{
			DepartmentType = vo.type,
		});
		NetWorkManager.Instance.Send<UpgradeDepartmentsRes>(CMD.DEPARTMENTC_UPGRADEDEPARTMENTS, buffer, OnUpgradeSupporter);
	}

	private void OnUpgradeSupporter(UpgradeDepartmentsRes res)
	{
		LoadingOverlay.Instance.Hide();
		_model.Update(res.MyDepartments);	
		GlobalData.PropModel.UpdateProps(new []{res.UserItem});
		//FlowText.ShowMessage("使用道具成功");
        FlowText.ShowMessage(I18NManager.Get("Supporter_Hint10"));
        View.SetData(_model,res.MyDepartments.DepartmentType);
//		Debug.LogError(res.UserItem);
	}

	private void ReceiveAward(DepartmentTypePB pb)
	{
		LoadingOverlay.Instance.Show();
		byte[] buffer = NetWorkManager.GetByteData(new DepartmentAwardsReq()
		{
			DepartmentType = pb
		});
//		Debug.LogError(pb);
		NetWorkManager.Instance.Send<DepartmentAwardsRes>(CMD.DEPARTMENTC_RECEIVEAWARD, buffer, OnReceiveGift);
	}

	private void OnReceiveGift(DepartmentAwardsRes res)
	{
		LoadingOverlay.Instance.Hide();
		RewardUtil.AddReward(res.Awards);
		_model.Update(res.MyDepartment,false);

		View.SetData(_model,res.MyDepartment.DepartmentType);

		if (res.Awards.Count>0)
		{
            //FlowText.ShowMessage("成功领取奖励");
            FlowText.ShowMessage(I18NManager.Get("Supporter_Hint11"));
            if (_supporterAwardWindow==null)
			{
				_supporterAwardWindow=PopupManager.ShowWindow<SupporterAwardWindow>("Supporter/Prefabs/SupporterAwardWindow");
			}
		}
		else
		{
			//FlowText.ShowMessage("没有奖励？");
            FlowText.ShowMessage(I18NManager.Get("Supporter_Hint12"));
        }
		
		//没有礼物的时候也可以触发！
		_supporterAwardWindow.SetData(res.Awards);
		
		foreach (var award in res.Awards)
		{
			if (award.Resource==ResourcePB.Gem)
			{
				SdkHelper.StatisticsAgent.OnReward(award.Num,"应援会部门等级提升");   
			}
		}

	}
}
