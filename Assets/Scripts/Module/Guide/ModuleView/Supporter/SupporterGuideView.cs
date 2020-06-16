using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Guide.ModuleView.Supporter
{
    public class SupporterGuideView : View
    {
        private RectTransform _guideView;
//        private RectTransform _highlight;
//        private Transform _particle;
        private Text _guideText;
        private int _step;
        private float _lastClickTime;
        private Image _bg;

        private Tweener _stepTweener;
        private RectTransform pos2;
        private RectTransform pos3;
        private RectTransform pos4;
        private RectTransform pos5;

        private void Awake()
        {
            _guideView = transform.GetRectTransform("GuideView");
//            _highlight = transform.GetRectTransform("Arrow");
//            _particle = transform.Find("Particle");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");

            //_highlight.gameObject.Hide();

            _bg = transform.GetImage();

            _step = 1;

            //第一步
            _guideText.text = I18NManager.Get("Guide_SupporterGuideViewHint1");
            _step++;

            PointerClickListener.Get(gameObject).onClick = OnNextStep;
        }

        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 1f)
                return;
            if (_step == 2)
            {
                _bg.DOColor(new Color(0, 0, 0, 0), 0.3f);
//                
//                _highlight.GetImage().color = new Color(1, 1, 1, 0.5f);
//                _highlight.gameObject.Show();
//                _particle.gameObject.Show();
//                _highlight.GetImage().DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);


 
                
                
                pos2= transform.GetRectTransform("List/TopLayout/Title/Step2");
                pos2.gameObject.Show();
//                _highlight.transform.SetParent(pos2.transform, false);
//                _particle.SetParent(pos2.transform, false);
//                _highlight.CopyRectTransform(pos2);
                GuideArrow.DoAnimation(pos2.transform);
//
//              
//                _stepTweener=_highlight.DOLocalMove(new Vector3(237,-290), 0.5f).SetLoops(-1, LoopType.Yoyo);//253,-306
                
                RectTransform gv2 = transform.GetRectTransform("GuideView2");
                _guideView.CopyRectTransform(gv2);
                
                gv2.SetNormalPivot();

                //                _guideView.DOLocalMove(gv2.localPosition, 0.5f);
                //                _guideView.DOAnchorPos(gv2.anchoredPosition, 0.5f);

                _guideText.text = I18NManager.Get("Guide_SupporterGuideViewHint2");
            }
            else if (_step == 3)
            {
                pos2.gameObject.Hide();
                pos3= transform.GetRectTransform("List/TopLayout/Active/Step3");
                pos3.gameObject.Show();
//                _highlight.transform.SetParent(pos3.transform.parent, false);
//                _particle.SetParent(pos3.transform.parent, false);
                GuideArrow.DoAnimation(pos3);
//                _highlight.CopyRectTransform(pos3);
//                _stepTweener.Kill();
//                _stepTweener=_highlight.DOLocalMoveX(20,0.5f).SetLoops(-1, LoopType.Yoyo);
                //                _highlight.anchoredPosition = pos3.anchoredPosition;
//                _highlight.sizeDelta = pos3.sizeDelta;


                RectTransform gv2 = transform.GetRectTransform("GuideView2");
                _guideView.CopyRectTransform(gv2);

                gv2.SetNormalPivot();

                //                _guideView.DOAnchorPos(gv3.anchoredPosition, 0.5f);
                //                _guideView.DOLocalMove(gv2.localPosition, 0.5f);

                _guideText.text = I18NManager.Get("Guide_SupporterGuideViewHint3");

            }
            else if(_step == 4)
            {
                pos3.gameObject.Hide();
                pos4= transform.GetRectTransform("List/TopLayout/Active/Step4");
                pos4.gameObject.Show();
//                _highlight.transform.SetParent(pos4.transform.parent, false);
//                _particle.SetParent(pos4.transform.parent, false); 
                GuideArrow.DoAnimation(pos4);
//                _highlight.CopyRectTransform(pos4);
//                _stepTweener.Kill();
//                _stepTweener=_highlight.DOLocalMoveX(20,0.5f).SetLoops(-1, LoopType.Yoyo);
                
                RectTransform gv3 = transform.GetRectTransform("GuideView3");
                _guideView.CopyRectTransform(gv3);

                gv3.SetNormalPivot();

                //                _guideView.DOAnchorPos(gv3.anchoredPosition, 0.5f);
                //                _guideView.DOLocalMove(gv3.localPosition, 0.5f);

                _guideText.text = I18NManager.Get("Guide_SupporterGuideViewHint4");                           
            }
//            else if(_step==5)
//            {
//               // pos3.gameObject.Hide();
//                pos4.gameObject.Hide();
//                pos5= transform.GetRectTransform("AirborneGameBtn/Step5");
//                pos5.gameObject.Show();
//                //_highlight.transform.SetParent(pos4.transform.parent, false);
//                GuideArrow.DoAnimation(pos5);
////                _highlight.CopyRectTransform(pos4);
////                RectTransform rect = _highlight.GetRectTransform();
////                
////                _stepTweener.Kill();
////                _stepTweener=_highlight.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
////                    rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
//                
//                RectTransform gv3 = transform.GetRectTransform("GuideView4");
//                _guideView.CopyRectTransform(gv3);
//
//                gv3.SetNormalPivot();
//
//                //                _guideView.DOAnchorPos(gv3.anchoredPosition, 0.5f);
//                //                _guideView.DOLocalMove(gv3.localPosition, 0.5f);
//
//                _guideText.text = I18NManager.Get("Guide_SupporterGuideViewHint5");
//            }
            else
            {
                SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_LOCAL, ModuleConfig.MODULE_SUPPORTER));
            }


            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }
    }
}