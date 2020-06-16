using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using UnityEngine;
using EventType = game.main.EventType;

public class StoryController : Controller
{
    public StoryView View;
    private StoryModel _storyModel;
    public int[] Appointmentdata = {0, 0};
    private byte[] _lastChallengeData;

    public override void Start()
    {
        _storyModel = GetData<StoryModel>();

//        if(_storyModel.Level==null)
//        {
//            return;
//        }
//
//        int step = -1;
//        if (_storyModel.Level.IsPass == false)
//        {
//            switch (_storyModel.Level.LevelMark)
//            {
//                case "1-2":
//                    step = GuideConst.MainStep_MainStory1_2_Start;
//                    break;
//                case "1-3":
//                    step = GuideConst.MainStep_MainStory1_3_Start;
//                    break;
//                case "1-4":
//                    step = GuideConst.MainStep_MainStory1_4_Start;
//                    break;
//            }
//            
//            if(step != -1)
//            {
//                GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, step);
//            }
//        }
    }

    public void LoadStoryById(string id)
    {
        Debug.Log("story id = " + id);

        View.Reset();
        _storyModel.LoadStroyById(id, (list) => { View.InitData(list); });
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_STORY_READY:
                Main.ChangeMenu(MainMenuDisplayState.HideAll);
                break;
            case MessageConst.CMD_STORY_BRANCH_SELECTED:
                string id = (string) body[0];
                View.Reset();
                _storyModel.LoadStroyById(id, (list) => { View.InitBranch(list); });
                break;
            
            case MessageConst.CMD_STORY_END:
                if (_storyModel.StoryType == StoryType.MainStory)
                {
                    //主线剧情
                    if (_storyModel.Level.IsPass)
                    {
                        //剧情只在第一次看的时候发送后端请求
                        OnGetChallengeData(null);
                        return;
                    }
                    
                    var req = new ChallengeReq();
                    req.LevelId = _storyModel.Level.LevelId;
                    _lastChallengeData = NetWorkManager.GetByteData(req);

                    LoadingOverlay.Instance.Show();
                    
                    GetService<BattleService>()
                        .Request(_lastChallengeData)
                        .SetCallback(OnGetChallengeData, OnGetChallengeDataFail)
                        .Execute();
                }
                else if (_storyModel.StoryType == StoryType.Visit)
                {
                    Debug.Log(" MessageConst.CMD_STORY_END");
                    //主线剧情
                    if (_storyModel.VisitLevel.IsPass)
                    {
                        //剧情只在第一次看的时候发送后端请求
                        OnGetChallengeData(null);
                        return;
                    }

                    var req = new VisitingChallengeReq();
                    req.LevelId = _storyModel.VisitLevel.LevelId;
                    _lastChallengeData = NetWorkManager.GetByteData(req);

                    LoadingOverlay.Instance.Show();
                    
                    GetService<VisitBattleService>()
                        .Request(_lastChallengeData)
                        .SetCallback(OnGetChallengeData, OnGetChallengeDataFail)
                        .Execute();
                    
                }
                else if (_storyModel.StoryType == StoryType.CreateUser)
                {
                    //序章创建用户
//                    EventDispatcher.TriggerEvent(EventConst.CreateUserEnd);
                    
                  //  GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide,GuideConst.MainStep_Pass0_1_End);
                    ModuleManager.Instance.Remove(ModuleConfig.MODULE_STORY);
                    
                }
                else if (_storyModel.StoryType == StoryType.LoveAppointment)
                {
                    //恋爱约会
                    //如果已经解锁就不必发送消息。
                    //在这里给后端发送通关约会的协议。
                    
                    EventDispatcher.TriggerEvent(EventConst.LoveStoryEnd,Appointmentdata);
                    //SendMessage(new Message(MessageConst.CMD_lOVEAPPOINTMENT_STORYEND,Appointmentdata));
                    ModuleManager.Instance.GoBack();

                }
                else if (_storyModel.StoryType == StoryType.ActivityCapsule)
                {
                    EventDispatcher.TriggerEvent<string, System.Action>(EventConst.ActivityCapsuleTemplateWatchOverStory, _storyModel.StoryId, ()=>
                    {
                        ModuleManager.Instance.GoBack();
                    });
                    
                    EventDispatcher.TriggerEvent<string, System.Action>(EventConst.WatchActivityStoryOver, _storyModel.StoryId, ()=>
                    {
                        ModuleManager.Instance.GoBack();
                    });
                }
                break;
        }
    }

    private void OnGetChallengeDataFail(HttpErrorVo vo)
    {

        FlowText.ShowMessage(I18NManager.Get("Story_Hint1", vo.ErrorCode));
        
        //FlowText.ShowMessage("剧情错误："+vo.ErrorCode);
        ModuleManager.Instance.GoBack();
        LoadingOverlay.Instance.Hide();
    }


    private void OnGetChallengeData(ChallengeRes res)
    {
        SendMessage(new Message(MessageConst.TO_GUIDE_BATTLE_RESULT, Message.MessageReciverType.DEFAULT,
            res, _storyModel.Level));
        
        LoadingOverlay.Instance.Hide();
        if (_storyModel.Level != null && (_storyModel.Level.LevelId == 103 || _storyModel.Level.LevelId == 109))
        {
            if (GuideManager.CurStage()!=GuideStage.MainLine1_1Level_1_3Level_Stage)
            {
                ModuleManager.Instance.GoBack();
            }
            
//            var curStage = GuideManager.CurStage(GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide));
//            if (curStage != GuideStage.MainLine1_1Level_1_3Level_Stage)
//            {
//                ModuleManager.Instance.GoBack();
//            }

        }
        else
        {
            ModuleManager.Instance.GoBack();
        }
    }
}