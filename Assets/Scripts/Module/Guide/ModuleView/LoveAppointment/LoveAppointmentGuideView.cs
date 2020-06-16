using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Download;
using Common;
using DataModel;
using DG.Tweening;
using GalaAccount.Scripts.Framework.Utils;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class LoveAppointmentGuideView : View
    {
        private Transform lovemain;
        private Transform pageArrow;
        private Transform stroyList;
        private Transform _guideView2;
        private Transform pointarrow;
        private Transform storyview;
        private Transform viewcontent;
        private Transform goBackBtn;
        
        private UserCardVo _userCardVo;
        private AppointmentRuleVo _appointmentRuleVo;
        private int page = 0;
        private int pos = 0;
        private int loveappointindex = 0;
        private CacheVo _cacheVo;

        private void Awake()
        {
            lovemain = transform.Find("BG/Bg");
            storyview = transform.Find("StoryProgress");
            LoveStep1();
        }

//        public void HandleStep()
//        {
//            gameObject.Show();
//
//            Debug.LogError("Show?!");
//            lovemain = transform.Find("Bg");
//
//            lovemain.gameObject.SetActive(true);
//            Image arrow = lovemain.Find("GuideStep1/Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
//
//            PointerClickListener.Get(lovemain.Find("GuideStep1/ClickArea").gameObject).onClick = go =>
//            {
//                //跳转到恋爱剧情模块 
//                SendMessage(new Message(MessageConst.GUIDE_LOVEAPPOINTMENT_ENTERLOVECHOOSE));
//                LoveStep1();
//            };
//        }

        private void LoveStep1()
        {
            //首先我要知道首次抽卡获得的人。
            lovemain.gameObject.SetActive(false);
            foreach (var v in GlobalData.CardModel.UserCardList)
            {
                if (v.CardVo.Credit == CreditPB.Sr)
                {
                    _userCardVo = v;
                    break;
                }
            }

            if (_userCardVo == null)
            {
                FlowText.ShowMessage("Error Card!");
            }
            else
            {
                PlayerPB playerPb = _userCardVo.CardVo.Player;
                transform.Find("BG/Viewport").gameObject.SetActive(true);
                viewcontent= transform.Find("BG/Viewport/Content");

                int idx = 0;
                int index = 0;
                //这里是个雷区！
                switch (playerPb)
                {
                    case PlayerPB.ChiYu:
                        idx = 10004;
                        index = 0;
                        break;
                    case PlayerPB.QinYuZhe:
                        idx = 10002;
                        index = 1;
                        break;
                    case PlayerPB.TangYiChen:
                        idx = 10001;
                        index = 2;
                        break;
                    case PlayerPB.YanJi:
                        idx = 10003;
                        index = 3;
                        break;
                }


                var tranroot = viewcontent.GetChild(index);
                var arrowTransform = transform.Find("BG/LoveChooseArrow");
                arrowTransform.gameObject.SetActive(true);
                arrowTransform.SetParent(tranroot, false);
                GuideArrow.DoAnimation(arrowTransform);
//                var arrow = arrowTransform.Find("Arrow").GetComponent<Image>();
//                RectTransform rect = arrow.rectTransform;
//                rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                    rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);

                PointerClickListener.Get(arrowTransform.Find("ClickArea").gameObject).onClick = go =>
                {
                    //发送跳转到某张卡片的协议！发送角色类型Id，并且要把目标角色的恋爱规则传回来！简单！
                    Debug.LogError(idx);
                    GlobalData.CardModel._curRoleId = idx;
                    SendMessage(new Message(MessageConst.CMD_APPOINTMENT_GUIDE_JOURNALCHOOSE,
                        Message.MessageReciverType.UnvarnishedTransmission));
                };
            }

            //到时候要看是switch还是直接用playerpb
        }

        //假设其他模块会传规则过来了
        public void LoveSetp2(List<AppointmentRuleVo> data)
        {
            //首先要引导翻页,看要翻几次的意思！
            viewcontent.gameObject.SetActive(false);
            loveappointindex = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].ActiveCards.Contains(_userCardVo.CardId))
                {
                    loveappointindex = i;
                    _appointmentRuleVo = data[i];
                }
            }

            pos = loveappointindex % 15;
            page = loveappointindex / 15 + 1; //所在第几页？！
            stroyList = transform.Find("BG/List");
            pageArrow = transform.Find("BG/PageArrow");
            stroyList.gameObject.SetActive(true);
            //是第二页的话就要不断的引导点击下一页！
            if (page - 1 > 0)
            {

                pageArrow.gameObject.SetActive(true);
                GuideArrow.DoAnimation(pageArrow.Find("Right"));
//                var arrow = pageArrow.Find("Right/Arrow").GetComponent<Image>();
//                RectTransform rect = arrow.rectTransform;
//                rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                    rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
                PointerClickListener.Get(pageArrow.Find("Right/ClickArea").gameObject).onClick = go =>
                {
                    //先发送协议要换页显示！sendmessage
                    SendMessage(new Message(MessageConst.CMD_APPOINTMENT_GUIDE_NEXTPAGE,
                        Message.MessageReciverType.UnvarnishedTransmission));
                    page--;
                    NextPage();
                };
            }
            else
            {
                ChooseStory();
            }
        }

        private void NextPage()
        {
            Debug.LogError("page"+page);
            if (page <= loveappointindex / 15 + 1)
            {
                //展示选卡界面！
                ChooseStory();
            }
        }

        private void ChooseStory()
        {
            pageArrow.gameObject.SetActive(false);
            Debug.LogError("pos"+pos);
            var tranroot = transform.Find("BG/List/Content/JournalItem").GetChild(pos);
            pointarrow = transform.Find("BG/PointArea");
            pointarrow.SetParent(tranroot);
            pointarrow.transform.localPosition=new Vector3(0,0);
            pointarrow.gameObject.SetActive(true);
            GuideArrow.DoAnimation(pointarrow);
//            var arrow = pointarrow.Find("Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            PointerClickListener.Get(pointarrow.Find("ClickArea").gameObject).onClick = go =>
            {
                //最好都是延迟展示
                //发送进入剧情选择的协议
                SendMessage(new Message(MessageConst.UIDE_LOVEAPPOINTMENT_ENDSUCCESS));
                //我要知道是否已经解锁了！
                
                SendMessage(new Message(MessageConst.CMD_APPOINTMENT_ACTIVE_LOVESTORY,
                    Message.MessageReciverType.UnvarnishedTransmission,_appointmentRuleVo));
                LoveStep3();
            };
        }
        
        private void LoveStep3()
        {
            stroyList.gameObject.SetActive(false);
            pointarrow.gameObject.SetActive(false);
            
            storyview.gameObject.SetActive(true);
            GuideArrow.DoAnimation(storyview.Find("Stage/Circle"));
//            var arrow = storyview.Find("Stage/Circle/Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x + 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            PointerClickListener.Get(storyview.Find("Stage/Circle/ClickArea").gameObject).onClick = go =>
            {
                //进入想要去的剧情！//good!//需要,gate和vo
                var gate=new AppointmentGateRuleVo(_appointmentRuleVo.GateInfos[0]);
//                SendMessage(new Message(MessageConst.CMD_APPOINTMENT_ENSUREOPENGATE,
//                    Message.MessageReciverType.UnvarnishedTransmission,gate,_appointmentRuleVo));
                _cacheVo = CacheManager.CheckLoveStoryCache(_userCardVo.CardId);
                if (_cacheVo!=null&&_cacheVo.needDownload)
                {
                    CacheManager.DownloadLoveStoryCache(_userCardVo.CardId, str =>
                    {
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
                    },null, str =>
                    {
                        Debug.LogError("Cancle?!");
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
                    });
                    Debug.LogError("download");
                    
                }
                else
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, gate,_appointmentRuleVo.Id);
                }
                gameObject.Hide();
            };
        }

        public void LoveStep3_2()
        {
            _guideView2 = transform.Find("BG/GuideView");
            var _guideText = transform.GetText("BG/GuideView/DialogFrame/Text");
            var oldarrow = storyview.Find("Stage/Circle");
            oldarrow.gameObject.SetActive(false);
       
            var arrowtran = storyview.Find("Stage/Circle2");
            arrowtran.gameObject.SetActive(true);
                
            storyview.gameObject.Show();
            transform.Find("BG/Viewport").gameObject.SetActive(false);
            
            GuideArrow.DoAnimation(arrowtran);
//            var arrow=arrowtran.Find("Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x + 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            PointerClickListener.Get(storyview.Find("Stage/Circle2/ClickArea").gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_LoveStroy_2); 
                //弹出弹窗！
                arrowtran.gameObject.SetActive(false);
                _guideView2.gameObject.SetActive(true);
                _guideText.text = I18NManager.Get("Guide_LoveAppointmentGuideCardStarUp");
                var alertWindow = transform.Find("BG/AlertWindow");
                alertWindow.gameObject.SetActive(true);
                var btn = transform.Find("BG/AlertWindow/okBtn").GetButton();
                btn.onClick.AddListener(() =>
                {
                    LoveStep4();
                    alertWindow.gameObject.SetActive(false);
                    _guideView2.gameObject.SetActive(false);
                });

//                var tipsWindow = PopupManager.ShowAlertWindow(I18NManager.Get("Guide_LoveAppointmentGuideCardStarTips"));
//                tipsWindow.CanClickBGMask = false;
//                tipsWindow.WindowActionCallback = evt =>
//                {
//                    if (evt==WindowEvent.Ok)
//                    {
//                        LoveStep4();
//                    }
//                };

            };
        }
        

        public void LoveStep4()
        {
            //开始引导后退！要引导4步！！
            gameObject.Show();
            storyview.gameObject.SetActive(false);
            goBackBtn = transform.Find("BackArrow");
            goBackBtn.gameObject.SetActive(true);
            GuideArrow.DoAnimation(goBackBtn);
//            var arrow = goBackBtn.Find("Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            int gobackStep = 0;
            PointerClickListener.Get(goBackBtn.gameObject).onClick = go =>
            {
                switch (gobackStep)
                {
                    case 0:
                        //后退到journalItem
                        SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_JOURNALCHOOSE,
                            Message.MessageReciverType.UnvarnishedTransmission));
                        break;
                    case 1:
                        //后退到角色选择界面
                        SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_CHOOSEROLE,
                            Message.MessageReciverType.UnvarnishedTransmission,1));
                        break;
                    case 2:
                        //后退到恋爱主界面
                        SendMessage(new Message(MessageConst.CMD_APPOINTMENT_GUIDE_BACKTOLOVEMAIN,
                            Message.MessageReciverType.UnvarnishedTransmission));
                        break;
                    case 3:
                        //后退到主界面
                        gameObject.Hide();
                        SendMessage(new Message(MessageConst.CMD_LOVE_GUIDE_GOBACKTOMAINVIEW,
                            Message.MessageReciverType.UnvarnishedTransmission));
                        break;
                }
                Debug.LogError(gobackStep);
                gobackStep++;
                
            };




        }
        
        
        


    }
}