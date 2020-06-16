

using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

public class CapsuleLevelModel : Model
{
  private Dictionary<int, CapsuleLevelVo> _capsuleBattleLevelDict;
  private List<MyCapsuleLevelVo> _myCapsuleLevelInfoList;

 // private List<UserActivityLevelInfoPB> UserActivityLevelInfos => GlobalData.ActivityModel.ActivityListRes.UserActivityLevelInfos.ToList();
  private List<CapsuleChapterVo> _chapterList;
  
  private CapsuleLevelVo _firstNormalLevel;
  private CapsuleLevelVo _newNormalLevel;

  /// <summary>
  /// 当前活跃的关卡
  /// </summary>
  private CapsuleLevelVo ActiveLevel;
  
  
  /// <summary>
  /// 关卡跳转
  /// </summary>
  public JumpData JumpData;
  
  public bool DoJump = false;

  private ActivityVo _curActivity;


  private List<UserActivityLevelInfoPB> UserActivityLevelInfos()
  {
      return GlobalData.ActivityModel.GetActivityTemplateListRes(_curActivity.ActivityType).UserActivityLevelInfos
          .ToList();
  }
  
  public List<ActivityLevelRulePB> GetLevelRule(int activityId)
  {
      List<ActivityLevelRulePB> rule = new List<ActivityLevelRulePB>();
      var pbs = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityLevelRules;
      foreach (var t in pbs)
      {        
          if (t.ActivityId==activityId)
          {
               rule.Add(t);
          }
      }
      return rule;
  }
  
  
  /// <summary>
  /// 设置扭蛋战斗关卡Data
  /// </summary>
  /// <param name="pbs"></param>
  /// <param name="plotRule"></param>
  /// <param name="infoRule"></param>
  public void SetCapsuleBattleData(ActivityVo curActivity)
  {

      _curActivity = curActivity;
      var pbs = GetLevelRule(curActivity.ActivityId);//GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityLevelRules;
         
      _capsuleBattleLevelDict = new Dictionary<int, CapsuleLevelVo>();
      _chapterList = new List<CapsuleChapterVo>();
      
      CapsuleChapterVo chapter = null;
      
      Dictionary<int, CapsuleChapterVo> chapterDict = new Dictionary<int, CapsuleChapterVo>();
      
      var plotRule = GlobalData.LevelModel.PlotRule;
      var infoRule = GlobalData.LevelModel.InfoRule;
            
      foreach (var t in pbs)
      {                   
          var level = new CapsuleLevelVo();        
          level.SetData(t,plotRule,infoRule);        
          _capsuleBattleLevelDict.Add(level.LevelId,level);

          if (chapterDict.ContainsKey(level.ChapterGroup) == false)
          {
              chapter = new CapsuleChapterVo(); 
              chapterDict[level.ChapterGroup] = chapter;
              chapter.LevelList = new List<CapsuleLevelVo>();
              chapter.HardLevelList = new List<CapsuleLevelVo>();
              chapter.ChapterId = level.ChapterGroup;

              for (int j = 0; j < infoRule.Count; j++)
              {
                  var info = infoRule[j];
                  if (info.InfoType == 1 && info.InfoId == level.ChapterGroup)
                  {
                     chapter.ChapterName = info.LevelName;
                     chapter.ChapterDesc = info.LevelDesc;
                     break;
                  }

              }
          }

          if (level.Hardness == GameTypePB.Difficult)
          {
              chapterDict[level.ChapterGroup].HardLevelList.Add(level);   
          }
          else
          {
              chapterDict[level.ChapterGroup].LevelList.Add(level);
          }
      }

      foreach (var chapterVo in chapterDict)
      {
          _chapterList.Add(chapterVo.Value);
          if (chapterDict.ContainsKey(chapterVo.Value.ChapterId + 1))
          {
              chapterVo.Value.NextChapterVo = chapterDict[chapterVo.Value.ChapterId + 1];
          }

          if (chapterDict.ContainsKey(chapterVo.Value.ChapterId - 1))
          {
              chapterVo.Value.PrevChapterVo = chapterDict[chapterVo.Value.ChapterId - 1];
          }
      }
      
      Debug.LogError("ChapterList.Count===>"+_chapterList.Count);
      
  }

