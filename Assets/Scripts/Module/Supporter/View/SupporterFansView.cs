using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Supporter.Data;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using DG.Tweening;
using game.tools;
using Module.Supporter.Data;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace game.main
{
    public class SupporterFansView : View
    {
        private Button _ruleBtn;
        private Transform _financial;
        private Transform _resource;
        private Transform _active;
        private Transform _transmission;
        private Text _nameText;
        private Text _levelText;
        private Text _expText;
        private ProgressBar _expProgress;
        private Transform _fansright;
        private Transform _fansleft;
        private SupporterModel _data;
        private Button _renameBtn;
        private Button _friendBtn;
        private Button _airborneGameBtn;
        private Button _takePhotoBtn;
        private int _financialnum;
        private int _resourecenum;
        private int _activenum;
        private int _transmissionnum;

        private Transform _friendredpoint;

        string[] dia =
        {
            I18NManager.Get("Supporter_Dia1"),I18NManager.Get("Supporter_Dia2"),I18NManager.Get("Supporter_Dia3"),I18NManager.Get("Supporter_Dia4"),I18NManager.Get("Supporter_Dia5"),I18NManager.Get("Supporter_Dia6"),
            I18NManager.Get("Supporter_Dia7"),I18NManager.Get("Supporter_Dia8"),I18NManager.Get("Supporter_Dia9"),I18NManager.Get("Supporter_Dia10"),I18NManager.Get("Supporter_Dia11")
        };

        private int[] supporterArray = {0, 0, 0, 0, 0};

//	public int TestCount=0;
//	public int TestLevel;
//	public float CurProgress=0f;

        //private DepartmentTypePB supporterType = DepartmentTypePB.Support;


        private void Awake()
        {
            _ruleBtn = transform.Find("RuleBtn").GetComponent<Button>();
            _ruleBtn.onClick.AddListener(() =>
            {
                //SetTestAnimation(_financial.Find("FansList"));
            });

            _nameText = transform.Find("List/TopLayout/Title/NameTxt").GetComponent<Text>();

            _financial = transform.Find("List/TopLayout/Financial");
            _resource = transform.Find("List/TopLayout/Resource");
            _active = transform.Find("List/TopLayout/Active");
            _transmission = transform.Find("List/TopLayout/Transmission");
            _financial.Find("FansList").gameObject.Hide();
            _resource.Find("FansList").gameObject.Hide();
            _active.Find("FansList").gameObject.Hide();
            _transmission.Find("FansList").gameObject.Hide();

            SpawnFansAnimation(_financial.Find("FansList"));
            SpawnFansAnimation(_resource.Find("FansList"));
            SpawnFansAnimation(_active.Find("FansList"));
            SpawnFansAnimation(_transmission.Find("FansList"));

            //_nameText.text = GlobalData.PlayerModel.PlayerVo.UserName + "的应援会";
            _nameText.text = I18NManager.Get("Supporter_NameText", GlobalData.PlayerModel.PlayerVo.UserName);

            _levelText = transform.Find("List/TopLayout/LevelContainer/LevelTxt").GetComponent<Text>();
            _expText = transform.Find("List/TopLayout/LevelContainer/ExpTxt").GetComponent<Text>();
            _expProgress = transform.Find("List/TopLayout/LevelContainer/ProgressBar").GetComponent<ProgressBar>();

            _fansright = transform.Find("Fans/Fans1");
            _fansleft = transform.Find("Fans/Fans2");
            _renameBtn = transform.Find("List/TopLayout/Title/RenameBtn").GetComponent<Button>();
            _renameBtn.onClick.AddListener(RenameOnClick);

            _friendBtn = transform.Find("FriendsBtn").GetButton();
            _friendBtn.onClick.AddListener(GotoFriendView);


//            _airborneGameBtn = transform.Find("AirborneGameBtn").GetButton();
//            _airborneGameBtn.onClick.AddListener(GotoAirborneGameView);

            _takePhotoBtn = transform.Find("TakePhoto").GetButton();
            _takePhotoBtn.onClick.AddListener(GotoTakePhoto);

            _friendredpoint = transform.Find("FriendsBtn/RedPoint");


            transform.Find("MoreFansBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.MODULE_SUPPOTER_GOTO_MORN_FANS));
            });


            _financial.Find("Item").GetComponent<LongPressButton>().OnDown = FinacialUpEvent;

            _resource.Find("Item").GetComponent<LongPressButton>().OnDown = ResouceUpEvent;

            _active.Find("Item").GetComponent<LongPressButton>().OnDown = ActiveEvent;

            _transmission.Find("Item").GetComponent<LongPressButton>().OnDown = TransmissionEvent;

            PointerClickListener.Get(_financial.Find("Gift").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPOTER_RECEIVEGIFT, _data.Financial.type));
            };
            PointerClickListener.Get(_resource.Find("Gift").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPOTER_RECEIVEGIFT, _data.Resource.type));
            };
            PointerClickListener.Get(_active.Find("Gift").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPOTER_RECEIVEGIFT, _data.Active.type));
            };
            PointerClickListener.Get(_transmission.Find("Gift").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPOTER_RECEIVEGIFT, _data.Transmission.type));
            };

            //StarTween();
            //StarTween();
        }

        private void GotoFriendView()
        {
            var openLevel = GuideManager.GetOpenUserLevel(ModulePB.Department, FunctionIDPB.DepartmentFriends);
            if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
            {
                FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));

            }
            else
            {
                SendMessage(new Message(MessageConst.MODULE_SUPPOTER_GOTO_FRIENDS)); 
            }

        }
