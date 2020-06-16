using System;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using UnityEngine;

/// <summary>
/// 好感度 赠送礼物ItemVO
/// WGQ
/// </summary>
public class FavorabilityGiveGiftsItemVO : IComparable<FavorabilityGiveGiftsItemVO>
{

    public enum ItemGradeType
    {    
        /// <summary>
        /// 专属基础道具
        /// </summary>
        ExclusiveBasicsItem,
        
        /// <summary>
        /// 通用基础道具
        /// </summary>
        GMBasicsItem,
        
        /// <summary>
        /// 专属高级道具
        /// </summary>
        ExclusiveAdvancedItem, 
        
        /// <summary>
        /// 通用高级道具
        /// </summary>
        GMAdvancedItem,         
    }
   
    public int ItemId;   
    public int ItemType;    
    public string ItemDesc;   
    public List<PlayerPB> PlayerPBs = new List<PlayerPB>();
    public ItemGradeType  GradeType;
    public string Image;
    public bool IsShowRedFrame=false; 
    public int ItemNum;         
    public string Name;         
    public int Power;           
    public int Exp;                                            
    public bool IsShowDes = false;
    
    public FavorabilityGiveGiftsItemVO(FavorabilityItemPB  pb)
    {
        ItemId = pb.ItemId;
        ItemType = pb.ItemType;
        ItemDesc = pb.ItemDesc;
        PlayerPBs.Add(pb.Player);
        ItemNum = GlobalData.PropModel.GetUserProp(ItemId).Num;
        Image = GlobalData.PropModel.GetUserProp(ItemId).GetTexturePath();
        Name = GlobalData.PropModel.GetUserProp(ItemId).Name;
        Power = GlobalData.PropModel.GetUserProp(ItemId).Power;
        Exp = GlobalData.PropModel.GetUserProp(ItemId).Exp;
        SetItemGrade();
    }


    private void SetItemGrade()
    {
        if (ItemType==0)
        {
            GradeType = PlayerPBs.Count==1 ? ItemGradeType.ExclusiveBasicsItem : ItemGradeType.GMBasicsItem;
        }
        else if (ItemType ==1)
        {
            GradeType = PlayerPBs.Count==1 ? ItemGradeType.ExclusiveAdvancedItem : ItemGradeType.GMAdvancedItem;
        }
    }
    
    
    
    public int CompareTo(FavorabilityGiveGiftsItemVO other)
    {
        throw new NotImplementedException();
    }

   


}





