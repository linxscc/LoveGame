using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class LoveAppointmentModule : ModuleBase
{
	private LoveChoosePanel _loveappointmentPanel;
	private JournalChoosePanel _journalChoosePanel;
	private LoveStoryPanel _loveStoryPanel;
	private NoRoleWindow _noRoleWindow;
	private AppointmentRuleVo _AppointmentVo;
	
	public enum AppointmentViewState
	{
		ChooseRole,
		ChooseJournal,
		ChooseLoveStory
	}

	public override void Init()
	{
		GuideManager.RegisterModule(this);
		if (_AppointmentVo!=null)
		{
			OpenStoryGate();
			_loveStoryPanel.SetData(_AppointmentVo,GlobalData.AppointmentData);
		}
		else
		{
			ShowAppointmentPanel();
		}
			
	}

	private void ShowAppointmentPanel()
	{
		_loveappointmentPanel=new LoveChoosePanel();
		_loveappointmentPanel.Init(this);
		_loveappointmentPanel.Show(0);
	}

	public override void OnShow(float delay)
	{
		GuideManager.OpenGuide(this);
		base.OnShow(delay);
		
        _loveStoryPanel?.OnShow();
		
    }
    
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.CMD_APPOINTMENT_SHOW_CHOOSEROLE:
				if (_loveappointmentPanel==null)
				{
					ShowAppointmentPanel();
				}
				_loveappointmentPanel.Show(0);
				_journalChoosePanel.Hide();
				if (message?.Body!=null)
				{
					_loveappointmentPanel.BackView();
				}
				break;
			case MessageConst.CMD_APPOINTMENT_SHOW_JOURNALCHOOSE:
				if (_journalChoosePanel == null)
				{
					_journalChoosePanel=new JournalChoosePanel();
					_journalChoosePanel.Init(this);
				}
				_loveappointmentPanel?.Hide();
				_journalChoosePanel.Show(0);
				_journalChoosePanel.ShowBackBtn();
				_loveStoryPanel?.Hide();
				if (message?.Body != null)
				{
					GlobalData.CardModel._curRoleId = (int) message.Body;
					_journalChoosePanel.SetData(GlobalData.CardModel._curRoleId);
				}
				else
				{
					_journalChoosePanel.SetData(GlobalData.CardModel._curRoleId);
				}

				break;
			case MessageConst.CMD_APPOINTMENT_SHOW_LOVESTORY:
				OpenStoryGate();
				_loveStoryPanel.SetData((AppointmentRuleVo)body[0],GlobalData.AppointmentData,false);
				break;
			case MessageConst.CMD_APPOINTMENT_ACTIVE_LOVESTORY:
				OpenStoryGate();
				_loveStoryPanel.SetData((AppointmentRuleVo)body[0],GlobalData.AppointmentData,true);
				break;
			case MessageConst.CMD_APPOINTMENT_SHOW_NOCARD:
				if (_noRoleWindow==null)
				{
					_noRoleWindow=PopupManager.ShowWindow<NoRoleWindow>("LoveAppointment/Prefabs/NoRoleWindows");
				}
				_noRoleWindow.SetData((AppointmentRuleVo)message.Body);
				break;
			case MessageConst.CMD_lOVEAPPOINTMENT_STORYEND:
				_loveStoryPanel.SetStoryEndData((int[])message.Body);
				break;
			case MessageConst.CMD_APPOINTMENT_GUIDE_JOURNALCHOOSE:
				//打开日记关卡的界面,看来必须是个Panel

				if (_journalChoosePanel == null)
				{
					_journalChoosePanel=new JournalChoosePanel();
					_journalChoosePanel.Init(this);
//					_journalChoosePanel._journalChooseController.AppointmentModel =
//						GlobalData.AppointmentData;
				}
				_loveappointmentPanel.Hide();
				_journalChoosePanel.Show(0);
				_journalChoosePanel.ShowBackBtn();
				_loveStoryPanel?.Hide();
				if (message?.Body != null)
				{
					Debug.LogError((int) message.Body);
					GlobalData.CardModel._curRoleId = (int) message.Body;
					_journalChoosePanel.GuideLoveAppointment(GlobalData.CardModel._curRoleId);
				}
				else
				{
					_journalChoosePanel.GuideLoveAppointment(GlobalData.CardModel._curRoleId);
				}

				break;
			case MessageConst.CMD_APPOINTMENT_GUIDE_NEXTPAGE:
				_journalChoosePanel.GuideToNextPage();
				break;
			case MessageConst.CMD_APPOINTMENT_GUIDE_BACKTOLOVEMAIN:
				_loveappointmentPanel.GoBackToLoveMain();
				break;
			
		}
	}

	private void OpenStoryGate()
	{
		if (_loveStoryPanel == null)
		{
			_loveStoryPanel=new LoveStoryPanel();
			_loveStoryPanel.Init(this);
		}
		_journalChoosePanel?.Hide();
		_loveStoryPanel.Show(0);
		

	}

	public override void SetData(params object[] paramsObjects)
	{
		if (paramsObjects.Length>0)
		{

			_AppointmentVo = (AppointmentRuleVo)paramsObjects[0];

		}
	}

	public void Destroy()
	{
	}
}