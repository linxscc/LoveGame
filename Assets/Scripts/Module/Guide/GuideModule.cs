using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Guide.ModuleView;
using Assets.Scripts.Module.Guide.ModuleView.Task;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Module.Guide.ModuleView;
using Module.Guide.ModuleView.Card;
using Module.Guide.ModuleView.DrawCard;
using Module.Guide.ModuleView.Gameplay;
using Module.Guide.ModuleView.Recollection;
using Module.Guide.ModuleView.Story;
using Module.Guide.ModuleView.Supporter;
using Module.Guide.ModuleView.Visit;
using UnityEngine;
using Utils;

public class GuideModule : ModuleBase
{
    public override void Init()
    {
    }

    /// <summary>
    ///     处理非引导模块发过来的消息
    /// </summary>
    /// <param name="message"></param>
    public void HandleModuleMessage(Message message)
    {
        SendMessage(message);
    }


    public override void OnMessage(Message message)
    {
        var name = message.Name;
        var body = message.Params;
        switch (name)
        {
            case MessageConst.MOUDLE_GUIDE_END_LOCAL:
                //本地记录的引导结束
                _currentPanel.Destroy();
                GuideManager.SetStepState(GuideStae.Close, (string) message.Body);
                GuideManager.Hide();
                break;

            case MessageConst.MOUDLE_GUIDE_STEP_REMOTE:
                SetRemoteGuideStep((GuideTypePB) body[0], (int) body[1]);
                break;

            case MessageConst.MOUDLE_GUIDE_END_SERVER:
                _currentPanel.Destroy();
                GuideManager.Hide();
                break;
        }

        UnvarnishedTransmissionMessage(message);
    }

    /// <summary>
    ///     设置新手引导统计点
    /// </summary>
    /// <param name="step"></param>
    /// <param name="guideType"></param>
    public void SetStatisticsRemoteGuideStep(int step)
    {
        var type = GuideTypePB.MainGuideRecord;
        var lastStep = GuideManager.GetRemoteGuideStep(type); //获取上次的点
        Debug.LogError("获取统计上次的点===>" + lastStep);
        if (step <= lastStep)
        {
            Debug.LogError("拦截掉重复发的点===>" + step);
            return;
        }

        ClientTimer.Instance.DelayCall(() =>
        {
            Debug.LogError("发送统计的点p===>" + step);
            var req = new GuideReq
            {
                GuideType = type,
                GuideId = step
            };
            var buffer = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<GuideRes>(CMD.USERC_GUIDE, buffer, res =>
            {
                Debug.LogError("更新统计点===>" + res.UserGuide.GuideId + "更新的统计类型===>" + res.UserGuide.GuideType);
                GuideManager.UpdateRemoteGuide(res.UserGuide);
            });
        }, 0.3f);
    }


    public void SetRemoteGuideStep(GuideTypePB guideType, int step)
    {
        //step = 1000;
        var currentStep = GuideManager.GetRemoteGuideStep(guideType);
        //引导步骤大于要设置的步骤的时候不做处理
        Debug.LogError("currentStep" + currentStep + "step" + step);
        if (currentStep >= step)
            return;
        LoadingOverlay.Instance.Show();

        var req = new GuideReq
        {
            GuideType = guideType,
            GuideId = step
        };

        Debug.LogError("guideType:" + guideType + "  step:" + step);

        var buffer = NetWorkManager.GetByteData(req);

        NetWorkManager.Instance.Send<GuideRes>(CMD.USERC_GUIDE, buffer, OnSetGuideSuccess);
    }

    private void OnSetGuideSuccess(GuideRes res)
    {
        GuideManager.UpdateRemoteGuide(res.UserGuide);
        LoadingOverlay.Instance.Hide();
        Debug.Log("<color='#00ff66'>引导步骤：" + res.UserGuide + "</color>");

        RewardUtil.AddReward(res.Award);

        SendMessage(new Message(MessageConst.MOUDLE_GUIDE_RECEIVE_REMOTE_STEP, Message.MessageReciverType.DEFAULT,
            res));
        SendMessage(new Message(MessageConst.MOUDLE_GUIDE_RECEIVE_REMOTE_STEP,
            Message.MessageReciverType.UnvarnishedTransmission, res));
    }


    private void UnvarnishedTransmissionMessage(Message message)
    {
        if (message.Type != Message.MessageReciverType.UnvarnishedTransmission)
            return;

        GuideManager.UnvarnishedTransmissionMessage(message);
    }

    public void ShowSupporterModule()
    {
        CreatePanel(new SupporterGuidePanel());
    }

    public void ShowVisitModule()
    {
        CreatePanel(new VisitGuidePanel());
    }

    public void ShowTakePhotosGameModule()
    {
        CreatePanel(new TakePhotosGameGuidePanel());
    }

    public void ShowVisitBlessModule()
    {
        CreatePanel(new VisitGuideBlessPanel());
    }