//        private void GotoAirborneGameView()
//        {
//            var openLevel = GuideManager.GetOpenUserLevel(ModulePB.Department, FunctionIDPB.DepartmentGame);
//            if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
//            {
//                FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
//
//            }
//            else
//            {
//                SendMessage(new Message(MessageConst.MODULE_SUPPOTER_GOTO_AIRBORNEGAME)); 
//            }
//        }

        private void GotoTakePhoto()
        {

            SendMessage(new Message(MessageConst.MODULE_SUPPOTER_GOTO_TAKEPHOTO));
        }


        private void StarTween()
        {
            Tweener t1 = _financial.Find("Gift").GetImage().DOFade(0.6f, 1f);
            t1.SetAutoKill(false);
            t1.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            Tweener t2 = _resource.Find("Gift").GetImage().DOFade(0.6f, 1f);
            t2.SetAutoKill(false);
            t2.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            Tweener t3 = _active.Find("Gift").GetImage().DOFade(0.6f, 1f);
            t3.SetAutoKill(false);
            t3.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            Tweener t4 = _transmission.Find("Gift").GetImage().DOFade(0.6f, 1f);
            t4.SetAutoKill(false);
            t4.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }


        private void FinacialUpEvent()
        {
            UpgradeSupporter(_data.Financial);
        }

        private void ResouceUpEvent()
        {
            UpgradeSupporter(_data.Resource);
        }

        private void ActiveEvent()
        {
            UpgradeSupporter(_data.Active);
        }

        private void TransmissionEvent()
        {
            UpgradeSupporter(_data.Transmission);
        }

        private ModificationNameWindow window;
        private void RenameOnClick()
        {
            //FlowText.ShowMessage("功能暂未开放，敬请期待");

           // FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));
            ShowModificationNameWindow();
        }
        private void ShowModificationNameWindow()
        {
            if (window==null)
            {
                window = PopupManager.ShowWindow<ModificationNameWindow>("GameMain/Prefabs/ModificationNameWindow");
                window.SetData(GlobalData.PlayerModel.PlayerVo);
            }
        }

        public void UpdataSupporterFansViewName()
        {
            _nameText.text = I18NManager.Get("Supporter_NameText", GlobalData.PlayerModel.PlayerVo.UserName);
        }

       

        private void UpgradeSupporter(SupporterVo vo)
        {
            if (vo.Prop.Num > 0 && vo.Level < 101)
            {
                if (vo.AwardPbs.Count > 0)
                {
                    FlowText.ShowMessage(I18NManager.Get("Supporter_Hint2"));
                    //FlowText.ShowMessage("请先收下礼物");
                    return;
                }


                SendMessage(new Message(MessageConst.CMD_SUPPOTER_UPGRADE, vo));

                vo.CostNum = 0;
            }
            else if (vo.Prop.Num <= 0)
            {
                FlowText.ShowMessage(I18NManager.Get("Supporter_Hint3")); //("通过应援活动获得");
            }
            else if (vo.Level >= 101)
            {
                FlowText.ShowMessage(I18NManager.Get("Supporter_Hint4")); //("已满级");
            }
        }

        public void RefreshRedPoint()
        {
            _friendredpoint.gameObject.SetActive(GlobalData.DepartmentData.CanGetFriendsPower);
        }
        
        public void SetData(SupporterModel model, DepartmentTypePB departmentTypePb = DepartmentTypePB.Support)
        {
            _friendredpoint.gameObject.SetActive(GlobalData.DepartmentData.CanGetFriendsPower);
            _data = model;
            var playerVo = GlobalData.PlayerModel.PlayerVo;
            //_levelText.text = "Lv." + playerVo.Level;
            _levelText.text = I18NManager.Get("Common_Level", playerVo.Level);
            _expText.text = playerVo.CurrentLevelExp + "/" + playerVo.NeedExp;

            _expProgress.Progress = (int) ((float) playerVo.CurrentLevelExp / playerVo.NeedExp * 100);

            //其实最好的办法是有更新的才需要重新设置！
            //最好的办法是switch case !
            if (departmentTypePb == DepartmentTypePB.Support)
            {
                //初始化的时候！
                SetItemData(model.Financial, _financial);
                SetItemData(model.Resource, _resource);
                SetItemData(model.Active, _active);
                SetItemData(model.Transmission, _transmission);
            }
            else
            {
                switch (departmentTypePb)
                {
                    case DepartmentTypePB.Financial:
                        SetItemData(model.Financial, _financial);
                        break;
                    case DepartmentTypePB.Resource:
                        SetItemData(model.Resource, _resource);
                        break;
                    case DepartmentTypePB.Active:
                        SetItemData(model.Active, _active);
                        break;
                    case DepartmentTypePB.Transmission:
                        SetItemData(model.Transmission, _transmission);
                        break;
                }
            }
        }

        private void SetItemData(SupporterVo vo, Transform item, bool test = false)
        {
            var leveltext = item.Find("Board/LevelText").GetComponent<Text>();
            //leveltext.text = "Lv." + (vo.Level+1);
            item.Find("Item/RedPoint/NumTxt").GetComponent<Text>().text = vo.Prop.Num + "";
            item.Find("Item/RedPoint").gameObject.SetActive(vo.Prop.Num != 0);
            var valuetext = item.Find("Value").GetComponent<Text>();
            var slider = item.Find("PopBG").GetComponent<ProgressBar>();
            slider.DeltaX = 0;

//		Debug.LogError(vo.AwardPbs.Count);


            if (vo.AniState == 1 || test)
            {
                var oldNum = Int32.Parse(Util.RemoveStr(leveltext.text, "Lv."));

                //正式的动画调度
                slider.TweenSlider((float) vo.Exp / vo.ExpNeed * 100, () =>
                    {
                        FlowText.ShowMessage(I18NManager.Get("Supporter_Hint5")); //("应援会升级成功");
                        //leveltext.text = "Lv." + (vo.Level+1);
                    }, vo.Level + 1 - oldNum);
                Util.TweenTextNum(valuetext, 6f, vo.Power); //应援热度：
                Util.TweenTextNum(leveltext, 6f, vo.Level + 1, "Lv.");
            }
            else
            {
                valuetext.text = vo.Power + "";
                leveltext.text = "Lv." + (vo.Level + 1);
                slider.Progress = (int) ((float) vo.Exp / vo.ExpNeed * 100);
            }


            item.Find("Gift").gameObject.SetActive(vo.AwardPbs.Count > 0 && vo.AniState != 1);
            SetFansAnimation(item.Find("FansList"), vo, item.Find("Gift"));
        }

        private void SpawnFansAnimation(Transform fansroot)
        {
            for (int i = 0; i < fansroot.childCount; i++)
            {
                var dialogTran = fansroot.GetChild(i).transform.Find("Dialog");
                var dialog = fansroot.GetChild(i).transform.Find("Dialog/Text").GetComponent<Text>();
                dialogTran.gameObject.Hide();
                var _skg = fansroot.GetChild(i).Find("SpineSkeletonGraphic").GetComponent<SkeletonGraphic>();

                //在这里可以随机生成Spine动画
                var ani = RandomAni();
                SkeletonDataAsset sdData = SpineUtil.BuildSkeletonDataAsset(ani, _skg);
                _skg.skeletonDataAsset = sdData;
                _skg.Initialize(true);
                _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(1), true);
                _skg.transform.localScale = Vector3.one * 0.5f;
                fansroot.GetChild(i).transform.localPosition =
                    new Vector2(RandomX(), fansroot.GetChild(i).transform.localPosition.y);
                _skg.transform.localEulerAngles = new Vector3(0, RandomForward());
                _skg.timeScale = RandomRate();
                PointerClickListener.Get(fansroot.GetChild(i).gameObject).onClick = go =>
                {
                    if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(2) ||
                        dialogTran.gameObject.activeInHierarchy)
                    {
                        //Debug.LogError(_skg.AnimationState.ToString());
                        return;
                    }

                    dialog.text = RandomDia();
                    if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(3))
                    {
                        dialog.text = I18NManager.Get("Supporter_Hint6"); //"请收下礼物~(ﾉ>ω<)ﾉ";
                    }

                    DOTween.Sequence().AppendCallback(() => { dialogTran.gameObject.SetActive(true); })
                        .AppendInterval(2f).OnComplete(() =>
                        {
                            dialogTran.gameObject.SetActive(false);
                            if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(2))
                            {
                                return;
                            }
                            else if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(3))
                            {
                                //dialog.text = "请收下礼物";
                                return;
                            }

                            _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(1), true);
                        });
                };
            }

            fansroot.gameObject.Show();
        }

        private void SetFansAnimation(Transform fansroot, SupporterVo vo, Transform gift)
        {
            for (int i = 0; i < fansroot.childCount; i++)
            {
                var _skg = fansroot.GetChild(i).Find("SpineSkeletonGraphic").GetComponent<SkeletonGraphic>();
                if (vo.AniState == 1)
                {
                    int targetX = fansroot.GetChild(i).transform.localPosition.x > 0 ? -390 : 390;
                    //int targetX2=RandomX();
                    int rotY = SetForward(fansroot.GetChild(i).transform.localPosition.x, targetX);

                    Tweener runtween =
                        fansroot.GetChild(i).transform.DOLocalMoveX(targetX, 3f).SetEase(Ease.Linear); //匀速运动
                    Tweener backtween =
                        fansroot.GetChild(i).transform.DOLocalMoveX(fansroot.GetChild(i).transform.localPosition.x, 3f)
                            .SetEase(Ease.Linear);

                    _skg.transform.localEulerAngles = new Vector3(0, rotY);
                    _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(2), true);
                    _skg.timeScale = 2f; //运动频率

                    var dialogTran = fansroot.GetChild(i).transform.Find("Dialog");
                    var dialog = fansroot.GetChild(i).transform.Find("Dialog/Text").GetComponent<Text>();
                    dialog.text = I18NManager.Get("Supporter_Hint7"); //"有新任务啦~(灬ºωº灬)";
                    dialogTran.gameObject.SetActive(true);
                    supporterArray[(int) vo.type] = 1;
                    DOTween.Sequence().Append(runtween).AppendCallback(() =>
                    {
                        _skg.transform.localEulerAngles = new Vector3(0, rotY > 0 ? 0 : 180); //
                    }).Append(backtween).OnComplete(() =>
                    {
                        dialogTran.gameObject.SetActive(false);
                        supporterArray[(int) vo.type] = 0;
                        _skg.AnimationState.SetAnimation(0,
                            vo.AwardPbs.Count > 0
                                ? GlobalData.NpcModel.GetAnimationState(3)
                                : GlobalData.NpcModel.GetAnimationState(1), true);
                        if (vo.AwardPbs.Count > 0)
                        {
                            gift.gameObject.SetActive(true);
                        }

                        _skg.timeScale = RandomRate(); //运动频率
                    });
                }
                else if (vo.AwardPbs.Count > 0)
                {
                    //Debug.LogError("挥手状态！");
                    if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(2))
                    {
                        return;
                    }

                    _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(3), true);
                }
