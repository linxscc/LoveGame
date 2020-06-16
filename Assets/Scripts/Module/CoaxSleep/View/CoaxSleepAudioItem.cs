using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using DataModel;
using GalaAccount.Scripts.Framework.Utils;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class CoaxSleepAudioItem : MonoBehaviour
{
   private Text _titleTxt;
   private Text _timeTxt;
   private Text _descTxt;
   private Text _unlockTxt;
   
   private Button _playBtn;
   private Button _atOnceUnlockBtn;
   private Button _unlockBtn;

   private MyCoaxSleepAudioData _data;

   private Image _bg;
   
   private void Awake()
   {
      _bg = transform.GetImage();
      
      _titleTxt = transform.GetText("Title");
      _timeTxt = transform.GetText("Time/Text");
      _descTxt = transform.GetText("Desc");
      _unlockTxt = transform.GetText("UnlockDesc");
      
      _playBtn =transform.GetButton("PlayBtn");
      _atOnceUnlockBtn =transform.GetButton("AtOnceUnlockBtn");
      _unlockBtn =transform.GetButton("UnlockBtn");
      
      _playBtn.onClick.AddListener(OnPlayBtn);
      _atOnceUnlockBtn.onClick.AddListener(OnAtOnceUnlockBtn);
      _unlockBtn.onClick.AddListener(OnUnlockBtn);
   }

   private void OnUnlockBtn()
   {
      TriggerGemUnlockEvent();
   }

   private void OnAtOnceUnlockBtn()
   {
      TriggerGemUnlockEvent();
   }

   private void TriggerGemUnlockEvent()
   {
      EventDispatcher.TriggerEvent(EventConst.OnClickUnlockToGem,_data);
   }
   
   
   private void OnPlayBtn()
   {
      PlayerPrefs.SetString("CoaxSleepId"+_data.AudioId,_data.AudioId.ToString());
      EventDispatcher.TriggerEvent(EventConst.OnClickCoaxSleepPlay,_data);  
      _playBtn.transform.Find("Red").gameObject.SetActive(false);
   }

   
   
   public void SetData(MyCoaxSleepAudioData data)
   {
      _data = data;
      _bg.sprite = ResourceManager.Load<Sprite>(data.ItemBgPath);
      _titleTxt.text = data.AudioName;
      _descTxt.text = data.AudioDesc;
      _timeTxt.text = data.PlayTimeStr;

      _playBtn.gameObject.Hide();
      _unlockBtn.gameObject.Hide();
      _atOnceUnlockBtn.gameObject.Hide();
      
      if (data.IsUnlock)
      {
         _playBtn.gameObject.Show();
         _playBtn.transform.Find("Red").gameObject.SetActive(!PlayerPrefs.HasKey("CoaxSleepId"+data.AudioId));
         if (data.IsOnlyGemUnlock)
         {
            _unlockTxt.text = I18NManager.Get("CoaxSleep_Unlock1", data.Gem)+data.UnlockTypeDesc;
         }
         else
         {
            _unlockTxt.text = I18NManager.Get("CoaxSleep_Unlock2",data.UnlockLoveNum,data.NpcName,data.Gem)+data.UnlockTypeDesc;       
         }
      }
      else
      {
         if (data.IsOnlyGemUnlock)
         {
            _unlockTxt.text = I18NManager.Get("CoaxSleep_Unlock1", data.Gem);
            _unlockBtn.gameObject.Show();
         }
         else
         {
            _unlockTxt.text = I18NManager.Get("CoaxSleep_Unlock2", data.UnlockLoveNum, data.NpcName,data.Gem);
            _atOnceUnlockBtn.gameObject.Show();
         }
         
      }
      
      
   }
}
