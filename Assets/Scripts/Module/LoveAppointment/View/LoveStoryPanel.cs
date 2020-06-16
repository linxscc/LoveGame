using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using game.main;
using Module.Card.Controller;
using UnityEngine;


public class LoveStoryPanel : ReturnablePanel
{


	private LoveStoryView _loveStoryView;
	public LoveStoryController _loveStoryController;

	//private AppointmentRuleVo _appointmentRuleVo;
	private PlayerPB _currentTab = PlayerPB.None;
	private bool _jumpFromCard = false;
    
	public override void Init(IModule module)
	{
		//SetComplexPanel();//待研究这个方法
		base.Init(module);

		_loveStoryView = (LoveStoryView)InstantiateView<LoveStoryView>("LoveAppointment/Prefabs/LoveStoryView");
		_loveStoryController=new LoveStoryController();
		RegisterController(_loveStoryController);
		_loveStoryController.View = _loveStoryView;
		
		_loveStoryController.Start();
	}

	public void OnShow()
	{
		ShowBackBtn();
		UpdateUserAppointmet();
		//如果是新手引导期间，要通知新手引导模块！
		EventDispatcher.TriggerEvent(EventConst.GuideToLoveStoryGoBack);
	}
	
	public void UpdateUserAppointmet()
	{
		_loveStoryController.UpdateAppointmentData();
	}
	
	public void SetData(AppointmentRuleVo vo,AppointmentModel appointmentModel,bool needactive=false)
	{
		appointmentModel.SetCurAppointmentRule(vo);
		if (needactive)
		{
			_loveStoryView.SetEmptyTexture();
			_loveStoryController.SetData(vo);
		}
		else
		{
			_loveStoryView.SetData(vo,appointmentModel);
		}
		//_loveStoryView.Show();
	}

	public void SetData(AppointmentRuleVo vo,AppointmentModel appointmentModel)
	{
		UserAppointmentVo uservo = appointmentModel.GetUserAppointment(vo.Id);
		appointmentModel.SetCurAppointmentRule(vo);
		_jumpFromCard = true;
		if (uservo==null||uservo.ActiveState==0)
		{
			if (uservo==null)
			{
				Debug.LogError("uservo null");
			}
			
			_loveStoryView.SetEmptyTexture();
			_loveStoryController.SetData(vo);
		}
		else
		{
			_loveStoryView.SetData(vo,appointmentModel);
		}



	}
	

	public void SetStoryEndData(int[] array)
	{
		_loveStoryController.UpdateStoryEndData(array);
	}
	
	public override void Show(float delay)
	{
		base.Show(0);
		Main.ChangeMenu(MainMenuDisplayState.HideAll);
		_loveStoryController.Show();
		//这个会导致刷新问题！
		ShowBackBtn();
	}
    
	
	public override void OnBackClick()
	{
		//_journalChooseController.SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_CHOOSEROLE));
		if (_jumpFromCard)
		{
			 ModuleManager.Instance.GoBack();
		}
		else
		{
			_loveStoryController.SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_JOURNALCHOOSE));
		}
		

	}
        
	public override void Hide()
	{
		_loveStoryView.gameObject.Hide();
	}

}