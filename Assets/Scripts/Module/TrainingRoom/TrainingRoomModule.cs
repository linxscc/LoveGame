using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Componets;
using DataModel;
using UnityEngine;


public class TrainingRoomModule : ModuleBase
{
	private TrainingRoomPanel _trainingRoomPanel;
	private SongChoosePanel _songChoosePanel;
	private RankingPanel _rankingPanel;
	private ExchangeShopPanel _exchangeShopPanel;
	private ChooseCardPanel _chooseCardPanel;
	
	public override void Init()
	{	
		Debug.LogError("进入练习室");
		LoadingOverlay.Instance.Show();
		NetWorkManager.Instance.Send<Rules>(CMD.MUSICGAMEC_RULES, null, OnGetMusicGameRule,null,true,GlobalData.VersionData.VersionDic[CMD.MUSICGAMEC_RULES]);

		RegisterModel<TrainingRoomModel>();
		
		ClientData.LoadItemDescData(null);
	}

	private void OnGetMusicGameRule(Rules res)
	{
		GlobalData.TrainingRoomModel.InitRules(res);       
		NetWorkManager.Instance.Send<OpenMusicGame>(CMD.MUSICGAMEC_OPENMUSICGAME,null,GetMusicGameData);
	}
	
	private void GetMusicGameData(OpenMusicGame res)
	{		
		LoadingOverlay.Instance.Hide();
		GlobalData.TrainingRoomModel.InitOpenMusicGame(res);	
		
		_trainingRoomPanel =new TrainingRoomPanel();
		_trainingRoomPanel.Init(this);
		_trainingRoomPanel.Show(0.5f);
	}
	

	
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.MODULE_TRAININGROOM_GOTO_PLAY_PANEL:	
				_songChoosePanel =new SongChoosePanel();
				_songChoosePanel.Init(this);
				_songChoosePanel.Show(0.5f);					
				break;
			case MessageConst.MODULE_TRAININGROOM_GOTO_RANKING_PANEL:				
				_rankingPanel =new RankingPanel();			
				_rankingPanel.Init(this);
				_rankingPanel.Show(0.5f);				
				break;
			case MessageConst.MODULE_TRAININGROOM_GOTO_SHOP_PANEL:					
				_exchangeShopPanel=new ExchangeShopPanel();				
				_exchangeShopPanel.Init(this);
				_exchangeShopPanel.Show(0.5f);		
				break;	
			case MessageConst.MODULE_TRAININGROOM_GOTO_CHOOSECARD_PANEL:				
				_chooseCardPanel  = new ChooseCardPanel();			
				_chooseCardPanel.Init(this);
				_chooseCardPanel.Show(0.5f);			
				break;						
			case MessageConst.MODULE_TRAININGROOM_SHOW_BACKBTN:		
				_trainingRoomPanel.ShowBackBtn();				
				break;
			case MessageConst.MODULE_TRAININGROOM_SHOW_SONGCHOOSEVIEW_BACKBTN:			
				_songChoosePanel.ShowBackBtn();				
				break;
			case MessageConst.MODULE_TRAININGROOM_GET_RES_FAILED:
				_trainingRoomPanel.OnBackClick();
				break;
			case MessageConst.MODULE_TRAININGROOM_CHOOSE_NUM_ENOUGH:
				_chooseCardPanel.Destroy();
				_songChoosePanel.ShowBackBtn();
				SendMessage(new Message(MessageConst.CMD_TRAININGROOM_CREATE_CHOOSE_CARD));
				break;
			
		}
	}


	

	
	
	
}
