using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using UnityEngine;

public class FansDetailPanel : ReturnablePanel
{


	private FansDetailView _fansDetailView;
	public FansDetailController _fansDetailController;
	private UserEncourageActVo _curUserEncourageActVo;
	

	private PlayerPB _currentTab = PlayerPB.None;
    
	public override void Init(IModule module)
	{
		SetComplexPanel();//待研究这个方法
		base.Init(module);
		_fansDetailView = (FansDetailView)InstantiateView<FansDetailView>("SupporterActivity/Prefabs/FansDetailView");
		_fansDetailController=new FansDetailController();
		RegisterController(_fansDetailController);
		_fansDetailController.View = _fansDetailView;
		_fansDetailController.Start();

	}

	
	public override void Show(float delay)
	{
		base.Show(0);
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		_fansDetailView.gameObject.Show();
		ShowBackBtn();
	}


	public void SetData(UserEncourageActVo vo)
	{
		_fansDetailController.SetData(vo);
		_curUserEncourageActVo = vo;
	}

	public void BackReview()
	{
		ShowBackBtn();
		_fansDetailController.SetData(_curUserEncourageActVo);
	}
	
	public override void Hide()
	{
		_fansDetailView.gameObject.Hide();
	}

	public override void OnBackClick()
	{
		_fansDetailController.SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GOBACK));
	}
	
	
	public void BackView()
	{
	}
}
