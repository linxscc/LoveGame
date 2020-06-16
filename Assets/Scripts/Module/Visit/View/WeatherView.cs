using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;

public class WeatherView : View
{
    VisitVo _curVisitVo;

    Transform _result;
    Button _jump;
    Button _blessing;
    Button _description;
    Transform _SuccessRate;
    private void Awake()
    {
        _result = transform.Find("Result");

        _SuccessRate = transform.Find("SuccessRate");
        _jump = transform.Find("Jump").GetComponent<Button>();
        _jump.onClick.AddListener(Jump);
        _blessing = transform.Find("Blessing").GetComponent<Button>();
        // _blessing.onClick.AddListener(Blessing);
        //UIEventListener.Get(_blessing.gameObject).onDown = OnBlessingDown;
     //   UIEventListener.Get(_blessing.gameObject).onUp = OnBlessingUp;
        UIEventListener.Get(_blessing.gameObject).onClick = OnBlessingClick;
        _description = transform.Find("DescriptionBtn").GetComponent<Button>();
        _description.onClick.AddListener(Description);
        UIEventListener.Get(transform.Find("Description").gameObject).onClick = CloseDescription;
        UIEventListener.Get(_result.gameObject).onClick = (obj) =>
        {
            if (!_isCanClickResult)
            {
                return;
            }
            _result.gameObject.Hide();
            SetButtonShow();
            SendMessage(new Message(MessageConst.CMD_VISIT_WEATHER_RESULT_CLICK, Message.MessageReciverType.CONTROLLER));
        };
        InitBar();

        _blessing.transform.GetText("Text").text = I18NManager.Get("Visit_Weather_LongPressBless");

    }

    private void OnBlessingClick(GameObject go)
    {
        if(isButtonDown)
        {
            OnBlessingUp(null);
        }
        else
        {
            OnBlessingDown(null);
        }
    }

    /// <summary>
    /// 祈福按钮按下
    /// </summary>
    /// <param name="eventData"></param>
    private void OnBlessingDown(PointerEventData eventData)
    {
        if (GlobalData.PlayerModel.PlayerVo.Gem < _curVisitVo.BlessCost)
        {
            FlowText.ShowMessage(I18NManager.Get("Shop_NotEnoughGem"));
            return;
        }
        isButtonDown = true;
        SetButtonHide();
    }
    /// <summary>
    /// 祈福按钮抬起
    /// </summary>
    bool _isSendBless = false;//是否发送协议
    private void OnBlessingUp(PointerEventData eventData)
    {
        isButtonDown = false;
        if (_isSendBless)
            return;
        _isSendBless = true;
        var result = GetCurBlessResult();
        Blessing(result);
    }

    /// <summary>
    /// 判断祈福结果
    /// </summary>
    /// <returns></returns>
    private BlessResult GetCurBlessResult()
    {
        float CursorPosX = _cursor.localPosition.x;

        if (Mathf.Abs(CursorPosX - _best.localPosition.x) < best * 0.5f)
        {
            return BlessResult.Best;
        }
        if (Mathf.Abs(CursorPosX - _best.localPosition.x) < better * 0.5f)
        {
            return BlessResult.Better;
        }
        return BlessResult.Invalid;
    }


    private void ChangeWeather(VISIT_WEATHER from, VISIT_WEATHER to)
    {
        Debug.Log("ChangeWeather from " + from + "   to   " + to);
        _from = from;
        _to = to;
        //GameObject formObj = transform.Find("Weather/" + from).gameObject;
        // Animator fromAni = formObj.GetComponent<Animator>();
        //fromAni.SetInteger("State", 1);


        ClientTimer.Instance.DelayCall(()=> {

            SetCurWeather(to);
            SetButtonShow();
            _isSendBless = false;
        },1f);


        BlessingEnd();

    }


    public void StartChange()
    {
        GameObject changeObj = transform.Find("Weather/Change").gameObject;
        changeObj.SetActive(true);
        //GameObject formObj = transform.Find("Weather/" + _from).gameObject;
        //Animator fromAni = formObj.GetComponent<Animator>();
        //fromAni.SetInteger("State", 0);
    }
    public void StartEnter()
    {
        GameObject toObj = transform.Find("Weather/" + _to).gameObject;

        toObj.SetActive(true);
        Animator toAni = toObj.GetComponent<Animator>();
        toAni.SetInteger("State", 2);
    }

