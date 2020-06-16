using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Proto;
using DataModel;
using UnityEngine;

public class MyCoaxSleepAudioData
{

    public int AudioId;        //音频Id
    public PlayerPB PlayerPb;  //角色id
    public string AudioName;   //音频名称
    public string AudioDesc;   //音频描述
    public List<CoaxUnlockRulePB> CoaxUnlockGoals; //解锁要求 所有数组里面条件都是或的关系 index =0,星钻解锁要求；index=1，恋爱剧情解锁要求
    public string UnlockDesc;  //解锁描述
    public bool IsUnlock=false;      //是否解锁
    public string PlayTimeStr;      //播放时长文本  
    public float PlayTimes;
    public string RetrospectDesc; // 回顾描述
    public bool IsOnlyGemUnlock=false;//是否仅仅是消耗钻石解锁
    public string NpcName;
    public string UnlockTypeDesc;
    public int Gem=0;
    public int UnlockLoveNum = 0;
    public string ItemBgPath;    //Item背景图
    public string OnPlayBgPath;  //播放界面背景
    
    public MyCoaxSleepAudioData(CoaxSleepAudioRulePB rule,UserCoaxSleepInfoPB userInfos,CoaxSleepAudioDescJsonData descJsonData)
    {
        AudioId = rule.AudioId;
        PlayerPb = rule.Player;
        AudioName = rule.AudioName;
        AudioDesc = rule.AudioDesc;
        CoaxUnlockGoals = rule.CoaxUnlockGoals.ToList();
        UnlockDesc = rule.UnlockDesc;
       
        PlayTimes = descJsonData.Time;       
        RetrospectDesc = descJsonData.Desc;
        SetGemAndUnlockLoveNum();
        SetIsUnlock(userInfos);      
        SetNpcName();
        IsOnlyGemUnlock = CoaxUnlockGoals.Count == 1 && CoaxUnlockGoals[0].Gem != 0;
        ItemBgPath = "CoaxSleep/PlayerIcon_"+(int)PlayerPb;
        OnPlayBgPath = "CoaxSleep/"+(int)PlayerPb;
        SetPlayTimeStr();
    }

    private void SetPlayTimeStr()
    {
      
        string str = PlayTimes.ToString();
        bool isExist = str.Contains(".");
        if (isExist)       
            str=  str.Replace(".", ":");
       
        PlayTimeStr = str;
    }
    
    
    /// <summary>
    /// 判断是否解锁
    /// </summary>
    /// <param name="userInfos"></param>
    private void SetIsUnlock(UserCoaxSleepInfoPB userInfos)
    {      
        var isHave = userInfos.Audios.ContainsKey(AudioId);      
        if (isHave)
        {
            IsUnlock = true;
            var type =(AudiosUnlockTypePB) userInfos.Audios[AudioId];
            switch (type)
            {
                case AudiosUnlockTypePB.UnlockGem:
                    UnlockTypeDesc = I18NManager.Get("CoaxSleep_Shop");
                    break;
                case AudiosUnlockTypePB.UnlockPlot:
                    UnlockTypeDesc =I18NManager.Get("CoaxSleep_Get");
                    break;             
            }
          
            return;
        }

        var unlockLoveNum = UnlockLoveNum;
        var curUnlockLoveNum = GlobalData.AppointmentData.GetAppointmentUnlockNum(PlayerPb);
      
        if (curUnlockLoveNum >= unlockLoveNum)
        {
            IsUnlock = true;
            UnlockTypeDesc = I18NManager.Get("CoaxSleep_Get");
        }
     
    }

    private void SetNpcName()
    {
        NpcName = I18NManager.Get("Common_Role" + (int) PlayerPb);
    }

    private void SetGemAndUnlockLoveNum()
    {
        foreach (var t in CoaxUnlockGoals)
        {
            Gem += t.Gem;
            UnlockLoveNum += t.UnlockLoveNum;
        }
    }
}
