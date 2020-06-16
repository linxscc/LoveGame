using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using game.main;
using Module.Card.Controller;


public class JournalChoosePanel : ReturnablePanel
{


	private JournalChooseView _journalChooseView;
	public JournalChooseController _journalChooseController;


	private PlayerPB _currentTab = PlayerPB.None;
    
	public override void Init(IModule module)
	{
		SetComplexPanel();//待研究这个方法
		base.Init(module);
        
		_journalChooseView=(JournalChooseView)InstantiateView<JournalChooseView>("LoveAppointment/Prefabs/JournalChooseView");
		_journalChooseController=new JournalChooseController();
		RegisterController(_journalChooseController);
		_journalChooseController.View = _journalChooseView;

		_journalChooseView.Show();
		
	}


	public void SetData(int vo)
	{
		_journalChooseController.SetData(vo);
	}

	public void GuideLoveAppointment(int vo)
	{
		_journalChooseController.GuideData(vo);
	}

	public void GuideToNextPage()
	{
		_journalChooseView.NextPage();
	}
	
	public override void Show(float delay)
	{
		base.Show(0);
		Main.ChangeMenu(MainMenuDisplayState.HideAll);
		_journalChooseView.gameObject.Show();
		ShowBackBtn();
	}
    
	public override void OnBackClick()
	{
		_journalChooseController.SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_CHOOSEROLE,1));
	}
        
	public override void Hide()
	{
		_journalChooseView.gameObject.Hide();
	}

}