using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.CreateUser;
using Common;
using game.main;
using UnityEngine;
using EventType = game.main.EventType;
using Com.Proto;

public class StoryModule : ModuleBase {
	
	private StoryPanel _storyPanel;
	private int[] _appointmentData=new int[]{0,0};
	private string _storyId;
	private StoryTelephonePanel _storyTelephonePanel;
	private StorySmsPanel _storySmsPanel;
	private StoryRecordWindow _storyRecordWindow;

	public override void Init()
	{
		AudioManager.Instance.TweenVolumTo(0.3f,0.2f);
		
	    _storyPanel = new StoryPanel();
		_storyPanel.Init(this);
		_storyPanel.AppointmentId = _appointmentData;
		_storyPanel.Start(_storyId);

		if (GetData<StoryModel>().StoryType == StoryType.CreateUser)
		{
			_storyPanel.ForceAutoPlay();
		}
		GuideManager.RegisterModule(this);
	}
	
	//注意，进入恋爱进入剧情也需要有后退键
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
	    StoryModel model = RegisterModel<StoryModel>();
        switch (name)
        {
	        case MessageConst.CMD_STORY_READY:
		        if (model.StoryType == StoryType.MainStory)
		        {
			        _storyPanel.ShowBackBtn();
			        _storyPanel.NeedToHideSkip(true);
		        }
                else if (model.StoryType == StoryType.Visit)
                {
                    _storyPanel.ShowBackBtn();
	                _storyPanel.NeedToHideSkip(true);
                }
                else if (model.StoryType==StoryType.LoveAppointment)
		        {
			        _storyPanel.ShowBackBtn();	
			        
			        _storyPanel.NeedToHideSkip(AppConfig.Instance.isTestMode);
#if UNITY_EDITOR
			        _storyPanel.NeedToHideSkip(true);
#endif
			        if (!GuideManager.IsPass1_9())
			        {
				        _storyPanel.NeedToHideSkip(true);  
			        }
		        }
                else if (model.StoryType == StoryType.ActivityCapsule)
                {
                    _storyPanel.NeedToHideSkip(true);
                    _storyPanel.ShowBackBtn();
                }
                else if(model.StoryType == StoryType.CreateUser)
		        {
			        _storyPanel.NeedToHideSkip(false);
			        _storyPanel.HideAllBtn();
		        }
		        
		        break;
	        
	        case MessageConst.MODULE_STORY_SHOW_RECORD_VIEW:
		        _storyRecordWindow = PopupManager.ShowWindow<StoryRecordWindow>("Story/Prefabs/StoryRecordWindow");
		        _storyRecordWindow.SetData(model.GetCurrentDialog());
		        break;
	        
	        case MessageConst.CMD_STORY_RECODE_DIALOG:
		        model.AddDialog(body[0], (string)body[1]);
		        break;
	        
	        case MessageConst.CMD_STORY_ON_EVENT:
		        EventType eventType = (EventType) body[0];
		        string eventId = (string) body[1];
		        bool isAutoPlay = (bool) body[2];

		        HideAllPanel();
		        
		        if (eventType == EventType.Telephone)
		        {
			        if (_storyTelephonePanel == null)
			        {
				        _storyTelephonePanel = new StoryTelephonePanel();
				        _storyTelephonePanel.Init(this);
			        }
			        _storyTelephonePanel.Show(isAutoPlay);
			        _storyTelephonePanel.LoadJson(eventId, _currentPanel != _storyTelephonePanel);

			        _currentPanel = _storyTelephonePanel;

		        }
		        else if (eventType == EventType.Story)
		        {
			        _storyPanel.Show(isAutoPlay);
			        
			        SendMessage(new Message(MessageConst.CMD_STORY_BRANCH_SELECTED,
				        Message.MessageReciverType.CONTROLLER, eventId));

			        _currentPanel = _storyPanel;
		        }
		        else if (eventType == EventType.Sms)
		        {
			        if (_storySmsPanel == null)
			        {
				        _storySmsPanel = new StorySmsPanel();
				        _storySmsPanel.Init(this);
			        }
			        _storySmsPanel.Show(isAutoPlay);
			        _storySmsPanel.LoadJson(eventId, _currentPanel != _storySmsPanel);

			        _currentPanel = _storySmsPanel;
		        }
		        break;
        }
    }

	private void HideAllPanel()
	{
		_storyPanel?.Hide();
		_storyTelephonePanel?.Hide();
		_storySmsPanel?.Hide();
	}


	public override void SetData(params object[] paramsObjects)
	{
		StoryModel model = RegisterModel<StoryModel>();
		model.Reset();
		
		//这里还要赋值StoryType
		if(paramsObjects[0] is LevelVo)
		{
			model.Level = (LevelVo) paramsObjects[0];
			_storyId = model.Level.StoryId;
			model.StoryType = StoryType.MainStory;
		}
		else if (paramsObjects[0] is AppointmentGateRuleVo)
		{
			//约会的关卡。
			var gate = (AppointmentGateRuleVo) paramsObjects[0];
			int appointmentid = (int) paramsObjects[1];
			
			_storyId = gate.SceneId;
			Debug.Log(gate.Gate+" "+_storyId);
			_appointmentData=new[] {gate.Gate, appointmentid};
			model.StoryType = StoryType.LoveAppointment;
        }
        else if (paramsObjects[0] is VisitLevelVo)
        {
            VisitLevelVo vo  = (VisitLevelVo)paramsObjects[0];
            model.VisitLevel = vo;
            _storyId = vo.StoryId;
            model.StoryType = StoryType.Visit;
        }
        else if (paramsObjects[0] is ActivityPlotRulePB)
        {
            ActivityPlotRulePB capsuleStoryRule = (ActivityPlotRulePB)paramsObjects[0];
            _storyId = capsuleStoryRule.PlotId;
            //_storyId = "24-10";
            model.StoryType = StoryType.ActivityCapsule;
        }
        else
		{
			_storyId = (string) paramsObjects[0];
			if (_storyId == "0-1")
			{
				model.StoryType = StoryType.CreateUser;
			}
		}
	}


	public override void Remove(float delay)
	{
		if (_storyRecordWindow != null)
		{
			_storyRecordWindow.Close();
		}
		base.Remove(delay);
		
		_storyPanel.Destroy();
		
		AudioManager.Instance.PlayDefaultBgMusic();
		AudioManager.Instance.TweenVolumTo(1,3,0.2f);
	}
}
