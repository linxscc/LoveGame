using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using DG.Tweening;
using game.tools;
using System;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module.Guide;

namespace Module.Guide.ModuleView.LoveDiary
{
    public class LoveDiaryGuideView : View
    {
        private RectTransform _guideView;
       //private RectTransform _highlight;
        private Text _guideText;
        private int _step;
        private float _lastClickTime;
        private Image _bg;

        private Image _bgImage;
        //private Image _arrow;
        private Transform _arrowObj;

        private void Awake()
        {
            _guideView = transform.GetRectTransform("GuideView");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");
            _bgImage = transform.GetImage("BgImage");
        //    _arrow = transform.GetImage("Arrow/Image");
            _arrowObj = transform.Find("Arrow");
        //    _bg = transform.GetImage();
      //      _bg.gameObject.Hide();
            _step = 1;
            //第一步
            //_guideText.text = "应援会内可以查看应援会的详细情况";
            _step++;
       
            PointerClickListener.Get(_bgImage.gameObject).onClick = OnNextStep;
            _bgImage.gameObject.Hide();
            _arrowObj.gameObject.Hide();
            _guideView.gameObject.Hide();
        }

        private void Start()
        {
            //var lowerTweener = _arrow.transform.DOBlendableLocalMoveBy(new Vector3(-100, 0, 0), 0.6f);
            //lowerTweener.SetLoops(-1, LoopType.Yoyo);
            //lowerTweener.Play();
            OnNextStep(null);
        }

        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 0.3f)
                return;
            if (_step == 2)
            {
                _bgImage.gameObject.Hide();
                DateTime dt= DateUtil.GetTodayDt();
                List<DateTime> dts = DataModel.LoveDiaryModel.ToDays(dt);
                int idx = 0;
                for(int i=0;i<dts.Count;i++)
                {
                    if(dts[i].Year==dt.Year&& dts[i].Month == dt.Month && dts[i].Day == dt.Day )
                    {
                        idx = i;
                        break;
                    }
                }
                Transform posBtn = transform.Find("Scroll View/Viewport/Content").GetChild(idx);
                RectTransform pos2 = posBtn.GetComponent<RectTransform>();

                _arrowObj.gameObject.Show();
                GuideArrow.DoAnimation(_arrowObj);

                _arrowObj.eulerAngles = new Vector3(0, 0, 270);
                _arrowObj.transform.position = pos2.position;
                _arrowObj.transform.localPosition += new Vector3(0, -30, 0);
                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint1");
                _guideView.gameObject.Show();

                if (idx < 21)
                {
                    _guideView.transform.localPosition = new Vector3(0, -888, 0);
                }
                else
                {
                    _guideView.transform.localPosition = new Vector3(0, 187, 0);
                }

                Transform tf1 = transform.Find("Scroll ViewClick");                
                tf1.parent = posBtn;
                tf1.transform.localPosition = new Vector3(0, 0, 0);
                tf1.GetButton().onClick.AddListener(()=> {
                    OnNextStep(null);
                });

            }
            else if (_step == 3)
            {

                transform.Find("Scroll View").gameObject.Hide();
                DateTime dt = DateUtil.GetTodayDt();
                SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_TEMPLATE_PANEL, Message.MessageReciverType.UnvarnishedTransmission, dt));

                RectTransform pos3 = transform.GetRectTransform("List/TopLayout/Active/Step3");

                _guideView.transform.localPosition = new Vector3(0, -888, 0);

                Transform TemplateViewScroll = transform.Find("TemplateViewScroll View");
                TemplateViewScroll.gameObject.Show();
                Transform tf = transform.Find("TemplateViewScroll View/Viewport/Content/TemplateItem");
                UIEventListener.Get(tf.gameObject).onClick = (p) =>
                {
                    TemplateViewScroll.gameObject.Hide();
                    OnNextStep(null);
                };

                _arrowObj.gameObject.Show();
                _arrowObj.eulerAngles = new Vector3(0, 0, 180);
                _arrowObj.transform.position = tf.transform.position;

            }
            else if (_step == 4)
            {
                _arrowObj.gameObject.Hide();
                int elementId= 5001;
                SendMessage(new Message(MessageConst.CMD_LOVEDIARY_TEMPLATE_SELECT, Message.MessageReciverType.UnvarnishedTransmission, elementId));
  
                RectTransform pos4 = transform.GetComponent<RectTransform>();

                _bg.DOColor(new Color(0, 0, 0, 0.4f), 0.3f);
                RectTransform gv3 = transform.GetRectTransform("GuideView3");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();

                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint3");
                _bgImage.gameObject.Show();

            }
            else if (_step == 5)
            {
                //_bgImage.gameObject.Hide();
                _bgImage.gameObject.Show();
     
                SendMessage(new Message(MessageConst.CMD_LOVEDIARY_ENTER_SELECT_IMAGE, Message.MessageReciverType.UnvarnishedTransmission));
                //SendMessage(new Message(MessageConst.CMD_LOVEDIARY_TEMPLATE_SELECT, Message.MessageReciverType.UnvarnishedTransmission, elementId));
                //_highlight.gameObject.Show();
                RectTransform pos4 = transform.GetRectTransform("List/BCLayout/Step4");

                _arrowObj.gameObject.Hide();
               // _arrowObj.eulerAngles = new Vector3(0, 0, 270);
              //  _arrowObj.transform.position = _highlight.position;

                _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
                RectTransform gv3 = transform.GetRectTransform("GuideView2");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();

                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint4");
            }
            else if (_step == 6)
            {
                SendMessage(new Message(MessageConst.CMD_LOVEDIARY_ENTER_SELECT_NONE, Message.MessageReciverType.UnvarnishedTransmission));

 
                _bgImage.gameObject.Hide();

                RectTransform pos4 = transform.GetRectTransform("List/MCLayout/Step5");

                _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
                RectTransform gv3 = transform.GetRectTransform("GuideView3");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();

                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint5");

                Transform Label = transform.Find("Label");
                Label.gameObject.Show();
                Label.GetButton("SelectBtn").onClick.AddListener(()=>{
                    OnNextStep(null);
                    Label.gameObject.Hide();
                });

                _arrowObj.gameObject.Show();
                _arrowObj.eulerAngles = new Vector3(0, 0, 120);
                 _arrowObj.transform.position = Label.position;

            }
            else if (_step == 7)
            {
                _bgImage.gameObject.Show();
                _arrowObj.gameObject.Hide();
                SendMessage(new Message(MessageConst.CMD_LOVEDIARY_ENTER_SELECT_LABEL, Message.MessageReciverType.UnvarnishedTransmission,true));
                RectTransform gv3 = transform.GetRectTransform("GuideView4");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();

                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint6");
            }
            else if (_step == 8)
            {

                _bgImage.gameObject.Show();
                _arrowObj.gameObject.Hide();
               // SendMessage(new Message(MessageConst.CMD_LOVEDIARY_ENTER_SELECT_LABEL, Message.MessageReciverType.UnvarnishedTransmission, false));
                RectTransform gv3 = transform.GetRectTransform("GuideView4");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();
                _guideText.text = I18NManager.Get("Guide_LoveDiaryGuideViewHint7");
                GuideManager.Hide();

            }
            else
            {
                SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_SERVER, ModuleConfig.MODULE_LOVEDIARY));
            }

            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }
    }
}