    private void CloseDescription(GameObject obj)
    {
        transform.Find("Description").gameObject.SetActive(false);
    }
    private void Description()
    {
        Debug.Log("Description");
        transform.Find("Description").gameObject.SetActive(true);
    }

    private void SetWeatherDescription(List<WeatherPB> weatherPBs)
    {
        foreach (var v in weatherPBs)
        {
            Debug.LogError(v.WeatherId + " " + v.Name);
            transform.Find("Description/Bg/" + v.WeatherId + "/Text").GetComponent<Text>().text
                = I18NManager.Get("Visit_Weather_DescriptTips", v.Name, v.VisitingNum);

            string pathName = "UIAtlas_Visit_levelWeather" + v.WeatherId;
            Image w = transform.Find("Description/Bg/" + v.WeatherId + "/Image").GetComponent<Image>();
            w.sprite =
                AssetManager.Instance.GetSpriteAtlas(pathName);
            w.SetNativeSize();
        }

        //transform.Find("Description/Bg/0/Text").GetComponent<Text>().text = I18NManager.Get("Visit_Weather_DescriptTips", fine);
        //transform.Find("Description/Bg/1/Text").GetComponent<Text>().text = I18NManager.Get("Visit_Weather_DescriptTips", cloudy);
        //transform.Find("Description/Bg/2/Text").GetComponent<Text>().text = I18NManager.Get("Visit_Weather_DescriptTips", rain);
    }


    VISIT_WEATHER _preBless;
    bool _isCanClickResult = false;



    public void FailedBless()
    {
        SetButtonShow();
        Debug.Log("FailedBless");
        _result.gameObject.Hide();
        _isCanClickResult = true;
        _isSendBless = false;

    }
    private void Blessing(BlessResult result)
    {

        if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
        {
            _isSendBless = false;
            return;
        }

        _isCanClickResult = false;
        //  SetButtonHide();
        _blessing.gameObject.Hide();
        Debug.Log("Blessing");
        _result.gameObject.Show();
        _preBless = _curVisitVo.CurWeather;

        SendMessage(new Message(MessageConst.CMD_VISIT_WEATHER_BLESSING_CLICK, Message.MessageReciverType.CONTROLLER, result));
    }

    private void SetButtonShow()
    {
         _jump.gameObject.Show();

        if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
        {
            _blessing.gameObject.Hide();
        }
        else
        {
            _blessing.gameObject.Show();
        }
        // _blessing.gameObject.Show();
        _description.gameObject.Show();
        // _SuccessRate.gameObject.Show();
        SendMessage(new Message(MessageConst.MODULE_VISIT_WEATHER_SET_BACKBTNSHOWORHIDE, Message.MessageReciverType.DEFAULT, true));
    }
    private void SetButtonHide()
    {
         _jump.gameObject.Hide();
       //_blessing.gameObject.Hide();
        _description.gameObject.Hide();
        //   _SuccessRate.gameObject.Hide();
        SendMessage(new Message(MessageConst.MODULE_VISIT_WEATHER_SET_BACKBTNSHOWORHIDE, Message.MessageReciverType.DEFAULT, false));
    }
    public void BlessingEnd()
    {
        int dff = _curVisitVo.GetMaxVisitTimeByWeather(_to) -
        _curVisitVo.GetMaxVisitTimeByWeather(_from);

        if(_to==_from)
        {
            FlowText.ShowMessage(I18NManager.Get("Visit_Weather_FailedBlessAddTimeTips"));
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Visit_Weather_BlessAddTimeTips", dff));
        }
       
        _isCanClickResult = true;
        SetSuccessRate();

    }

    private void Jump()
    {
        SendMessage(new Message(MessageConst.CMD_VISIT_WEATHER_JUMP_CLICK, Message.MessageReciverType.CONTROLLER));
    }

