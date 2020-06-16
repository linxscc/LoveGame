using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class BattleIntroductionPopup : Window
    {
        private Button _startWorkBtn;

        private Button _playOnceBtn;

        private Button _playTimesBtn;
        private LevelVo _levelVo;

        private Image _tipImage;
        private RepeatedField<LevelBuyRulePB> _levelBuyRules;

        private void Awake()
        {
            AssetManager.Instance.LoadModuleAtlas(ModuleConfig.MODULE_BATTLE);

            _startWorkBtn = transform.Find("BtnContainer/StartWorkBtn").GetComponent<Button>();
            _playOnceBtn = transform.Find("BtnContainer/PlayOnceBtn").GetComponent<Button>();
            _playTimesBtn = transform.Find("BtnContainer/PlayTimesBtn").GetComponent<Button>();
            _tipImage = transform.Find("BtnContainer/TipImage").GetComponent<Image>();

            _startWorkBtn.onClick.AddListener(() => { OnStartBattle(0); });
            _playOnceBtn.onClick.AddListener(() => { OnStartBattle(1); });
            _playTimesBtn.onClick.AddListener(() =>
            {
                int chanllangeTimes = GlobalData.PlayerModel.PlayerVo.Energy / _levelVo.CostEnergy;
                if (chanllangeTimes > 5)
                    chanllangeTimes = 5;
                OnStartBattle(chanllangeTimes);
            });
        }

        private void OnStartBattle(int num)
        {
            if (_levelVo.Hardness == GameTypePB.Difficult && _levelVo.ChallangeTimes >= _levelVo.MaxChallangeTimes)
            {
                BuyCount();
                return;
            }

            if (_levelVo.CostEnergy > GlobalData.PlayerModel.PlayerVo.Energy)
            {
                FlowText.ShowMessage(I18NManager.Get("MainLine_BattleIntroductionPopupHint1"));
                //  FlowText.ShowMessage("体力不足");
                return;
            }

            Close();
            ClientTimer.Instance.DelayCall(()=>
            {
                EventDispatcher.TriggerEvent(EventConst.EnterBattle, num, _levelVo);
            }, 0.2f);
           
        }

        public void Init(LevelVo vo, RepeatedField<LevelBuyRulePB> levelBuyRules)
        {
            _levelVo = vo;
            _levelBuyRules = levelBuyRules;

            transform.Find("DescBg/IntroductionTxt").GetComponent<Text>().text = vo.LevelDescription;
            // transform.Find("ScoreTxt").GetComponent<Text>().text = "应援热度：" + vo.Score;
            transform.Find("ScoreTxt").GetComponent<Text>().text =
                I18NManager.Get("MainLine_BattleIntroductionPopupScore", vo.Score);
            transform.Find("Title/Text").GetComponent<Text>().text = vo.LevelName;

            Text powerUseTxt = transform.Find("PowerUse/PowerUseTxt").GetComponent<Text>();
            //  powerUseTxt.text = "消耗体力：" + vo.CostEnergy;
            powerUseTxt.text = I18NManager.Get("MainLine_BattleIntroductionPopupPower", vo.CostEnergy);

            Image star1 = transform.Find("StarContainer/Star1").GetComponent<Image>();
            Image star2 = transform.Find("StarContainer/Star2").GetComponent<Image>();
            Image star3 = transform.Find("StarContainer/Star3").GetComponent<Image>();

            Color lightColor = Color.white;
            Color grayColor = new Color(160 / 255.0f, 160 / 255.0f, 160 / 255.0f);

            _playOnceBtn.gameObject.SetActive(false);
            _playTimesBtn.gameObject.SetActive(false);
            _tipImage.gameObject.Hide();

            if (vo.CurrentStar == 0)
            {
//                _tipImage.gameObject.Show();

                star1.color = grayColor;
                star2.color = grayColor;
                star3.color = grayColor;
            }
            else if (vo.CurrentStar == 1)
            {
                star1.color = lightColor;
                star2.color = grayColor;
                star3.color = grayColor;
            }
            else if (vo.CurrentStar == 2)
            {
                star1.color = lightColor;
                star2.color = lightColor;
                star3.color = grayColor;
                _tipImage.gameObject.Hide();
                _playOnceBtn.gameObject.SetActive(true);
                _playTimesBtn.gameObject.SetActive(true);
            }
            else
            {
                _playOnceBtn.gameObject.SetActive(true);
                _playTimesBtn.gameObject.SetActive(true);
                _tipImage.gameObject.Hide();
            }


            HandleDrops();
            SetChanllangeTimes();
        }

        public void Refresh()
        {
            if (_levelVo.Hardness == GameTypePB.Difficult)
            {
                SetChanllangeTimes();
            }
        }

        private void HandleDrops()
        {
            Transform container = transform.Find("DropProps");

            if (container.childCount > 0)
            {
                for (int i = 0; i < container.childCount; i++)
                {
                    Destroy(container.GetChild(i));
                }
            }

            for (int i = 0; i < _levelVo.DropsList.Count; i++)
            {
                GameObject item = InstantiatePrefab("MainLine/Prefabs/DropItem");
                item.transform.SetParent(container, false);
                item.transform.GetComponent<DropItem>().SetData(_levelVo.DropsList[i]);
                item.GetComponent<ItemShowEffect>().OnShowEffect(i * 0.2f);
            }
        }

        private void SetChanllangeTimes()
        {
            _playTimesBtn.transform.Find("Text").GetComponent<Text>().text =
                I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes1");

            //简单
            Transform tips = transform.Find("Tips");
            tips.gameObject.Hide();
//            if (_levelVo.Hardness == GameTypePB.Ordinary)
//            {
//                tips.gameObject.Hide();
//                if (GlobalData.PlayerModel.PlayerVo.Energy < _levelVo.CostEnergy * 5)
//                {
//                    _playTimesBtn.gameObject.SetActive(false);
//                }            
//            }

//            int curTimes = _levelVo.MaxChallangeTimes - _levelVo.ChallangeTimes;


            int chanllangeTimes = GlobalData.PlayerModel.PlayerVo.Energy / _levelVo.CostEnergy;
            if (chanllangeTimes > 5)
                chanllangeTimes = 5;

            _playTimesBtn.gameObject.SetActive(chanllangeTimes > 1 && _levelVo.CurrentStar > 1);

//            _playTimesBtn.gameObject.SetActive(chanllangeTimes > 1 && _levelVo.CurrentStar > 1 && GlobalData.PlayerModel.PlayerVo.Energy >= _levelVo.CostEnergy * 2 );

            if (chanllangeTimes > 0)
            {
//                string[] timeArr = new[]
//                {
//                    I18NManager.Get("Common_One"), I18NManager.Get("Common_Two"), I18NManager.Get("Common_Three"),
//                    I18NManager.Get("Common_Four"), I18NManager.Get("Common_Five")
//                };

                _playTimesBtn.transform.Find("Text").GetComponent<Text>().text =
                    I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes", chanllangeTimes);
            }

//            tips.gameObject.Show();

            //tips.Find("Text").GetComponent<Text>().text =
            //    "今日剩余应援次数：" + curTimes + "/" +
            //    _levelVo.MaxChallangeTimes;
//            tips.Find("Text").GetComponent<Text>().text = I18NManager.Get("MainLine_BattleIntroductionPopupTips", curTimes, _levelVo.MaxChallangeTimes);


//            PointerClickListener.Get(tips.gameObject).onClick = go =>
//            {
//                if (curTimes == 0)
//                {
//                    BuyCount();
//                }
//                else
//                {
//                    FlowText.ShowMessage(I18NManager.Get("MainLine_BattleIntroductionPopupHint2"));
//                    //FlowText.ShowMessage("还有未使用的应援机会");
//                }
//            };
        }

        private void BuyCount()
        {
            if (_levelBuyRules.Count <= _levelVo.BuyCount)
            {
                FlowText.ShowMessage(I18NManager.Get("MainLine_BattleIntroductionPopupHint3"));
                return;
            }

            int cost = _levelBuyRules[_levelVo.BuyCount].Gem;
            int times = _levelBuyRules.Count - _levelVo.BuyCount;

            PopupManager.ShowConfirmWindow(I18NManager.Get("MainLine_BattleIntroductionPopupHint4", cost, times),
                I18NManager.Get("MainLine_BattleIntroductionPopupHint5")).WindowActionCallback = evt =>
                //PopupManager.ShowConfirmWindow("应援机会已用完\n是否花费"+cost+"星钻增加3次应援机会\n(还可购买"+times+"次)", "购买应援机会").WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                    EventDispatcher.TriggerEvent(EventConst.BuyLevelCount, _levelVo);
            };
        }
    }
}