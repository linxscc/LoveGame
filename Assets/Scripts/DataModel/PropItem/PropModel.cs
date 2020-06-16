using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

/// <summary>
/// 道具信息
/// </summary>
public class PropModel
{
    private Dictionary<int, ItemPB> _propBaseDict;

    private Dictionary<int, UserPropVo> _propItemDict;


    private RepeatedField<MallItemRulePB> _mallItemRulePBs;

    public void InitPropBase(ItemsRes res)
    {
        RepeatedField<ItemPB> list = res.Items;
        _propBaseDict = new Dictionary<int, ItemPB>();
        for (int i = 0; i < list.Count; i++)
        {
            ItemPB pb = list[i];
            _propBaseDict.Add(pb.ItemId, pb);
        }

        _mallItemRulePBs = res.MallItemRule;
    }


    public MallItemRulePB GetMallItemInfo(int itemId)
    {
        MallItemRulePB item = null;
        for (int i = 0; i < _mallItemRulePBs.Count; i++)
        {
            if (itemId == _mallItemRulePBs[i].ItemId)
            {
                item = _mallItemRulePBs[i];
            }
        }

        return item;
    }


    public UserPropVo GetUserProp(int propId)
    {
        UserPropVo item = null;
        _propItemDict.TryGetValue(propId, out item);
        if (item == null)
            item = new UserPropVo(propId);
        return item;
    }


    public bool IsGetUserProp(int propId)
    {
        return _propItemDict.ContainsKey(propId);
    }

    public ItemPB GetPropBase(int propId)
    {
        ItemPB item = null;
        _propBaseDict.TryGetValue(propId, out item);

        if (item == null)
            Debug.Log("道具 " + propId + "不存在");

        return item;
    }

    public void InitMyProps(MyItemRes res)
    {
        _propItemDict = new Dictionary<int, UserPropVo>();

        for (int i = 0; i < res.UserItems.Count; i++)
        {
            UserPropVo vo = new UserPropVo(res.UserItems[i]);

            _propItemDict.Add(vo.ItemId, vo);
           
         //   Debug.LogError("我拥有的道具："+vo.Name+"："+vo.ItemId);
           
            
        }
    }

    public void AddProps(UserItemPB[] items)
    {
        foreach (UserItemPB pb in items)
        {
            if (_propItemDict.ContainsKey(pb.ItemId))
            {
                _propItemDict[pb.ItemId].Num += pb.Num;
            }
            else
            {
                UserPropVo vo = new UserPropVo(pb);
                _propItemDict.Add(vo.ItemId, vo);
            }
        }

        EventDispatcher.TriggerEvent(EventConst.PropUpdated);
    }

    public void UpdateProps(UserItemPB[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            UserItemPB pb = items[i];
            if (pb == null)
            {
                Debug.LogError("null");
                return;
            }

            if (_propItemDict.ContainsKey(pb.ItemId))
            {
                _propItemDict[pb.ItemId].Num = pb.Num;
            }
            else
            {
                UserPropVo vo = new UserPropVo(pb);
                _propItemDict.Add(vo.ItemId, vo);
            }
        }

        EventDispatcher.TriggerEvent(EventConst.PropUpdated);
    }

    public void UpdateProps(RepeatedField<UserItemPB> resUserItems)
    {
        foreach (var v in resUserItems)
        {
            //Debug.LogError(v);
            UserItemPB pb = v;
            if (_propItemDict.ContainsKey(pb.ItemId))
            {
                _propItemDict[pb.ItemId].Num = pb.Num;
            }
            else
            {
                UserPropVo vo = new UserPropVo(pb);
                _propItemDict.Add(pb.ItemId, vo);
            }
        }
    }

    public string GetPropPath(int propId)
    {
        switch (propId)
        {
            case PropConst.GoldIconId:
            case PropConst.GemIconId:
            case PropConst.PowerIconId:
                return "Prop/particular/" + propId;

            default:
                return "Prop/" + propId;
        }
    }
    
    public string GetGiftPropPath(string propId)
    {
        return "Prop/GiftPack/" + propId;
    }

    public string GetPropStrPath(string imagename)
    {
        return "Prop/" + imagename;
    }


    public void AddProp(AwardPB award)
    {
        if (award.Resource != ResourcePB.Item)
            throw new Exception("不是道具 award.Resource:" + award.Resource);

        if (_propItemDict.ContainsKey(award.ResourceId))
        {
            _propItemDict[award.ResourceId].Num += award.Num;
        }
        else
        {
            UserPropVo vo = new UserPropVo(award.ResourceId);
            vo.Num = award.Num;
            _propItemDict.Add(vo.ItemId, vo);
        }

        EventDispatcher.TriggerEvent(EventConst.PropUpdated);
    }

    public void AddProps(RepeatedField<AwardPB> awards)
    {
        foreach (var award in awards)
        {
            if (award.Resource != ResourcePB.Item)
                throw new Exception("不是道具 award.Resource:" + award.Resource);

            if (_propItemDict.ContainsKey(award.ResourceId))
            {
                _propItemDict[award.ResourceId].Num += award.Num;
            }
            else
            {
                UserPropVo vo = new UserPropVo(award.ResourceId);
                vo.Num = award.Num;
                _propItemDict.Add(vo.ItemId, vo);
            }
        }

        EventDispatcher.TriggerEvent(EventConst.PropUpdated);
    }

    public string SpliceCardName(int roleId)
    {
        string cardName = "";
        switch (roleId)
        {
            case 0:
                cardName = "None";
                break;
            case PropConst.CardEvolutionPropTang:
                cardName = I18NManager.Get("Common_Role1");
                break;
            case PropConst.CardEvolutionPropQin:
                cardName = I18NManager.Get("Common_Role2");
                break;
            case PropConst.CardEvolutionPropYan:
                cardName = I18NManager.Get("Common_Role3");
                break;
            case PropConst.CardEvolutionPropChi:
                cardName = I18NManager.Get("Common_Role4");
                break;
        }

        return cardName;
    }
}