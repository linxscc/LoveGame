using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using UnityEngine;
public class CoaxSleepOnPlayAudioPanel : ReturnablePanel
{
    private string _path = "CoaxSleep/Prefabs/CoaxSleepOnPlayAudioView";    
    private CoaxSleepOnPlayAudioController _controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<CoaxSleepOnPlayAudioView>(_path);
        _controller =new CoaxSleepOnPlayAudioController{View = viewScript};
        RegisterView(viewScript);
        RegisterController(_controller);
        _controller.Start();
        SendGuideMessage();
    }
    
    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }
    
    public void OnShow()
    {
        ShowBackBtn();
       // Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }
    
    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.CMD_CPAXSLEEP_DESTROY_PANEL,"OnPlayAudioPanel"));
    }
    
    
    public void GetCurData(MyCoaxSleepAudioData data)
    {
        _controller.GetCurData(data);
    }
 
    public override void Destroy()
    {
        base.Destroy();
        AudioManager.Instance.PlayDefaultBgMusic();
    }

    private void SendGuideMessage()
    {
        var coaxSleep = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);

        if (coaxSleep== FunctionGuideStage.Function_CoaxSleep_TowStage)
        {
            SendMessage(new Message(MessageConst.CMD_GUIDE_COAXSLEEP_TOW_STAGE)); 
        }
       
    }
}
