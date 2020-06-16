using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JournalChooseView : View
{
	private Transform _rolesContent;
	//private Text _title;
	private GameObject _journalList;
	private ProgressBar _progressBar;
	private RawImage _bgImage;
	private Image _tipsSprite;
	private AppointmentModel _appointmentModel;
	
	private Transform _goleft;
	private Transform _goRight;

	private int _totalnum=0;
	private int curIndex = 0;
	private int _roleId = 0;
	
	private Vector2 prePressPos;
	
	//需要一个字典来分页
	private Dictionary<int, List<AppointmentRuleVo>> _loveStoryPageDic;
	
	private void Awake()
	{
		_rolesContent = transform.Find("List/Content");
		//_title = transform.Find("Title/Text").GetComponent<Text>();
		_bgImage = transform.Find("BgImage").GetComponent<RawImage>();
		_tipsSprite = transform.Find("NoteSprite").GetComponent<Image>();
		if (_journalList == null)
		{
			_journalList = InstantiatePrefab("LoveAppointment/Prefabs/JournalItem/JournalItem");
			_journalList.transform.SetParent(_rolesContent,false);
		}

		PointerClickListener.Get(_tipsSprite.gameObject).onClick = go =>
		{
			//FlowText.ShowMessage("功能暂未开放，敬请期待");
			SendMessage(new Message(MessageConst.CMD_APPOINTMENT_JUMPTODAILY));
		};
		
		_goleft = transform.Find("Arrow/Left");
		_goRight = transform.Find("Arrow/Right");
		
		UIEventListener.Get(_bgImage.gameObject).onDown = OnDown;
		UIEventListener.Get(_bgImage.gameObject).onUp = OnUp;
		
		
		PointerClickListener.Get(_goleft.gameObject).onClick = go =>
		{
			curIndex--;
			SetPageState(curIndex);
			if (_loveStoryPageDic.ContainsKey(curIndex))
			{
				SetOnePageData(_loveStoryPageDic[curIndex]);     
			}
			else
			{
				SetOnePageData(_loveStoryPageDic[0]);     
			}

		};
		PointerClickListener.Get(_goRight.gameObject).onClick = go => { NextPage(); };
		ArrowTween();
		
		
		
	}

	public void NextPage()
	{
		if (curIndex < (_totalnum - 1) / 15 && _totalnum > 15)
		{
			curIndex++;
			SetPageState(curIndex);
			if (_loveStoryPageDic.ContainsKey(curIndex))
			{
				SetOnePageData(_loveStoryPageDic[curIndex]);     
			}
			else
			{
				Debug.LogError(curIndex);
				SetOnePageData(_loveStoryPageDic[0]);     
			}
		}
	}
	
	private void OnUp(PointerEventData data)
	{
		float dis = (data.position - prePressPos).magnitude;
		bool isRight = (prePressPos.x - data.position.x) > 0 ? true : false;
		if (dis>100)
		{
			ScrollingDisplay(isRight);           
		}
	}

	private void OnDown(PointerEventData data)
	{
		prePressPos = data.pressPosition;
	}

	private void ScrollingDisplay(bool isRight)
	{
		if (isRight)
		{
			//首先要判断有没有上一个卡牌，没有就return
			if (curIndex<(_totalnum-1)/15&&_totalnum>15)//
			{				
				curIndex++;
				Debug.LogError(curIndex);
				SetPageState(curIndex);
				if (_loveStoryPageDic.ContainsKey(curIndex))
				{
					SetOnePageData(_loveStoryPageDic[curIndex]);     
				}
				else
				{
					SetOnePageData(_loveStoryPageDic[0]);     
				}
   
			}  
		}
		else
		{			
			//首先要判断有没有下一个卡牌没有的话就return
			if (curIndex>0&&(_totalnum-1)/15>0)//curIndex<(_totalnum-1)/15&&_totalnum>15
			{             
				curIndex--;
				Debug.LogError(curIndex);
				SetPageState(curIndex);
				if (_loveStoryPageDic.ContainsKey(curIndex))
				{
					SetOnePageData(_loveStoryPageDic[curIndex]);     
				}
				else
				{
					SetOnePageData(_loveStoryPageDic[0]);     
				}   
			}  
		}
	}

	private void ArrowTween()
	{
		Tweener alpha1 = _goleft.DOLocalMoveX(-499, 0.5f);
		alpha1.SetAutoKill(false);
		alpha1.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
		
		Tweener alpha2 = _goRight.DOLocalMoveX(499, 0.5f);
		alpha2.SetAutoKill(false);
		alpha2.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
	}
	
	private void GoToJournal(GameObject go)
	{
		AppointmentRuleVo vo = (AppointmentRuleVo)PointerClickListener.Get(go).parameter;
		//要先判断这张卡片有没有。
		var card = GlobalData.CardModel.GetUserCardById(vo.ActiveCards[0]);
		if (card==null)
		{
			SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_NOCARD,vo));
		}
		else
		{
			//发送解锁协议,判断这个User是否已经激活了这个约会
			var user = _appointmentModel.GetUserAppointment(vo.Id);
			//Debug.LogError("activeState"+user?.ActiveState);
			SendMessage(user?.ActiveState == 1
				? new Message(MessageConst.CMD_APPOINTMENT_SHOW_LOVESTORY, Message.MessageReciverType.DEFAULT,vo)
				: new Message(MessageConst.CMD_APPOINTMENT_ACTIVE_LOVESTORY,Message.MessageReciverType.DEFAULT, vo));
		}
	}

	public void SetData(List<AppointmentRuleVo> vo,int roleId,AppointmentModel appointmentModel)
	{
		_appointmentModel = appointmentModel;
		var needfresh = _roleId != roleId;
		if (needfresh)
		{
			curIndex = 0;
		}
		_roleId = roleId;
		if (_loveStoryPageDic==null)
		{
			_loveStoryPageDic=new Dictionary<int, List<AppointmentRuleVo>>();
		}
//		_loveStoryPageDic.Clear();
		//要分页！
		if (vo==null)
		{
			return;
		}
		
		_totalnum = vo.Count;
		int pages = (vo.Count / 15)+1;
//		Debug.LogError(pages+" "+vo.Count);
		for (int i = 0; i < pages; i++)
		{
			List<AppointmentRuleVo> newvolist=new List<AppointmentRuleVo>();
			for (int j = i*15; j < (i+1)*15&&j < vo.Count; j++)
			{
//				Debug.LogError(j);
				newvolist.Add(vo[j]);
			}

			if (_loveStoryPageDic.ContainsKey(i))
			{
				_loveStoryPageDic[i] =newvolist;
			}
			else
			{
//				Debug.Log(i+" "+newvolist.Count);
				_loveStoryPageDic.Add(i,newvolist);	
			}
			
		}

		SetPageState(curIndex==0?0:curIndex);
		SetOnePageData(_loveStoryPageDic[curIndex==0?0:curIndex]);
	}
	
	public void SetOnePageData(List<AppointmentRuleVo> vo)
	{
		//_title.text = GlobalData.AppointmentModel.SpliceCardName(roleId) + "·好感度为10";
//         Debug.LogError(vo.Count);
		_bgImage.texture = ResourceManager.Load<Texture>("Background/JournalBG"+(_roleId-10000),ModuleName);
		
		for (int i = 0; i < _journalList.transform.childCount; i++)
		{
			_journalList.transform.GetChild(i).gameObject.Hide();
		}

		
		for (int i = 0; i < vo.Count; i++)
		{
			_journalList.transform.GetChild(i).gameObject.Show();
			Text text = _journalList.transform.GetChild(i).Find("TitleName").GetComponent<Text>();
			var cardname = vo[i].Name;//GlobalData.CardModel.GetCardBase(vo[i].Id).CardName;
			text.text = cardname;//cardname;//
			RawImage notepic = _journalList.transform.GetChild(i).Find("NotePic").GetComponent<RawImage>();
			var card = GlobalData.CardModel.GetUserCardById(vo[i].ActiveCards[0]);
			//card?.CardVo.SquareCardPath(card?.UseEvo==EvolutionPB.Evo1&&card?.Evolution==EvolutionPB.Evo1)
			notepic.texture = ResourceManager.Load<Texture>(card!=null&&card.Evolution>=EvolutionPB.Evo2?vo[i].EvoSmallPicPath:vo[i].SmallPicPath, ModuleName);//这个应该要放Vo.ID读卡牌的图。
			if (notepic.texture == null)
			{
				//Debug.LogError("No this pic "+card?.CardVo.SquareCardPath(card.UseEvo==EvolutionPB.Evo1&&card.Evolution==EvolutionPB.Evo1));
				notepic.texture = ResourceManager.Load<Texture>(card!=null&&card.Evolution>=EvolutionPB.Evo2?vo[i].EvoSmallPicPath:vo[i].SmallPicPath, ModuleName);
			}
					
			GameObject redpoint = _journalList.transform.GetChild(i).Find("Redpoint").gameObject;
			bool showredpoint = false;

			if (card==null)
			{
				notepic.color=Color.grey;
			}
			else
			{
				var userappointment = _appointmentModel.GetUserAppointment(vo[i].Id);
				if (userappointment!=null)
				{
					showredpoint= _appointmentModel.NeedSetRedPoint(userappointment,card.CardId);
				}


				notepic.color=Color.white;
			}
			redpoint.SetActive(showredpoint);
			PointerClickListener.Get(_journalList.transform.GetChild(i).gameObject).parameter = vo[i];
			PointerClickListener.Get(_journalList.transform.GetChild(i).gameObject).onClick = GoToJournal;
			
		}
	}

	/// <summary>
	/// 设置左右箭头的逻辑。
	/// </summary>
	/// <param name="index"></param>
	private void SetPageState(int page)
	{
		if (page == 0 && _totalnum>15 )
		{
			SetArrowState(false, true);
		}
		else if (page > 0 && page < (_totalnum-1)/15 )
		{
			SetArrowState(true, true);
		}
		else if (page == (_totalnum-1)/15 && _totalnum/15 >0 )
		{
			SetArrowState(true, false);
		}
		else
		{
			SetArrowState(false, false);
		}
	}
	
	/// <summary>
	/// 箭头状态
	/// </summary>
	/// <param name="left"></param>
	/// <param name="right"></param>
	private void SetArrowState(bool left, bool right)
	{
		_goleft.gameObject.SetActive(left);
		_goRight.gameObject.SetActive(right);
	}
	
	
	/// <summary>
	/// 设置红点。
	/// </summary>
	public void SetRedpoint()
	{
		//日记的红点还稍微比较简单一些。
		//直接判断是否有可以解锁的关卡或者拍照就可以了
		
		
	}
}