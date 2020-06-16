using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Common;
using game.main;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

public class LoveView :  View
{
    GameObject _Appointment;
    GameObject _Diary;
    GameObject _Clock;
    GameObject _Sleep;
    private void Awake()
    {
        _Appointment = transform.Find("Bg/AppointmentBtn").gameObject;
        _Diary = transform.Find("Bg/DiaryBtn").gameObject;
        _Clock = transform.Find("Bg/ClockBtn").gameObject;
        _Sleep = transform.Find("Bg/SleepBtn").gameObject;
        _Sleep.gameObject.Show();
        _Appointment.transform.Find("RedPoint").gameObject.SetActive(Util.GetIsRedPoint(Constants.REDPOINT_LOVE_BTN_LOVEAPPOINT));
      
        UIEventListener.Get(_Appointment).onClick=((g) =>
        {
       
            
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEAPPOINTMENT,false, true);
          //  Close();
        });
        UIEventListener.Get(_Diary).onClick = ((g) =>
        {

            if (!GuideManager.IsOpen( ModulePB.Love,FunctionIDPB.LoveDiary))
            {
                string desc = GuideManager.GetOpenConditionDesc(ModulePB.Love, FunctionIDPB.LoveDiary);
                FlowText.ShowMessage(desc);
                return;
            }
            
//            if (GlobalData.LevelModel.FindLevel("2-4").IsPass==false)          
//            //if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)<GuideConst.MainStep_Favorability_ChangeRole)
//            {
//                FlowText.ShowMessage(I18NManager.Get("Guide_Battle6","2-4"));
//                return;
//            }
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEDIARY,false,true);
           // Close();
        });
        UIEventListener.Get(_Clock).onClick=((g) =>
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            Debug.Log("_Clock");
            FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));//("暂未开放");
            // PopupManager.ShowAlertWindow("功能暂未开放");
            // Close();
        });
        //  UIEventListener.Get(transform.Find("Bg").gameObject).onClick = CloseWin;


        UIEventListener.Get(_Sleep).onClick = ((g) =>
        {
           // AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            Debug.Log("_Sleep");
           if (!GuideManager.IsOpen( ModulePB.Love,FunctionIDPB.CoaxSleep))
           {
               string desc = GuideManager.GetOpenConditionDesc(ModulePB.Love, FunctionIDPB.CoaxSleep);
               FlowText.ShowMessage(desc);
               return;
           }



            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_COAXSLEEP, false, true);
        });
    }

    public void UpdateView()
    {     
        _Appointment.transform.Find("RedPoint").gameObject.SetActive(GlobalData.CardModel.ShowLoveRedPoint);
    }
}