//			else if(vo.AniState==2)
//			{
//				if (_skg.AnimationState.ToString()==GlobalData.NpcModel.GetAnimationState(2))
//				{
//					return;
//				}
//				_skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(1), true);
//			}
                else
                {
                    if (_skg.AnimationState.ToString() == GlobalData.NpcModel.GetAnimationState(2))
                    {
                        return;
                    }

                    _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(1), true);
                }
            }
        }


        /// <summary>
        /// 测试跑路动画，待删除
        /// </summary>
        /// <param name="fansroot"></param>
        private void SetTestAnimation(Transform fansroot)
        {
            SetItemData(_data.Resource, _resource, true);
            for (int i = 0; i < fansroot.childCount; i++)
            {
                var _skg = fansroot.GetChild(i).Find("SpineSkeletonGraphic").GetComponent<SkeletonGraphic>();
                int targetX = fansroot.GetChild(i).transform.localPosition.x > 0 ? -390 : 390;
                //int targetX2=RandomX();
                int rotY = SetForward(fansroot.GetChild(i).transform.localPosition.x, targetX);

                Tweener runtween = fansroot.GetChild(i).transform.DOLocalMoveX(targetX, 3f).SetEase(Ease.Linear); //匀速运动
                Tweener backtween =
                    fansroot.GetChild(i).transform.DOLocalMoveX(fansroot.GetChild(i).transform.localPosition.x, 3f)
                        .SetEase(Ease.Linear);

                _skg.transform.localEulerAngles = new Vector3(0, rotY);

                var dialogTran = fansroot.GetChild(i).transform.Find("Dialog");
                var dialog = fansroot.GetChild(i).transform.Find("Dialog/Text").GetComponent<Text>();
                dialog.text = I18NManager.Get("Supporter_Hint7"); //"有新任务啦~(灬ºωº灬)";
                dialogTran.gameObject.SetActive(true);
                //设置动画速度。
//				Debug.LogError(_skg.AnimationState);
                _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(2), true);
                _skg.timeScale = 2f; //运动频率
                DOTween.Sequence().Append(runtween).AppendCallback(() =>
                {
                    _skg.transform.localEulerAngles = new Vector3(0, rotY > 0 ? 0 : 180); //
                }).Append(backtween).OnComplete(() =>
                {
                    dialogTran.gameObject.SetActive(false);
                    _skg.AnimationState.SetAnimation(0, GlobalData.NpcModel.GetAnimationState(1), true);
                    _skg.timeScale = RandomRate(); //运动频率
                });
            }
        }


        private int RandomX()
        {
            var a = Random.Range(-380, 260);
            //Debug.LogError(a);
            return a;
        }

        private string RandomAni()
        {
            var a = Random.Range(0, 5);
            switch (a)
            {
                case 0:
                    return "nian_nian";
                case 1:
                    return "mm";
                case 2:
                    return "nan_nan";
                case 3:
                    return "shui_shou_fu";
                case 4:
                    return "mei_zhuang";
                default:
                    return "nian_nian";
            }
        }

        private int RandomForward()
        {
            var a = Random.Range(0, 2);
            switch (a)
            {
                case 0:
                    return 180;
                case 1:
                    return 0;
            }

            return 0;
        }

        private int SetForward(float posx, float targetX)
        {
            if (posx >= targetX)
            {
                return 180;
            }
            else
            {
                return 0;
            }
        }

        private string RandomDia()
        {
            var dianame = $"Supporter_Dia{Random.Range(1, 10)}";
            //Debug.LogError(dianame);
            return I18NManager.Get(dianame); //dia[Random.Range(0, 8)];
        }

        private float RandomRate()
        {
            return Random.Range(0.8f, 1.2f);
        }

