using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Common;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class VisitBattleIntroductionPopup : Window
    {
        private Button _startWorkBtn;

        private Button _playOnceBtn;

        private Button _playTimesBtn;
        private VisitLevelVo _levelVo;

        private Image _tipImage;
        string[] timeArr;

        //最大扫荡次数（5次）
        int _maxTime = 5;


        Button _addTime;
        private void Awake()
        {
            AssetManager.Instance.LoadModuleAtlas(ModuleConfig.MODULE_BATTLE);
            //            CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this.);
            _startWorkBtn = transform.Find("BtnContainer/StartWorkBtn").GetComponent<Button>();
            _playOnceBtn = transform.Find("BtnContainer/PlayOnceBtn").GetComponent<Button>();
            _playTimesBtn = transform.Find("BtnContainer/PlayTimesBtn").GetComponent<Button>();
            _tipImage = transform.Find("BtnContainer/TipImage").GetComponent<Image>();

            _startWorkBtn.onClick.AddListener(() => {
                OnStartBattle(0);
            });
            _playOnceBtn.onClick.AddListener(() => {
                OnStartBattle(1);
            });
            _playTimesBtn.onClick.AddListener(() =>
            {
                // int curTimes = _levelVo.MaxChallangeTimes - _levelVo.ChallangeTimes;
                //OnStartBattle(curTimes);


                //int curletfTimes = _maxTime - _levelVo.CurChallengeTime;
                int curletfTimes = VisitVo.MaxSingleVisitTime - _levelVo.CurChallengeTime;
                if (curletfTimes > _maxTime)
                    curletfTimes = _maxTime;
                OnStartBattle(curletfTimes);
            });

            timeArr = new[] {
                I18NManager.Get("Common_One"),
                I18NManager.Get("Common_Two"),
                I18NManager.Get("Common_Three"),
                I18NManager.Get("Common_Four"),
                I18NManager.Get("Common_Five"),
                I18NManager.Get("Common_Six"),
                I18NManager.Get("Common_Seven"),
                I18NManager.Get("Common_Eight"),
                I18NManager.Get("Common_Nine"),
                I18NManager.Get("Common_Ten")};

            //简单
            _addTime = transform.Find("Tips/AddBtn").GetButton();
            _addTime.onClick.AddListener(()=> {

                //if (_visitVo.CurWeather == VISIT_WEATHER.Fine)
                //{
                //    FlowText.ShowMessage(I18NManager.Get("Visit_CurBestWeather"));
                //}
                //else
                //{
                Hide();
                ShowResetWindow();
               // }
            });

            _playOnceBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes", 1);

        }


        private void ShowResetWindow()
        {
           int resetCost =   _visitVo.GetResetCost(_levelVo.BuyCount + 1);

            string showText = I18NManager.Get("Visit_Level_BugTips", resetCost);
            PopupManager.ShowConfirmWindow(showText).WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    EventDispatcher.TriggerEvent(EventConst.VisitLevelResetLevelTime, _levelVo.LevelId);
                    //  Close();
                }
                Show();
            };
        }

        private void Start()
        {
            ClientData.LoadItemDescData(null);
            ClientData.LoadSpecialItemDescData(null);
        }
        private void OnDestroy()
        {
            ClientData.Clear();
        }

        private bool CheckIsCanStartBattle(int num)
        {
            //int curletfTimes = _maxTime - _levelVo.CurChallengeTime;
            //if(curletfTimes<=0)
            //{
            //    FlowText.ShowMessage(I18NManager.Get("Visit_TimeNotEnough"));
            //    return false;
            //}

            if(num==0)
            {
                num = 1;
            }

            if(num>_visitVo.MaxVisitTime-_visitVo.CurVisitTime)//探班次数不足
            {
                if(_visitVo.CurWeather!=VISIT_WEATHER.Fine)
                {
                    string showText = I18NManager.Get("Visit_TimeNotEnoughAndEnterWeater");
                    PopupManager.ShowConfirmWindow(showText).WindowActionCallback = evt =>
                    {
                        if (evt == WindowEvent.Ok)
                        {
                            EventDispatcher.TriggerEvent(EventConst.VisitLevelItemGotoWeather);
                            Close();
                        }
                        Show();
                    };
           
                }
                else
                {
                    string showText = I18NManager.Get("Visit_TimeNotEnough");
                    PopupManager.ShowAlertWindow(showText).WindowActionCallback = evt =>
                    {
                        Show();
                    };
                }
                return false;
            }

            if(num > VisitVo.MaxSingleVisitTime - _levelVo.CurChallengeTime)
            {
                ShowResetWindow();
                return false;
            }


            //if(GlobalData.PlayerModel.PlayerVo.EncourageEnergy < 5 * num)
            //{
            //    FlowText.ShowMessage(I18NManager.Get("Visit_PowerNotEnough"));
            //    return false;
            //}

            return true;
      
        }

        private void OnStartBattle(int num)
        {
            if (!CheckIsCanStartBattle( num))
            {
                return;
            }
            Close();
            EventDispatcher.TriggerEvent(EventConst.EnterVisitBattle, num, _levelVo.LevelId);
        }
        VisitVo _visitVo;
        public void Init(VisitLevelVo vo, VisitVo visitVo)
        {
            _levelVo = vo;
            _visitVo = visitVo;
            //_maxTime = VisitVo.MaxSingleVisitTime;
            transform.Find("DescBg/IntroductionTxt").GetComponent<Text>().text = vo.LevelDescription;
            transform.Find("ScoreTxt").GetComponent<Text>().text = I18NManager.Get("Visit_VisitScore") + vo.Score;
            transform.Find("Title/Text").GetComponent<Text>().text = vo.LevelName;

            //Text powerUseTxt = transform.Find("PowerUse/PowerUseTxt").GetComponent<Text>();
            //powerUseTxt.text = "消耗体力：" + vo.CostEnergy;

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
                _tipImage.gameObject.Show();

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
            if (_levelVo.Hardness == GameTypePB.Visiting)
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
                GameObject item = InstantiatePrefab("Visit/Prefabs/Item/VisitDropItem");
                item.transform.SetParent(container, false);
                item.transform.GetComponent<VisitDropItem>().SetData(_levelVo.DropsList[i]);
                item.GetComponent<ItemShowEffect>().OnShowEffect(i * 0.2f);
            }
        }

        private void SetChanllangeTimes()
        {
            Transform tips = transform.Find("Tips");
            //Debug.LogWarning("MaxSingleVisitTime:" + VisitVo.MaxSingleVisitTime);
            int curletfTimes = VisitVo.MaxSingleVisitTime - _levelVo.CurChallengeTime;
            int curPlayTimesLeftCount = curletfTimes;

            if (curPlayTimesLeftCount > _maxTime)
                curPlayTimesLeftCount = _maxTime;
            

            if (curletfTimes > 10)
                curletfTimes = 10;
       
            if (curPlayTimesLeftCount > 1)
            {
                if(_levelVo.CurrentStar>=2)
                {
                    _playTimesBtn.gameObject.Show();
                }
                else
                {
                    _playTimesBtn.gameObject.Hide();
                }
              //  _playTimesBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes", timeArr[curletfTimes - 1]);
                _playTimesBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes", curPlayTimesLeftCount);
                _addTime.gameObject.Hide();
            }
            else if(curletfTimes==1)
            {
                _playTimesBtn.gameObject.Hide();
                _addTime.gameObject.Hide();
            }
            else
            {
                _playTimesBtn.gameObject.Hide();
                _addTime.gameObject.Show();
            }

          //  int maxTime= GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.VISITING_LEVEL_MAX_TIMES);
          //  Debug.LogError(maxTime);
            
            transform.Find("Tips/Text").GetComponent<Text>().text=I18NManager.Get("Visit_BattleIntroductionPopupTips",
                _visitVo.CurWeatherName, curletfTimes, VisitVo.MaxSingleVisitTime);
            tips.gameObject.Show();     
        }


    }
}