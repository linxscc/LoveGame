using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Componets;
using DataModel;
using DG.Tweening;
using game.tools;
using Google.Protobuf.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class ActivityChooseView : View
    {
        private Transform _activityList;

        //private Button _changeActivityBtn;
        private Text _autoChangeTips;

        private Text _costtime;

        //private EncourageActRuleVo _encouragevo;
        private long _nexttime;

        private TimerHandler _handle;

        //private List<EncourageActDoneRuleVo> _doneEncourageRules;
        private List<UserEncourageActVo> _useencourageActVos;

        //private bool _hassend = false;

        private GameObject _actAni;

        private List<GameObject> _aniList;
        private SupporterActivityModel _supporterActivityModel;

        private Transform _addSign; //加号
        private bool _lock = false;
        private int lastcost = 0;

        private Transform _curChooseTransform;


        private void Awake()
        {
            _activityList = transform.Find("Viewport/Content");
            //_changeActivityBtn = transform.Find("ChangeActivityBtn").GetComponent<Button>();
            _autoChangeTips = transform.Find("TipsBg/Tips").GetComponent<Text>();
            _costtime = transform.Find("Icon/Num").GetComponent<Text>();
            //_changeActivityBtn.onClick.AddListener(ChangeAcitvity);
//            _addperHour = transform.Find("SupporterEnergy/AddPerHour").GetComponent<Text>();
            _addSign = transform.Find("SupporterEnergy/CostNum/AddSign/OnClick");
            _aniList = new List<GameObject>();

            for (int i = 0; i < _activityList.childCount; i++)
            {
                _activityList.GetChild(i).gameObject.Hide();
            }

            PointerClickListener.Get(_addSign.gameObject).onClick = BuyEncouragePower;

            PointerClickListener.Get(transform.Find("Icon/Icon").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_BuyInner));
            };
        }


        private void BuyEncouragePower(GameObject go)
        {
            SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_BUYENCOURAGEPOWER));
        }


        private void ChangeAcitvity()
        {
            //FlowText.ShowMessage("更换活动");
            //加入三个应援活动都完成了，就提示“已完成所有应援活动”
            int count = 0;
            foreach (var v in _supporterActivityModel.UserEncourageActVos)
            {
                if (v.AwardState == 1)
                {
                    count++;
                }
            }

            if (count >= 3)
            {
                FlowText.ShowMessage(I18NManager.Get("SupporterActivity_HasDoneAct"));
                return;
            }

            SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_REFRESH, Message.MessageReciverType.CONTROLLER));
        }

        public void SetData(List<UserEncourageActVo> vo, SupporterActivityModel supporterActivityModel, int refrhtime,
            long nexttime = 0)
        {
            _supporterActivityModel = supporterActivityModel;
            _useencourageActVos = vo;
            _lock = false;
            int notstartactcount = 0;
            for (int i = 0; i < _activityList.childCount; i++)
            {
                if (vo.Count < 1 || vo.Count < i + 1)
                {
                }
                else
                {
                    SetActivityItemData(_activityList.GetChild(i), vo[i]);
                    if (vo[i].StartState == 0)
                    {
                        PointerClickListener.Get(_activityList.GetChild(i).gameObject).parameter = vo[i];
                        PointerClickListener.Get(_activityList.GetChild(i).gameObject).onClick = GoToFansModule;
                        notstartactcount++;
                    }
                    else
                    {
                        PointerClickListener.Get(_activityList.GetChild(i).gameObject).onClick = null;
                        PointerClickListener.Get(_activityList.GetChild(i).gameObject).onClick = null;
                    }
                }
            }

            refreshCostGoldNum(refrhtime);


            //_costGlod.text = "" + lastcost;

            if (_handle != null)
            {
                ClientTimer.Instance.RemoveCountDown(_handle);
            }

            _handle = ClientTimer.Instance.AddCountDown("UpdateAutoChange", Int64.MaxValue, 1f, UpdateAutoChange, null);
            if (nexttime != 0)
            {
                _supporterActivityModel.NextTime = nexttime;
            }

//            SetSupporterEnergy();
        }

        public void refreshCostGoldNum(int refreshTime)
        {
            lastcost = _supporterActivityModel.GetRefreshCost(refreshTime + 1).Gold;
            for (int i = 0; i < _activityList.childCount; i++)
            {
                var costGoldNum = _activityList.GetChild(i).Find("NotSupport/Icon/Num")
                    .GetComponent<Text>();
                costGoldNum.text = lastcost.ToString();
            }
        }

        private void UpdateAutoChange(int time)
        {
            //缺少一个逻辑，时间到的时候要变成可领取状态。
            var nexttimeleft = _supporterActivityModel.NextTime - ClientTimer.Instance.GetCurrentTimeStamp();
            if (nexttimeleft > 0)
            {
                _autoChangeTips.text = I18NManager.Get("SupporterActivity_LaterRefresh", DateUtil.GetTimeFormat4(nexttimeleft));
            }
            else
            {
                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GETMYENCOURAGE));
            }

            //ClientTimer.Instance.AddCountDown("UpdateLeftTime", Int64.MaxValue, 1f, UpdateLeftTime, null);
            UpdateLeftTime();
        }

        private void UpdateLeftTime()
        {
            for (int i = 0; i < _activityList.childCount; i++)
            {
                if (_useencourageActVos.Count == 0 || i > _useencourageActVos.Count)
                {
                    return;
                }


                var time = _supporterActivityModel.EncourageRuleDic[_useencourageActVos[i].ActId].NeedTime * 60 *
                           1000 + _useencourageActVos[i].AcceptTime -
                           ClientTimer.Instance.GetCurrentTimeStamp();
                if (_useencourageActVos[i].StartState != 0 && time > 0)
                {
                    var trantime = _activityList.GetChild(_useencourageActVos[i].PosIndex).Find("HasSupport/ActivityState/DoingActivity/TimeLabel")
                        .GetComponent<Text>();
                    SetTimeLabel(trantime, time);
                }

                if (_useencourageActVos[i].StartState != 0 && !_useencourageActVos[i].CanReceiveAward &&
                    _useencourageActVos[i].AwardState == 0 && time < 0 && !_lock)
                {
                    _lock = true;
                    SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GETMYENCOURAGE));
                }
            }
        }

        public void SetLeftTimes(int leftTime)
        {
            _costtime.text = I18NManager.Get("SupporterActivity_LeftTime")+ leftTime;
        }

        private void SetTimeLabel(Text timelabel, long time)
        {
            if (time > 0)
            {
                timelabel.text = I18NManager.Get("SupporterActivity_Left") + DateUtil.GetTimeFormat4(time);
            }
        }

        public void GoToFansModule()
        {
            GoToFansModule(_activityList.GetChild(0).gameObject);
        }

        private void GoToFansModule(GameObject go)
        {
            var userEng = (UserEncourageActVo) PointerClickListener.Get(go).parameter;
            var needlevel = _supporterActivityModel.EncourageRuleDic[userEng.ActId].DepartmentLevel;
            //Debug.LogError(needlevel);
            if (userEng.StartState==1)
            {
                return;
            }
            
            
            if (GlobalData.PlayerModel.PlayerVo.Level >= needlevel)
            {
                _curChooseTransform = go.transform.Find("NotSupport");
                Debug.Log("_curChooseTransform"+_curChooseTransform.parent.GetSiblingIndex()+"userEng"+userEng.PosIndex);
                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_SHOW_FANSDETAIL, userEng));
            }
            else
            {
                FlowText.ShowMessage(I18NManager.Get("SupporterActivity_LevelNoEnough"));
            }
        }

        public void RefreshStartItemData(UserEncourageActVo vo,bool isReceiveAwrad=false)
        {
            if (isReceiveAwrad)
            {
                PointerClickListener.Get(_activityList.GetChild(vo.PosIndex).gameObject).parameter = vo;
                PointerClickListener.Get(_activityList.GetChild(vo.PosIndex).gameObject).onClick = GoToFansModule;
            }
            SetActivityItemData(_activityList.GetChild(vo.PosIndex), vo);
        }

        private void SetActivityItemData(Transform tran, UserEncourageActVo vo) //,EncourageActDoneRuleVo donerule
        {
            var encouragevo = _supporterActivityModel.EncourageRuleDic[vo.ActId];

            var notsupporttran = tran.Find("NotSupport");
            var hassupporttran = tran.Find("HasSupport");

            var hasActBg = hassupporttran.Find("HasActBG");

            hasActBg.Find("SupportAct1").gameObject.SetActive(encouragevo.MovieId == 1); //取决于要不要显示
            hasActBg.Find("SpineRoot").gameObject.SetActive(encouragevo.MovieId != 1);

            _actAni = encouragevo.MovieId == 1
                ? hasActBg.Find("SupportAct1").gameObject
                : hasActBg.Find("SpineRoot/SpineSkeletonGraphic").gameObject;


            tran.gameObject.Show();
            SetActItemStatePos(hassupporttran, notsupporttran, vo.StartState == 0, tran.GetSiblingIndex() == 1);

            if (vo.StartState == 0)
            {
                SetNotSupportData(notsupporttran, encouragevo, vo);
                SetDefaultState(hassupporttran, false);
                notsupporttran.SetAsLastSibling();
                _actAni.transform.Find("02_scene")?.GetComponent<MovingTween>()?.PaseTween();
                if (encouragevo.MovieId != 1)
                {
                    SetSupporterSpine(_actAni, GetAni(encouragevo.MovieId));
                }
            }
            else
            {
                //变成应援活动的时候才需要切换动画
                if (vo.NeedToChangeAni)
                {
                    _supporterActivityModel.GetUserActVo(vo.Id).NeedToChangeAni = false;
                    ChangeActTween(notsupporttran, hassupporttran, tran.GetSiblingIndex() == 1, () =>
                    {
                        //动画到某阶段的时候突然间改变           
                        SetHasSupportData(hassupporttran, vo, encouragevo);
                        SetDefaultState(notsupporttran, true);
                        hassupporttran.SetAsLastSibling();
                    });
                }
                else
                {
                    SetHasSupportData(hassupporttran, vo, encouragevo);
                    SetDefaultState(notsupporttran, true);
                    hassupporttran.SetAsLastSibling();
                }
            }
        }

        private void SetNotSupportData(Transform tran, EncourageActRuleVo encouragevo, UserEncourageActVo uservo)
        {
            var nametran = tran.Find("SupporterActivityName/Text");
            nametran.gameObject.Show();
            var name = nametran.GetComponent<Text>();
            var timetran = tran.Find("SandGlasss/TimeLabel");
            timetran.gameObject.Show();
            var timeLabel = timetran.GetComponent<Text>();
            name.text = encouragevo.Title;
            timeLabel.text = I18NManager.Get("SupporterActivity_Time",encouragevo.NeedTime / 60);//"时间：" +  + "小时";  

            var rewardList = tran.Find("RewardList");
            var changeBtn = tran.Find("ChangeBtn").GetButton();
            var costText = tran.Find("Icon/Num").GetText();
            costText.text = lastcost.ToString();

            changeBtn.onClick.RemoveAllListeners();
            changeBtn.onClick.AddListener(() =>
            {
                //FlowText.ShowMessage("更换活动协议！");
                _curChooseTransform = tran;
                if (uservo.StartState==0)
                {
                    SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_REFRESH,
                        Message.MessageReciverType.CONTROLLER, uservo)); 
                }
            });

            //可能三种情况：1.只出现粉丝2.只出现钻石。3.只出现道具。
            for (int i = 0; i < rewardList.childCount; i++)
            {
                rewardList.GetChild(i).gameObject.Hide();
            }

            //刷新的时候就会出现BUG了
            RepeatedField<AwardPB> enactaward=new RepeatedField<AwardPB>();
            enactaward.Clear();
            
            if (encouragevo.RandomFansNum>0)
            {
                enactaward.Add(new AwardPB() {Num = encouragevo.RandomFansNum, Resource = ResourcePB.Fans, ResourceId = 1});
            }
            enactaward.Add(encouragevo.Awards);
            
            for (int i = 0; i < enactaward.Count; i++)
            {
                rewardList.GetChild(i).gameObject.Show();
                SetPropData(rewardList.GetChild(i), enactaward[i]);
                
                               
            }
            
        }

        public void RefreshOneActivity(UserEncourageActVo uservo)
        {
            var encouragevo = _supporterActivityModel.EncourageRuleDic[uservo.ActId];

            //Debug.LogError(_curChooseTransform.parent.GetSiblingIndex());
            var tranparent = _curChooseTransform.parent;
            var hassupportertran = tranparent.Find("HasSupport");
            PointerClickListener.Get(_activityList.GetChild(tranparent.GetSiblingIndex()).gameObject).parameter =
                uservo;


            RefreshActTween(_curChooseTransform, hassupportertran, tranparent.GetSiblingIndex() == 1, () =>
            {
                var hasActBg = hassupportertran.Find("HasActBG");

                hasActBg.Find("SupportAct1").gameObject.SetActive(encouragevo.MovieId == 1); //取决于要不要显示
                hasActBg.Find("SpineRoot").gameObject.SetActive(encouragevo.MovieId != 1);

                var chooseAni = encouragevo.MovieId == 1
                    ? hasActBg.Find("SupportAct1").gameObject
                    : hasActBg.Find("SpineRoot/SpineSkeletonGraphic").gameObject;
                //动画到某阶段的时候突然间改变           
                chooseAni.transform.Find("02_scene")?.GetComponent<MovingTween>()?.PaseTween();
                if (encouragevo.MovieId != 1)
                {
                    SetSupporterSpine(chooseAni, GetAni(encouragevo.MovieId));
                }

                SetNotSupportData(_curChooseTransform, encouragevo, uservo);
            });
        }


        /// <summary>
        /// 已经应援后的状态
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="vo"></param>
        /// <param name="state"></param>
        private void
            SetHasSupportData(Transform tran, UserEncourageActVo vo,
                EncourageActRuleVo rulevo) //,EncourageActDoneRuleVo donerule
        {
            if (rulevo.MovieId == 1)
            {
                _actAni = tran.Find("HasActBG/SupportAct1").gameObject;
            }
            else
            {
                _actAni = tran.Find("HasActBG/SpineRoot/SpineSkeletonGraphic").gameObject;
            }

//        Debug.LogError(vo.StartState);
            tran.Find("SupporterActivityName").gameObject.Show();
            tran.Find("SupporterActivityName/Text").GetComponent<Text>().text = rulevo.Title;
            //tran.Find("NoSupporterBg/SupporterActivityName/Text").GetComponent<Text>().text = rulevo.Title;
            var rulebtn = tran.Find("Rule");
            rulebtn.gameObject.Show();
            var statetran = tran.Find("ActivityState");
            PointerClickListener.Get(rulebtn.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_REWARDTIPS,
                    Message.MessageReciverType.CONTROLLER, rulevo));
            };

            var time = rulevo.NeedTime * 60 * 1000 - (ClientTimer.Instance.GetCurrentTimeStamp() - vo.AcceptTime);

            //时间走完之前，或者非直接完成，都会出现倒计时。以下代码为核心代码！
            if (time > 0 && vo.ImmediateFinishState != 1 && vo.AwardState != 1
            ) //ClientTimer.Instance.GetCurrentTimeStamp(vo.AcceptTime)!=null)
            {
                statetran.GetChild(0).gameObject.SetActive(true);
                statetran.GetChild(1).gameObject.SetActive(false);
                statetran.GetChild(2).gameObject.SetActive(false);
                var speedup = statetran.GetChild(0).Find("SpeedUpActivityBtn").GetComponent<Button>();
                var num = statetran.GetChild(0).Find("SpeedUpActivityBtn/Icon/Num").GetComponent<Text>();
                var timelabel = statetran.GetChild(0).Find("TimeLabel").GetComponent<Text>();
                //Debug.LogError("剩余时间"+ClientTimer.Instance.TransformTime(donerule.Time*60*1000-(ClientTimer.Instance.GetCurrentTimeStamp()-vo.AcceptTime)));
                timelabel.text = I18NManager.Get("SupporterActivity_Left")+ DateUtil.GetTimeFormat4(time);
                num.text = "X" + _supporterActivityModel.GetGemByTime(ClientTimer.Instance.GetCurHour(time));

                speedup.onClick.RemoveAllListeners();
                speedup.onClick.AddListener(() =>
                {
                    _curChooseTransform = tran;
                    SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_DONEIMMEDIATE,
                        Message.MessageReciverType.CONTROLLER, vo));
                });

                //只有在倒计时的时候才要有动画
                _actAni.transform.Find("02_scene")?.GetComponent<MovingTween>()?.PlayTween();

                if (rulevo.MovieId != 1)
                {
                    SetSupporterSpine(_actAni, GetAni(rulevo.MovieId), true);
                }

                return;
            }


            //已完成和可领取的状态都要有动画
            _actAni.transform.Find("02_scene")?.GetComponent<MovingTween>()?.PaseTween();
            if (rulevo.MovieId != 1)
            {
                SetSupporterSpine(_actAni, GetAni(rulevo.MovieId));
            }

            switch (vo.AwardState)
            {
                case 0:
                    statetran.GetChild(0).gameObject.SetActive(false);
                    statetran.GetChild(1).gameObject.SetActive(true);
                    statetran.GetChild(2).gameObject.SetActive(false);
                    var rewardBtn = statetran.GetChild(1).Find("FinishActivityBtn").GetComponent<Button>();
                    rewardBtn.onClick.RemoveAllListeners();
                    rewardBtn.onClick.AddListener(() =>
                    {
                        SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GETREWARD,
                            Message.MessageReciverType.CONTROLLER, vo));
                    });
                    //设置为可领奖状态
                    for (int i = 0; i < _useencourageActVos.Count; i++)
                    {
                        if (_useencourageActVos[i].ActId == vo.ActId)
                        {
                            _useencourageActVos[i].CanReceiveAward = true;
                        }
                    }


                    break;
                case 1:
                    statetran.GetChild(0).gameObject.SetActive(false);
                    statetran.GetChild(1).gameObject.SetActive(false);
                    statetran.GetChild(2).gameObject.SetActive(true);
                    rulebtn.gameObject.Hide();
                    break;
            }
        }


        private void SetPropData(Transform tran, AwardPB award)
        {
            var item = tran.Find("Item").GetComponent<RawImage>();
            var itemName = tran.Find("ItemName").GetComponent<Text>();
            var itemNum = tran.Find("ItemNum").GetComponent<Text>();

            string path = "";
            string name = "";
            if (award.Resource == ResourcePB.Item)
            {
                name = GlobalData.PropModel.GetPropBase(award.ResourceId).Name;
            }
            else
            {
                name = ViewUtil.ResourceToString(award.Resource);
            }

            if (award.Resource == ResourcePB.Gold)
            {
                //  vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
                path = "Prop/particular/" + PropConst.GoldIconId;
            }
            else if (award.Resource == ResourcePB.Gem)
            {
                path = "Prop/particular/" + PropConst.GemIconId;
            }
            else if (award.Resource == ResourcePB.Power)
            {
                path = "Prop/particular/" + PropConst.PowerIconId;
            }
            else if (award.Resource == ResourcePB.Fans)
            {
                //item.texture = ResourceManager.Load<Texture>("Prop/" + 205);
                path = "Prop/" + 900011;
                name = I18NManager.Get("Supporter_Hint9");
            }
            else
            {
                path = "Prop/" + award.ResourceId;
            }

            item.texture = ResourceManager.Load<Texture>(path, ModuleName);
            if (item.texture == null)
            {
                item.texture = ResourceManager.Load<Texture>("Prop/" + 900011, ModuleName);
            }

            itemName.text = name;
            itemNum.text = award.Num.ToString();
        }



        /// <summary>
        /// 开始状态决定下层的某些UI是否显示
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="isStart"></param>
        private void SetDefaultState(Transform tran, bool isStart)
        {
            if (isStart)
            {
                var rewardList = tran.Find("RewardList");
                for (int i = 0; i < rewardList.childCount; i++)
                {
                    rewardList.GetChild(i).gameObject.Hide();
                }

                var timeLabel = tran.Find("SandGlasss/TimeLabel");
                timeLabel.gameObject.Hide();
                var _name = tran.Find("SupporterActivityName/Text");
                _name.gameObject.Hide();
            }
            else
            {
                var statetran = tran.Find("ActivityState");
                var _name = tran.Find("SupporterActivityName");
                _name.gameObject.Hide();
                for (int i = 0; i < statetran.childCount; i++)
                {
                    statetran.GetChild(i).gameObject.Hide();
                }
            }
        }


        /// <summary>
        /// 左右切换的动画
        /// </summary>
        /// <param name="tran1"></param>
        /// <param name="tran2"></param>
        /// <param name="isleft"></param>
        /// <param name="action"></param>
        private void ChangeActTween(Transform tran1, Transform tran2, bool isleft, Action action)
        {
//        Debug.LogError("isleft?"+isleft);
            if (isleft)
            {
                Tweener move1 = tran1.transform.DOLocalMoveX(tran1.localPosition.x - 388, 0.5f).SetEase(Ease.OutSine);
                Tweener move2 = tran2.transform.DOLocalMoveX(tran2.localPosition.x + 388, 0.5f).SetEase(Ease.OutSine);
                Tweener move3 = tran1.transform.DOLocalMoveX(-388, 0.5f).SetEase(Ease.OutSine); //tran1.localPosition.x 
                Tweener move4 = tran2.transform.DOLocalMoveX(-3, 0.5f).SetEase(Ease.OutSine); //tran2.localPosition.x

                DOTween.Sequence().Append(move1).Join(move2).AppendInterval(0.3f).AppendCallback(() => { action(); })
                    .Append(move3).Join(move4);
            }
            else
            {
                Tweener move1 = tran1.transform.DOLocalMoveX(tran1.localPosition.x + 388, 0.5f).SetEase(Ease.OutSine);
                Tweener move2 = tran2.transform.DOLocalMoveX(tran2.localPosition.x - 388, 0.5f).SetEase(Ease.OutSine);
                Tweener move3 = tran1.transform.DOLocalMoveX(388, 0.5f)
                    .SetEase(Ease.OutSine); //+ 380 //tran1.localPosition.x
                Tweener move4 = tran2.transform.DOLocalMoveX(3, 0.5f)
                    .SetEase(Ease.OutSine); // - 380 //tran2.localPosition.x

                DOTween.Sequence().Append(move1).Join(move2).AppendInterval(0.3f).AppendCallback(() => { action(); })
                    .Append(move3).Join(move4);
            }

//        Debug.LogError(tran1.localPosition.x);
        }

        private void RefreshActTween(Transform tran1, Transform tran2, bool isleft, Action action)
        {
            if (isleft)
            {
                Tweener move1 = tran1.transform.DOLocalMoveX(tran1.localPosition.x - 888, 0.5f).SetEase(Ease.OutSine);
                Tweener move2 = tran2.transform.DOLocalMoveX(tran2.localPosition.x + 888, 0.5f).SetEase(Ease.OutSine);
                Tweener move3 = tran1.transform.DOLocalMoveX(tran1.localPosition.x, 0.5f)
                    .SetEase(Ease.OutSine); //tran1.localPosition.x 
                Tweener move4 = tran2.transform.DOLocalMoveX(tran2.localPosition.x, 0.5f)
                    .SetEase(Ease.OutSine); //tran2.localPosition.x
               
                LoadingOverlay.Instance.ShowMask(true);
                DOTween.Sequence().Append(move1).Join(move2).AppendInterval(0.3f).AppendCallback(() => { action(); })
                    .Append(move3).Join(move4).OnComplete(() =>
                    {
                        LoadingOverlay.Instance.ShowMask(false);
                    });
            }
            else
            {
                Tweener move1 = tran1.transform.DOLocalMoveX(tran1.localPosition.x + 888, 0.5f).SetEase(Ease.OutSine);
                Tweener move2 = tran2.transform.DOLocalMoveX(tran2.localPosition.x - 888, 0.5f).SetEase(Ease.OutSine);
                Tweener move3 = tran1.transform.DOLocalMoveX(tran1.localPosition.x, 0.5f)
                    .SetEase(Ease.OutSine); //+ 380 //tran1.localPosition.x
                Tweener move4 = tran2.transform.DOLocalMoveX(tran2.localPosition.x, 0.5f)
                    .SetEase(Ease.OutSine); // - 380 //tran2.localPosition.x
                LoadingOverlay.Instance.ShowMask(true);
                DOTween.Sequence().Append(move1).Join(move2).AppendInterval(0.3f).AppendCallback(() => { action(); })
                    .Append(move3).Join(move4).OnComplete(() =>
                    {
                        LoadingOverlay.Instance.ShowMask(false);  
                    });
            }
        }


        private void SetActItemStatePos(Transform hasGo, Transform notGo, bool state, bool isSecond)
        {
            //属于还未进行的状态
            if (state)
            {
                notGo.localPosition = isSecond ? new Vector3(-91, 0) : new Vector3(91, 0);
                hasGo.localPosition = isSecond ? new Vector3(256, 0) : new Vector3(-388, 0);
            }
            else
            {
                if (isSecond)
                {
                    notGo.localPosition = new Vector3(-388, 0);
                    hasGo.localPosition = new Vector3(-3, 0);
                }
                else
                {
                    notGo.localPosition = new Vector3(388, 0);
                    hasGo.localPosition = new Vector3(3, 0);
                }
            }
        }


        /// <summary>
        /// 这个要根据不同的活动类型来播放不同的动画
        /// </summary>
        /// <param name="actType"></param>
        /// <returns></returns>
        private string GetAni(int actType)
        {
//        Debug.LogError("ActType "+actType);
            switch (actType)
            {
                case 1:
                    return "";
                case 2:
                    return "wei_bo";
                case 3:
                    return "xiu_xian";
                case 4:
                    return "jsp";
                case 5:
                    return "zhi_zuo";
                case 6:
                    return "bh";
                default:
                    return "xiu_xian";
            }
        }


        /// <summary>
        /// 设置Spine的动画状态
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="aniname"></param>
        /// <param name="State"></param>
        private void SetSupporterSpine(GameObject gameObject, string aniname, bool State = false)
        {
            var _skg = gameObject.GetComponent<SkeletonGraphic>();
            if (_skg == null)
            {
               // Debug.LogError("_skg==null");
                return;
            }

            SkeletonDataAsset sdData;
            if (_skg.skeletonDataAsset == null)
            {
                sdData = SpineUtil.BuildSkeletonDataAsset(aniname, _skg);
                _skg.skeletonDataAsset = sdData;
                _skg.Initialize(true);
            }
            else if (_skg.skeletonDataAsset.name != aniname)
            {
                sdData = SpineUtil.BuildSkeletonDataAsset(aniname, _skg);
                _skg.skeletonDataAsset = sdData;
                _skg.Initialize(true);
            }

            _skg.transform.localScale = Vector3.one * 1.67f;
//        Debug.LogError(aniname);
            _skg.AnimationState.SetAnimation(0, "doing", true);
            _skg.timeScale = State ? 1 : 0;
        }

        private void OnDestroy()
        {
            ClientTimer.Instance.RemoveCountDown(_handle);
        }
    }
}