//	public void SetFansData(FansVo vo1,FansVo vo2)
//	{
//		SetFansView(vo1,_fansright);
//		SetFansView(vo2,_fansleft,true); //需要复杂相同一份UI绑定
//	}

//	private void SetFansView(FansVo vo, Transform target,bool isSecondeFan=false)
//	{
//		if (vo == null)
//		{
//			return;
//		}
//		target.gameObject.Show();
//		if(vo.Num<=0&&isSecondeFan)
//		{
//
//			target.Find("NoFansLeft").gameObject.SetActive(true);
//			target.Find("HasFansLeft").gameObject.SetActive(false);
//			return;
//		}
//		else if (vo.Num > 0 && isSecondeFan)
//		{
//			target.Find("NoFansLeft").gameObject.SetActive(false);
//			target.Find("HasFansLeft").gameObject.SetActive(true);
//			target = target.Find("HasFansLeft");
////			target = hasfans;
//		}
//
//		target.Find("FansName").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_fansNameTag" + vo.FansId);
//		target.Find("NumTag").GetComponent<Image>().sprite=AssetManager.Instance.GetSpriteAtlas("UIAtlas_Supporter_fansNumTag" + vo.FansId);
//		target.Find("Introduction").GetComponent<Text>().text = vo.Description;
//		target.Find("NumText").GetComponent<Text>().text = "人数："+vo.Num.ToString();
//		target.Find("SupporterPower").GetComponent<Text>().text = $"<color={vo.FansTextColor}>{vo.Power}</color>";//vo.Power.ToString();
//		target.Find("FansName/Text").GetComponent<Text>().text = vo.Name;
//		target.Find("Image").GetComponent<RawImage>().texture=ResourceManager.Load<Texture>(vo.FansTexturePath);
//
//	}
    }
}