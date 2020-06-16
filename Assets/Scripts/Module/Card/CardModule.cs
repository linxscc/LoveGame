using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CardModule : ModuleBase {
	
	private CardCollectionPanel _cardCollectionPanel;
	private CardDetailPanel _cardDetailPanel;
	private FullScreenCardPanel _fullScreenCardPanel;
	//private int _jumpfromOtherModule = 0;//从其他模块跳转
	private UserCardVo _userCardVo;       //用完后就赋值为null！
	int state = 0;

	public enum CardViewState
	{
		MyCard,
		Puzzle,
		Resolve
	}
	
	public override void Init()
    {
	    GuideManager.RegisterModule(this);
	    
	    if (_userCardVo!=null)
	    {
		    if (_cardDetailPanel == null)
		    {
			    _cardDetailPanel = new CardDetailPanel();
			    _cardDetailPanel.Init(this);
		    }
		    _cardDetailPanel.Show(0);
		    _cardDetailPanel.EnterFromOther = true;
		    _cardDetailPanel.SetData(_userCardVo);
		    _userCardVo = null;
	    }
	    else
	    {
		    //跳转回来的时候，有可能是在星缘回忆界面！
		    if (_cardDetailPanel!=null)
		    {
			    return;
		    }


		    if (_cardCollectionPanel==null)
		    {
			    _cardCollectionPanel = new CardCollectionPanel();
			    _cardCollectionPanel.Init(this);
			    RegisterModel<DrawData>();
			    _cardCollectionPanel.Show(0);  
		    }

		    if (state != 0)
		    {
			    _cardCollectionPanel.ChangeView(CardViewState.Resolve);
			    _cardCollectionPanel.SetResolveState();
		    }
			
	    }

    }

	public override void OnShow(float delay)
	{
		base.OnShow(delay);

		
		_cardDetailPanel?.OnShow();
		GuideManager.OpenGuide(this);
	}

	public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_CARD_COLLECTION_SHOW_CARD_DETAIL_VIEW:
	            if (_cardDetailPanel == null)
	            {
		            _cardDetailPanel = new CardDetailPanel();
		            _cardDetailPanel.Init(this);
	            }
	            _cardDetailPanel.EnterFromOther = false;
	            _cardDetailPanel.Show(0);
	            _cardCollectionPanel.Hide();
	            _cardDetailPanel.SetData((UserCardVo)body[0]);
	            break;
            
	        case MessageConst.MODULE_CARD_COLLECTION_BACK_TO_CARD_LIST_VIEW:
		        if(_cardDetailPanel != null)
			        _cardDetailPanel.Hide();
		        _cardCollectionPanel.Show(0);
		        _cardCollectionPanel.ChangeTabBar(GlobalData.CardModel.CurPlayerPb,false);
		        break;
	        
	        case MessageConst.MODULE_CARD_COLLECTION_CHANEG_VIEW:
		        _cardCollectionPanel.ChangeView((CardViewState) message.Body);
		        break;
	        
	        case MessageConst.MODULE_CARD_TABBAR_SELECT_CHANGE:
		        OnTabChange((PlayerPB) message.Body);
		        break;
	        
	        case MessageConst.MODULE_CARD_SHOW_FULLSCREEN_CARD:
		        _fullScreenCardPanel = new FullScreenCardPanel();
		        _fullScreenCardPanel.Init(this);
		        _fullScreenCardPanel.SetTexture((RawImage) body[0],(RawImage) body[1]);
		        
		        if(_cardDetailPanel != null)
			        _cardDetailPanel.Hide();

		        _fullScreenCardPanel.Show(0);
		        
		        break;
	        case MessageConst.MODULE_CARD_CLOSE_FULLSCREEN:
		        _fullScreenCardPanel.Destroy();
		        _cardDetailPanel.BackFromFullScreen();
		        break;
	        
        }
    }

	private void OnTabChange(PlayerPB pb)
	{
		GlobalData.CardModel.CurPlayerPb = pb;
		_cardCollectionPanel.ChangeTabBar(GlobalData.CardModel.CurPlayerPb);
		
	}
	
	public override void SetData(params object[] paramsObjects)
	{
//		if (paramsObjects==null)
//		{
//			return;
//			
//		}
		
		

		if (paramsObjects.Length > 0)
		{
			string targetTap = "";
			if (paramsObjects[0] is string)
			{
				targetTap= (string) paramsObjects[0];
			}
			
			if (paramsObjects[0].GetType()==typeof(UserCardVo))
			{
				_userCardVo = (UserCardVo) paramsObjects[0];
			}
			else if(targetTap=="CardResolve")
			{
				//临时作为跳转到星缘回溯模块
				if (_cardCollectionPanel == null)
				{					
					this.OnShow(0);
					_cardDetailPanel?.Hide();
					_cardCollectionPanel = new CardCollectionPanel();
					_cardCollectionPanel.Init(this);
					ModuleManager.Instance.Remove(ModuleConfig.MODULE_LOVEAPPOINTMENT);
					_cardCollectionPanel.Show(0);
					RegisterModel<DrawData>();
					_cardCollectionPanel.ChangeView(CardViewState.Resolve);
					_cardCollectionPanel.SetResolveState();
					return;
				}
				
				if (_cardDetailPanel!=null)
				{
					_cardDetailPanel.OnBackClick();
					ModuleManager.Instance.Remove(ModuleConfig.MODULE_LOVEAPPOINTMENT);
					_cardDetailPanel.EnterFromOther = true;
					_cardCollectionPanel.ChangeView(CardViewState.Resolve);
					_cardCollectionPanel.SetResolveState();
				}
				else
				{
					//todo 之后要做到切换任意一个界面！
					state = 3;
				}
				
				
			}

		}
		
		
	}
	

	public override void Remove(float delay)
	{
		base.Remove(delay);
		_cardCollectionPanel?.Destroy();
		if(_cardDetailPanel != null)
			_cardDetailPanel.Destroy();
	}

	public void Destroy()
    {

    }
}
