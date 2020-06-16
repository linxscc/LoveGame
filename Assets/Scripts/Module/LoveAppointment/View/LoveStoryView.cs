using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class LoveStoryView : View
{

	private RawImage _storyImage;
	//private ProgressBar _progressBar;
	private Transform _stage;
	//private Text _title;
	private Text _tips;
	private GameObject _smallheart;
	private Transform _heartContainer;
	private Transform _heartBGContainer;
	private Transform _endPic;
	private Transform _titleSign;
	private AppointmentModel _appointmentModel;
	
	//特效专用的View元素
	private Animator _animator;
	private Transform _cutPhoto;
	private RawImage _cutphotoimg;
	private GameObject _particle;
	private Tweener endpicTween;
	private GameObject _mask;
	private Button _downloadVoice;
	private CacheVo _cacheVo;
	private int _cardId;
	private UserAppointmentVo userappointment;
	private AppointmentRuleVo _appointmentRuleVo;
	private Image _lovestage;
	

	private void Awake()
	{
		_storyImage = transform.Find("Photo/Mask/StoryImage").GetComponent<RawImage>();
		_stage = transform.Find("StoryProgress/Stage");
		for (int i = 0; i < _stage.childCount; i++)
		{
			_stage.GetChild(i).gameObject.Hide();
			
		}
		_heartContainer = transform.Find("Photo/HeartContainer");
		_heartBGContainer = transform.Find("Photo/HeartContainerBG");
		_lovestage = transform.Find("Photo/StageTips").GetImage();
		_tips = transform.Find("Tips").GetComponent<Text>();
		_smallheart = _tips.transform.GetChild(0).gameObject;
		_endPic = transform.Find("StoryProgress/End");
		_titleSign = transform.Find("Title");
		_mask=transform.Find("Mask").gameObject;

		PointerClickListener.Get(_titleSign.gameObject).onClick = go =>
		{
			//FlowText.ShowMessage("功能暂未开放，敬请期待");
//			Debug.LogError("Go");
//			EndPicSuccess();
//			ShakePhotoTween();
			SendMessage(new Message(MessageConst.CMD_APPOINTMENT_JUMPTODAILY));
		};

		_animator = transform.GetComponent<Animator>();
		_cutPhoto = transform.Find("CutPhoto");
		_cutphotoimg = transform.Find("CutPhoto/mask/rawimage").GetRawImage();
		_particle = transform.Find("Particle").gameObject;
		_downloadVoice = transform.Find("DownLoadVoice").GetButton();
		_downloadVoice.onClick.AddListener(() =>
		{
			//直接弹出确认框
			if (_cacheVo==null)
			{
				  return;
			}

			long allsize=0;
			foreach (var v in _cacheVo.sizeList)
			{
				allsize += v;
			}

			var nochooseWindow = PopupManager.ShowWindow<ConfirmNoChooseWindow>(Constants.ConfirmNoChooseWindowPath);
			nochooseWindow.Content =
				I18NManager.Get("Download_ConfirmDownloadLoveStory", Math.Round(allsize * 1f / 1048576f, 2));
			nochooseWindow.CancelText = I18NManager.Get("Common_Cancel1");
			nochooseWindow.OkText = I18NManager.Get("Common_OK1");
			nochooseWindow.WindowActionCallback = evt =>
			{
				if (evt == WindowEvent.Ok)
				{
					CacheManager.DownloadLoveStoryCache(_cardId, str =>
					{
						SetDownLoadState(_cardId);
					});
				}
			};




		});
	}

	public void SetData(AppointmentRuleVo vo,AppointmentModel appointmentModel,bool playfinish=false)    
	{
		endpicTween?.Pause();
		_endPic.localEulerAngles=Vector3.zero;
		_cutPhoto.gameObject.SetActive(false);
		_appointmentModel = appointmentModel;

		if (vo==null)
		{
			//Debug.LogError("Who call me");
			return;
		}

		_appointmentRuleVo = vo;
		var card = GlobalData.CardModel.GetUserCardById(_appointmentRuleVo.ActiveCards[0]);
		//_title.text =vo.Name;
		_storyImage.texture=ResourceManager.Load<Texture>(card!=null&&card.Evolution>=EvolutionPB.Evo2?_appointmentRuleVo.EvoStoryPicPath:_appointmentRuleVo.StoryPicPath,ModuleName);

		_cardId = card.CardId;
		SetDownLoadState(_cardId);
		SetSweetnessStage(vo.Sweetness);
		
		userappointment = _appointmentModel.GetUserAppointment(_appointmentRuleVo.Id);

		_endPic.GetComponent<RectTransform>().anchoredPosition=new Vector2(387,-381);
		
		//根据恋爱日记的状态来判断是否已经钉起来了？
		//要获取到恋爱日记的元素里的奖励来判断是否已经拍立得了。
		bool hasPhotoNickUp = false;
		foreach (var v in _appointmentRuleVo.GateInfos)
		{
			foreach (var e in v.Awards)
			{
				if (GlobalData.DiaryElementModel.IsUserElement(e.ResourceId))
				{
					hasPhotoNickUp = true;
				}
			}

		}
		int gateidx = userappointment.FinishGateInfos.Count - 1;
		//并且要判断通关后的最后一关的奖励数据是否为空！
		if (gateidx>=0&&gateidx<=4)
		{
			//如果红点出现了的话，首先要做的是做晃动的动画。	
			var enableshow = !hasPhotoNickUp && _appointmentRuleVo.GateInfos[gateidx].Awards.Count > 0;
			_endPic.Find("RedPoint").gameObject.SetActive(enableshow);
			_endPic.Find("Side").gameObject.SetActive(enableshow);
			if (enableshow)
			{
				ShakePhotoTween();
			}
			
		}
		else
		{
			_endPic.Find("RedPoint").gameObject.SetActive(false);
			_endPic.Find("Side").gameObject.SetActive(false);
		}

		if (!playfinish)
		{
			_endPic.gameObject.SetActive(!hasPhotoNickUp);	
		}
		else
		{
			EndPicSuccess();
		}


		PointerClickListener.Get(_endPic.gameObject).onClick = null;
//userappointment.ClearState==0&&userappointment.FinishGateInfos.Count>=4
		
		PointerClickListener.Get(_endPic.gameObject).onClick = go =>
		{

			if (gateidx>=2&&_appointmentRuleVo.GateInfos[gateidx].Awards.Count>0)//程序上设定起码两关才能够点击拍立得
			{
				if (!hasPhotoNickUp)
				{
					var gateinfo = _appointmentRuleVo.GateInfos[gateidx];
					//Debug.LogError("userappointment.FinishGateInfos.Count"+userappointment.FinishGateInfos.Count);
					//Debug.LogError("clearState"+userappointment.ClearState);				
					SendMessage(new Message(MessageConst.CMD_APPOINTMENT_ACTIVE_PHOTOCLEARUP,Message.MessageReciverType.CONTROLLER,_appointmentRuleVo.Id,gateinfo.Gate));
				}
			}
			else
			{
				FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint1"));
			}
		};
		

		SetStars(card);
		SetTips(userappointment.ActiveGateInfos.Count);//这个需要改成ActivityGateInfos的数量!,vo.GateInfos[userappointment.ActiveGateInfos.Count-1].Star
		for (int i = 0; i < _appointmentRuleVo.GateInfos.Count&&i<_stage.childCount; i++)
		{
			_stage.GetChild(i).gameObject.Show();
			Text chapter = _stage.GetChild(i).Find("Text").GetComponent<Text>();
			RawImage prop = _stage.GetChild(i).Find("Image").GetComponent<RawImage>();
			Text num=_stage.GetChild(i).Find("Num").GetComponent<Text>();
			GameObject lockimg = _stage.GetChild(i).Find("Lock").gameObject;
			GameObject sign=_stage.GetChild(i).Find("Sign").gameObject;
			GameObject readPoint = _stage.GetChild(i).Find("ReadPoint").gameObject;
			GameObject redpoint = lockimg.transform.GetChild(0).gameObject;
			redpoint.SetActive(false);
			
			var gate=new AppointmentGateRuleVo(_appointmentRuleVo.GateInfos[i]);
			foreach (var v in gate.CosumesDic)
			{
				if (v.Key==0)
				{
				//	Debug.LogError(v.Key);//第一关情况
				}
				else
				{
					prop.texture = ResourceManager.Load<Texture>($"Prop/{v.Key}",ModuleName);
					num.text = v.Value==0?"":"X"+v.Value;
				}
			}
			var activestate = _appointmentModel.IsGateActive(_appointmentRuleVo.Id, gate.Gate);
			var pregate=new AppointmentGateRuleVo(_appointmentRuleVo.GateInfos[i==0?0:i-1]);
			var prestate=_appointmentModel.IsGateActive(_appointmentRuleVo.Id, pregate.Gate);
			chapter.text =SetTitle(i+1);
			
			HandleActive(sign.gameObject,lockimg.gameObject,activestate != GateState.NotAcive);
			
			bool enbale = activestate == GateState.NotAcive && prestate == GateState.Finish;
			HandlePropShow(prop.gameObject, num.gameObject, chapter.gameObject, enbale);
			
			if (enbale)
			{
				if (card.Star>=_appointmentRuleVo.GateInfos[i].Star&&(int)card.Evolution>=_appointmentRuleVo.GateInfos[i].Evo)
				{
					redpoint.Show();
				}
			}

			if (i==0)
			{
				lockimg.gameObject.SetActive(activestate!=GateState.Finish);
				redpoint.SetActive(activestate!=GateState.Finish);
			}
			else
			{
				readPoint.SetActive(activestate == GateState.Active);
			}
	
			//只有解锁后的关卡才能开启剧情故事

			PointerClickListener.Get(_stage.GetChild(i).gameObject).onClick = go =>
			{
				if (activestate!=GateState.NotAcive)
				{
					int needtoTips = 1;
					if (PlayerPrefs.HasKey("RecordLoveStory"+_cardId))
					{
						needtoTips = PlayerPrefs.GetInt("RecordLoveStory" + _cardId);
					}
					if (_cacheVo.needDownload&&needtoTips==1)
					{
						//NetworkReachability.ReachableViaLocalAreaNetwork是指wifi下载
						if (Application.internetReachability==NetworkReachability.ReachableViaLocalAreaNetwork)
						{
							//直接下载
							CacheManager.DownloadLoveStoryCache(_cardId, str =>
							{
								SetDownLoadState(_cardId);
								ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
							},null, str =>
							{
								Debug.LogError("Cancle?!");
//								ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,vo.Id);
							});
							Debug.LogError("download");
						}
						else
						{
							long allsize=0;
							foreach (var v in _cacheVo.sizeList)
							{
								allsize += v;
							}
							//弹出确认框
							CacheManager.ConfirmNeedToDownload(I18NManager.Get("Download_ConfirmDownloadLoveStory2",Math.Round(allsize*1f/1048576f,2))
								,I18NManager.Get("Download_JumpDownload"),_cardId, str =>
							{
								Debug.LogError(str);
								SetDownLoadState(_cardId);
								ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
							}, () =>
							{
								ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
							});
								
						}
					}
					else
					{
						ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
					}

				}
				else
				{
					if (prestate!=GateState.Finish)
					{
                        FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint2"));//("需完成上一关剧情");
					}
					else
					{
						//弹出是否消耗道具的选择窗口。
						SendMessage(new Message(MessageConst.CMD_APPOINTMENT_ENSUREOPENGATE,Message.MessageReciverType.CONTROLLER,gate,_appointmentRuleVo));	
						
					}
				}
			};
		}	
	}

	private void SetSweetnessStage(string stage)
	{
		switch (stage)
		{
			case "0":
				_lovestage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_friendStage");
				break;
			case "1":
				_lovestage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_nearloverStage");
				break;
			case "2":
				_lovestage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_loverStage");
				break;
			default:
				Debug.LogError("SweetNess:"+stage);
				break;
		}
		
		
		
	}
	
	
	private void SetDownLoadState(int cardid)
	{
		_cacheVo = CacheManager.CheckLoveStoryCache(cardid);
		_downloadVoice.gameObject.SetActive(_cacheVo.needDownload);
		
		
		
	}
	
	private void ShakePhotoTween()
	{
		endpicTween= _endPic.DOLocalRotate(new Vector3(0, 0, 30), 0.3f);
		endpicTween.SetAutoKill(false);
		endpicTween.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
	}

	/// <summary>
	/// 这个是拍立得成功的动画
	/// </summary>
	public void EndPicSuccess()
	{
		_mask.SetActive(true);
		_cutphotoimg.texture = _storyImage.texture;
		_cutPhoto.gameObject.SetActive(true);
		_animator.Play("PolaroidAnim");
		ClientTimer.Instance.DelayCall(() =>
		{
			_mask.SetActive(false);
			_endPic.gameObject.SetActive(false);
		}, 1.4f);

	}

	private void HandleActive(GameObject sign,GameObject lockimg,bool unlock)
	{
		sign.SetActive(unlock);
		lockimg.SetActive(!unlock);
	}

	private void HandlePropShow(GameObject prop,GameObject num,GameObject chater,bool show)
	{
		prop.SetActive(show);
		num.SetActive(show);
		chater.SetActive(!show);
	}
	
	private void SetUnlockTips(int finishCount,bool hasEvo)
	{
		if (hasEvo&&finishCount<4)
		{
			SetTips(3);
		}
		else
		{
			SetTips(finishCount);
		}
		
	}
	
	private void SetTips(int level)
	{
		_tips.gameObject.SetActive(true);
		switch (level)
		{
			case 0:
			case 1:
                _tips.text = I18NManager.Get("LoveAppointment_LoveStoryViewTips1",_appointmentRuleVo.GateInfos[1].Star+1);//"星缘达到2     可解锁下一节恋爱剧情";
				_smallheart.Show();
				break;
			case 2:
				_tips.text = I18NManager.Get("LoveAppointment_LoveStoryViewTips2",_appointmentRuleVo.GateInfos[2].Star+1);//"星缘达到4     可解锁下一节恋爱剧情";
                _smallheart.Show();
				break;
			case 3:
				_tips.text = I18NManager.Get("LoveAppointment_LoveStoryViewTips3",_appointmentRuleVo.GateInfos[3].Evo);//"星缘进化·二后可解锁下一节恋爱剧情";
                _smallheart.Hide();
				break;
			case 4:
				_tips.gameObject.SetActive(false);
				break;
					
		}
	}

	
	private void SetStars(UserCardVo userCardVo)
	{
		for (int i = 0; i <_heartContainer.childCount; i++)
		{
			RawImage img = _heartContainer.GetChild(i).GetComponent<RawImage>();
			img.gameObject.SetActive(i <userCardVo.Star);
		}

		for (int i = 0; i <_heartBGContainer.childCount; i++)
		{
			RawImage img = _heartBGContainer.GetChild(i).GetComponent<RawImage>();
			img.gameObject.SetActive(i<userCardVo.MaxStars);
		}
	}

	private string SetTitle(int id)
	{
		switch (id)
		{
			case 1:
                return I18NManager.Get("Common_Drama1");//"剧情一";
			case 2:
				return I18NManager.Get("Common_Drama2");//"剧情二";
            case 3:
				return I18NManager.Get("Common_Drama3");//"剧情三";
            case 4:
				return I18NManager.Get("Common_Drama4");//"剧情四";
        }
		return "";
	}



	public override void Show(float delay = 0)
	{
		base.Show(delay);
		//Debug.LogError("Who call me ?");
	}

	public void SetEmptyTexture()
	{
		_storyImage.texture = null;
	}
}