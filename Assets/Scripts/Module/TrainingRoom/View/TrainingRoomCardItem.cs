﻿using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrainingRoomCardItem : MonoBehaviour ,IPointerClickHandler{


	
	private Image _cardQualityImage;
	private Text _name;
	private Transform _heartBar;
	private RawImage _cardImage;
	private Text _level;
	private Text _ability;
	private TrainingRoomCardVo _data;
	private UserMusicGameVO _curMusicGame;
	
	private CanvasGroup _cg;
	private void Awake()
	{
		_cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
		_name = transform.Find("NameText").GetComponent<Text>();
		_heartBar = transform.Find("QualityBg/HeartBar");
		_cardImage =transform.Find("Mask/CardImage").GetComponent<RawImage>();
		_level = transform.Find("QualityBg/LevelText").GetComponent<Text>();
		_ability =transform.Find("Ability").GetComponent<Text>();
		_cg = GetComponent<CanvasGroup>();
	}


	public void SetData(TrainingRoomCardVo vo)
	{
		_data = vo;
		_curMusicGame = GlobalData.TrainingRoomModel.CurMusicGame;
		
		_cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.UserCardVo.CardVo.Credit));
		_name.text = vo.UserCardVo.CardVo.CardName;

		for (int i = 0; i < 5; i++)
		{
			Transform item = _heartBar.GetChild(i);
			var redHeart = item.Find("RedHeart");
			redHeart.gameObject.SetActive(vo.UserCardVo.Star > i);
			item.gameObject.SetActive(i < vo.UserCardVo.MaxStars);
		}
		
		Texture texture = ResourceManager.Load<Texture>(vo.UserCardVo.CardVo.MiddleCardPath(vo.UserCardVo.UserNeedShowEvoCard()), ModuleConfig.MODULE_CARD);
		if (texture ==null)
		{
			texture = ResourceManager.Load<Texture>(vo.UserCardVo.CardVo.MiddleCardPath(),ModuleConfig.MODULE_CARD);
		}
		_cardImage.texture = texture;
		_level.text = vo.UserCardVo.Level.ToString();

		
		_ability.text = vo.AbilityDesc + ":" + vo.AbilityNum;

		if (_data.IsChoose)
		{
			_cg.alpha = 0.6f;	
		}
		else
		{
			_cg.alpha = 1.0f;
		}
	}


	
	
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (_data.AbilityNum<_curMusicGame.NeedAbilityNum)
		{
			FlowText.ShowMessage("星缘不符合要求");
			return;
		}

		_data.IsChoose = !_data.IsChoose ;
		
		if (_data.IsChoose )
		{ 
			if (_curMusicGame.NeedStarNum ==GlobalData.TrainingRoomModel.ChooseCards.Count)
			{
				FlowText.ShowMessage("已满足要求");
				return;
			}
			_cg.alpha = 0.6f;			
			Debug.LogError("选中");
			EventDispatcher.TriggerEvent(EventConst.OkChooseCard,_data);	
		}
		else
		{
			_cg.alpha = 1.0f;
			Debug.LogError("取消选中");
			EventDispatcher.TriggerEvent(EventConst.CancelChooseCard,_data);	
		}
	}
}
