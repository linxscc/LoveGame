using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using UnityEngine;

namespace Module.Guide.ModuleView.Activity
{
	public class ActivityGuideView : View
	{

		private Transform _content;
		private Transform _award;
		private Transform _goToMainLine;
		
		
		
		
		
		private void Awake()
		{
			_content = transform.Find("Bg");
			_award = transform.Find("Award");
			_goToMainLine = transform.Find("GoToMainLine");
			
			_award.Find("GetAward").localScale =new Vector3(0,0,1f);
			_goToMainLine.Find("Bg").localScale =new Vector3(0,0,1f);
			
			ClientData.LoadItemDescData(null);
			ClientData.LoadSpecialItemDescData(null);
		}


		public void Step1()
		{
		    //找到点击区域
		    var firstDay =_content.Find("SevenDaysAward/1");
		    _content.gameObject.Show();
		    
		   GuideArrow.DoAnimation(firstDay);
		    
		    PointerClickListener.Get(firstDay.gameObject).onClick = go =>
		    {
			    //打领取的点
			   //  GuideManager.SetRemoteGuideStep( GuideTypePB.MainGuide,GuideConst.MainLineStep_OnClick_SevenDayActiviy_FirstDay_Award);
			  
			   //统计数据点
			   GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_SevenDayActiviy_FirstDay_Award);
				
			   //发消息去领第一天奖励				
			    SendMessage(new Message(MessageConst.CMD_ACTIVITY_SEVENDAYGUIDE,Message.MessageReciverType.UnvarnishedTransmission));
			    SetShowAwardData();
			    ClientTimer.Instance.DelayCall(() =>
			    {
				    _content.gameObject.Hide();				  
				    ShowAward();				    				   
			    }, 0.3F);
			    
		    };
		    
		}


		private void SetShowAwardData()
		{
			var parent = _award.Find("GetAward/Awards");			
			var list = GuideManager.GetGuideAwardsToRule(GuideTypePB.MainGuide);
			List<AwardPB>temp =new List<AwardPB>();
			
			temp.Add(list[1]);
			temp.Add(list[2]);
			for (int i = 0; i < temp.Count; i++)
			{
			    RewardVo vo = new RewardVo(temp[i]);	
			    var item= parent.GetChild(i);
			    item.transform.GetText("PropNameText").text = vo.Name;
			    item.transform.GetText("Num").text = vo.Num.ToString();
				item.transform.GetRawImage("PropImage").texture = ResourceManager.Load<Texture>(vo.IconPath,ModuleName);
				PointerClickListener.Get(item.gameObject).onClick = go =>
				{					
					var desc = ClientData.GetItemDescById(vo.Id,vo.Resource);
					if (desc!=null)
					{
						FlowText.ShowMessage(desc.ItemDesc); 			
					}
				};	 	
			}
		}
		
		private void ShowAward()
		{
			_award.gameObject.Show();
		    _award.Find("GetAward").DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
				{					
					PointerClickListener.Get(_award.gameObject).onClick = go =>
						{
							_award.Find("GetAward").DOScale(new Vector3(0, 0, 1), 0.2F).SetEase(Ease.OutExpo)
								.onComplete = ShowGoToMainLine;
						};
				});
			
		}

		private void ShowGoToMainLine()
		{
			_award.gameObject.Hide();
			_goToMainLine.gameObject.Show();
			_goToMainLine.Find("Bg").DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
			{
				_goToMainLine.GetButton("Bg/Button").onClick.AddListener(() =>
				{
					GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_GoToMainLine);
					
					_goToMainLine.Find("Bg").DOScale(new Vector3(0, 0, 1), 0.2F).SetEase(Ease.OutExpo).OnComplete(() => 
					{ 						
							gameObject.Hide();						
							ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE);
					});
				});
			});
			
		}
		
	}
}
