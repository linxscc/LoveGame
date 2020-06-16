using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Guide.ModuleView.Supporter
{
    public class SupporterActGuideView : View
    {
        private RectTransform _guideView;
        //private RectTransform _highlight;
        private Text _guideText;
        private int _step;
        private float _lastClickTime;
        private Image _bg;
//        private PointerClickListener _clickArea;
        //private Tweener _lightTweener;
        private RectTransform pos2;
        private RectTransform pos3;
        private RectTransform pos4;
        private RectTransform pos5;

        private bool hasPlaypos2=false;
        private bool hasPlaypos3=false;
        private bool hasPlaypos4=false;
        private bool hasPlaypos5=false;

        private Button _stepListen2;
        
        

        private void Awake()
        {
            _guideView = transform.GetRectTransform("GuideView");
            //_highlight = transform.GetRectTransform("Highlight");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");

           // _highlight.gameObject.Hide();

            _bg = transform.GetImage();

            _step = 1;

            //_guideText.text = "通过完成应援活动可获得相应奖励";
            _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint1");
            _step++;

            PointerClickListener.Get(gameObject).onClick = OnNextStep;
        }

        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 1f)
                return;
            
            //这个是错的，应该是点击完才移除刷新！

            Debug.LogError(_step);
            switch (_step)
            {
                case 2:
                    _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
                
                    //_highlight.GetImage().color = new Color(1, 1, 1, 0.5f);
                    //_highlight.gameObject.Show();
                    //_highlight.GetImage().DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    
//                    transform.RemoveComponent<PointerClickListener>();
                    pos2 = transform.GetRectTransform("Viewport/Content/ActivityItem/HasSupport/Step2");
                    pos2.gameObject.Show();
                    if (!hasPlaypos2)
                    {
                        hasPlaypos2 = true;
                        GuideArrow.DoAnimation(pos2);
                    }
 
//                    _highlight.transform.SetParent(pos2.transform.parent, false);
//                    _highlight.CopyRectTransform(pos2);
//                
//                    _lightTweener=_highlight.DOLocalMoveX(_highlight.localPosition.x+10,0.5f).SetLoops(-1, LoopType.Yoyo);
                    RectTransform gv2 = transform.GetRectTransform("GuideView2");
                    _guideView.CopyRectTransform(gv2);
                
                    gv2.SetNormalPivot();
             
                    _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint2");

                    //这个需要点击到了那个可点击区域才可以下一步            _step++;！
                    //_clickArea =PointerClickListener.Get();//_highlight.gameObject
                    _stepListen2=transform.Find("Viewport/Content/ActivityItem/HasSupport/Step2Listener").GetButton();
                    _stepListen2.onClick.RemoveAllListeners();
                    _stepListen2.onClick.AddListener(() =>
                    {
//                        SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GUIDETOFANSMODULE,Message.MessageReciverType.UnvarnishedTransmission));
//                        EnterActivity();
                        SendMessage(new Message(MessageConst.MOUDLE_GUIDE_STEP_REMOTE,Message.MessageReciverType.DEFAULT,GuideTypePB.EncourageActGuide,1020));//新手引导ID？？  
                        _stepListen2.onClick.RemoveAllListeners();
                        _step+=2;
                    });
                    break;
                case 3:
                    Debug.LogError("wrong");
                    break;
                case 4:
                    pos3.gameObject.Hide();
                    pos4 = transform.GetRectTransform("SupporterProps/SupporterPropList/PropItem2/Step4");
//                    _highlight.transform.SetParent(pos4.transform.parent, false);
//                    _highlight.CopyRectTransform(pos4);
                    pos4.gameObject.Show();
 
                    if (!hasPlaypos4)
                    {
                        hasPlaypos4 = true;
                        GuideArrow.DoAnimation(pos4);
                    }
                
                    RectTransform gv4 = transform.GetRectTransform("GuideView4");
                    _guideView.CopyRectTransform(gv4);

                    gv4.SetNormalPivot();

//                    _highlight.GetImage().raycastTarget = false;
//                    
//                    _lightTweener.Kill();
//                    _lightTweener=_highlight.DOLocalMoveX(_highlight.localPosition.x+10,0.5f).SetLoops(-1, LoopType.Yoyo);
                    
                    _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint3");
                    _step++;
                    break;
                case 5:
                    pos4.gameObject.Hide();
                    pos5 = transform.GetRectTransform("GoBtn/Step5");
//                    _highlight.transform.SetParent(pos5.transform.parent, false);
//                    _highlight.CopyRectTransform(pos5);
                    pos5.gameObject.Show();
                    if (!hasPlaypos5)
                    {
                        hasPlaypos5 = true;
                        GuideArrow.DoAnimation(pos5);
                    }
            
                    RectTransform gv5 = transform.GetRectTransform("GuideView5");
                    _guideView.CopyRectTransform(gv5);
                    
//                    _lightTweener.Kill();
//                    _lightTweener=_highlight.DOLocalMoveX(_highlight.localPosition.x+10,0.5f).SetLoops(-1, LoopType.Yoyo);

                    gv5.SetNormalPivot();
                
                
                    _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint4");
//                    _highlight.GetImage().raycastTarget = true;
                    //这一步很关键，需要跟网络通讯！
                    //_step++;  
                    var gobtn=transform.Find("GoBtn/Step5Listener").GetButton();//_highlight.gameObject
                    gobtn.onClick.RemoveAllListeners();
                    gobtn.onClick.AddListener(() =>
                    {
//                        Debug.LogError("Come!");
//                        SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GOBACK,Message.MessageReciverType.UnvarnishedTransmission));
//                        StartActSuccess();
                        SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GUIDETODOINGACT,Message.MessageReciverType.UnvarnishedTransmission));
                    });                
                    break;