    RectTransform _invalid;
    RectTransform _better;
    RectTransform _best;
    RectTransform _cursor;
    GameObject _bar;
    private void InitBar()
    {
        _bar = transform.Find("Bar").gameObject;
        _invalid = transform.Find("Bar/Invalid").GetRectTransform();
        _better = transform.Find("Bar/Better").GetRectTransform();
        _best = transform.Find("Bar/Best").GetRectTransform();
        _cursor = transform.Find("Bar/Cursor").GetRectTransform();
    }
    int invalid = 1000;
    int better = 600;
    int best = 150;
    private void SetWeatherBar(WeatherPB pB)
    {

        best = pB.Best * 10;
        better = best + pB.Better * 10;
        invalid = better + pB.Same * 10;

        _invalid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, invalid);
        _better.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, better);
        _best.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, best);
        _cursor.anchoredPosition = new Vector2(-invalid * 0.5f, _cursor.anchoredPosition.y);

        int min = (int)( -invalid * 0.5f + better);
        int max = (int)(invalid * 0.5f - better*0.5f);
        int random = UnityEngine.Random.Range(min, max);
        _better.anchoredPosition = new Vector2(random, _better.anchoredPosition.y);
        _best.anchoredPosition = new Vector2(random, _best.anchoredPosition.y);

    }
    bool _isButtonDown = false;
    bool isButtonDown
    {
        set
        {
            if (_isButtonDown == value)
                return;
            _isButtonDown = value;
            Debug.LogError(_isButtonDown);
            _blessing.transform.GetText("Text").text = _isButtonDown ?
                I18NManager.Get("Visit_Weather_Stop") :
                I18NManager.Get("Visit_Weather_LongPressBless");
            _blessing.transform.Find("Image").gameObject.SetActive(!_isButtonDown);
        }
        get
        {
            return _isButtonDown;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isButtonDown = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            isButtonDown = false;
        }
        if (isButtonDown)
        {
            UpdateCursor(Time.deltaTime);
        }
    }
    int isForward = 1;
    float speed = 1500;
    float maxLenth = 500;
    private void UpdateCursor(float delta)
    {
        float offset = delta * speed * isForward;
        float curX = _cursor.localPosition.x;
        float newX = curX + offset;

        if (Mathf.Abs(newX) > maxLenth)
        {
            newX = -newX + 2 * isForward * maxLenth;
            isForward = isForward * -1;

        }
        _cursor.SetPositionOfPivot(new Vector2(newX, _cursor.localPosition.y));
    }

    public void SetData(VisitVo curVisitVo, List<WeatherPB> weatherRules, List<WeatherBlessPB> weatherBlessRules, bool isBless = false)
    {
        _curVisitVo = curVisitVo;

        if (isBless)
        {
            ChangeWeather(_preBless, curVisitVo.CurWeather);
        }
        else
        {
            SetCurWeather(_curVisitVo.CurWeather);
            SetSuccessRate();
        }
        //Debug.Log("WeatherView  " + curVisitVo.CurWeatherName);
        SetWeatherDescription(weatherRules);
        //SetCurWeather();
        SetBlessingCost();
    }

    void SetBlessingCost()
    {
        transform.Find("Blessing/Image/Text").GetText().text = _curVisitVo.BlessCost.ToString();
    }


    void SetSuccessRate()
    {
        if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
        {
            _blessing.gameObject.Hide();
        }
        else
        {
            _blessing.gameObject.Show();
        }

        // Debug.Log("SetSuccessRate  " + _curVisitVo.CurSuccessRate);
        // transform.Find("SuccessRate/Text").GetComponent<Text>().text = I18NManager.Get("Visit_SuccessRate_Tips", _curVisitVo.CurSuccessRate);
        //Debug.Log("Weather/" + _curVisitVo.CurWeather);

        transform.Find("CurWeather/Text").GetComponent<Text>().text = I18NManager.Get("Visit_Weather_Cur", _curVisitVo.NpcName, _curVisitVo.CurWeatherName, _curVisitVo.MaxVisitTime);
        //int num = weatherRules.find
        transform.Find("CurDescription/Text").GetComponent<Text>().text = I18NManager.Get("Visit_Weather_VisitTime", _curVisitVo.MaxVisitTime);

    }

    private void SetCurWeather(VISIT_WEATHER vw)
    {
        SetWeatherBar(_curVisitVo.CurWeatherPB);
        _result.gameObject.Hide();
        string pathPre = "Visit/Weather/" + vw;
        transform.Find("Weather/Image").GetComponent<Image>().sprite = ResourceManager.Load<Sprite>(pathPre, ModuleConfig.MODULE_VISIT);
        transform.Find("Weather/Image").GetComponent<Image>().SetNativeSize();
        // transform.Find("Weather/" + _curVisitVo.CurWeather).gameObject.Show();
 
        if (vw == VISIT_WEATHER.Fine)
        {
            _blessing.gameObject.Hide();
            _bar.Hide();
            transform.Find("BestWeather").gameObject.Show();
        }
        else
        {
            _bar.Show();
            _blessing.gameObject.Show();
            transform.Find("BestWeather").gameObject.Hide();

        }
    }

    VISIT_WEATHER _from;
    VISIT_WEATHER _to;
    private void OnAnimatorEnd()
    {

    }

}
