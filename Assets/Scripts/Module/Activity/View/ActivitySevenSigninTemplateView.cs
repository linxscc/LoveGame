using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ActivitySevenSigninTemplateView : View
{
   private RawImage _bg;
   private Transform _sevenDaysAward;
   private Text _timeTxt;
   private TimerHandler _countDown;
   private long _endTimeStamp;
   private void Awake()
   {
      _bg = transform.GetRawImage("Bg");
      _sevenDaysAward = transform.Find("Bg/SevenDaysAward");
      _timeTxt = transform.GetText("Bg/Hint1");
      SetBg();
   }


   private void SetBg()
   {
      _bg.texture = ResourceManager.Load<Texture>("Activity/SevenDayActivityBg1");
   }
        

   public void SetResidueDay(long endTimeStamp)
   {
      _endTimeStamp = endTimeStamp;
      var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
      var surplusDay = DateUtil.GetSurplusDay(curTimeStamp, endTimeStamp);
      if (surplusDay!=0)
      {
         _timeTxt.text = I18NManager.Get("Activity_SevenActivityResidueDays",surplusDay);	
      }
      else
      {
         _countDown = ClientTimer.Instance.AddCountDown("CountDown",Int64.MaxValue, 1f, CountDown, null);
         CountDown(0);
      }
   }
   
   private void CountDown(int obj)
   {
      string timeStr = "";
      var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
      long time = _endTimeStamp - curTimeStamp;
	
      if (time<1000)
      {
         DestroyCountDown();
         _timeTxt.text = "0:0:0";
         return;
      }
      else
      {
         long s = (time / 1000) % 60;
         long m = (time / (60 * 1000)) % 60;
         long h = time / (60 * 60 * 1000);
         timeStr = $"{h:D2}:{m:D2}:{s:D2}";
      }
      _timeTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2",timeStr);		
   }
   
   public void CreateSevenSigninData(List<SevenSigninTemplateAwardVo> list)
   {
     
      var item = GetPrefab("Activity/Prefabs/SevenSigninTemplateAwardItem");
      for (int i = 0; i < list.Count; i++)
      {
         var go = Instantiate(item, _sevenDaysAward.GetChild(i).transform, false);
         go.name = list[i].DayId.ToString();
         go.GetComponent<SevenSigninTemplateAwardItem>().SetData(list[i]);
      }
      
   }

   public void Refresh(int day)
   {
      if (day!=7)
      {
        for (int i = 0; i < _sevenDaysAward.childCount; i++)
        {
            var itemDay = int.Parse(_sevenDaysAward.GetChild(i).GetChild(0).gameObject.name);
            if (day==itemDay)
            {
              _sevenDaysAward.GetChild(i).GetChild(0).gameObject.transform.Find("GetBtn").gameObject.SetActive(false);
              _sevenDaysAward.GetChild(i).GetChild(0).gameObject.transform.Find("Mask").gameObject.SetActive(true);
            }              
         } 
      }
      else
      {
        var item = _sevenDaysAward.Find("7").GetChild(0).gameObject;
        item.transform.Find("Last/GetBtn").gameObject.SetActive(false);
        item.transform.Find("Last/Mask").gameObject.SetActive(true);
        item.transform.Find("Last/NameBg/Image/Text").GetText().text = I18NManager.Get("Activity_EverydayPowerAlreadyGet"); 
        item.transform.Find("Last/NameBg").gameObject.SetActive(true);
        item.transform.Find("Last/Icon/Day").gameObject.Show();
      }
      
   }



   public void DestroyCountDown()
   {
      if (_countDown!=null)
      {
         ClientTimer.Instance.RemoveCountDown(_countDown);	
      }
   }
   
   
}
