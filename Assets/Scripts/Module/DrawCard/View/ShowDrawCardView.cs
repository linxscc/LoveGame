using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using game.main;
using UnityEngine;
using UnityEngine.UI;
public class ShowDrawCardView : View
{
	
	private Button _shareBtn;
	private Button _skipBtn;
	private Image _cardImg;
	private Button _showImgBtn;
	private Image _isNew;
	private Text _talkTxt;
	
	//shareView
	private Image _shareImg;
	private Button _shareImgBtn;
	private Image _showLevel;
	private Image _showSignature;
	private Button _shareBtn1;
	private Button _shareBtn2;
	
	//结算的页面
	private Transform _settlementView;
	private Transform _itemParent;
	private Text _hintTxt;
	private Button _bgBtn;
	private Button _backBtn;				
	//刚开始已经显示了第0张，所以后来是从第一张开始的
	private int _index;
	//产生的卡牌数量(假数据)
	
	
	
	private List<DrawCardModel> _cardList;
	private void Awake()
	{
		_shareBtn = transform.Find("ShowImg/ShareBtn").GetComponent<Button>();
		_skipBtn = transform.Find("ShowImg/SkipBtn").GetComponent<Button>();
		_cardImg = transform.Find("ShowImg/CardImg").GetComponent<Image>();
		_talkTxt = transform.Find("ShowImg/TalkImg/TalkTxt").GetComponent<Text>();
		_showImgBtn = transform.Find("ShowImg").GetComponent<Button>();
		_isNew = transform.transform.Find("ShowImg/IsNew").GetComponent<Image>();
		//展示命名
		_shareImg=transform.Find("ShareImg").GetComponent<Image>();
		_shareBtn1 = transform.Find("ShareImg/ShareBtn1").GetComponent<Button>();
		_shareBtn2 = transform.Find("ShareImg/ShareBtn2").GetComponent<Button>();
		_shareImgBtn= transform.Find("ShareImg").GetComponent<Button>();
		_showLevel = transform.Find("ShareImg/ShowLevel").GetComponent<Image>();
		_showSignature= transform.Find("ShareImg/ShowSignature").GetComponent<Image>();
		
		//结算页面
		_settlementView = transform.Find("SettlementView").transform;
		_itemParent = _settlementView.Find("SettlementShow/Viewport/ItemParent").transform;		
		_backBtn = transform.Find("SettlementView/BackBtn").GetComponent<Button>();		
		_hintTxt = transform.Find("SettlementView/HintTxt").GetComponent<Text>();
		_bgBtn = transform.Find("SettlementView/SettlementShow/Viewport").GetComponent<Button>();
		
		_showImgBtn.onClick.AddListener(CutCard);
		_shareImgBtn.onClick.AddListener(() =>
		{
			_shareImg.gameObject.SetActive(false);
		});
		//弹出图片并加载
		_shareBtn.onClick.AddListener(FlauntCard);
		_skipBtn.onClick.AddListener(SettlementShow);
		//返回主页面   这个draw涉及到了子页面所以后来需要对部分隐藏
		_backBtn.onClick.AddListener(BackDrawCardView);
		_bgBtn.onClick.AddListener(BackDrawCardView);
	}

    public override void Show(float delay = 0)
	{
		base.Show(delay);
		gameObject.SetActive(true);	
		_index = 0;
		ShowCard();
	}

	private void BackDrawCardView()
	{
		Hide();
		SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD));
		for(int i = 0;i < _cardList.Count;i++)  
		{  
			 Destroy(_itemParent.GetChild(i).gameObject);  
		}
		_hintTxt.text = "";
		_settlementView.gameObject.SetActive(false);
	}
	
	public void InitData(List<DrawCardModel> cardList)
	{
		_cardList = cardList;
	}
	
	
	public void FlauntCard()
	{
		//炫耀按钮点击显示对应的图片   
		//1加载对应图，加载签名，SR标记//后来增加list
		_shareImg.gameObject.SetActive(true);
	}

	//展示卡牌
	private void ShowCard()
	{
		_isNew.gameObject.SetActive(false);
                        //很高兴见到你啊 啊啊啊啊
        _talkTxt.text =I18NManager.Get( "DrawCard_Hint11") + _cardList[_index].CardId;
		if (_cardList[_index].IsNew)
		{
			_isNew.gameObject.SetActive(true);
		}
		if (_cardList[_index].CardLevel == "SR" || _cardList[_index].CardLevel == "SSR")
		{
			FlauntCard();
		}
		_index++;
	}

	//点击换图
	private void  CutCard()
	{
		//不到最后于一张 增加索引依次显示图片就好了
		if (_index <= _cardList.Count - 1)
		{
			ShowCard();
		}
		else if(_index>_cardList.Count-1)
		{
			//最后一张的处理进行最后页面的一个显示过程
			SettlementShow();
		}									
	}
	//结算页面
	private void SettlementShow()
	{
		int newCount=0;
		//这里可以做10个隐藏起来用的时候就用  不用的时候就隐藏做一个对象池
		//TODO
		_settlementView.gameObject.SetActive(true);
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		for (int i = 0; i < _cardList.Count; i++)
		{						
			GameObject item = InstantiatePrefab("DrawCard/Prefabs/ShowCardItem");
			item.transform.SetParent(_itemParent);
			item.transform.localScale=new Vector3(1,1,0);
			
			if (_cardList[i].IsNew)
			{
				newCount++;
				item.transform.Find("ShowCardItem/IsGet").gameObject.SetActive(true);
			}
			item.transform.Find("ShowCardItem/CardName").GetComponent<Text>().text = _cardList[i].CardName;
			if (_cardList[i].CardLevel == "SR" || _cardList[i].CardLevel == "SSR" || _cardList[i].CardLevel == "R")
				item.transform.Find("ShowCardItem/CardLevel").GetComponent<Image>().sprite =
					AssetManager.Instance.GetSpriteAtlas("UIAtlas_Battle_"+_cardList[i].CardLevel);
					

			else
			{
				item.transform.Find("ShowCardItem/CardLevel").gameObject.SetActive(false);
			}


		}
        //_hintTxt.text = "拥有了" + newCount + "个新的卡牌";
        _hintTxt.text = I18NManager.Get("DrawCard_Hint12");
    }

	public override void Hide()
	{
		gameObject.SetActive(false);
		
	}
}
