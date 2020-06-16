using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleSuperStarView : View
{
   private Transform _middleCards;
    private List<BattleUserCardVo> _cards;

    private LevelVo _info;

    private int _totalPower = 0;

    private LoopVerticalScrollRect _smallCardList;
    private CapsuleBattleModel _battleModel;

    private void Awake()
    {
        _smallCardList = transform.Find("BgBottom/SmallCardList").GetComponent<LoopVerticalScrollRect>();
        _smallCardList.poolSize = 8;
        _smallCardList.prefabName = "Battle/Prefabs/SmallHeroCard";
        _smallCardList.UpdateCallback = ListUpdataCallback;

        _cards = new List<BattleUserCardVo>();

        _middleCards = transform.Find("BgTop/StarContainer");

        var okBtn = transform.Find("BgTop/OkBtn").GetComponent<Button>();

        okBtn.onClick.AddListener(Confirm);

        Transform tabBar = transform.Find("TabScrollView/Viewport/TabBar");
        for (int i = 0; i < tabBar.childCount; i++)
        {
            PointerClickListener.Get(tabBar.GetChild(i).gameObject).onClick = OnTabClick;
        }

        for (int i = 0; i < 6; i++)
        {
            PointerClickListener.Get(_middleCards.GetChild(i).gameObject).onClick = go =>
            {
                BattleUserCardVo data = go.GetComponent<MiddleHeroCard>().GetData();
                if (data != null)
                    SendMessage(new Message(MessageConst.CMD_BATTLE_REMOVE_HERO_CARD,
                        Message.MessageReciverType.CONTROLLER, data));
            };
        }
    }

    public void Confirm()
    {
        if (_cards.Count == 0)
        {
            SendMessage(new Message(MessageConst.CMD_BATTLE_SUPERSTAR_CONFIRM_ERROR1));
            return;
        }

        List<int> tempList = new List<int>();

        for (int i = 0; i < _cards.Count; i++)
        {
            tempList.Add((int) _cards[i].UserCardVo.CardVo.Player);
        }

        HashSet<int> hs = new HashSet<int>(tempList);

        Queue<int> roleIds = new Queue<int>();
        foreach (var t in hs)
        {
            roleIds.Enqueue(t);
        }

        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SEND_ROLE_ID, Message.MessageReciverType.DEFAULT, roleIds));
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SUPERSTAR_CONFIRM, Message.MessageReciverType.DEFAULT,
            _totalPower, _cards));
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
                _battleModel.FilterCard((int) AbilityPB.Composing);
                break;
            case "GlamourBtn":
                _battleModel.FilterCard((int) AbilityPB.Charm);
                break;
            case "PopularityBtn":
                _battleModel.FilterCard((int) AbilityPB.Popularity);
                break;
            case "SingBtn":
                _battleModel.FilterCard((int) AbilityPB.Singing);
                break;
            case "DanceBtn":
                _battleModel.FilterCard((int) AbilityPB.Dancing);
                break;
            case "WillpowerBtn":
                _battleModel.FilterCard((int) AbilityPB.Perseverance);
                break;
        }

        _smallCardList.RefreshCells();
        _smallCardList.totalCount = _battleModel.UserCardList.Count;
    }


    public void SetData(LevelVo info, CapsuleBattleModel battleModel)
    {
        _info = info;
        _battleModel = battleModel;

        _battleModel.FilterCard();

        ResetMiddleHeroCard();
        transform.Find("BgTop/NeedStrengthText").GetComponent<Text>().text =
            I18NManager.Get("Battle_SuperStarViewNeedStrengthText",ViewUtil.AbilitiesToString(info.Abilitys));
           
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
        go.GetComponent<SmallHeroCard>().SetData(_battleModel.UserCardList[index]);
    }

    public void AddHeroCard(BattleUserCardVo vo)
    {
        for (int i = 0; i < 6; i++)
        {
            MiddleHeroCard card = _middleCards.GetChild(i).GetComponent<MiddleHeroCard>();
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

    public void RemoveCard(BattleUserCardVo vo)
    {
        for (int i = 0; i < 6; i++)
        {
            MiddleHeroCard card = _middleCards.GetChild(i).GetComponent<MiddleHeroCard>();
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
            MiddleHeroCard card = _middleCards.GetChild(i).GetComponent<MiddleHeroCard>();
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
        Util.TweenTextNum(transform.Find("BgTop/Panel/Popularity/ValueText").GetComponent<Text>(), 0.3f,
            PopularityAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Sing/ValueText").GetComponent<Text>(), 0.3f, SingingAdditon);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Dancing/ValueText").GetComponent<Text>(), 0.3f, DancingPoint);
        Util.TweenTextNum(transform.Find("BgTop/Panel/Willpower/ValueText").GetComponent<Text>(), 0.3f,
            WillpowerAdditon);

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
