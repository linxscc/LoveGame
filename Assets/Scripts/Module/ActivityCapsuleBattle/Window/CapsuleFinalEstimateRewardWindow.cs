using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleFinalEstimateRewardWindow : Window
{
    private Text _userNameText; //用户名
    private Text _levelText; //等级Text
    private Text _expAddText; //经验增加Text  
    private Text _expText; //经验Text
    private ProgressBar _progressBar;
    private Text _vip; //vip文案展示
    private Button _finishBtn;

   
    
    
    private void Awake()
    {
        _userNameText = transform.GetText("Experience/UserNameText");
        _levelText = transform.GetText("Experience/LevelText");
        _expAddText = transform.GetText("Experience/ExpAdd/Text");
        _expText = transform.GetText("Experience/ExpText");
        _vip = transform.GetText("Experience/ExpAdd/VIP");
        _vip.text = I18NManager.Get("Battle_VIP");
        _finishBtn = transform.GetButton("FinishBtn");
        _progressBar = transform.Find("Experience/ProgressBar").GetComponent<ProgressBar>();
        _finishBtn.onClick.AddListener(() =>
        {
            base.Close();
            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_FINISH));
        });      
    }

    public void SetData(CapsuleBattleResultData data, PlayerModel playerModel)
    {
        transform.Find("StarAndGrade/Star").GetComponent<StarComponent>().ShowStar(data.Star);
        transform.Find("StarAndGrade/Text/Text").GetComponent<Text>().text = data.Cap.ToString();
        var propContainer = transform.Find("Reward/Props");
        var cardContainer = transform.Find("Cards");
       
       
        for (int i = 0; i < data.RewardList.Count; i++)
        {
            GameObject item = InstantiatePrefab("Battle/Prefabs/RewardItem");
            item.transform.SetParent(propContainer, false);
            RewardItem rewardItem = item.transform.gameObject.AddComponent<RewardItem>();
            DrawActivityDropItemVo extReward;
            data.DrawActivityDropItemDict.TryGetValue(i, out extReward);
            rewardItem.SetData(data.RewardList[i], extReward);
            item.GetComponent<ItemShowEffect>().OnShowEffect(0.3f + i * 0.2f);
        }

        SetProgress(data, playerModel);
        
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

 

    private void SetProgress(CapsuleBattleResultData data,PlayerModel playerModel)
    {
        var player = playerModel.PlayerVo;  
        _levelText.text = "Lv." + player.Level;
        _expAddText.text = "+" + data.Exp + " exp";
        
        var isOnVip = GlobalData.PlayerModel.PlayerVo.IsOnVip;
        _vip.gameObject.SetActive(isOnVip);
        _userNameText.text = I18NManager.Get("Battle_Name", player.UserName);
        int lastNeedExp = player.NeedExp;
        _expText.text = player.CurrentLevelExp + "/" + player.NeedExp;
        _progressBar.DeltaX = 0;
        _progressBar.Progress = (int) ((float) player.CurrentLevelExp / player.NeedExp * 100);
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
                DOTween.To(() => _progressBar.Progress, x => _progressBar.Progress = x, 100, 0.3f).SetDelay(0.9f)
                        .onComplete =
                    () =>
                    {
                        _expText.text = player.CurrentLevelExp + "/" + player.NeedExp;
                        _progressBar.DeltaX = 0;
                        _progressBar.Progress = 0;
                        DOTween.To(() => _progressBar.Progress, x => _progressBar.Progress = x, rate, 0.3f)
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
                DOTween.To(() => _progressBar.Progress, x => _progressBar.Progress = x, rate, 0.3f).SetDelay(0.9f);
                Util.TweenTextNum(_expText, 0.3f, player.CurrentLevelExp, "", "/" + player.NeedExp);
            }
        }
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
        var starRectTra = transform.Find("Experience/ProgressBar/Star").GetComponent<RectTransform>();
        var mask = transform.Find("Experience/ProgressBar/Mask").GetComponent<RectTransform>().GetWidth();
        starRectTra.localPosition = new Vector3(mask, 0, 0);
    }

    protected override void OnClickOutside(GameObject go)
    {
    }
}
