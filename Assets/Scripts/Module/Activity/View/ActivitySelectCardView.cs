//using System;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Com.Proto;
//using DataModel;
//using game.main;
//using Google.Protobuf.Collections;
//using System.Collections.Generic;
//using Assets.Scripts.Framework.GalaSports.Service;
//using DG.Tweening;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//public class ActivitySelectCardView : View {
//
//	
//	private Transform _roleCard;
//	private Transform _line;
//	private ToggleGroup _toggleGroup;
//	private Button _get;
//	private Button _left;
//	private Button _right;
//	
//	
//	private int _maxCount;
//	private float _speed=0.5f;
//	private int _curIndex;
//	private int _nextIndex;
//
//	private SevenDaysLoginAwardVO _vo;
//
//	private GameObject _mouseGestures;
//	private Vector2 _prePressPos;
//	
//	private void Awake()
//	{
//		
//		if (ModuleManager.OffY / 2!=0)
//		{
//			transform.Find("Content").GetComponent<RectTransform>().anchoredPosition =new Vector2(0,0f); 
//		}
//		
//		_roleCard = transform.Find("Content/RoleCard");
//		_line = transform.Find("Content/Tog/Line");
//		_toggleGroup = _line.GetComponent<ToggleGroup>();
//
//		_get = transform.GetButton("Content/GetBtn");
//		_left = transform.GetButton("Content/LeftBtn");
//		_right = transform.GetButton("Content/RightBtn");
//		
//		_get.onClick.AddListener(Get);
//		_left.onClick.AddListener(Left);
//		_right.onClick.AddListener(Right);
//
//		ArrowsAni(_left.transform, new Vector2(2f, 2f), 1.0f);
//		ArrowsAni(_right.transform, new Vector2(2f, 2f), 1.0f);
//		_mouseGestures = transform.Find("MouseGestures").gameObject;
//
//		UIEventListener.Get(_mouseGestures.gameObject).onDown = OnDown;
//		UIEventListener.Get(_mouseGestures.gameObject).onUp = OnUp;
//	}
//
//	private void OnUp(PointerEventData data)
//	{
//		float dis = (data.position - _prePressPos).magnitude;
//		bool isRight = (_prePressPos.x - data.position.x) > 0 ? true : false;
//
//		if (dis > 100)
//		{
//			ScrollingDisplay(isRight);
//		}
//		
//	}
//
//	private void ScrollingDisplay(bool isRight)
//	{
//		if (isRight)
//		{
//			Right();
//		}
//		else
//		{
//			Left();
//		}
//	}
//
//	private void OnDown(PointerEventData data)
//	{
//		 _prePressPos = data.pressPosition;
//	}
//
//
//	private void ArrowsAni(Transform arrows,Vector2 endValue ,float duration)
//	{
//		arrows.DOScale(endValue, duration).SetLoops(-1, LoopType.Yoyo);
//	}
//
//	/// <summary>
//	/// 生成角色（背景图片/角色进化图/页码Tog）
//	/// </summary>
//	/// <param name="list"></param>
//	public void CreateSevenDaysSelectCardInfo(SevenDaysLoginAwardVO vo)
//	{
//
//		var list = vo.Rewards;
//		_vo = vo;
//		_maxCount = list.Count;
//        
//		var roleImage = GetPrefab("Activity/Prefabs/SevenDayRoleImage");      
//		var tog = GetPrefab("Activity/Prefabs/SevenDayTog");
//     
//		for (int i = 0; i < list.Count; i++)
//		{
//			var roleImagePre = Instantiate(roleImage,_roleCard,false) as GameObject;
//			roleImagePre.transform.localScale = Vector3.one;
//			roleImagePre.name = list[i].Id.ToString();
//            
//			var  roleBgRawImage = roleImagePre.GetComponent<RawImage>();
//			roleBgRawImage.texture = ResourceManager.Load<Texture>("Activity/SevendaysActivityBG_"+list[i].Id);
//          
//			var togPre =Instantiate(tog,_line,false)as GameObject;
//			togPre.transform.localScale = Vector3.one;
//			togPre.name = list[i].ToString();
//			togPre.GetComponent<Toggle>().group = _toggleGroup;
//            
//			if (i == 0)
//			{
//				_curIndex = i;               
//				togPre.GetComponent<Toggle>().isOn = true;
//                
//			}
//			else
//			{
//				roleImagePre.GetComponent<RawImage>().color =new Color(roleBgRawImage.color.r,roleBgRawImage.color.g,roleBgRawImage.color.b,0);             
//			}
//		}
//                    
//	}
//	
//	
//	
//	private void Right()
//	{		
//		_nextIndex = _curIndex + 1;
//		if (_nextIndex==_maxCount)
//		{
//			_nextIndex = 0;
//		}
//		RetrogressAni();
//		
//		
//	}
//
//	private void Left()
//	{
//		_nextIndex = _curIndex - 1;
//		if (_nextIndex<0)
//		{
//			_nextIndex = _maxCount - 1;
//		}
//
//		RetrogressAni();
//	}
//
//	
//	
//	private void Get()
//	{
//
//		int curOkIndex = _curIndex;
//		string content =I18NManager.Get("Activity_Hint9",_vo.BigCardName[curOkIndex]) ;
//		string title = I18NManager.Get("Common_Hint");
//		//确认弹窗
//		PopupManager.ShowConfirmWindow(content,title).WindowActionCallback = evt =>
//		{
//
//			if (evt==WindowEvent.Ok)
//			{		
//				//发请求
//				SendMessage(new Message(MessageConst.CMD_ACTIVITY_SEND_SEVEN_ACTIVITY_REQ,Message.MessageReciverType.CONTROLLER,_vo,curOkIndex));							
//			}
//			
//		};
//
//
//		
//
//	}
//
//	
//	private void RetrogressAni()
//	{
//		var curBg = _roleCard.GetChild(_curIndex).GetComponent<RawImage>();
//		var nextBg = _roleCard.GetChild(_nextIndex).GetComponent<RawImage>();
//
//		Tween curBgAlpha = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),_speed );
//		Tween nextBgAlpha = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),_speed );
//		
//		Sequence tween = DOTween.Sequence()
//			.Join(curBgAlpha)			
//			.Join(nextBgAlpha);
//
//		tween.onComplete = () =>
//		{
//			_curIndex = _nextIndex;			
//			_line.GetChild(_curIndex).GetComponent<Toggle>().isOn = true;
//		};
//
//	}
//	
//	
//	
//	
//	
//	
//	
//	
//	
//	
//	
//	
//}
