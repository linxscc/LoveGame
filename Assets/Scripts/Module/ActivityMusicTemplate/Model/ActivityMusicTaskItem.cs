using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using DataModel;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicTaskItem : MonoBehaviour
{
   private Text _desc;   
   private Button _getBtn;
   private Button _gotoBtn;
   private GameObject _finish;
   private Button _itemBtn;
   private Text _num;
   private RawImage _icon;
   private ActivityMissionVo _data;
   
   private void Awake()
   {

      _itemBtn = transform.GetButton("Reward/Item");
      _num = transform.GetText("Reward/Item/Num");
      _icon = transform.GetRawImage("Reward/Item");
      
      _desc = transform.GetText("Desc");     
      _getBtn = transform.GetButton("GetBtn");
      _gotoBtn = transform.GetButton("GotoBtn");
      _finish = transform.Find("AlreadyGet").gameObject;
      
      
      _getBtn.onClick.AddListener(OnClickGetBtn);
      _gotoBtn.onClick.AddListener(OnClickGoToBtn);
      _itemBtn.onClick.AddListener(OnClickItemBtn);
   }

   private void OnClickItemBtn()
   {
      var desc = ClientData.GetItemDescById(_data.Rewards[0].Id,_data.Rewards[0].Resource);
      if (desc!=null)
      {
         FlowText.ShowMessage(desc.ItemDesc); 			
      }     
   }

   private void OnClickGetBtn()
   {
      //领取奖励
      EventDispatcher.TriggerEvent(EventConst.GetActivityMusicTaskAward,_data);
   }
   
   private void OnClickGoToBtn()
   {
      if (!_data.JumpTo.Contains("music"))
      {
         PopupManager.CloseAllWindow();
      }    
      EventDispatcher.TriggerEvent(EventConst.ActivityMusicTaskGoto,_data.JumpTo);       
   }

  


   public void SetData(ActivityMissionVo vo)
   {
      _data = vo;     
      _desc.text = vo.ActivityMissionDesc;         
      SetState(vo.Status);
      SetAwardData();
   }
   
   /// <summary>
   /// 设置领取状态
   /// </summary>
   /// <param name="status"></param>
   private  void SetState(MissionStatusPB status)
   {
      switch (status)
      {
         case MissionStatusPB.StatusUnfinished:
             _getBtn.gameObject.Hide();
             _gotoBtn.gameObject.Show();
             _finish.Hide();
            break;
         case MissionStatusPB.StatusUnclaimed:
              _getBtn.gameObject.Show();
              _gotoBtn.gameObject.Hide();
              _finish.Hide();
            break;
         case MissionStatusPB.StatusBeRewardedWith:
              _getBtn.gameObject.Hide();
              _gotoBtn.gameObject.Hide();
              _finish.Show();
            break;        
      }
   }
   
   
   
   /// <summary>
   /// 设置奖励信息(数量，图片，点击描述)
   /// </summary>
   private void SetAwardData()
   {     
      _icon.texture =ResourceManager.Load<Texture>(_data.Rewards[0].IconPath);
      _num.text = _data.Rewards[0].Num.ToString();      
   }
}
