using System;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;

public class HeadVo:IComparable<HeadVo>
{
    public int Id; //元素Id
    public ElementTypePB ElementType;
    public ElementModulePB ElementModule;
    public string Name;
    public int NeedUnlock;
    public UnlockRulePB UnlockClaim;
    public string Desc;
    public string Path;
    public PlayerPB PlayerPb;
    public bool IsUnlock = false;
        
    private UserCardVo _userCardVo;
    
    
    public HeadVo(ElementPB pb)
    {
        Id = pb.Id;
        ElementType = pb.ElementType;
        ElementModule = pb.ElementModule;
        Name = pb.Name;
        NeedUnlock = pb.NeedUnlock;
        UnlockClaim = pb.UnlockClaim;
        _userCardVo = GlobalData.CardModel.GetUserCardById(UnlockClaim.CardId);
        Desc = pb.Desc;
        SetPlayerTypeAndPath(pb.UnlockClaim.CardId, pb.Id);
        SetIsUnlock();
    }

    /// <summary>
    /// 设置角色类型和图片路径
    /// </summary>
    /// <param name="cardId">卡牌Id</param>
    /// <param name="id">元素Id</param>
    private void SetPlayerTypeAndPath(int cardId, int id)
    {
        var isCardIdZero = cardId == 0;
        if (isCardIdZero)
        {
            PlayerPb = PlayerPB.None;
            Path = "Head/OtherHead/" + id;
        }
        else
        {
            var playerId = cardId / 1000;
            PlayerPb = (PlayerPB) playerId;

            var isEvolutionBefore = id % 100 == 11;
            Path = isEvolutionBefore ? "Head/" + cardId : "Head/EvolutionHead/" + cardId;
        }
      //  Debug.LogError("Id--->"+Path);
    }

    private void SetIsUnlock()
    {
        bool isUnlock = GlobalData.DiaryElementModel.IsUserElement(Id);

        if (isUnlock)
        {
            IsUnlock = true;
        }
        else
        {
            var cardId = UnlockClaim.CardId;
            var evolutionLevel = UnlockClaim.EvolutionLevel;

            var userCardData = GlobalData.CardModel.GetUserCardById(cardId);
           
            if (userCardData != null && (int) userCardData.Evolution >= evolutionLevel)
            {
                IsUnlock = true;
            }
            else if (userCardData == null && cardId == 0) //默认头像
            {
                IsUnlock = true;
            }
        }
    }

    public int CompareTo(HeadVo other)
    {
        int result = 0;
        if (_userCardVo == null||other._userCardVo==null)
        {
            result =-1;
        }
        else if (other._userCardVo.CardVo.Credit.CompareTo(_userCardVo.CardVo.Credit) != 0)
        {
            result = -other._userCardVo.CardVo.Credit.CompareTo(_userCardVo.CardVo.Credit);
        }
        else if (other._userCardVo.Star.CompareTo(_userCardVo.Star) != 0)
        {
            result = other._userCardVo.Star.CompareTo(_userCardVo.Star);
        }
        else if (other._userCardVo.Level.CompareTo(_userCardVo.Level) != 0)
        {
            result = other._userCardVo.Level.CompareTo(_userCardVo.Level);
        }    
                       
        return result;
    }
   
}