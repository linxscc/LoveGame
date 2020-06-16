using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Newtonsoft.Json;
using UnityEngine;


using Random = UnityEngine.Random;

public class ActivityPopupWindowModel : Model
{

    private  List<ActivityPopupWindowData> _randomBeforeDates;
    private List<ActivityPopupWindowData> _randomLateDates;
    
    private readonly string _key = GlobalData.PlayerModel.PlayerVo.UserId+"IsShow";

    private Dictionary<int, List<ActivityPopupWindowData>> _dictionary;
    private List<ActivityPopWindowVo> _popData;
    
    public ActivityPopupWindowModel ()
    {
      //  DeleteKey();
        InitActivityPopWindowData();
        InitializeDate();
    }

    private void InitActivityPopWindowData()
    {
        string fileName = "activitypopwindowdata";
        string text =new AssetLoader().LoadTextSync(AssetLoader.GetLocalConfiguratioData("ActivityPopWindowData",fileName));
        _popData =  JsonConvert.DeserializeObject<List<ActivityPopWindowVo>>(text);      
    }
    
    private bool IsHasKey()
    {
        return PlayerPrefs.HasKey(_key);
    }

    public void DeleteKey()
    {
        PlayerPrefs.DeleteKey(_key);  
    }

    public void SetKey()
    {
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var curTodayDateTime = DateUtil.GetDataTime(curTimeStamp) ;
        var curTodaySixPoint = new DateTime(curTodayDateTime.Year,curTodayDateTime.Month,curTodayDateTime.Day,6,0,0);
        var tomorrowResetTimeStamp =DateUtil.GetTimeStamp(curTodaySixPoint.AddDays(1)) ;
        string value = tomorrowResetTimeStamp.ToString();           
        PlayerPrefs.SetString(_key,value);                      
    }

    public bool IsShow()
    {
        if (IsHasKey())
        {          
            string value = PlayerPrefs.GetString(_key);
            long resetTimeStamp = long.Parse(value) ;         
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            if (curTimeStamp<resetTimeStamp)
            {
                return false; 
            }
            else
            {
                DeleteKey();
                return true;
            }
        }
        else
        {
            return true;
        }       
    }
    
    private void InitializeDate()
    {
        _randomBeforeDates =new List<ActivityPopupWindowData>();
        _randomLateDates =new  List<ActivityPopupWindowData>();       
        _dictionary = new Dictionary<int, List<ActivityPopupWindowData>>();        
        AddActivityModuleData();
        RandomMethod();
    }

    private void RandomMethod()
    {
        foreach (var t in _randomBeforeDates)
        {
            var group = t.Group;
            var isKey = _dictionary.ContainsKey(group);
            if (!isKey)
            {
                _dictionary[group] = new List<ActivityPopupWindowData> {t};
            }
            else
            {
                _dictionary[group].Add(t);
            }           
        }

        foreach (var t in _dictionary)
        {
            var list = t.Value;
            var isMore = list.Count > 1;
            if (isMore)
            {
               var index =Random.Range(0, list.Count);
               _randomLateDates.Add(list[index]);
            }
            else
            {
                _randomLateDates.Add(list[0]); 
            }
        }
        
        if (_randomLateDates.Count>0)
        {
            Sort(); 
        }

    }   
    private void Sort()
    {
        _randomLateDates.Sort((x,y)=>x.Sort.CompareTo(y.Sort));  
    }
    
    public List<ActivityPopupWindowData> GetDate()
    {           
        return _randomLateDates;
    }

    private void AddActivityModuleData()
    {
        var list = _popData;

        foreach (var t in list)
        {
            var data = new ActivityPopupWindowData(t);
          //  Debug.Log("活动弹窗名===>"+data.Name+";IsShow===>"+data.IsShow+";IsCanJumpTo===>"+data.IsCanJumpTo+";Group===>"+data.Group);            
            if (data.IsShow)
            {
               _randomBeforeDates.Add(data);
            }
        } 
    }
 
}
