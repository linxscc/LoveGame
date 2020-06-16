using System.Collections;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using Module.Battle.Data;
using Module.VisitBattle.Data;
using UnityEngine;
using UnityEngine.UI;

public class VisitFinalEstimateRewardView : View
{
    private Text _userNameText; //用户名
    private Text _levelText; //等级Text
    private Text _expAddText; //经验增加Text  
    private Text _expText; //经验Text
    private ProgressBar progressBar;

    private Text _vip; //vip文案展示

    private void Awake()
    {
        _userNameText = transform.Find("Experience/UserNameText").GetComponent<Text>();
        _levelText = transform.Find("Experience/LevelText").GetComponent<Text>();
        _expAddText = transform.Find("Experience/ExpAdd/Text").GetComponent<Text>();
        _expText = transform.Find("Experience/ExpText").GetComponent<Text>();
        progressBar = transform.Find("Experience/ProgressBar").GetComponent<ProgressBar>();
        _vip = transform.GetText("Experience/ExpAdd/VIP");

        _vip.text = I18NManager.Get("Battle_VIP");
    }

    void Start()
    {
        var finishBtn = transform.Find("FinishBtn").GetComponent<Button>();
        if (finishBtn != null)
        {
            finishBtn.onClick.AddListener(delegate()
            {
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_FINISH));
            });
        }

        var tryAgainBtn = transform.Find("TryAgainBtn").GetComponent<Button>();
        if (tryAgainBtn != null)
        {
            tryAgainBtn.onClick.AddListener(delegate()
            {
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_RESTART));
            });
        }
    }

    public void SetData(VisitBattleResultData data, PlayerModel playerModel)
    {
        transform.Find("StarAndGrade/Star").GetComponent<VisitStarComponent>().ShowStar(data.Star);
        // transform.Find("StarAndGrade/Text").GetComponent<Text>().text = "应援热度：<b> " + data.Cap + "</b>";
        transform.Find("StarAndGrade/Text/Text").GetComponent<Text>().text = data.Cap.ToString();
        var propContainer = transform.Find("Reward/Props");
        var cardContainer = transform.Find("Cards");
        for (int i = 0; i < data.RewardList.Count; i++)
        {
            GameObject item = InstantiatePrefab("VisitBattle/FinalEstimate/VisitBattleRewardItem");
            item.transform.SetParent(propContainer, false);
            
            DrawActivityDropItemVo extReward;
            data.DrawActivityDropItemDict.TryGetValue(i, out extReward);
            
            item.transform.gameObject.AddComponent<VisitBattleRewardItem>().SetData(data.RewardList[i], extReward);
            item.GetComponent<ItemShowEffect>().OnShowEffect(0.3f + i * 0.2f);
        }

        PlayerVo player = playerModel.PlayerVo;

        _levelText.text = "Lv." + player.Level;
        _expAddText.text = "+" + data.Exp + " exp";

        var isOnVip = GlobalData.PlayerModel.PlayerVo.IsOnVip;
        _vip.gameObject.SetActive(isOnVip);

        _userNameText.text = I18NManager.Get("Battle_Name", player.UserName);

        int lastNeedExp = player.NeedExp;
        _expText.text = player.CurrentLevelExp + "/" + player.NeedExp;

        progressBar.DeltaX = 0;
        progressBar.Progress = (int) ((float) player.CurrentLevelExp / player.NeedExp * 100);

        bool isLevelup = playerModel.AddExp(data.Exp);

        int rate = (int) ((float) player.CurrentLevelExp / player.NeedExp * 100);

        if (isLevelup)
        {
            if (player.Level >= 100)
            {
                _expText.text = "MAX";
            }
            else
            {
                DOTween.To(() => progressBar.Progress, x => progressBar.Progress = x, 100, 0.3f).SetDelay(0.9f)
                        .onComplete =
                    () =>
                    {
                        _expText.text = player.CurrentLevelExp + "/" + player.NeedExp;
                        progressBar.DeltaX = 0;
                        progressBar.Progress = 0;
                        DOTween.To(() => progressBar.Progress, x => progressBar.Progress = x, rate, 0.3f)
                            .SetDelay(1.3f);
                        Util.TweenTextNum(_expText, 0.3f, player.CurrentLevelExp, "", "/" + player.NeedExp);
                        _levelText.text = "Lv." + player.Level;
                    };

                Util.TweenTextNum(_expText, 0.3f, lastNeedExp, "", "/" + lastNeedExp);
            }
        }
        else
        {
            if (player.Level >= 100)
            {
                _expText.text = "MAX";
            }
            else
            {
                DOTween.To(() => progressBar.Progress, x => progressBar.Progress = x, rate, 0.3f).SetDelay(0.9f);
                Util.TweenTextNum(_expText, 0.3f, player.CurrentLevelExp, "", "/" + player.NeedExp);
            }
        }

        for (int i = 0; i < data.UserCards.Count; i++)
        {
            GameObject item = InstantiatePrefab("Battle/FinalEstimate/RewardHeroCard");
            item.transform.SetParent(cardContainer, false);
            item.AddComponent<ItemShowEffect>().OnShowEffect(1 + i * 0.2f);

            var cardData = GlobalData.CardModel.GetUserCardById(data.UserCards[i].CardId);
            item.transform.GetComponent<RewardHeroCard>().SetData(cardData, data.CardExp);
        }

        StartCoroutine(StarRotation());
    }

    IEnumerator StarRotation()
    {
        var smallStar = transform.Find("Experience/ProgressBar/Star/smallStar").GetComponent<RectTransform>();
        var bigStar = transform.Find("Experience/ProgressBar/Star/bigStar").GetComponent<RectTransform>();
        while (true)
        {
            smallStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            bigStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            yield return null;
        }
    }

    private void Update()
    {
        var _starRectTra = transform.Find("Experience/ProgressBar/Star").GetComponent<RectTransform>();
        var Mask = transform.Find("Experience/ProgressBar/Mask").GetComponent<RectTransform>().GetWidth();
        _starRectTra.localPosition = new Vector3(Mask, 0, 0);
    }
}