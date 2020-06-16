using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using UnityEngine;

public class DrawCardPanel : ReturnablePanel
{
    private DrawCardController _control;

    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<DrawCardView>("DrawCard/Prefabs/DrawCard");

        _control = new DrawCardController();
        _control.View = (DrawCardView) viewScript;
        _control.Start();

        RegisterModel<DrawData>();
        RegisterController(_control);
    }

    public void UpdateCardNum()
    {
        _control.UpdateCardNum();
    }


    public void UpdateTicke()
    {
        var num = GlobalData.PropModel.GetUserProp(PropConst.DrawCardByGem).Num;
        _control.View.UpdateTicke(num);
    }
    
    public override void Show(float delay)
    {
        Debug.LogError("Show.....................");
        //todo 引导检测打点检测
   
        ////if (Common.GuideManager.IsMainGuidePass())
        //if(!Common.GuideManager.IsMainGuidePass(GuideConst.MainStep_DrawCard_GetCard))
        //{
        //    Debug.LogError("Show.....................MainStep_DrawCard_GetCard");
        //    //SendMessage(new Message(MessageConst.TO_GUIDE_DRAWCARD_GEM, Message.MessageReciverType.DEFAULT));
        //    SendMessage(new Message(MessageConst.TO_GUIDE_DRAWCARD_GEM, Message.MessageReciverType.DEFAULT));
        //}
        //else if(!Common.GuideManager.IsMainGuidePass(GuideConst.MainStep_DrawCard_GetFans))
        //{
        //    Debug.LogError("Show.....................MainStep_DrawCard_GetFans");
        //    SendMessage(new Message(MessageConst.TO_GUIDE_DRAWCARD_GOLD, Message.MessageReciverType.DEFAULT));
        //}

        _control.View.Show(delay);
        ShowBackBtn();    
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

    public void MoveGoldView()
    {
        //_control.View.SetContentPos();
        _control.View.SetTabBtnPage(DrawCardView.GOLD_UI);
    }
    
    
    

    public override void OnBackClick()
    {
        base.OnBackClick();
    }

    public override void Destroy()
    {
        
        base.Destroy();
    }
    
   

}