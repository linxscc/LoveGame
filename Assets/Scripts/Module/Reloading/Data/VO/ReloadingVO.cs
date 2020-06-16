
using System;
using System.Collections.Generic;
using System.IO;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;


public class ReloadingVO:IComparable<ReloadingVO>
 {

   public int ItemId;
   public DressUpTypePB ItemType;             //道具类型(0服装1背景)
   public PlayerPB Player;
   public MapField<int, int> MoneyClaim;    //货币要求
   public ClothesGoalPB ClothesGoal;          //解锁要求
   public string UnlockDesc;                  //解锁描述
   

   public string Name;
   public List<string> BgImages; //大背景
   public bool IsGet;        //是否获得
   public bool IsPitchOn=false;    //是否选中
   public string IconPath;   //Icon的路径
   public int Weight;       //权重

   public bool IsShowRedDot = false; //红点
   
   
   
 public ReloadingVO(DressUpUnlockRulePB pB)
 {
     ItemId = pB.ItemId;
     ItemType = pB.ItemType;
     Player = pB.Player;
     MoneyClaim = pB.MoneyClaim;
     ClothesGoal = pB.ClothesGoal;
     UnlockDesc = pB.UnlockDesc;
       
     BgImages = new List<string> {pB.MornImage, pB.NoonImage, pB.AfternoonImage};

     InitIsGet();
     InitIconPath();
     InitName();
     InitWeight();
     SetRedDot();
    
    
 }


 private void SetRedDot()
 {
     var playerId = GlobalData.PlayerModel.PlayerVo.UserId;
     var key =playerId.ToString()+ItemId;
     var isHaveKey = PlayerPrefs.HasKey(key);

     if (isHaveKey==false)
     {       
         PlayerPrefs.SetInt(key,0);
         if (IsGet)
         {
             IsShowRedDot = true;
         }
     }
     else
     {
       var value =  PlayerPrefs.GetInt(key);
       if (value==0)
       {
           IsShowRedDot = true;
       }
       else if(value==1)
       {
           IsShowRedDot = false;  
       }
     }

     if (IsGet==false)
     {
         IsShowRedDot = false;  
     }
    
 }
 
 private void InitWeight()
 {
     Weight = IsGet ? 0 : 1;
 }


 private void InitIconPath()
 {
     switch (ItemType)
     {
         case DressUpTypePB.TypeClothes:
             IconPath = "Prop/" + ItemId;
             break;
         case DressUpTypePB.TypeBackground:
             IconPath = "Prop/"+ItemId;
             break;       
     }
 }

 private void InitName()
 {
     Name = GlobalData.PropModel.GetPropBase(ItemId).Name;// IsGet ? GlobalData.PropModel.GetPropBase(ItemId).Name : GlobalData.PropModel.GetPropBase(ItemId).Name;
 }

 private void InitIsGet()
 {

      var isHave = GlobalData.PropModel.IsGetUserProp(ItemId);

     
      if (isHave)
      {          
          IsGet = true;        
      }
      else
      {
          switch (ItemType)
          {
              case DressUpTypePB.TypeClothes:
                  IsGetClothes();
                  break;
              case DressUpTypePB.TypeBackground:
                  IsGetBackground();
                  break;            
          } 
      }
      
 }


 private void IsGetClothes()
 {
     var changeNum = ClothesGoal.ChangeNum;
     var extraNum = ClothesGoal.ExtraNum;
     var clothesGoalType = ClothesGoal.ClothesGoalType;

    
     
     switch (clothesGoalType)
     {
         case ClothesGoalTypePB.ClearanceNotGoal:
             IsGet = true;
             break;        
         case ClothesGoalTypePB.ClearanceNotGet:
             break;
         case ClothesGoalTypePB.ClothesCard:
             bool isGetCard = GlobalData.CardModel.GetUserCardById(changeNum) != null;    
             if (isGetCard)
             {
                 IsGet = extraNum == (int) GlobalData.CardModel.GetUserCardById(changeNum).Evolution;         
             }
             else
             {
                 IsGet = false;
             }
             break;
         case ClothesGoalTypePB.ClearanceGame:
             break;
         case ClothesGoalTypePB.ClearanceVisiting:
             break;     
     }
 }

 private void IsGetBackground()
 {           
        var clothesGoalType = ClothesGoal.ClothesGoalType; 
         switch (clothesGoalType)
         {
             case ClothesGoalTypePB.ClearanceNotGoal:
                 Debug.LogError("ClearanceNotGoal===>"+ItemId);
                 IsGet = true;
                 break;             
             case ClothesGoalTypePB.ClearanceNotGet:
             
                 IsGet = false;
                 break;
             case ClothesGoalTypePB.ClothesCard:
                 break;
             case ClothesGoalTypePB.ClearanceGame:
               
                 var mainLineLevelId = ClothesGoal.ChangeNum;

                 if (GlobalData.LevelModel.GetLevelInfo(mainLineLevelId)!=null)
                 {
                     IsGet =GlobalData.LevelModel.GetLevelInfo(mainLineLevelId).IsPass;
                 }
                 else
                 {
                     IsGet = false;
                 }
                 
                 break;
             case ClothesGoalTypePB.ClearanceVisiting:
               
              
                bool isReachVisitingLevel =GlobalData.PlayerModel.PlayerVo.Level >=GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);
               if (isReachVisitingLevel)
               {
                   var visitLevelId = ClothesGoal.ChangeNum;
                   IsGet = GlobalData.FavorabilityMainModel.IsPassVisit(visitLevelId);  
               }
               else
               {
                   IsGet = false;
               }            
               break;              
     }
     
     
 }
      
 public int CompareTo(ReloadingVO other)
 {
     int result = 0;
     if (other.Weight.CompareTo(Weight)!=0)
     {
         result = other.Weight.CompareTo(Weight);
     }
     return  -result;
 }

}





