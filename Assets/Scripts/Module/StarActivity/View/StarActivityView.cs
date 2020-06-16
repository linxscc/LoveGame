using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;
using UnityEngine.EventSystems;

public class StarActivityView :View
	{


		private Transform _togglesTran;              
		private Text _activityResidueTime;
		private LoopVerticalScrollRect _loopVertical;
		private RectTransform _progressBar;
		private Transform _tabBarTran;
		private List<UserMissionVo> _data;		
		private MissionModel _missionModel;
		private float _max;

		private RectTransform _togRect;
		private void Awake()
		{
			_togglesTran = transform.Find("Content/Toggles");
			_activityResidueTime = transform.GetText("Content/ActivityTime/Text");
			_progressBar = transform.GetRectTransform("Content/ProgressBar/Mask");
			_tabBarTran = transform.Find("Content/TabBar");
			_max = transform.Find("Content/ProgressBar").GetRectTransform().sizeDelta.x;
			_togRect = transform.GetRectTransform("BgContent/TopBg");
			
			_loopVertical = transform.Find("Content/TaskContent").GetComponent<LoopVerticalScrollRect>();
			_loopVertical.prefabName = "StarActivity/Prefabs/StarActivityItem";
			_loopVertical.poolSize = 8;

			SetTogPos();
			SetMaskOnClick();
			SetToggleOnClickEvent();
		}

		private void SetTogPos()
		{
			_togRect.anchoredPosition = new Vector2(0, ModuleManager.OffY );
		}

		private void SetMaskOnClick()
		{
			for (int i = 0; i <_togglesTran.childCount ; i++)
			{
				var mask = _togglesTran.GetChild(i).Find("Mask").gameObject;
				PointerClickListener.Get(mask).onClick = go =>
				{
					FlowText.ShowMessage(I18NManager.Get("StarActivity_Hint"));
				};
			}
		}

		
		
		private void OnEnable()
		{
			if (_loopVertical!=null)
				_loopVertical.RefreshCells();
		}


		
		
		/// <summary>
		/// 生成数据（里面要穿参数）
		/// </summary>
		public void SetData(MissionModel missionModel,int day)
		{						
			_missionModel = missionModel;			
			_data = _missionModel.GetStarActivityMission(day);
			
			
			CreateTask();
			SetToggleState(day);
			_togglesTran.gameObject.Show();
			SetToggleRedDot(_missionModel.GetRedDotDays());
			SetProgress();
			SetActivityTime();
			
			
		}

		private void SetToggleState(int day)
		{
			var openDay = _missionModel.GetOpenDay();
			if (openDay>= _togglesTran.childCount)
			{
				openDay = _togglesTran.childCount;
			}

			if (_missionModel.IsPreviewStarActivity())
			{												
				for (int i = 0; i < _togglesTran.childCount; i++)
				{
					var toggleTra = _togglesTran.GetChild(i);
					var toggleDay = int.Parse(toggleTra.gameObject.name);
					if (toggleDay<=openDay) //开放
					{
						toggleTra.Find("Mask").gameObject.Hide();
						if (day==toggleDay)
						{
							var isOn = toggleTra.GetChild(0).GetComponent<Toggle>().isOn;
							if (!isOn)
							{
								toggleTra.GetChild(0).GetComponent<Toggle>().isOn = true;
							}
					 
							toggleTra.GetChild(0).Find("Star1").gameObject.Show();
							toggleTra.GetChild(0).Find("Star2").gameObject.Hide();
							if (day==openDay+1)
							{
								toggleTra.Find("Mask").GetComponent<Empty4Raycast>().enabled=false;
								toggleTra.Find("Mask").gameObject.Show();
								
							}
						}
						else
						{
							var isOn =toggleTra.GetChild(0).GetComponent<Toggle>().isOn; 
							if (isOn)
							{
								toggleTra.GetChild(0).GetComponent<Toggle>().isOn = false;
							}					
							toggleTra.GetChild(0).Find("Star1").gameObject.Hide();
							toggleTra.GetChild(0).Find("Star2").gameObject.Show();
						}
					}
					else if(toggleDay == openDay+1) // 预览
					{
						toggleTra.Find("Mask").GetComponent<Empty4Raycast>().enabled=false;
						toggleTra.Find("Mask").gameObject.Show();
						var isOn =toggleTra.GetChild(0).GetComponent<Toggle>().isOn;
						if (isOn)
						{
							toggleTra.GetChild(0).Find("Star1").gameObject.Show();
							toggleTra.GetChild(0).Find("Star2").gameObject.Hide();
						}
						else
						{
							toggleTra.GetChild(0).Find("Star1").gameObject.Hide();
							toggleTra.GetChild(0).Find("Star2").gameObject.Show();
						}
					}
					else if(toggleDay>openDay+1)//没开放
					{
						toggleTra.Find("Mask").gameObject.Show();
						toggleTra.Find("Red").gameObject.Hide();

						toggleTra.GetChild(0).GetComponent<Toggle>().interactable = false;
						toggleTra.GetChild(0).GetComponent<Toggle>().isOn = false;
				
				
						toggleTra.GetChild(0).Find("Star1").gameObject.Hide();
						toggleTra.GetChild(0).Find("Star2").gameObject.Show();				
					}
				}								
			}
			else
			{
				for (int i = 0; i < _togglesTran.childCount; i++)
				{
					var toggleTra = _togglesTran.GetChild(i);
					var toggleDay = int.Parse(toggleTra.gameObject.name);
					if (toggleDay<=openDay) //开放
					{
						toggleTra.Find("Mask").gameObject.Hide();
						if (day==toggleDay)
						{
							var isOn = toggleTra.GetChild(0).GetComponent<Toggle>().isOn;
							if (!isOn)
							{
								toggleTra.GetChild(0).GetComponent<Toggle>().isOn = true;
							}
					 
							toggleTra.GetChild(0).Find("Star1").gameObject.Show();
							toggleTra.GetChild(0).Find("Star2").gameObject.Hide();
						}
						else
						{
							var isOn =toggleTra.GetChild(0).GetComponent<Toggle>().isOn;
							if (isOn)
							{
								toggleTra.GetChild(0).GetComponent<Toggle>().isOn = false;
							}					
							toggleTra.GetChild(0).Find("Star1").gameObject.Hide();
							toggleTra.GetChild(0).Find("Star2").gameObject.Show();
						}
					}
					else //没开放
					{
						toggleTra.Find("Mask").gameObject.Show();
						toggleTra.Find("Red").gameObject.Hide();

						toggleTra.GetChild(0).GetComponent<Toggle>().interactable = false;
						toggleTra.GetChild(0).GetComponent<Toggle>().isOn = false;
				
				
						toggleTra.GetChild(0).Find("Star1").gameObject.Hide();
						toggleTra.GetChild(0).Find("Star2").gameObject.Show();				
					}
				}	
			}			
				
			
				
			
			
		
			
		}
		

		/// <summary>
		/// 设置星动之约活动倒计时
		/// </summary>
		private void SetActivityTime()
		{
			var openDay = _missionModel.GetOpenDay();
			var allDay = _missionModel.GetStarActivityAllDay();		
			var residueDay = allDay - openDay;
			if (residueDay==0)
			{
				ClientTimer.Instance.AddCountDown("SetStarActivityCountDown", Int64.MaxValue, 1f, SetStarActivityCountDown, null);
				SetStarActivityCountDown(0);
			}
			else
			{
				_activityResidueTime.text = I18NManager.Get("StarActivity_LeftDays",residueDay); //residueDay+"天后结束";
			}
		}


		private void SetStarActivityCountDown(int obj)
		{
			string timeStr = "";
			var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
			var overTimeStamp = _missionModel.GetStarActivityOverTimeStamp();
			long time = overTimeStamp - curTimeStamp;

			if (time<1000)
			{
				timeStr = "0";
			}
			else
			{
				long s = (time / 1000) % 60;
				long m = (time / (60 * 1000)) % 60;
				long h = time / (60 * 60 * 1000);
				timeStr =string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
			}

			_activityResidueTime.text = timeStr;
		}


		private void SetProgressData()
		{
//			if (_missionModel.StarActivityInfoPb==null)
//			{
//			  _progressBar.sizeDelta =new Vector2(0,_progressBar.sizeDelta.y);	
//			}
//			else
//			{
//				var playerProgress = _missionModel.StarActivityInfoPb.Progress;
//				var allMissionNum = _missionModel.StarActivityNum;
//				var progress =(int) (playerProgress * _max / allMissionNum);
//				_progressBar.sizeDelta =new Vector2(progress,_progressBar.sizeDelta.y);
//			}

			_progressBar.sizeDelta =new Vector2(_missionModel.StarActivityProgress(),_progressBar.sizeDelta.y);
		}
		
		/// <summary>
		/// 设置进度
		/// </summary>
		private void SetProgress()
		{

			SetProgressData();

			for (int i = 0; i < _tabBarTran.childCount; i++)
			{
				int weight = _missionModel.GetStarReceiveWeight(i);//ReceiveWeight(i);
				var frameBg =_tabBarTran.GetChild(i).Find("FrameBg");
				var icon =frameBg.GetRawImage("Icon");
				var	redDot = frameBg.Find("RedDot");
				var	num = _tabBarTran.GetChild(i).GetText("WeightTxt");
				num.text = weight.ToString();
				var	mask = _tabBarTran.GetChild(i).Find("Mask");
				var iconNum = frameBg.GetText("IconNum");
				var rewards =_missionModel.GetStarActivityRewardPBByCount(weight);				
				PointerClickListener.Get(icon.gameObject).onClick = null;
				bool isGift =rewards.Count>1;
				if (!isGift)
				{
					RewardVo vo= new RewardVo(rewards[0]);
					icon.texture =ResourceManager.Load<Texture>(vo.IconPath);
					icon.gameObject.Show();
					icon.transform.GetChild(0).gameObject.SetActive(vo.Resource== ResourcePB.Puzzle);
					iconNum.text = vo.Num.ToString();
				}
				else
				{
					string path = "Prop/GiftPack/tongyong2";
					icon.texture =ResourceManager.Load<Texture>(path);
					icon.gameObject.Show();
					iconNum.transform.gameObject.Hide();
					icon.transform.GetChild(0).gameObject.Hide();
				}

				if (_missionModel.StarActivityInfoPb==null)
				{
					redDot.gameObject.Hide();
                    mask.gameObject.Hide();
                    PointerClickListener.Get(icon.gameObject).onClick = go =>
                    {
	                    if (!isGift)
	                    {
		                    FlowText.ShowMessage(ClientData.GetItemDescById(rewards[0].ResourceId, rewards[0].Resource).ItemDesc);
	                    }
	                    else
	                    {
		                    //打开奖励预览窗口
		                  var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
		                  window.SetData(rewards.ToList(),true,ModuleConfig.MODULE_STAR_ACTIVITY);
	                    }
                    };
				}
				else
				{
					//是否领取过 isGet为true领取过，false未领取
					var isGet = _missionModel.StarActivityInfoPb.List.Contains(weight);
					//是否可以领取 isMayGet为true可以领，false未领取
					var isMayGet =  _missionModel.StarActivityInfoPb.Progress>=weight;

					if (isMayGet&&!isGet)//可以领，没领过
					{
					    redDot.gameObject.Show();
					    PointerClickListener.Get(icon.gameObject).onClick = go =>
					    {
						    var isNew = _missionModel.IsNewStarActivity();
						    var type = isNew ? MissionTypePB.NewStarryCovenant : MissionTypePB.StarryCovenant;
						    
						    SendMessage(new Message(MessageConst.CMD_STAR_ACTIVITY_ACTIVE_REWARD,
							    Message.MessageReciverType.CONTROLLER,type,weight));  
					    };                      
					}
					else if(isMayGet &&isGet)//可以领，领过了
					{
						frameBg.gameObject.Hide();
						mask.gameObject.Show();
					}
					else if (!isMayGet) //不可以领
					{
						redDot.gameObject.Hide();
						mask.gameObject.Hide();	
						PointerClickListener.Get(icon.gameObject).onClick = go =>
						{
							if (!isGift)
							{
								FlowText.ShowMessage(ClientData.GetItemDescById(rewards[0].ResourceId, rewards[0].Resource).ItemDesc);
							}
							else
							{
								var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
								window.SetData(rewards.ToList(),true,ModuleConfig.MODULE_STAR_ACTIVITY);
							}
						};
					}
				}				
			}
			
		}


		private void SetToggleOnClickEvent()
		{
			for (int i = 0; i < _togglesTran.childCount; i++)
			{
				Toggle toggle = _togglesTran.GetChild(i).GetChild(0).GetComponent<Toggle>();
				var day = i + 1;
				toggle.gameObject.transform.GetText("Label").text= I18NManager.Get("StarActivity_Day",day);
				toggle.onValueChanged.AddListener(OnToggleChange);
			}	
		}


		private void OnToggleChange(bool isOn)
		{
			if (isOn == false){return;}
			if (EventSystem.current.currentSelectedGameObject==null){return;}
			string name = EventSystem.current.currentSelectedGameObject.name;
			int day;
			bool success = Int32.TryParse(name, out day);
			if (success)
			{
			   SendMessage(new Message(MessageConst.CMD_STAR_ACTIVITY_TOGGLE_SELECT_DAY,day));	
			}

		}
		
		/// <summary>
		/// 设置Toggle红点
		/// </summary>
		/// <param name="redDots"></param>
		private void SetToggleRedDot(List<int> days)
		{
			if (days.Count==0)
			{
				for (int i = 0; i < _togglesTran.childCount; i++)
				{
					_togglesTran.GetChild(i).Find("Red").gameObject.Hide();
				}
			}
			else
			{

				for (int i = 0; i < _togglesTran.childCount; i++)
				{
					var id =int.Parse( _togglesTran.GetChild(i).name);
					var isShow= days.Contains(id);
					if (isShow)
					{
						_togglesTran.GetChild(i).Find("Red").gameObject.Show();
					}
					else
					{
						_togglesTran.GetChild(i).Find("Red").gameObject.Hide();
					}
				}		
			}
		}


	
		//生成任务
		private void CreateTask()
		{
			
			_loopVertical.UpdateCallback = ListUpdateCallBack;
			_loopVertical.totalCount = _data.Count;
			_loopVertical.RefreshCells();
		}

		private void ListUpdateCallBack(GameObject go, int index)
		{
			go.GetComponent<StarActivityItem>().SetData(_data[index],_missionModel);
		}
		
}

