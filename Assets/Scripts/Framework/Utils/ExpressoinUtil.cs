using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataModel;
using game.main;

public class ExpressoinUtil {


    /// <summary>
    /// 语音收藏
    /// </summary>
    /// <param name="NpcId"></param>
    /// <returns></returns>
    public static List<CardAwardPreInfo> GetDialogCollects(int NpcId)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        infos.AddRange(GetLoveDiaryLabelAwardPreInfoOfNpcId(NpcId));
        infos.AddRange(GetDrawCardAwardPreInfo(NpcId));
        infos.AddRange(GetGiftAwardPreInfoOfNpcId(NpcId));
        infos.AddRange(GetLoginAwardPreInfoOfNpcId(NpcId));
        infos.AddRange(GetMomolePreInfoOfNpcId(NpcId));
        infos.Sort();
        return infos;
    }


    public static List<CardAwardPreInfo> GetMomolePreInfoOfNpcId(int NpcId)
    {
        
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        var head = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.HEAD);
        foreach (var v in head)
        {
            if (v.Dialog == "")
                continue;
            string cotent = v.DialogName;
            if (infos.Find((m) => { return m.dialogId == v.Dialog; }) != null)
                continue;
            bool isUnlock = true;//送礼语音一定解锁
            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.MomoleVioce, v.Dialog, v.Id, v.UnlockDescription));
        }

        var body = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.BODY);
        foreach (var v in body)
        {
            if (v.Dialog == "")
                continue;
            string cotent = v.DialogName;
            if (infos.Find((m) => { return m.dialogId == v.Dialog; }) != null)
                continue;
            bool isUnlock = true;//送礼语音一定解锁
            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.MomoleVioce, v.Dialog, v.Id, v.UnlockDescription));
        }
        return infos;
    }

    public static List<CardAwardPreInfo> GetLoginAwardPreInfoOfNpcId(int NpcId)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        var gift = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.LOGIN);
        foreach (var v in gift)
        {
            string cotent = v.DialogName;
            if (infos.Find((m) => { return m.dialogId == v.Dialog; }) != null)
                continue;
            bool isUnlock = true;//送礼语音一定解锁
            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.LoginVioce, v.Dialog,  v.Id, v.UnlockDescription));
        }
        return infos;
    }

    public static List<CardAwardPreInfo> GetGiftAwardPreInfoOfNpcId(int NpcId)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        var gift = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.GIFT);
        foreach (var v in gift)
        {
            string cotent = v.DialogName;
            if (infos.Find((m) => { return m.dialogId == v.Dialog; }) != null)
                continue;
            bool isUnlock = true;//送礼语音一定解锁
            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.GiftVioce, v.Dialog, v.Id, v.UnlockDescription));
        }
        return infos;
    }


    /// <summary>
    /// 根据人物获取相关信息
    /// </summary>
    /// <param name="NpcId"></param>
    /// <returns></returns>
    public static List<CardAwardPreInfo> GetLoveDiaryLabelAwardPreInfoOfNpcId(int NpcId)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();

        //日记语音
        var lovediary = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.LOVEDIARY);
        foreach (var v in lovediary)
        {
            string cotent = v.DialogName;

            int elementId = int.Parse(v.DialogId);

            bool isUnlock = GlobalData.DiaryElementModel.IsCanUseElement(elementId);
            var info = CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.LoveDiaryLabelVoice, v.Dialog, v.Id, v.UnlockDescription);

            string storeKey = Constants.REDPOINT_YUYINSHOUCANG + elementId;
            info.isNew = Util.GetIsRedPoint(storeKey);
            info.key = storeKey;
//            if(info.isNew)
//            {
//               // Util.DeleteRedPoint(storeKey, false);
//            }
            
            infos.Add(info);
        }
        return infos;
    }



    public static List<CardAwardPreInfo> GetDrawCardAwardPreInfo(int NpcId)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        var drawcards = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.DRAWCARD);
        foreach (var v in drawcards)
        {
            string cotent = v.DialogName;
            bool isUnlock = true;//抽卡语音一定解锁
            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.DrawCardVioce,v.Dialog,v.Id,v.UnlockDescription));
        }

        return infos;
    }

    private static CardAwardPreInfo CreateCardAwardPreInfo(string content, bool isUnlock, CardAwardPreType cardAwardPreType, string dialogId = "", int expressionId = 0, string unlockDescription = "", int priority = 0)
    {
        var info = new CardAwardPreInfo();
        info.content = content;
        info.isUnlock = isUnlock;
        info.priority = priority;
        info.dialogId = dialogId;
        info.expressionId = expressionId;
        info.cardAwardPreType = cardAwardPreType;
        info.UnlockDescription = unlockDescription;
        info.isNew = false;
        return info;
    }


}
