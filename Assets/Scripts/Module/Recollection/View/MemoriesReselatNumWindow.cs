using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class MemoriesReselatNumWindow : Window
{

	private Text _contentTxt;
	private Text _mayBuyNumTxt;
	private Button _cancelBtn;
	private Button _okBtn;
	private int _max;
	
	private void Awake()
	{
		_contentTxt = transform.GetText("contentText");
		_mayBuyNumTxt = transform.GetText("MayBuyNumTxt");
		_cancelBtn = transform.GetButton("cancelBtn");
		_okBtn = transform.GetButton("okBtn");
		
		_max = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_NUM_MAX);
		
		_cancelBtn.onClick.AddListener((() => { Close(); }));
		
		_okBtn.onClick.AddListener((() =>
		{
			
			WindowEvent = WindowEvent.Ok;
			Close();
		}));
	}


	public override void Close()
	{
		base.Close();
		if (GuideManager.GetRemoteGuideStep(GuideTypePB.CardMemoriesGuide) < 1040)
		{
			EventDispatcher.TriggerEvent(EventConst.MemoriesReselatNumWindowClose);
		}		 
	}

	/// <summary>
	/// 设置数据
	/// </summary>
	/// <param name="mayBuyNum">可买次数</param>
	/// <param name="costGemNum">花费钻石数量</param>
	/// <param name="isInitiative">是否主动点</param>
	public void SetData(int mayBuyNum,int costGemNum,bool isInitiative)
	{
		_contentTxt.text = I18NManager.Get(isInitiative ? "Recollection_InitiativeOnClick" : "Recollection_NoInitiativeOnClick", costGemNum, _max, _max);
		_mayBuyNumTxt.text = I18NManager.Get("Recollection_MayBuyNum",mayBuyNum);
	}
}
