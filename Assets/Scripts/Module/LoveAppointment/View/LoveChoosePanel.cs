using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using Module.Card.Controller;


public class LoveChoosePanel : ReturnablePanel
{
	private LoveChooseView _loveChooseView;
	public LoveAppointmentlController _loveAppointmentController;



	private PlayerPB _currentTab = PlayerPB.None;
    
	public override void Init(IModule module)
	{
		SetComplexPanel();//待研究这个方法
		base.Init(module);

		_loveChooseView = (LoveChooseView)InstantiateView<LoveChooseView>("LoveAppointment/Prefabs/LoveChooseView");
		_loveAppointmentController=new LoveAppointmentlController();
		RegisterController(_loveAppointmentController);
		_loveAppointmentController.View = _loveChooseView;
		
		_loveAppointmentController.Start();
	}

	
	public override void Show(float delay)
	{
		base.Show(0);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		_loveChooseView.gameObject.Show();
		ShowBackBtn();
	}
    
        
	public override void Hide()
	{
		_loveChooseView.gameObject.Hide();
	}


	public void BackView()
	{
		_loveAppointmentController.BackToLoveChoose();
	}

	public void GoBackToLoveMain()
	{
		ModuleManager.Instance.GoBack();	
	}

}