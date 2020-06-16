
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using game.main;
using UnityEngine;
using UnityEngine.UI;


//抽奖选择窗口(一次或八次)
public class ActivityTemplateLotteryWindow : Window
{


	private Button _playOneBtn;
	private Button _playMoreBtn;

	private Text _haveNumTxt;
	private Text _onePriceTxt;
	private Text _morePriceTxt;

	private RawImage _oneImg;
	private RawImage _moreImg;
	private RawImage _haveNumImg;
	
	private GameObject _redDot;
	
	
	
		
	
	private void Awake()
	{
		_playOneBtn = transform.GetButton("PlayOneBtn");
		_playMoreBtn = transform.GetButton("PlayMoreBtn");

		
		_haveNumTxt = transform.GetText("HaveNumTxt");
		_onePriceTxt = transform.GetText("Left/Price");
		_morePriceTxt = transform.GetText("Right/Price");


		_oneImg =transform.GetRawImage("Left/Icon");
		_moreImg = transform.GetRawImage("Right/Icon");		
		
		_haveNumImg = transform.GetRawImage("HaveNumImg");
		
		_redDot = _playMoreBtn.transform.Find("RedDot").gameObject;
		
		_playOneBtn.onClick.AddListener(PlayOneBtn);
		_playMoreBtn.onClick.AddListener(PlayMoreBtn);
	}

	private void PlayOneBtn()
	{
		EventDispatcher.TriggerEvent(EventConst.ActivityTemplateOnPlay,1);
		base.CloseAnimation();
	}

	private void PlayMoreBtn()
	{
		EventDispatcher.TriggerEvent(EventConst.ActivityTemplateOnPlay,2);
		base.CloseAnimation();
	}

	private void SetIcon(int id)
	{
		_oneImg.texture = ResourceManager.Load<Texture>("Prop/"+id);
		_moreImg.texture = ResourceManager.Load<Texture>("Prop/"+id);
		_haveNumImg.texture = ResourceManager.Load<Texture>("Prop/"+id);
	}
	
	/// <summary>
	/// 抽卡
	/// </summary>
	/// <param name="haveNum">拥有数量</param>
	/// <param name="price">单价</param>
	/// <param name="playMoreNum">右边按钮多玩几次</param>
	/// <param name="presentedNum">额外赠送次数</param>
	public void SetData(int id, int haveNum,int price,int playMoreNum)
	{
		_haveNumTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplatePlayShowNum", haveNum);
		_playOneBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplatePlayOne");
		_playMoreBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplatePlayMore",playMoreNum);
		_onePriceTxt.text = "x" + price;
		_morePriceTxt.text = "x" + price * playMoreNum;

		SetIcon(id);
	
		if (haveNum>price*playMoreNum)
		{
			_redDot.Show();
		}
		else
		{
			_redDot.Hide();
		}
		
//		_playOneBtn.onClick.AddListener(() =>
//		{
//			Debug.LogError("haveNum===>"+haveNum);
//			Debug.LogError("price===>"+price);
////			if (haveNum<price)
////			{
////				FlowText.ShowMessage(I18NManager.Get("ActivityTemplate_ActivityTemplatePlayItemInsufficient"));
////				 return;
////			}
//			
//			
//		});
		
//		_playOneBtn.onClick.AddListener(() =>
//		{
////			if (haveNum<price*playMoreNum)
////			{
////				FlowText.ShowMessage(I18NManager.Get("ActivityTemplate_ActivityTemplatePlayItemInsufficient"));
////				return;
////			}
//			
//			
//			base.Close();
//		});
	}
}
