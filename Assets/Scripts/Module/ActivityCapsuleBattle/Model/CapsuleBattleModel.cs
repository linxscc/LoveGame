using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleBattleModel : Model
{
   public CapsuleLevelVo LevelVo;
   
   public List<BattleUserCardVo> UserCardList;
   
   public int SelectedCount = 0;
   
   public bool IsShowingAnimation = false;   
   public bool IsGetBattleResult = false;
   
   public RepeatedField<ChallengeCardNumRulePB> CardNumRules { get; set; }
   
   public Dictionary<int, int> ItemsDict;
   public Dictionary<int, int> FansDict;
   
   public int Power;
   public int Transmission;
   public int Resource;
   public int Financial;
   public int Active;
   public int Support;
   private int _addtional;


   public void InitSupporterValue()
   {
      foreach (var vo in GlobalData.DepartmentData.MyDepartments)
      {
         if (vo.UserDepartmentPb.DepartmentType == DepartmentTypePB.Support)
         {
            Support = vo.RulePb.Power;
            _addtional = Support / 4;
            break;
         }
      }
      
      foreach (var vo in GlobalData.DepartmentData.MyDepartments)
      {
         switch (vo.UserDepartmentPb.DepartmentType)
         {
            case DepartmentTypePB.Active:
               Active = vo.RulePb.Power + _addtional;
               Power += Active;
               break;
            case DepartmentTypePB.Financial:
               Financial = vo.RulePb.Power + _addtional;
               Power += Financial;
               break;
            case DepartmentTypePB.Resource:
               Resource = vo.RulePb.Power + _addtional;
               Power += Resource;
               break;
            case DepartmentTypePB.Transmission:
               Transmission = vo.RulePb.Power + _addtional;
               Power += Transmission;
               break;
         }
      }
   }
   
   
   public override void OnMessage(Message message)
   {
      base.OnMessage(message);
      string name = message.Name;
      object[] body = message.Params;
      switch (name)
      {
         case MessageConst.CMD_CAPSULEBATTLE_ITEM_DATA:
            ItemsDict = (Dictionary<int, int>) body[0];
            FansDict = (Dictionary<int, int>) body[1];
            break;
      }
   }
   
   public List<BattleUserCardVo> FilterCard(int AbilityType = -1)
   {
      AbilityPB pb = (AbilityPB) AbilityType;

      UserCardList.Sort((a, b) =>
      {
         int x = 0;
         int y = 0;
         switch (pb)
         {
            case AbilityPB.Singing:
               x = a.UserCardVo.Singing;
               y = b.UserCardVo.Singing;
               break;
            case AbilityPB.Dancing:
               x = a.UserCardVo.Dancing;
               y = b.UserCardVo.Dancing;
               break;
            case AbilityPB.Composing:
               x = a.UserCardVo.Original;
               y = b.UserCardVo.Original;
               break;
            case AbilityPB.Popularity:
               x = a.UserCardVo.Popularity;
               y = b.UserCardVo.Popularity;
               break;
            case AbilityPB.Charm:
               x = a.UserCardVo.Glamour;
               y = b.UserCardVo.Glamour;
               break;
            case AbilityPB.Perseverance:
               x = a.UserCardVo.Willpower;
               y = b.UserCardVo.Willpower;
               break;
            default:
               x = a.UserCardVo.TotalAblity();
               y = b.UserCardVo.TotalAblity();
               break;
         }

         if (y.CompareTo(x) != 0)
            return y.CompareTo(x);

         return a.UserCardVo.CardId.CompareTo(b.UserCardVo.CardId);
      });

      return UserCardList;
   }
   
   public void InitCardList(List<UserCardVo> list)
   {
      SelectedCount = 0;

      UserCardList = new List<BattleUserCardVo>();
      for (int i = 0; i < list.Count; i++)
      {
         BattleUserCardVo vo = new BattleUserCardVo();
         vo.UserCardVo = list[i];
         UserCardList.Add(vo);
      }
   }
   
   public bool IsCardPositionOpen(int index)
   {
      return GlobalData.PlayerModel.PlayerVo.Level >= CardNumRules[index].LevelMin;
   }
   
   /// <summary>
   ///  获得当前等级对应的卡牌开放数量
   /// </summary>
   /// <returns></returns>
   public int CardOpenNum()
   {
      int level = GlobalData.PlayerModel.PlayerVo.Level;

      for (int i = CardNumRules.Count - 1; i >= 0; i--)
      {
         if (level >= CardNumRules[i].LevelMin)
         {
            return CardNumRules[i].OpenNum;
         }
      }

      return 1;
   }
   
   public void Reset()
   {
      Power = 0;

      IsGetBattleResult = false;
   }
}
