using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide;
using game.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module;
using DG.Tweening;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using QFramework;

namespace Module.Guide.ModuleView.Phone
{
    public class PhoneGuideView : View
    {
    //    private GameObject _arrowObj;


        private GameObject _smsListItem;
        private GameObject _smsItem;
        private GameObject _InputBar;
        private GameObject _SmsSelectionItem;
        private GameObject _SmsSelection;
        private GameObject _backBtnObj;
        private Image _bgImage;
        private int _step;

        private Transform _goToMainLine;
        // Use this for initialization
        private void Awake()
        {
            _goToMainLine = transform.Find("GoToMainLine");
            _bgImage = transform.GetImage("BgImage");
            //_arrowObj = transform.Find("Arrow").gameObject;

            _smsListItem = transform.Find("ListContainer").gameObject;
            _smsItem = transform.Find("ListContainer/Contents/SmsItem").gameObject;
            _backBtnObj = transform.Find("BackBtn").gameObject;
            _step = 0;
            _smsItem.transform.GetButton().onClick.AddListener(()=> {
               // _smsItem.Hide();
              // GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainLineStep_OnClick_Npc_Sms);
              //  GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Npc_Sms);
                _smsListItem.Hide();
                SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_GOTO_SMSITEM, Message.MessageReciverType.UnvarnishedTransmission));
                OnNextStep();
            });

            _InputBar = transform.Find("InputBar").gameObject;
            _InputBar.transform.GetButton().onClick.AddListener(() =>
            {
                // GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Input);
                _InputBar.Hide();
                SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_OPEN_INPUTBAR, Message.MessageReciverType.UnvarnishedTransmission));
                OnNextStep();
            });

            _SmsSelection = transform.Find("SceneIdsSelections").gameObject;
            _SmsSelectionItem = transform.Find("SceneIdsSelections/Viewport/Content/SmsSelectionItem").gameObject;
            _SmsSelectionItem.transform.GetButton().onClick.AddListener(() => {
                _SmsSelection.Hide();
                OnNextStep();
            });

            PointerClickListener.Get(_bgImage.gameObject).onClick = (obj) =>
            {
                OnNextStep();
            };
            _bgImage.gameObject.Hide();
            _backBtnObj.GetComponent<Button>().onClick.AddListener(() => {
                OnNextStep();
            });
           // _arrowObj.Hide();
        }

        private void Start()
        {
            _step = 1;
            OnNextStep();
        }

        private void OnNextStep()
        {
            if (_step == 1)
            {
                _bgImage.gameObject.Hide();
            //    _arrowObj.Show();
                _smsListItem.Show();

                GuideArrow.DoAnimation(_smsItem.transform);
                //_arrowObj.transform.eulerAngles = new Vector3(0, 0, 70);
                //_arrowObj.transform.position = _smsItem.transform.position;
            }
            else  if (_step == 2)
            {
           //     _arrowObj.Show();
                _InputBar.Show();
                _bgImage.gameObject.Hide();
                GuideArrow.DoAnimation(_InputBar.transform);
                //_arrowObj.transform.eulerAngles = new Vector3(0, 0, 70);
                //_arrowObj.transform.position = _InputBar.transform.position;
            }else if(_step==3)
            {
             //   _arrowObj.Show();
                _bgImage.gameObject.Hide();
                _SmsSelection.Show();
                GuideArrow.DoAnimation(_SmsSelectionItem.transform);
                //_arrowObj.transform.eulerAngles = new Vector3(0, 0, 70);
                //_arrowObj.transform.position = _SmsSelectionItem.transform.position;
       
            }
            else if (_step == 4)
            {
                SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_OPEN_SELECTITEM, Message.MessageReciverType.UnvarnishedTransmission));
                _bgImage.gameObject.Hide();
                transform.GetComponent<Empty4Raycast>().enabled = false;
                transform.Find("SceneIdsSelections").Hide();
                var backBtn = transform.GetButton("BackBtn");
                backBtn.gameObject.Show();
                backBtn.onClick.AddListener(() =>
                {
                    FlowText.ShowMessage(I18NManager.Get("Guide_MainLine_PhoneOnClickBackBtn"));
                });

                //   _arrowObj.gameObject.Hide();
                // _SmsSelection.Hide();
                //GuideArrow.DoAnimation(_backBtnObj.transform);
                // _backBtnObj.Show();
            }
//            else if (_step == 5)
//            {
              
              
//                Debug.LogError("_step  is 5");
//                _backBtnObj.Show();
//                
//                SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission));
//                //   _bgImage.gameObject.Show();
//                //   _arrowObj.gameObject.Hide();
//                // _SmsSelection.Hide();
//           }
//            else if (_step == 6)
//            {
//                _backBtnObj.Show();
//                Debug.LogError("_step  is 6");
//                SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission));
//                //SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_SERVER, ModuleConfig.MODULE_PHONE));
//                // SendMessage(new Message(MessageConst.CMD_PHONE_GUIDE_OPEN_SELECTITEM, Message.MessageReciverType.UnvarnishedTransmission));
//                //_bgImage.gameObject.Show();
//                //_arrowObj.gameObject.Hide();
//                //_SmsSelection.Hide();
//            }

            _step++;
        }
        
        public void ShowGoToMainLine()
        {
            ClientTimer.Instance.DelayCall(() => {    
                transform.GetComponent<Empty4Raycast>().enabled = true;
                _bgImage.gameObject.Show();
                var guideView =  transform.Find("GuideView");
                guideView.GetText("DialogFrame/Text").text = I18NManager.Get("Guide_MainLine1_6_Star");
                guideView.gameObject.Show();

                PointerClickListener.Get(_bgImage.gameObject).onClick = go =>
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE);
                }; }, 1.0f);
            
        
         
//            _goToMainLine.gameObject.Show();
//            _goToMainLine.Find("Bg").DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
//            {
//                _goToMainLine.GetButton("Bg/Button").onClick.AddListener(() =>
//                {
//                    GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_GoToMainLine);
//					
//                    _goToMainLine.Find("Bg").DOScale(new Vector3(0, 0, 1), 0.2F).SetEase(Ease.OutExpo).OnComplete(() => 
//                    { 						
//                        gameObject.Hide();						
//                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE);
//                    });
//                });
//            });

        }
    }
}
