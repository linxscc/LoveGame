using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using game.main;
using UnityEngine;

public class BuyGemModule : ModuleBase
{


	private BuyGemPanel _buyGemPanel;


	public override void Init()
	{

			if (_buyGemPanel==null)
			{
				RegisterModel<BuyGemModel>();
				_buyGemPanel=new BuyGemPanel();
				_buyGemPanel.Init(this);				
			}
			_buyGemPanel.Show(0);
		

	}

	public override void OnShow(float delay)
	{
		base.OnShow(delay);
	}

	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
//			case MessageConst.CMD_MALL_ENTERTOBUYGEM:
//				if (_buyGemPanel==null)
//				{
//					RegisterModel<ShopModel>();
//					_buyGemPanel=new BuyGemPanel();
//					_buyGemPanel.Init(this);
//					_buyGemPanel.JumpFromShop = true;
//				}
//				_shopMainPanel.Hide();
//				_buyGemPanel.Show(0);
//				break;
//			case MessageConst.CMD_MALL_BACKTOMAINSHOP:
//				Debug.LogError("Back");
//				_shopMainPanel.Show(0);	
//				_buyGemPanel.Hide();
//				break;
	        
		}
	}

	
	public override void SetData(params object[] paramsObjects)
	{

		
	}
	

	public override void Remove(float delay)
	{
		base.Remove(delay);
	}

	public void Destroy()
	{

	}
}