//                case 6:
//                    StartActSuccess();
//                    break;
                default:
                    
                    transform.RemoveComponent<PointerClickListener>();
                    
                    SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_SERVER, ModuleConfig.MODULE_SUPPORTERACTIVITY));
                    break;
            }
            
            


            _lastClickTime = Time.realtimeSinceStartup;

        }

        private void ClickHightLight(GameObject go)
        {
            SendMessage(new Message(MessageConst.MOUDLE_GUIDE_STEP_REMOTE,Message.MessageReciverType.DEFAULT,GuideTypePB.EncourageActGuide,1020));//新手引导ID？？  
            _step++;
//            if (_clickArea!=null)
//            {
//                transform.Find("Viewport/Content/ActivityItem/HasSupport/Step2Listener").RemoveComponent<PointerClickListener>();
//
//            }
        }

        private void ClickGo(GameObject go)
        {

//            if (_clickArea!=null)
//            {
//                transform.Find("GoBtn/Step5Listener").RemoveComponent<PointerClickListener>();
//            }
        }
        
        public void EnterActivity()
        {
            _stepListen2.gameObject.Hide();
            pos2.gameObject.Hide();
            pos3 = transform.GetRectTransform("TopLayout/FansList/Fans/Step3");
//            _highlight.transform.SetParent(pos3.transform.parent, false);
//            _highlight.CopyRectTransform(pos3);
            pos3.gameObject.Show();
            if (!hasPlaypos3)
            {
                hasPlaypos3 = true;
                GuideArrow.DoAnimation(pos3);
            }


            RectTransform gv3 = transform.GetRectTransform("GuideView3");
            
            //_lightTweener.Kill();
//            _lightTweener=_highlight.DOLocalMoveX(_highlight.localPosition.x+10,0.5f).SetLoops(-1, LoopType.Yoyo);
            
            _guideView.CopyRectTransform(gv3);

            gv3.SetNormalPivot();
            
            _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint5");
//            _step++;
            Debug.LogError(_step);
        }

        public void StartActSuccess()
        {
            Debug.LogError("here");
            pos5.gameObject.Hide();
//            RectTransform pos6 = transform.GetRectTransform("ChangeActivityBtn/Step6");
//            pos6.gameObject.SetActive(true);
//            GuideArrow.DoAnimation(pos6);
//            _highlight.transform.SetParent(pos6.transform.parent, false);
//            _highlight.CopyRectTransform(pos6);
//                
//            RectTransform gv6 = transform.GetRectTransform("GuideView6");
//            _guideView.CopyRectTransform(gv6);
//
//            _lightTweener.Kill();
//            _lightTweener=_highlight.DOLocalMoveX(_highlight.localPosition.x+10,0.5f).SetLoops(-1, LoopType.Yoyo);
//            
//            gv6.SetNormalPivot();
//            _guideText.text = I18NManager.Get("Guide_SupporterActGuideViewHint6");
//            _step++; 
            

            transform.RemoveComponent<PointerClickListener>();
                    
            SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_SERVER, ModuleConfig.MODULE_SUPPORTERACTIVITY));
        }
        
    }
}