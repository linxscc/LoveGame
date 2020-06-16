using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;
using Random = UnityEngine.Random;
public class DrawData : Model
{
    private Dictionary<DrawPoolTypePB, List<ShowCardModel>> _drawCardPool = new Dictionary<DrawPoolTypePB, List<ShowCardModel>>();

	public List<ShowCardModel> DrawCardList;
    private List<DrawDialogInfo> _drawDialogInfos = null;
    
    public string GetRandomDialogById(int id)
    {
        if (_drawDialogInfos == null) 
        {
            _drawDialogInfos = ClientData.LoadDrawCardDialogData();
        }

        DrawDialogInfo drawDialogInfo = _drawDialogInfos.Find((m) => { return m.CardId == id; });
        if (drawDialogInfo != null && drawDialogInfo.Dialog.Count > 0) 
        {
            int random = Random.Range(0, drawDialogInfo.Dialog.Count);
            return drawDialogInfo.Dialog[random];
        }
        return "";
    }


    public void InitData(DrawProbRes res)
    {
        if (_drawCardPool == null)
            _drawCardPool = new Dictionary<DrawPoolTypePB, List<ShowCardModel>>();
        else
            _drawCardPool.Clear();
        foreach (var s in res.DrawProbs)
        {
            if (!_drawCardPool.ContainsKey(s.DrawPoolType))
                _drawCardPool.Add(s.DrawPoolType, new List<ShowCardModel>());
            if (s.Resource == ResourcePB.Fans)
            {
                FansRulePB funsRulePb = MyDepartmentData.GetFansRule(s.ResourceId);
                bool isHave = GlobalData.DepartmentData.GetFans(s.ResourceId) == null ? false : true;
                ShowCardModel showCardModel = new ShowCardModel(s.ResourceId, SortResouce.Fans, SortCredit.NO, s.DrawEvent, isHave, funsRulePb.FansName);
                _drawCardPool[s.DrawPoolType].Add(showCardModel);
                // Debug.Log(" funs ResourceId "+ s.ResourceId);
            }
            else if (s.Resource == ResourcePB.Card || s.Resource == ResourcePB.Puzzle)
            {
                CardPB cardPb = GlobalData.CardModel.GetCardBase(s.ResourceId);
                bool isHave = GlobalData.CardModel.GetUserCardById(cardPb.CardId) == null ? false : true;

                SortResouce sortResouce = s.Resource == ResourcePB.Card ? SortResouce.Card : SortResouce.Puzzle;
                SortCredit sortCredit = cardPb.Credit == CreditPB.Ssr ? SortCredit.SSR : cardPb.Credit == CreditPB.Sr ? SortCredit.SR : SortCredit.R;

                ShowCardModel showCardModel = new ShowCardModel(cardPb.CardId, sortResouce, sortCredit, s.DrawEvent, cardPb.Player, isHave, cardPb.CardName);
                _drawCardPool[s.DrawPoolType].Add(showCardModel);
            }
        }
        foreach(KeyValuePair<DrawPoolTypePB, List<ShowCardModel>> list in _drawCardPool)
        {
            list.Value.Sort((x, y) => {
                if (x.Credit.CompareTo(y.Credit) == 0)
                {
                    return x.CardId.CompareTo(y.CardId);
                }
                return x.Credit.CompareTo(y.Credit);
                //if (x.Resource.CompareTo(y.Resource)==0)
                //{
                //    if(x.Credit.CompareTo(y.Credit)==0)
                //    {
                //        return x.CardId.CompareTo(y.CardId);
                //    }
                //    return x.Credit.CompareTo(y.Credit);
                //}
                //return x.Resource.CompareTo(y.Resource);
            });
        }



       // if (DrawCardList==null)
       // {
       //     DrawCardList = new List<ShowCardModel>();
       // }
       // else
       // {
       //     DrawCardList.Clear();
       // }
       //// List<ShowCardModel> showCardModelList = new List<ShowCardModel>();
       // foreach (var s in res.DrawProbs)
       // {
       //     if (s.Resource == ResourcePB.Fans)
       //     {
       //         FansRulePB funsRulePb = MyDepartmentData.GetFansRule(s.ResourceId);
       //         bool isHave = GlobalData.DepartmentData.GetFans(s.ResourceId) == null ? false : true;
       //         ShowCardModel showCardModel = new ShowCardModel(s.ResourceId, SortResouce.Fans,SortCredit.NO, s.DrawEvent, isHave, funsRulePb.FansName);
       //         DrawCardList.Add(showCardModel);
       //        // Debug.Log(" funs ResourceId "+ s.ResourceId);
       //     }
       //     else if (s.Resource == ResourcePB.Card || s.Resource == ResourcePB.Puzzle)
       //     {
       //         CardPB cardPb = GlobalData.CardModel.GetCardBase(s.ResourceId);
       //         bool isHave = GlobalData.CardModel.GetUserCardById(cardPb.CardId) == null ? false : true;

       //         SortResouce sortResouce = s.Resource == ResourcePB.Card ? SortResouce.Card : SortResouce.Puzzle;
       //         SortCredit sortCredit = cardPb.Credit == CreditPB.Ssr ? SortCredit.SSR : cardPb.Credit == CreditPB.Sr ? SortCredit.SR : SortCredit.R;

       //         ShowCardModel showCardModel = new ShowCardModel(cardPb.CardId, sortResouce, sortCredit ,s.DrawEvent, cardPb.Player, isHave, cardPb.CardName);
       //         DrawCardList.Add(showCardModel);     
       //     }
       // }

       // DrawCardList.Sort((x, y) => {
       //     if (x.Credit.CompareTo(y.Credit) == 0)
       //     {
       //         return x.CardId.CompareTo(y.CardId);
       //     }
       //     return x.Credit.CompareTo(y.Credit);
       //     //if (x.Resource.CompareTo(y.Resource)==0)
       //     //{
       //     //    if(x.Credit.CompareTo(y.Credit)==0)
       //     //    {
       //     //        return x.CardId.CompareTo(y.CardId);
       //     //    }
       //     //    return x.Credit.CompareTo(y.Credit);
       //     //}
       //     //return x.Resource.CompareTo(y.Resource);
       // });

    }

    public List<ShowCardModel> GetCardList(DrawPoolTypePB poolType)
    {
        if (_drawCardPool.ContainsKey(poolType))
            return _drawCardPool[poolType];
        return new List<ShowCardModel>();
    }

    public int GetTotalNum(DrawEventPB drawEvent)
    {
        //return DrawCardList.Count;
        return DrawCardList.FindAll(match => { return match.DrawEvent == drawEvent; }).Count;
    }

    public int GetTotalNum(DrawPoolTypePB poolType, DrawEventPB drawEvent)
    {
        return GetCardList(poolType).FindAll(match => { return match.DrawEvent == drawEvent; }).Count;
    }

    /// <summary>
    /// 获取拥有的卡牌数量
    /// </summary>
    public int GetOwnNum(DrawPoolTypePB poolType, DrawEventPB drawEvent)
    {
        return GetCardList(poolType).FindAll(match => { return match.DrawEvent == drawEvent && match.IsNew == true; }).Count;
    }
}
