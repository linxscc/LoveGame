using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using BehaviorDesigner.Runtime.Tasks.Basic.Math;
using Common;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CoaxSleepOnPlayAudioView : View
{
   private Text _audioName;
   private Button _retrospectBtn; //回顾Btn
   private string _desc;//回顾文案描述
   
   private AudioClip _audioClip;

   private SliderUI _slider;
   private Text _leftTimeTxt;
   private Text _rightTimeTxt;

   private Button _playOrStopBtn;
   private Button _loopBtn;

   private bool _isStart = false;
   private bool _isLoop = false;

   private MyCoaxSleepAudioData _data;
   private string _localPath;


   private RawImage _bg;


   private GameObject _onPlayObj;
   private GameObject _onPauseObj;

 
   private void Awake()
   {
      _bg = transform.GetRawImage("BG");
      _audioName = transform.GetText("AudioName/Text");
      _retrospectBtn = transform.GetButton("RetrospectBtn");
      _retrospectBtn.onClick.AddListener(OnRetrospectBtn);
      _slider = transform.Find("Slider").GetComponent<SliderUI>();
      _leftTimeTxt =transform.GetText("LeftTimeTxt");
      _rightTimeTxt = transform.GetText("RightTimeTxt");

      _playOrStopBtn = transform.GetButton("PlayOrStopBtn");

      _onPlayObj = transform.Find("PlayOrStopBtn/OnPlay").gameObject;
      _onPauseObj = transform.Find("PlayOrStopBtn/OnPause").gameObject;
      
      _loopBtn = transform.GetButton("LoopBtn");
      
      _playOrStopBtn.onClick.AddListener(OnPlayOrStopBtn);
      _loopBtn.onClick.AddListener(LoopBtn);

      _slider.onValueChanged.AddListener(delegate { SetAudioTimeValueChange(); });
      
   }

   private void SetAudioTimeValueChange() {
  
     AudioManager.Instance.BackgroundAudioSource.time = _slider.value * _audioClip.length;
     SetLeftTime(_slider.value * _audioClip.length); 
   }
   private void Update()
   {
       
      
      if (_isStart)
      {
         var time = AudioManager.Instance.BackgroundAudioSource.time;        
         _slider.value =   time / _audioClip.length; 
      
         
        // SetLeftTime(time);       
         if (time -_audioClip.length==0)
         {
            OnPlayOver();
         }
      }

      
          
   }

   private void LoopBtn()
   {
      _isLoop = !_isLoop;      
      SetIsLoop();
      SetLoopState();
   }

   private void OnPlayOrStopBtn()
   {
      _isStart = !_isStart;     
      SetIsOnPlay();
      SetOnPlayAndOnPauseState();   
   }

  
   private void OnRetrospectBtn()
   {
      var window = PopupManager.ShowWindow<StoryRecordWindow>("Story/Prefabs/StoryRecordWindow");
      window.SetData(_desc);
   }


   public void SetData(MyCoaxSleepAudioData data,string localPath)
   {
      _data = data;

      _bg.texture = ResourceManager.Load<Texture>(data.OnPlayBgPath);
      _localPath = localPath;
      
      _desc = data.RetrospectDesc;
      _audioName.text = data.AudioName;
     
      
      SetAudio(data.AudioId); 
      
      SetRightTime(_audioClip.length);           
   }


   private void SetAudio(int id)
   {
      var path = AssetLoader.GetCoaxSleepMusicById(id.ToString());

      new AssetLoader().LoadAudio(AssetLoader.GetCoaxSleepMusicById(id.ToString()),
         (clip, loader) =>
         {
            _audioClip = clip;

            LoadAudioComplete();
            AssetManager.Instance.MarkSingleFileBundle(path, ModuleName);
         });
   }

   
   private void LoadAudioComplete()
   {
      AudioManager.Instance.BackgroundAudioSource.clip = _audioClip;
       
      InitProgress();
      InitSwitch();
      SetIsOnPlay();
      SetIsLoop();
      SetOnPlayAndOnPauseState();
      SetLoopState();

   }
   
   /// <summary>
   /// 初始化进度
   /// </summary>
   private void InitProgress()
   {     
      AudioManager.Instance.BackgroundAudioSource.time = 0;
      _slider.value = 0;   
   }
   
   private void InitSwitch()
   {
      _isStart = true;    
      _isLoop = false;
   }
   
  


   /// <summary>
   /// Set暂停和播放按钮
   /// </summary>
   private void SetOnPlayAndOnPauseState()
   {
      if (_isStart)
      {
         _onPlayObj.Show();
         _onPauseObj.Hide();      
      }
      else
      {
         _onPlayObj.Hide();
         _onPauseObj.Show();         
      }
   }

   
    
   
   /// <summary>
   /// 是否播放
   /// </summary>
   private void SetIsOnPlay()
   {
      if(_isStart)
         AudioManager.Instance.BackgroundAudioSource.Play();
      else
         AudioManager.Instance.BackgroundAudioSource.Stop();  
   }
   
   
   
   /// <summary>
   /// Set循环状态
   /// </summary>
   private void SetLoopState()
   {
     
     var str = _isLoop ? I18NManager.Get("CoaxSleep_CloseLoop") : I18NManager.Get("CoaxSleep_OpenLoop");

     var open = _loopBtn.transform.Find("Open").gameObject;
     var close = _loopBtn.transform.Find("Close").gameObject;

     if (_isLoop)
     {
        open.Show();
        close.Hide();
     }
     else
     {
        open.Hide();
        close.Show();
     }
     
      _loopBtn.transform.GetText("Text").text = str;
   }

   /// <summary>
   /// 是否循环
   /// </summary>
   private void SetIsLoop()
   {
      AudioManager.Instance.BackgroundAudioSource.loop = _isLoop; 
   }
   
   
   private void SetLeftTime(float time)
   {              
      _leftTimeTxt.text = GetTime((long) (time*1000));      
   }

   private void SetRightTime(float time)
   {          
      _rightTimeTxt.text = GetTime((long)(time*1000));            
   }
  
   private void OnPlayOver()
   {
      if (!_isLoop)
      {
         _isStart = false;
          SetIsOnPlay();
          SetOnPlayAndOnPauseState();
      }
   }
   
   
   
  
   private  string GetTime(long lefTime)
   {
      long s = (lefTime / 1000) % 60;
      long m = (lefTime / (60 * 1000)) % 60;     
      return string.Format("{0:D2}:{1:D2}",  m, s);
   }
   
   



   #region 哄睡SDK
  
   /// <summary>
   /// 设置歌单信息
   /// musicPlayerTitle--->音乐播放器标题 用于展示在通知栏中
   /// musicPlayerContent--->音乐播放器内容或者说是副标题 用于展示在通知栏中
   /// musicPlayerCoverPic--->音乐播放器封面图片 用于展示在通知栏中
   /// musicFilePath--->用于播放的音乐文件路径
   /// </summary>
   /// <returns></returns>
   private JSONObject SetPlaylistInfo()
   {
      JSONObject json =new JSONObject();
      json.AddField("musicPlayerTitle",_data.AudioName);
      json.AddField("musicPlayerContent",_data.AudioDesc);
      json.AddField("musicPlayerCoverPic",""); 
      json.AddField("musicFilePath",_localPath);

      JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
      arr.Add(json);
      
      return arr;
   }
   
   public void SetMusicSdk(MusicPlayerState state)
   {
      switch (state)
      {
         case MusicPlayerState.Init:
            SetInit();
            break;
         case MusicPlayerState.Play:
            SetPlay();
            break;
         case MusicPlayerState.Pause:
            SetPause();
            break;
         case MusicPlayerState.ChangeProgress:
            SetChangeProgress();
            break;
         case MusicPlayerState.Stop:
            SetStop();
            break; 
      }  
   }



   private void SetInit()
   {
      JSONObject jsonObject = new JSONObject();
      jsonObject.AddField("musicPlayerState","init");     
      jsonObject.AddField("musicPlayerDatas",SetPlaylistInfo());     
      Debug.LogError(jsonObject);
   }

   private void SetPlay()
   {
      JSONObject jsonObject = new JSONObject();
      jsonObject.AddField("musicPlayerState","play");
      jsonObject.AddField("musicPlayMode","0");
      
   }

   private void SetPause()
   {
      JSONObject jsonObject = new JSONObject();
      jsonObject.AddField("musicPlayerState","pause"); 
   }

   private void SetChangeProgress()
   {
      JSONObject jsonObject = new JSONObject();
      jsonObject.AddField("musicPlayerState","changeProgress");
      jsonObject.AddField("musicNewTime","0");
   }

   private void SetStop()
   {
       JSONObject jsonObject = new JSONObject();
       jsonObject.AddField("musicPlayerState","stop");
   }
   #endregion   
      
}

public enum MusicPlayerState
{
   Init,
   Play,
   Pause,
   ChangeProgress,
   Stop,  
}




