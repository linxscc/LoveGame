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
    public class VisitGuideView : View
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
        Transform _weatherBtn ;
        Transform _weatherObj;

        private GameObject _mapStoryItemBtn;
        private GameObject _mapStoryObj;

        private void Awake()
        {
            _weatherBtn = transform.Find("VisitBgImage/VisitSelectItem3/Visit/Button");
            _weatherObj = transform.Find("VisitBgImage");
            _guideView = transform.GetRectTransform("GuideView");
            _guideText = transform.GetText("GuideView/DialogFrame/Text");
            _bgImage = transform.GetImage("BgImage");
           // _arrow = transform.GetImage("Arrow/Image");
            _arrowObj = transform.Find("Arrow");

            _blessingObj = transform.Find("Blessing").gameObject;
            _jumpObj = transform.Find("Jump").gameObject;
            transform.Find("Blessing").GetComponent<Button>().onClick.AddListener(()=> {
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
            _weatherObj.gameObject.Hide();

            _mapStoryObj.Hide();
            _mapStoryObj = transform.Find("MapObj").gameObject;
            _mapStoryItemBtn = _mapStoryObj.transform.Find("Content/MapBg/mapStoryItem").gameObject;
        
     
        }

        private void Start()
        {
            _weatherBtn.GetButton().onClick.AddListener(() => {
                Debug.Log("VisitSelectItemWeatherClick");
                EventDispatcher.TriggerEvent<PlayerPB>(EventConst.VisitSelectItemVisitClick, PlayerPB.YanJi);
                OnNextStep(null);
            });
            _mapStoryItemBtn.transform.GetButton().onClick.AddListener(() => {
                //OnNextStep(null);
                Debug.Log("MapStoryItemClick");
                EventDispatcher.TriggerEvent(EventConst.VisitFirsetLevelItem);
                OnNextStep(null);
            });
            
            GuideArrow.DoAnimation(_arrowObj.transform);
            
            InitGuide();
        }
     
        public void InitGuide()
        {
            _lastClickTime = 0;
            _guideView.gameObject.Show();
            _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint1");
            _bgImage.gameObject.Show();
            _weatherObj.gameObject.Hide();


            _step = 1;
        }


        //private void OnNextStep(GameObject go)
        //{
        //    if (Time.realtimeSinceStartup - _lastClickTime < 0.3f)
        //        return;
        //    if (_step == 1)
        //    {
        //        _arrowObj.eulerAngles = new Vector3(0, 0, 0);
        //        _arrowObj.transform.position = _weatherBtn.position;
        //        _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
        //        _arrowObj.gameObject.Show();
        //        _bgImage.gameObject.Hide();
        //        _weatherObj.gameObject.Show();
        //        _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint2");

        //    }
        //    else if (_step == 2)
        //    {
        //        _bgImage.gameObject.Show();
        //        _weatherObj.gameObject.Hide();
        //        _arrowObj.eulerAngles = new Vector3(0, 0, 180);
        //        _arrowObj.transform.position = _blessingObj.transform.position;
        //        _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
        //        _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint3");

        //    }
        //    else if (_step == 3)
        //    {
        //        _bgImage.gameObject.Show(); 
        //        _arrowObj.eulerAngles = new Vector3(0, 0, 260);
        //        _arrowObj.transform.position = _jumpObj.transform.position;
        //        _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
        //        _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint4");


        //    }
        //    else if (_step == 4) 
        //    {
        //        SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_LOCAL, ModuleConfig.MODULE_VISIT));
        //    }


        //    _lastClickTime = Time.realtimeSinceStartup;
        //    _step++;
        //}

        private void OnNextStep(GameObject go)
        {
            if (Time.realtimeSinceStartup - _lastClickTime < 0.3f)
                return;
            if (_step == 1)
            {
                _guideView.gameObject.Hide();
                _arrowObj.eulerAngles = new Vector3(0, 0, 0);
                _arrowObj.transform.position = _weatherBtn.position;
                _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
                _arrowObj.gameObject.Show();
                _bgImage.gameObject.Hide();
                _weatherObj.gameObject.Show();
                _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint2");

            }
            else if (_step == 2)
            {
                _bgImage.gameObject.Hide();
                _guideView.gameObject.Hide();
                //_bgImage.gameObject.Show();
                _weatherObj.gameObject.Hide();
                _mapStoryObj.Show();
                _arrowObj.eulerAngles = new Vector3(0, 0, 0);
                _arrowObj.transform.position = _mapStoryItemBtn.transform.position;
                _arrowObj.transform.localPosition += new Vector3(0, 0, 0);
                _guideText.text = I18NManager.Get("Guide_VisitGuideViewHint3");

            }
            else if (_step == 3)
            {
                SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_LOCAL, ModuleConfig.MODULE_VISIT));
            }


            _lastClickTime = Time.realtimeSinceStartup;
            _step++;
        }


        private void LateUpdate()
        {
            if(_step==3)
            {
                _arrowObj.transform.position = _mapStoryItemBtn.transform.position;
            }
        }

    }
}
