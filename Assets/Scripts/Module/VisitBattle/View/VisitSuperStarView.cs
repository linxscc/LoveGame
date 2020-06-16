using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using Module.VisitBattle.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class VisitSuperStarView : View
{

    private Transform _middleCards;
    private List<VisitBattleUserCardVo> _cards;

    private VisitLevelVo _info;

    private int _totalPower = 0;

    private LoopVerticalScrollRect _smallCardList;
    private VisitBattleModel _battleModel;

    private void Awake()
    {
        _smallCardList = transform.Find("BgBottom/SmallCardList").GetComponent<LoopVerticalScrollRect>();
        _smallCardList.poolSize = 8;
        _smallCardList.prefabName = "VisitBattle/Prefabs/VisitSmallHeroCard";
        _smallCardList.UpdateCallback = ListUpdataCallback;

        _cards = new List<VisitBattleUserCardVo>();

        _middleCards = transform.Find("BgTop/StarContainer");

        var okBtn = transform.Find("BgTop/OkBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            if (_cards.Count == 0)
            {
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SUPERSTAR_CONFIRM_ERROR1));
                return;
            }

            SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SUPERSTAR_CONFIRM, Message.MessageReciverType.DEFAULT,
                _totalPower, _cards));
        });

        var quicklySelectBtn = transform.Find("BgTop/QuicklySelectBtn").GetComponent<Button>();
        quicklySelectBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SUPERSTAR_QUICKSELECT, Message.MessageReciverType.CONTROLLER));
        });

        Transform tabBar = transform.Find("TabScrollView/Viewport/TabBar");
        for (int i = 0; i < tabBar.childCount; i++)
        {
            PointerClickListener.Get(tabBar.GetChild(i).gameObject).onClick = OnTabClick;
        }

        for (int i = 0; i < 6; i++)
        {
            PointerClickListener.Get(_middleCards.GetChild(i).gameObject).onClick = go =>
            {
                VisitBattleUserCardVo data = go.GetComponent<VisitMiddleHeroCard>().GetData();
                if (data != null)
                    SendMessage(new Message(MessageConst.CMD_VISITBATTLE_REMOVE_HERO_CARD,
                        Message.MessageReciverType.CONTROLLER, data));
            };
        }
    }

    private void Start()
    {
        RectTransform rect = transform.GetComponent<RectTransform>();
        RectTransform bgBottom = transform.Find("BgBottom").GetComponent<RectTransform>();
        RectTransform bgTop = transform.Find("BgTop").GetComponent<RectTransform>();

        float containerHeight = rect.GetSize().y;
        float topHeight = bgTop.sizeDelta.y;
        float topTop = bgTop.offsetMax.y;
        float bottomHeight = containerHeight - (topHeight - topTop) - 90;

        bgBottom.sizeDelta = new Vector2(bgBottom.sizeDelta.x, bottomHeight);

        float tabScrollViewWidth = transform.Find("TabScrollView/").GetComponent<RectTransform>().GetSize().x;
        if (tabScrollViewWidth <= Main.StageWidth)
        {
            RectTransform rect2 = transform.Find("TabScrollView/Viewport").GetComponent<RectTransform>();
            rect2.sizeDelta = new Vector2(Main.StageWidth, rect2.sizeDelta.y);
        }
    }

    private void OnTabClick(GameObject go)
    {
        switch (go.name)
        {
            case "AllBtn":
                _battleModel.FilterCard();
                break;
            case "OriginalBtn":
                _battleModel.FilterCard((int)AbilityPB.Composing);
                break;
            case "GlamourBtn":
                _battleModel.FilterCard((int)AbilityPB.Charm);
                break;
            case "PopularityBtn":
                _battleModel.FilterCard((int)AbilityPB.Popularity);
                break;
            case "SingBtn":
                _battleModel.FilterCard((int)AbilityPB.Singing);
                break;
            case "DanceBtn":
                _battleModel.FilterCard((int)AbilityPB.Dancing);
                break;
            case "WillpowerBtn":
                _battleModel.FilterCard((int)AbilityPB.Perseverance);
                break;
        }

        _smallCardList.RefreshCells();
        _smallCardList.totalCount = _battleModel.UserCardList.Count;
    }


    public void SetData(VisitLevelVo info, VisitBattleModel battleModel)
    {
        _info = info;
        _battleModel = battleModel;

        _battleModel.FilterCard();

        ResetMiddleHeroCard();
        transform.Find("BgTop/NeedStrengthText").GetComponent<Text>().text =
            I18NManager.Get("Visit_Hint1") + ViewUtil.AbilitiesToString(info.Abilitys);
        transform.Find("BgTop/Panel/Original/ValueText").GetComponent<Text>().text = "0";
        transform.Find("BgTop/Panel/Glamour/ValueText").GetComponent<Text>().text = "0";
        transform.Find("BgTop/Panel/Popularity/ValueText").GetComponent<Text>().text = "0";
        transform.Find("BgTop/Panel/Sing/ValueText").GetComponent<Text>().text = "0";
        transform.Find("BgTop/Panel/Dancing/ValueText").GetComponent<Text>().text = "0";
        transform.Find("BgTop/Panel/Willpower/ValueText").GetComponent<Text>().text = "0";
        _smallCardList.RefreshCells();
        _smallCardList.totalCount = _battleModel.UserCardList.Count;
    }

    private void ListUpdataCallback(GameObject go, int index)
    {
        go.GetComponent<VisitSmallHeroCard>().SetData(_battleModel.UserCardList[index]);
    }

    public void AddHeroCard(VisitBattleUserCardVo vo)
    {
        for (int i = 0; i < 6; i++)
        {
            VisitMiddleHeroCard card = _middleCards.GetChild(i).GetComponent<VisitMiddleHeroCard>();
            if (card.GetData() == null)
            {
                card.SetData(vo);
                break;
            }
        }

        _smallCardList.RefreshCells();
        _cards.Add(vo);
        CountPoints();
    }

    public void RemoveCard(VisitBattleUserCardVo vo)
    {
        for (int i = 0; i < 6; i++)
        {
            VisitMiddleHeroCard card = _middleCards.GetChild(i).GetComponent<VisitMiddleHeroCard>();
            if (card.GetData().UserCardVo.CardId == vo.UserCardVo.CardId)
            {
                card.SetData(null);
                card.transform.SetAsLastSibling();
                break;
            }
        }

        ResetMiddleHeroCard();
        _smallCardList.RefreshCells();
        _cards.Remove(vo);
        CountPoints();
    }

    private void ResetMiddleHeroCard()
    {
        for (int i = 0; i < 6; i++)
        {
            VisitMiddleHeroCard card = _middleCards.GetChild(i).GetComponent<VisitMiddleHeroCard>();
           // card.InitCard(i, _battleModel.IsCardPositionOpen(i));
            card.InitCard(i, _battleModel.IsCardPositionOpen(i), _battleModel.CardNumRules);
        }
    }

    private void CountPoints()
    {
        int DancingPoint = 0;
        int GlamourAdditon = 0;
        int OriginalAdditon = 0;
        int PopularityAdditon = 0;
        int SingingAdditon = 0;
        int WillpowerAdditon = 0;
        foreach (var card in _cards)
        {
            DancingPoint += card.UserCardVo.Dancing;
            GlamourAdditon += card.UserCardVo.Glamour;
            OriginalAdditon += card.UserCardVo.Original;
            PopularityAdditon += card.UserCardVo.Popularity;
            SingingAdditon += card.UserCardVo.Singing;
            WillpowerAdditon += card.UserCardVo.Willpower;
        }

        foreach (var abilityPb in _battleModel.LevelVo.Abilitys)
        {
            switch (abilityPb)
            {
                case AbilityPB.Singing:
                    SingingAdditon *= 2;
                    break;
                case AbilityPB.Dancing:
                    DancingPoint *= 2;
                    break;
                case AbilityPB.Composing:
                    OriginalAdditon *= 2;
                    break;
                case AbilityPB.Popularity:
                    PopularityAdditon *= 2;
                    break;
                case AbilityPB.Charm:
                    GlamourAdditon *= 2;
                    break;
                case AbilityPB.Perseverance:
                    WillpowerAdditon *= 2;
                    break;
            }
        }

        Util.TweenTextNum(transform.Find("BgTop/Panel/Original/ValueText").GetComponent<Text>(), 0.3f, OriginalAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Glamour/ValueText").GetComponent<Text>(), 0.3f, GlamourAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Popularity/ValueText").GetComponent<Text>(), 0.3f, PopularityAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Sing/ValueText").GetComponent<Text>(), 0.3f, SingingAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Dancing/ValueText").GetComponent<Text>(), 0.3f, DancingPoint);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Willpower/ValueText").GetComponent<Text>(), 0.3f, WillpowerAdditon);

        foreach (var ab in _info.Abilitys)
        {
            switch (ab)
            {
                case AbilityPB.Charm:
                    _totalPower += GlamourAdditon;
                    break;
                case AbilityPB.Composing:
                    _totalPower += OriginalAdditon;
                    break;
                case AbilityPB.Dancing:
                    _totalPower += DancingPoint;
                    break;
                case AbilityPB.Perseverance:
                    _totalPower += WillpowerAdditon;
                    break;
                case AbilityPB.Popularity:
                    _totalPower += PopularityAdditon;
                    break;
                case AbilityPB.Singing:
                    _totalPower += SingingAdditon;
                    break;
            }
        }

        _totalPower = DancingPoint + GlamourAdditon + OriginalAdditon + SingingAdditon + WillpowerAdditon +
                      PopularityAdditon;
    }
}
