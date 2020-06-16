using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Common;
using DG.Tweening;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Guide.ModuleView.Visit
{
    public class VisitGuideBlessView : View
    {
        private RectTransform _guideView;
        private RectTransform _highlight;
        private Text _guideText;
        private int _step;
        private float _lastClickTime;
        //private Image _bg;

        private Image _bgImage;
        //    private Image _arrow;
        private Transform _arrowObj;

        private GameObject _blessingObj;
        private GameObject _jumpObj;
        Transform _weatherBtn;
        

        private void Awake()
        {
            _weatherBtn = transform.Find("WeatherBtn");
            _guideView = transform.GetRectTransform("GuideView");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");
            _bgImage = transform.GetImage("BgImage");
            // _arrow = transform.GetImage("Arrow/Image");
            _arrowObj = transform.Find("Arrow");

            _blessingObj = transform.Find("Blessing").gameObject;
            _jumpObj = transform.Find("Jump").gameObject;
            transform.Find("Blessing").GetComponent<Button>().onClick.AddListener(() => {
                Debug.LogError("Blessing ");

            });

            //_bg = transform.GetImage();
            _step = 0;
            // _step++;
            // OnNextStep(null);
            //  PointerClickListener.Get(gameObject).onClick = OnNextStep;
            //  PointerClickListener.Get(_highlight.gameObject).onClick = OnNextStep;
            UIEventListener.Get(_bgImage.gameObject).onClick = OnNextStep;
            _bgImage.gameObject.Hide();
            _weatherBtn.gameObject.Hide();
        }

        private void Start()
        {
            _weatherBtn.transform.GetButton().onClick.AddListener(() => {
                //OnNextStep(null);
                Debug.Log("MapStoryItemClick");
                //EventDispatcher.TriggerEvent<PlayerPB>(EventConst.VisitSelectItemWeatherClick, PlayerPB.YanJi);

                EventDispatcher.TriggerEvent(EventConst.VisitLevelItemGotoWeather);

                OnNextStep(null);
            });
            
            GuideArrow.DoAnimation(_arrowObj.transform);

            InitGuide();
        }

        public void InitGuide()
        {
            _lastClickTime = 0;
            _guideView.gameObject.Show();
            _arrowObj.eulerAngles = new Vector3(0, 0, 260);
            _arrowObj.transform.position = _weatherBtn.transform.position;
            _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
            _arrowObj.gameObject.Show();
            _bgImage.gameObject.Hide();
            _weatherBtn.gameObject.Show();
            _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint2");
            _step = 1;


        }


        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 0.3f)
                return;
            if (_step == 1)
            {
                _bgImage.gameObject.Show();
                _weatherBtn.gameObject.Hide();
                _arrowObj.eulerAngles = new Vector3(0, 0, 180);
                _arrowObj.transform.position = _blessingObj.transform.position;
                _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
                _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint3");

            }
            else if (_step == 2)
            {
                _bgImage.gameObject.Show();
                _arrowObj.eulerAngles = new Vector3(0, 0, 260);
                _arrowObj.transform.position = _jumpObj.transform.position;
                _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
                _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint4");


            }
            else if (_step == 3)
            {
                SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_LOCAL, GuideEnumType.VISIT_BLESS.ToString()));
            }


            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }

    }
}