    public void ShowSupporterActModule()
    {
        if (GuideManager.GetRemoteGuideStep(GuideTypePB.EncourageActGuide) >= 1020)
            return;

        CreatePanel(new SupporterActGuidePanel());
    }


    public void ShowLoveDiaryModule()
    {
        // var stage = GuideManager.CurStage();
        //
        // if (stage == GuideStage.LoveDiaryStage)
        // {
        //     CreatePanel(new LoveDiaryGuidePanel());    
        // }
    }

    //加载星缘回忆Module
    public void ShowRecollectionModule()
    {
        if (GuideManager.GetRemoteGuideStep(GuideTypePB.CardMemoriesGuide) > 0)
            return;

        CreatePanel(new RecollectionGuidePanel());
    }

    public void ShowMainPanelGuide()
    {
        var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);

        if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) >= GuideConst.MainStep_Over &&
            coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_TowStage)
            return;

        CreatePanel(new MainPanelGuidePanel());
    }

    public void ShowMainLineGuide()
    {
        var stage = GuideManager.CurStage();
        var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);

        if (stage == GuideStage.MainLine1_1Level_1_3Level_Stage ||
            stage == GuideStage.MainLine1_4Level_2_3Level_Stage ||
            coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_OneStage)
            CreatePanel(new MainLineGuidePanel());
    }

    public void ShowBattleGuide()
    {
        if (_currentPanel?.GetType() == typeof(BattleGuidePanel))
            return;

        if (!GlobalData.LevelModel.FindLevel("3-3").IsPass)
        {
            CreatePanel(new BattleGuidePanel());
        }
    }

    public void ShowPhoneModule()
    {
        // var stage = GuideManager.CurStage();
        //
        // if (stage == GuideStage.PhoneSmsStage)
        // {
        //     CreatePanel(new PhoneGuidePanel());
        // }
    }


    public void ShowStoryGuide()
    {
        LevelVo level = GlobalData.LevelModel.FindLevel("3-3");
        if (level!=null && !level.IsPass)
        {
            CreatePanel(new StoryGuidePanel());
        }
    }

    public void ShowCardGuide()
    {
        _currentPanel?.Destroy();

        if (GuideManager.TempState == TempState.AchievementOver ||
            GuideManager.CurStage() == GuideStage.CardLevelUpStage) CreatePanel(new CardGuidePanel());
    }


    /// <summary>
    ///     主线好感度引导
    /// </summary>
    public void ShowFavorabilityGuide()
    {
        var stage = GuideManager.CurStage();

        if (stage == GuideStage.FavorabilityShowRoleStage) CreatePanel(new FavorabilityGuidePanel());
    }

    /// <summary>
    ///     哄睡引导
    /// </summary>
    public void ShowCoaxSleepGuide()
    {
        var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);
        if (coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_End)
            return;

        CreatePanel(new CoaxSleepGuidePanel());
    }


    private void CreatePanel(Panel panel, bool destoryCurrentPanel = true)
    {
        GuideManager.Show();

        if (destoryCurrentPanel)
            _currentPanel?.Destroy();

        panel.Init(this);
        panel.Show(0);

        Debug.Log("GuideModule CreatePanel=======>" + panel.GetType());
        _currentPanel = panel;
    }


    public void ShowGameplayGuide()
    {
        _currentPanel?.Destroy();
        CreatePanel(new GamePlayGuidePanel());
    }

    public void ShowDrawCardGuide()
    {
        if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) >= GuideConst.MainLineStep_OnClick_GlodDrawCard)
        {
            _currentPanel?.Destroy();
            return;
        }

        CreatePanel(new DrawCardGuidePanel());
    }

    public void ShowLoveGuide()
    {
        var stage = GuideManager.CurStage();
        var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);
        if (stage == GuideStage.CardLevelUpStage ||
            stage == GuideStage.LoveStoryStage ||
            stage == GuideStage.LoveDiaryStage ||
            coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_OneStage &&
            GuideManager.IsPass4_12())
            CreatePanel(new LoveGuidePanel());
    }

    public void ShowLoveAppointmentGuide()
    {
        // if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) > GuideConst.MainLineStep_OnClick_GlodDrawCard)
        // {
        //     _currentPanel?.Destroy();
        //     return;
        // }

        if (GuideManager.CurStage() < GuideStage.Over)
            CreatePanel(new LoveAppointmentGuidePanel());
    }

    public void ShowAchievementGuide()
    {
        if (GuideManager.TempState == TempState.Level3_3_Fail) CreatePanel(new AchievementGuidePanel());
    }

    public void ShowActivityGuide()
    {
        var stage = GuideManager.CurStage();
        if (stage == GuideStage.SevenDaySigninActivityStage) CreatePanel(new ActivityGuidePanel());
    }

    public void ShowTaskGuide()
    {
        var stage = GuideManager.CurStage();
        if (stage == GuideStage.MainLineStep_Stroy1_9_Over) CreatePanel(new TaskGuidePanel());
    }
}