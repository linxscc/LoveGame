using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide;
using Common;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using QFramework;
using UnityEngine;

public class CoaxSleepGuideView : View
{


    /// <summary>
    /// 显示选男主界面引导
    /// </summary>
    public void ShowMainViewGuide()
    {
        GuideManager.SetRemoteGuideStep(GuideTypePB.LoveGuideCoaxSleep, GuideConst.FunctionGuide_CoaxSleep_OneStage);
        
        Transform mainView = transform.Find("MainView");
        mainView.gameObject.Show();

        var bottom = mainView.Find("Bottom");
        var chi = bottom.GetButton("Chi");
        var yan = bottom.GetButton("Yan");
        var tang = bottom.GetButton("Tang");
        var qin = bottom.GetButton("Qin");
        var returnBtn = mainView.Find("ReturnBtn").gameObject;

        PointerClickListener.Get(returnBtn).onClick = go =>
        {
            FlowText.ShowMessage(I18NManager.Get("Guide_CoaxSleep_ReturnBtnHint"));
        };
        

        chi.onClick.AddListener(() => { OnClickPlayer(PlayerPB.ChiYu,mainView); });        
        yan.onClick.AddListener(() => { OnClickPlayer(PlayerPB.YanJi,mainView); });
        tang.onClick.AddListener(() => { OnClickPlayer(PlayerPB.TangYiChen,mainView);});
        qin.onClick.AddListener(() => { OnClickPlayer(PlayerPB.QinYuZhe,mainView); });

        
    }

    private void OnClickPlayer(PlayerPB pb,Transform view)
    {
        view.gameObject.Hide();
        SendMessage(new Message(MessageConst.CMD_COAXSLEEP_GOTO_CUR_PLAYER,Message.MessageReciverType.UnvarnishedTransmission,pb));
        ShowPlayerView();
    }

    /// <summary>
    /// 显示选歌曲引导
    /// </summary>
    private void ShowPlayerView()
    {
        var playerView = transform.Find("PlayerView").gameObject;
        playerView.Show();
        PointerClickListener.Get(playerView).onClick = go => { playerView.Hide(); };
    }


  
    
    /// <summary>
    /// 显示播放页面引导
    /// </summary>
    public void ShowOnPlayView()
    {
        GuideManager.SetRemoteGuideStep(GuideTypePB.LoveGuideCoaxSleep, GuideConst.FunctionGuide_CoaxSleep_TwoStage); 
        
        
        Transform onPlayView = transform.Find("OnPlayView");
        onPlayView.gameObject.Show();
        var oneStep = onPlayView.Find("1").gameObject;
        var twoStep = onPlayView.Find("2").gameObject;
        var threeStep = onPlayView.Find("3").gameObject;

        var oneArrow = onPlayView.Find("1/Slider");
        var twoArrow = onPlayView.Find("2/LoopBtn");
        var threeArrow = onPlayView.Find("3/HintBg"); 
        GuideArrow.DoAnimation(oneArrow);
        GuideArrow.DoAnimation(twoArrow);
        GuideArrow.DoAnimation(threeArrow);
        oneStep.Show();
        
        PointerClickListener.Get(oneStep).onClick = go =>
        {
            oneStep.Hide();
            twoStep.Show();
            
        };

        PointerClickListener.Get(twoStep).onClick = go =>
        {
            twoStep.Hide();
            threeStep.Show();
        };
        PointerClickListener.Get(threeStep).onClick = go =>
        {
            threeStep.Hide();
            gameObject.Hide();
        };
    }
    
    
}
