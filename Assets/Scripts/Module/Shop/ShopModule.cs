using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using game.main;
using UnityEngine;

public class ShopModule : ModuleBase
{

	private ShopMainPanel _shopMainPanel;
//	private BuyGemPanel _buyGemPanel;
	private int _jumpTarget;


	public override void Init()
    {

		    if (_shopMainPanel==null)
		    {
			    RegisterModel<ShopModel>();
			    _shopMainPanel=new ShopMainPanel();
			    _shopMainPanel.JumpTarget = _jumpTarget;
			    _shopMainPanel.Init(this);
			    
		    }
		    _shopMainPanel.Show(0);		


	    

    }

	public override void OnShow(float delay)
	{
		_shopMainPanel.Show(0);
	}

	public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
             case MessageConst.CMD_MALL_ENTERTOBUYGEM:
	             _shopMainPanel.Hide();
//	             _buyGemPanel.Show(0);
	             break;
             case MessageConst.CMD_MALL_BACKTOMAIN:
//	             Debug.LogError("Back");
	             _shopMainPanel.GoBack();	
//	             _buyGemPanel.Hide();
	             break;
	        
        }
    }


	public override void SetData(params object[] paramsObjects)
	{
		//跳转到商城可以传页签过来
		if (paramsObjects.Length>0)
		{
			int page=(int)paramsObjects[0];
			if (page>=0)
			{
				_jumpTarget = page;
			}

		}
	}

	public void Destroy()
    {

    }
}
