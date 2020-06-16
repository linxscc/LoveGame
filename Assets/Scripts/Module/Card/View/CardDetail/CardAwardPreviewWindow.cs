using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CardAwardPreType
{
    LoveStory,//恋爱剧情
    Sms,//短信
    Call,//电话
    FriendCirlce,//朋友圈
    Weibo,//微博
    Voice,//语音
    LoveDiaryLabelVoice,//恋爱日记
    DrawCardVioce,//抽卡语音
    GiftVioce,//抽卡语音
    LoginVioce,//登陆语音
    MomoleVioce,//摸摸乐语音
}

public class CardAwardPreInfo : IComparable<CardAwardPreInfo>
{
    public string content;//展示文案
    public bool isUnlock;//是否解锁
    public int priority;//优先级
    public CardAwardPreType cardAwardPreType;//奖励预览类型
    public string dialogId;//音频
    public int expressionId;//表情
    public string UnlockDescription;//解锁描述
    public string StartTips;//开头文案
    public string key;
    public bool isNew//新鲜出炉的
    {
        get;
        set;
    }


    public string ShowContent
    {
        get
        {
            string str = "";

            switch(cardAwardPreType)
            {
                case CardAwardPreType.Sms:
                    str = I18NManager.Get("Phone_Sms") + "." + content;
                    break;
                case CardAwardPreType.Call:
                    str = I18NManager.Get("Phone_Call") + "." + content;
                    break;
                case CardAwardPreType.FriendCirlce:
                    str = I18NManager.Get("Phone_Friendscircle") + "." + content;
                    break;
                case CardAwardPreType.Weibo:
                    str = I18NManager.Get("Phone_Weibo") +"."+ content;
                    break;
                case CardAwardPreType.LoveStory:
                    str = I18NManager.Get("LoveAppointment_LoveStory") + "." + content;
                    break;
                case CardAwardPreType.LoveDiaryLabelVoice:
                    str = I18NManager.Get("LoveDiary_LabelVoice") + "." + content;
                    break;
                case CardAwardPreType.DrawCardVioce:
                    str = I18NManager.Get("DrawCard_DrawCardVoice") + "." + content;
                    break;
            }
           
            return StartTips +str;
        }
    }

    public int CompareTo(CardAwardPreInfo other)
    {
        if(isUnlock.CompareTo(other.isUnlock)!=0)
        {
            return -isUnlock.CompareTo(other.isUnlock);
        }

        if(cardAwardPreType.CompareTo(other.cardAwardPreType)!=0)
        {
            return cardAwardPreType.CompareTo(other.cardAwardPreType);
        }

        if (priority.CompareTo(other.priority) != 0)
        {
            return priority.CompareTo(other.priority);
        }

        return 0;

    }
}

public class CardAwardPreviewWindow : Window
{
   /// private LoopVerticalScrollRect _cardAwardList;

    //List<CardAwardPreInfo> _infos;
    private Text _name;
    private Transform _parent;

    private void Awake()
    {
        _name = transform.GetText("Bg/Name");
        _parent = transform.Find("Bg/ScrollRect/Content");
        //_cardAwardList = transform.Find("PropDropGroup/ProDropList").GetComponent<LoopVerticalScrollRect>();
        //_cardAwardList.prefabName = "Card/Prefabs/CardDetail/CardAwardPreviewItem";
        //_cardAwardList.poolSize = 2;
        //_cardAwardList.UpdateCallback = UpdateCardAwardInfo;
    }

    private void UpdateCardAwardInfo(GameObject arg1, int arg2)
    {
        //if(arg2>=_infos.Count)
        //{
        //    return;
        //}
        //arg1.GetComponent<CardAwardPreviewItem>().SetData(_infos[arg2]);
    }

    public void SetData(string name, List<CardAwardPreInfo> list)
    {
        _name.text = name;

        var prefab = GetPrefab("Card/Prefabs/CardDetail/CardAwardPreviewItem");

        foreach (var t in list)
        {
            var item = Instantiate(prefab, _parent, false);
            item.GetComponent<CardAwardPreviewItem>().SetData(t);
        }
    }

    public void OnClickClose()
    {
        base.Close();
    }

}