  /// <summary>
  /// 设置扭蛋战斗我的关卡Data
  /// </summary>
  /// <param name="pbs"></param>
  public void SetMyCapsuleBattleLevelData()
  {             
      _myCapsuleLevelInfoList = new List<MyCapsuleLevelVo>();
      MyCapsuleLevelVo lastLevel = null;
      int myLevel = GlobalData.PlayerModel.PlayerVo.Level;   
      foreach (var t in UserActivityLevelInfos())
      {        
          var level = new MyCapsuleLevelVo();
          level.SetData(t);
          _myCapsuleLevelInfoList.Add(level);        
          CapsuleLevelVo item = GetLevelInfo(level.LevelId);
          item.ChallangeTimes = level.Count;
          item.CapsuleBattleBuyCount = level.BuyCount;
          item.CurPlayNum = item.Count- level.Count;
          if (item.CurPlayNum<0)
          {
              item.CurPlayNum = 0;
          }

          item.IsFree = level.BuyCount==0;
          
          if (level.Star>0)
          {
              item.IsPass = true;
              item.CurrentStar = level.Star;
              item.Score = level.Score;
          }

           if (item.Hardness == GameTypePB.Ordinary)
          {
              if(myLevel >= item.DepartmentLevel)
                  lastLevel = level;  
          }
        
      }

      JudgeChaptersOpen();

      foreach (var levelVo in _capsuleBattleLevelDict)
      {
          if (levelVo.Value.BeforeLevelId == 0)
          {
              if (levelVo.Value.Hardness == GameTypePB.Difficult)
              {
                  List<CapsuleLevelVo> list = _chapterList[levelVo.Value.ChapterGroup - 1].LevelList;
                  levelVo.Value.IsOpen = list[list.Count - 1].IsPass;
              }
              else
              {
                  levelVo.Value.IsOpen = true; 
              }
          }
          else
          {
              var beforeLevel = GetLevelInfo(levelVo.Value.BeforeLevelId);
              if (levelVo.Value.Hardness == GameTypePB.Ordinary)
              {
                  levelVo.Value.IsOpen = beforeLevel.IsPass;
              }
              else if(levelVo.Value.Hardness == GameTypePB.Difficult && beforeLevel.IsPass)
              {
                  levelVo.Value.IsOpen = _chapterList[levelVo.Value.ChapterGroup - 1].IsHardOpen;
              }
          }
      }

      if (JumpData != null)
      {
          ActiveLevel =FindLevel(JumpData);
          if (ActiveLevel!=null)
          {
             DoJump = true;
             ClientData.CustomerSelectedCapsuleLevel = ActiveLevel;
             return; 
          }
      }
      DoJump = false;

      if (ClientData.CustomerSelectedCapsuleLevel ==null)
      {
          if (lastLevel != null)
          {
             CapsuleLevelVo levelVo = GetLevelInfo(lastLevel.LevelId);
             if (levelVo.IsPass && levelVo.AfterLevelId != 0)
             {
                 CapsuleLevelVo  tempLevel = GetLevelInfo(levelVo.AfterLevelId);
                 if (myLevel>=tempLevel.DepartmentLevel)
                 {
                       levelVo = tempLevel;
                 }
             }
             ActiveLevel = levelVo;
             ClientData.CustomerSelectedCapsuleLevel = ActiveLevel;
          }
          else
          {
              if (_myCapsuleLevelInfoList.Count==0)
              {
                  ActiveLevel = _chapterList[0].LevelList[0];
              }
          }
      }

      if (ClientData.CustomerSelectedCapsuleLevel != null)
      {
          ActiveLevel = ClientData.CustomerSelectedCapsuleLevel;
      }
      
      foreach (var t in _capsuleBattleLevelDict)
      {
          Debug.LogError(t.Key + ";IsOpen===>" + t.Value.IsOpen + ";IsPass===>" + t.Value.IsPass+";Id===>"+t.Value.ActivityId);
      }
    
  }

  private CapsuleLevelVo FindLevel(JumpData jumpData)
  {
      foreach (var vo in _capsuleBattleLevelDict)
      {
          if (jumpData.Type == "H")
          {
              if (vo.Value.Hardness == GameTypePB.Difficult && vo.Value.LevelMark == jumpData.Data)
              {
                  return vo.Value;
              }
          }
          else
          {
              if (vo.Value.Hardness == GameTypePB.Ordinary && vo.Value.LevelMark == jumpData.Data)
              {
                  return vo.Value;
              }
          }
          
      }
      return null;
  }
  

  private void JudgeChaptersOpen()
  {
      for (int i = 0; i < _chapterList.Count; i++)
      {
          CapsuleChapterVo chapter = _chapterList[i];
          var firstLevel = chapter.LevelList[0];
          int beforeLevelId = firstLevel.BeforeLevelId;

          if (beforeLevelId == 0)
          {
              _firstNormalLevel = firstLevel;
              chapter.IsNormalOpen = true;
          }
          else
          {
              var level = GetLevelInfo(beforeLevelId); 
              chapter.IsNormalOpen = level.IsPass;
          }
      }

      _newNormalLevel = GetNewLevel(_firstNormalLevel);
  }
  
  public void UpdateUserActivityLevelInfo(UserActivityLevelInfoPB pb)
  {
    
      GlobalData.ActivityModel.UpdateActivityLevel(_curActivity.ActivityType,pb);
      SetMyCapsuleBattleLevelData();         
  }
  

  public CapsuleLevelVo GetLevelInfo(int levelId)
  {
      if (!_capsuleBattleLevelDict.ContainsKey(levelId))
      {
          return null;
      }

      foreach (var t in _capsuleBattleLevelDict)
      {
          if (t.Key==levelId && t.Value.ActivityId==_curActivity.ActivityId)
          {
              return _capsuleBattleLevelDict[levelId]; 
          }
      }

      return null;

  }


  private CapsuleLevelVo GetNewLevel(CapsuleLevelVo levelVo)
  {
      if (levelVo.BeforeLevelId == 0 && levelVo.IsPass == false)
          return levelVo;
      CapsuleLevelVo level = GetNextLevelInfo(levelVo);

      if (level==null)
      {
          return null;  
      }

       if (level.IsPass)
       {
           return GetNewLevel(level);
       }
     
        return level;
  }
  
  private CapsuleLevelVo GetNextLevelInfo(CapsuleLevelVo vo)
  {
      if (_capsuleBattleLevelDict.ContainsKey(vo.AfterLevelId))
      {
          return _capsuleBattleLevelDict[vo.AfterLevelId];
      }

      return null;
  }
  
}
