using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static FavorabilityGiveGiftsItemVO;
using UnityEngine;
/// <summary>
/// 好感度 赠送礼物 Model
/// WGQ
/// </summary>
public class FavorabilityGiveGiftsModel : Model
{

    private List<FavorabilityGiveGiftsItemVO> _favorabilityItemLists;  //好感度升级道具信息（从服务器请求过来的原始道具集合）
    public List<FavorabilityGiveGiftsItemVO> UserGiveGiftsItemList;   //用户拥有赠送礼物道具的集合
    public FavorabilityGiveGiftsItemVO CurrentItemVo;  //当前选中的道具信息

    public FavorabilityGiveGiftsModel()
    {
        DisposePBList();
        AddData();     
    }
   
    private void DisposePBList()
    {
        var pbList = GlobalData.FavorabilityMainModel.FavorabilityItemPBLists;

        if (_favorabilityItemLists == null)
        {
            _favorabilityItemLists = new List<FavorabilityGiveGiftsItemVO>();
            foreach (var v in pbList)
            {
                var favorabilityItem = new FavorabilityGiveGiftsItemVO(v);
              
                if (_favorabilityItemLists.Count == 0)         
                {
                    _favorabilityItemLists.Add(favorabilityItem);
                }
                else                                            
                {
                    var lastIndex = _favorabilityItemLists.Count - 1;
                    if (_favorabilityItemLists[lastIndex].ItemId != favorabilityItem.ItemId)
                    {
                        _favorabilityItemLists.Add(favorabilityItem);
                    }
                    else
                    {
                        _favorabilityItemLists[lastIndex].PlayerPBs.Add(v.Player);
                    }
                }
            }
        }
        
    }

    private void AddData()
    {
        if (UserGiveGiftsItemList==null)
        {
            UserGiveGiftsItemList = new List<FavorabilityGiveGiftsItemVO>();
        }      
        var baseList = _favorabilityItemLists;    
        foreach (var t in baseList)
        {
            var userItemId = GlobalData.PropModel.GetUserProp(t.ItemId).ItemId;
            var userItemNum = GlobalData.PropModel.GetUserProp(t.ItemId).Num;
          
            if (userItemId==t.ItemId)    
            {
                if (userItemNum >= 1)
                {                  
                    UserGiveGiftsItemList.Add(t);                    
                }
            }
        }
        
        UserGiveGiftsItemList.Sort(Descendingorder);      
    }

    /// <summary>
    /// 降序排序
    /// </summary>
    /// <param name="x">Id</param>
    /// <param name="y">Id</param>
    /// <returns></returns>
    private int Descendingorder(FavorabilityGiveGiftsItemVO x, FavorabilityGiveGiftsItemVO y)
    {
        return y.ItemId.CompareTo(x.ItemId);
    }


    /// <summary>
    /// 更新集合中红框的显示
    /// </summary>
    /// <param name="vo"></param>
    public void UpdataListItemRedFrameShow(FavorabilityGiveGiftsItemVO vo)
    {
        foreach (var t in UserGiveGiftsItemList)
        {
            if (vo.ItemId== t.ItemId)
            {
                t.IsShowRedFrame = true;
                t.IsShowDes = true;
            }
            else
            {
                t.IsShowRedFrame = false;
                t.IsShowDes = false;
            }
        }
    }




    /// <summary>
    /// 是否有选中
    /// </summary>
    /// <returns>true判定有选中，false判定未选中</returns>
    public bool IsPitchOn()
    {
        foreach (var t in UserGiveGiftsItemList)
        {
            if (t.IsShowRedFrame)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 重置红框状态
    /// </summary>
    public void ResetRedFrameState()
    {
        foreach (var t in UserGiveGiftsItemList)
        {
            t.IsShowRedFrame = false;
            t.IsShowDes = false;
        }
    }

    //重置文本框
    //public void RestDesTextState()
    //{
    //    for (int i = 0; i < UserGiveGiftsItemList.Count; i++)
    //    {
    //        UserGiveGiftsItemList[i].IsShowDes = false;
    //    }
            
    //}

    public void UpDataListItemNum(int id ,int num)
    {
        foreach (var t in UserGiveGiftsItemList)
        {
            if (t.ItemId ==id)
            {
                t.ItemNum = num;
                break;
            }
        }
    }

   public void Remove(int index)
   {
        UserGiveGiftsItemList.RemoveAt(index);       
    }

  


    
}
