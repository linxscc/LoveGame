using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DG.Tweening;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module.Guide;

namespace Module.Guide.ModuleView.DrawCard
{
    public class DrawCardGuideView : View
    {
        private RectTransform _guideView;
        private RectTransform _highlight;
        private Text _guideText;
        private int _step;
        private float _lastClickTime;
        private Image _bg;

        private Image _bgImage;
        //private Image _arrow;
        private Transform _arrowObj;
        Transform _backBtn;
        
        Transform _pagingLowerBtn;

        Transform _Content;
        bool isGoldStep = false;
        private void Awake()
        {
            _pagingLowerBtn = transform.Find("TabBtnBar/Grid/1");
            _backBtn = transform.Find("BackBtn");
            _Content = transform.Find("GemItem");
            _guideView = transform.GetRectTransform("GuideView");
            _highlight = transform.GetRectTransform("Highlight");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");
            _bgImage = transform.GetImage("BgImage");
            //_arrow = transform.GetImage("Arrow/Image");
            _arrowObj = transform.Find("Arrow");
            _bg = transform.GetImage();
            _step = 0;
            // _step++;
            // OnNextStep(null);
            //  PointerClickListener.Get(gameObject).onClick = OnNextStep;
            //  PointerClickListener.Get(_highlight.gameObject).onClick = OnNextStep;
            // PointerClickListener.Get(_bgImage.gameObject).onClick = OnNextStep;
            _bgImage.gameObject.Hide();
            _highlight.gameObject.Hide();
        }

        private void Start()
        {
            GuideArrow.DoAnimation(_arrowObj);
            Transform tf = transform.Find("GemItem/GemOnceBtn");
            tf.GetButton().onClick.AddListener(() =>
            {
                if (isGoldStep)
                {
                    OnNextGoldStep(null);

                }
                else
                {
                    OnNextStep(null);

                }

            });


            _pagingLowerBtn.GetButton().onClick.AddListener(() =>
            {
                _pagingLowerBtn.gameObject.Hide();
                SendMessage(new Message(MessageConst.CMD_DRAWCARD_GOTO_GOLD_DRAW, Message.MessageReciverType.UnvarnishedTransmission));
                OnNextGoldStep(null);
            });
            _backBtn.GetButton().onClick.AddListener(BackClick);
            //var lowerTweener = _arrow.transform.DOBlendableLocalMoveBy(new Vector3(-100, 0, 0), 0.6f);
            //lowerTweener.SetLoops(-1, LoopType.Yoyo);
            //lowerTweener.Play();
        }

        private void BackClick()
        {
            if (isGoldStep)
            {
                _backBtn.gameObject.Hide();
                if (_step == 5)
                {
                    gameObject.Hide();
                    ModuleManager.Instance.GoBack();
                    //SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD, Message.MessageReciverType.UnvarnishedTransmission));
                    // OnNextGoldStep(null);
                }
                else if (_step == 6)
                {
                    ModuleManager.Instance.GoBack();
                }
            }
            else
            {
                _backBtn.gameObject.Hide();
                SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD, Message.MessageReciverType.UnvarnishedTransmission));
                InitGoldGuide();
            }
        }


        public void InitGoldGuide()
        {
            isGoldStep = true;
            _Content.gameObject.Hide();
            _guideView.gameObject.Show();
            _guideText.text = I18NManager.Get("Guide_DrawCardGuideViewHint2");
            _arrowObj.eulerAngles = new Vector3(0, 0, 45);
            _arrowObj.transform.position = _pagingLowerBtn.position;
            _arrowObj.transform.localPosition += new Vector3(100, -20, 0);
            _arrowObj.gameObject.Show();
            _pagingLowerBtn.gameObject.Show();
            _backBtn.gameObject.Hide();
            _step = 2;
            //var lowerTweener = _arrow.transform.DOBlendableLocalMoveBy(new Vector3(-100, 0, 0), 0.6f);
            //lowerTweener.SetLoops(-1, LoopType.Yoyo);
            //lowerTweener.Play();
        }
        public void InitGemGuide()
        {
            isGoldStep = false;
            _Content.gameObject.Show();
            _guideView.gameObject.Show();
            _guideText.text = I18NManager.Get("Guide_DrawCardGuideViewHint1");

            Transform tf = transform.Find("GemItem/GemOnceBtn");
            _arrowObj.eulerAngles = new Vector3(0, 0, 150);
            _arrowObj.transform.position = tf.position;
            _arrowObj.transform.localPosition += new Vector3(-200, 100, 0);
            _arrowObj.gameObject.Show();
            _backBtn.gameObject.Hide();
            _step = 3;
            //var lowerTweener = _arrow.transform.DOBlendableLocalMoveBy(new Vector3(-100, 0, 0), 0.6f);
            //lowerTweener.SetLoops(-1, LoopType.Yoyo);
            //lowerTweener.Play();
        }

        private void OnNextGoldStep(GameObject go)
        {


            if (Time.realtimeSinceStartup - _lastClickTime < 1f)
                return;
            if (_step == 2)
            {
                _guideView.gameObject.Hide();
                _pagingLowerBtn.gameObject.Hide();
                Transform tf = transform.Find("GemItem/GemOnceBtn");
                transform.Find("GemItem").gameObject.Show();
                _arrowObj.eulerAngles = new Vector3(0, 0, 150);
                _arrowObj.transform.position = tf.position;

                _arrowObj.transform.localPosition += new Vector3(-200, 100, 0);
                _arrowObj.gameObject.Show();
            }
            else if (_step == 3)
            {
                SendMessage(new Message(MessageConst.CMD_DRAWCARD_GOLD_DRAW_ONCE, Message.MessageReciverType.UnvarnishedTransmission));
                _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
                _arrowObj.gameObject.Hide();
                transform.Find("GemItem").gameObject.Hide();
                _guideView.gameObject.Hide();
                GuideManager.Hide();
            }
            else if (_step == 4)
            {
                _arrowObj.eulerAngles = new Vector3(0, 0, 60);
                _arrowObj.transform.position = _backBtn.transform.position;
                _arrowObj.transform.localPosition += new Vector3(-60, 70, 0);
                _arrowObj.gameObject.Show();

                _backBtn.gameObject.Show();


            }
            else if (_step == 5)
            {
                _arrowObj.eulerAngles = new Vector3(0, 0, 60);
                _arrowObj.transform.position = _backBtn.transform.position;
                _arrowObj.transform.localPosition += new Vector3(-60, 70, 0);
                _arrowObj.gameObject.Show();
                _backBtn.gameObject.Show();

            }
            else
            {


            }
            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }


        public void ShowNextStep()
        {
            OnNextGoldStep(null);
        }

        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 1f)
                return;
            if (_step == 2)
            {

            }
            else if (_step == 3)
            {
                _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
                _arrowObj.gameObject.Hide();
                transform.Find("GemItem").gameObject.Hide();
                _guideView.gameObject.Hide();
                GuideManager.Hide();
                SendMessage(new Message(MessageConst.CMD_DRAWCARD_GEM_DRAW_ONCE, Message.MessageReciverType.UnvarnishedTransmission));

            }
            else if (_step == 4)
            {
                _arrowObj.eulerAngles = new Vector3(0, 0, 150);
                _arrowObj.transform.position = _backBtn.transform.position;
                _arrowObj.gameObject.Show();

                _backBtn.gameObject.Show();
            }
            else if (_step == 5)
            {
            }
            else
            {
            }

            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }

    }
}
