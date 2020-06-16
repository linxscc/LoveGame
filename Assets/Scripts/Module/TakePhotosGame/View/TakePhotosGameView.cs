using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TakePhotosGameShowState
{
    None,
    Scale,
    Move,
    Blur
}

public class TakePhotosGameView : View
{
    private TakePhotosGameShowState _curShowState = TakePhotosGameShowState.None;


    List<Image> _scaleSilder;

    VirtualJoystick _joystick;
    RectTransform _photo;
    int weight = 1440;
    int hight = 1920;
    Vector2 _origentSize = new Vector2(1440, 1920);
    Vector2 _scaleRange = new Vector2(1, 5);
    public Vector2 ScaleRange
    {
        get
        {
            return _scaleRange;
        }
    }

    bool _isPause = false;

    private float _photoSpeed = 500;
    private Slider _scale;
    private GameObject _scaleBg;
    RawImage _photoImage;
    Texture _texture;

    // Blur iterations - larger number means more blur.
    [Range(0, 4)]
    public int iterations = 3;
    // Blur spread for each iteration - larger value means more blur
    [Range(0f, 3.0f)]
    public float blurSpread = 0.6f;
    [Range(1, 8)]
    public int downSample = 2;

    private int[] _iterationsRange = new int[2] { 0, 4 };
    private int[] _downSampleRange = new int[2] { 1, 8 };
    private int[] _blurSpreadRange = new int[2] { 0, 3 };

    Button _scaleOk;
    Button _moveOk;
    Button _blurOk;

    GameObject _Shutter;


    float dragBeginDataX;
    float startValue = 0;    float _scaleFactor = 1;

    Color scaleColor = new Color(0.7372549f, 0.9607843f, 0.9490196f);
    Material _blurM;
    RenderTexture _renderTexture;

    Vector3[] scaleChange = new Vector3[5]
    {
        new Vector3( 1.2f,1.2f,1f),
        new Vector3( 1.2f,1.4f,1f),
        new Vector3( 1.2f,1.5f,1f),
        new Vector3( 1.2f,1.4f,1f),
        new Vector3( 1.2f,1.2f,1f),
    };
    float curBlurValue = 0;

    public float blurSpeed = 0.6f;

    private Text _title;

    private void Awake()
    {
        _title = transform.GetText("Title/Text");
        _Shutter = transform.Find("PhotoEdit/Shutter").gameObject;
        _Shutter.Hide();
        _photo = transform.GetRectTransform("PhotoEdit/Photo");
        _photoImage = _photo.GetRawImage();
        _joystick = transform.Find("Move/VirtualJoystick").GetComponent<VirtualJoystick>();
        _scale = transform.Find("Scale/Slider").GetComponent<Slider>();
        _scale.onValueChanged.AddListener(OnScaleChange);
        _scaleBg = transform.Find("Scale/SliderBg").gameObject;
        _blurOk = transform.GetButton("Blur/Button");
        _blurOk.onClick.AddListener(() => {
            _isPause = true;
            HideAllShowState();
           SetCurShowState(TakePhotosGameShowState.None);
            _Shutter.Show();
            ClientTimer.Instance.DelayCall(() =>
            {
                SendMessage(new Message(MessageConst.CMD_TAKEPHOTOS_SHOW_SCORE, Message.MessageReciverType.CONTROLLER, TakePhotosGameShowState.Blur));
            },0.5f);
     
            AudioManager.Instance.PlayEffect("kaca2");
        });
        _moveOk = transform.GetButton("Move/Button");
        _moveOk.onClick.AddListener(() =>
        {
            _isPause = true;
            HideAllShowState();
            SetCurShowState(TakePhotosGameShowState.Blur);
            //AudioManager.Instance.PlayEffect("kaca2");

            SendMessage(new Message(MessageConst.CMD_TAKEPHOTOS_SHOW_SCORE, Message.MessageReciverType.CONTROLLER, TakePhotosGameShowState.Move));
        });
        _scaleOk = transform.GetButton("Scale/Button");
        _scaleOk.onClick.AddListener(() =>
        {
            _isPause = true;
            HideAllShowState();
            SetCurShowState(TakePhotosGameShowState.Move);
            SendMessage(new Message(MessageConst.CMD_TAKEPHOTOS_SHOW_SCORE, Message.MessageReciverType.CONTROLLER, TakePhotosGameShowState.Scale));
            //AudioManager.Instance.PlayEffect("kaca2");
        });
        UIEventListener.Get(_scaleBg.gameObject).onBeginDrag = OnScaleBeginDrag;
        UIEventListener.Get(_scaleBg.gameObject).onDrag = OnScaleDrag;
        UIEventListener.Get(_scaleBg.gameObject).onEndDrag = OnScaleEndDrag;
        _scaleSilder = new List<Image>();
        for(int i=0;i< _scale.transform.Find("Background").childCount;i++)
        {
            Image img = _scale.transform.Find("Background").GetChild(i).GetImage();
            _scaleSilder.Add(img);
        }
    }

    public void SetCurShowState(TakePhotosGameShowState showState)
    {
        HideAllShowState();
        _curShowState = showState;
    }

    public void ShowCurShowState()
    {
        var newOne = GetObjByState(_curShowState);
        if (newOne != null)
        {
            newOne.gameObject.Show();
        }
        SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_SHOW_BACKBTN, Message.MessageReciverType.DEFAULT));
    }


    public void HideAllShowState()
    {
        SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_HIDE_BACKBTN, Message.MessageReciverType.DEFAULT));
        GetObjByState(TakePhotosGameShowState.Blur).gameObject.Hide();
        GetObjByState(TakePhotosGameShowState.Move).gameObject.Hide();
        GetObjByState(TakePhotosGameShowState.Scale).gameObject.Hide();

    }


    private Transform GetObjByState(TakePhotosGameShowState showState)
    {
        return transform.Find(showState.ToString());
    }

    private void OnScaleEndDrag(PointerEventData eventData)
    {
        //Debug.LogError("OnScaleEndDrag");
        dragBeginDataX = eventData.position.x;
    }

    private void OnScaleDrag(PointerEventData eventData)
    {
        float OffSetX = eventData.position.x - dragBeginDataX;
        float OffSetV =OffSetX / 70;
        //Debug.LogError("OnScaleDrag OffSetX"+ OffSetX);
        VoidSetScaleValue(startValue+ OffSetV);
    }

    private void OnScaleBeginDrag(PointerEventData eventData)
    {
        //Debug.LogError("OnScaleBeginDrag");
        dragBeginDataX = eventData.position.x;
        startValue = _scale.value;
    }
 
    void VoidSetScaleValue(float v)
    {
        _scale.value = v;
    }



    public float GetScale()
    {
        return _scaleFactor;
    }

    public float GetBlur()
    {
        return curBlurValue;
    }

    public Vector2 GetPos()
    {
        return _photo.GetLocalCenterPos(); 
    }

    public Vector2 GetSize()
    {
        return _photo.GetSize();
    }
    public Texture GetTexture()
    {
        return _photoImage.texture ;
    }

    public void OnBlurChange(float value)
    {
        //Debug.Log(value);
        SetPhotoBlur(value);
    }


    private void OnScaleChange(float value)
    {
        float scaleFactor = _scaleRange[0] + (value/40) * (_scaleRange[1] - _scaleRange[0]);
        //Debug.LogError(" scaleFactor "+ scaleFactor);
        _scaleFactor = scaleFactor;
        Vector2 newPivot = Vector2.zero;

        Vector2 curSize = _photo.sizeDelta;
        Vector2 curPos = _photo.localPosition;

        newPivot = (curSize * .5f - curPos) / curSize;

        SetScaleByPivot(_photo, newPivot, _origentSize * scaleFactor);
        // _photo.SetSize(_origentSize * scaleFactor);
        ChangePivotNotChangePos(_photo, new Vector2(0.5f, 0.5f));
        ChangeScaleSlider((int)value);
    }

    private void ChangeScaleSlider(int v)
    {
        int start = v - 2;
        Vector3 sc;
        Color c;
        for (int i=0;i<_scaleSilder.Count;i++)
        {
            if(i>=start&&i<start+5)
            {
                sc = new Vector3(1.2f, 1.5f, 1f);
                c = scaleColor;
            }
            else
            {
                sc = new Vector3(1f, 1f, 1f);
                c = Color.white;
            }
            _scaleSilder[i].color = c;
            _scaleSilder[i].transform.localScale = sc;
        }

    }

    public void SetData(TakePhotosGameRunningInfo takePhotosGameRunningInfo)
    {
        string num = I18NManager.Get("Common_Number" + (takePhotosGameRunningInfo.GetCurPhotoOrder + 1).ToString());
        _title.text = I18NManager.Get("TakePhotosGame_TrainText", num);
        _Shutter.Hide();
        _texture = takePhotosGameRunningInfo.originTexture;
        string materialPath = "TakePhotosGame/Material/TakePhotosBlur";
        _blurM = ResourceManager.Load<Material>(materialPath);

        curBlurValue = 0.5f;
        SetPhotoBlur(0.3f);
        _scale.value = 0;
        OnScaleChange(0);
        _photo.SetPositionOfPivot(Vector2.zero);
        SetCurShowState(TakePhotosGameShowState.Scale);
        ShowCurShowState();
        _isPause = false;
    }
    /// <summary>
    /// value range 0-1
    /// </summary>
    /// <param name="value"></param>
    void SetPhotoBlur(float value)
    {
        if (_blurM == null)
            return;

        int _iterationsOffset = _iterationsRange[1] - _iterationsRange[0];
        int _downSampleOffset = _downSampleRange[1] - _downSampleRange[0];
        int _blurSpreadOffset = _blurSpreadRange[1] - _blurSpreadRange[0];
        int iterations = _iterationsRange[0];
        if (value == 0f)
        {
            iterations = _iterationsRange[0];
        }
        else if (value < 0.03f)
        {
            iterations = _iterationsRange[0] + 1;
        }
        else if (value < 0.1f)
        {
            iterations = _iterationsRange[0] + 2;
        }
        else
        {
            iterations = _iterationsRange[0] + 3;
        }

        int downSample = _downSampleRange[0];
        if (value<0.03f)
        {
            downSample = _downSampleRange[0];
        }
        //else if (value < 0.8f)
        //{
        //    downSample = _downSampleRange[0] + 1;
        //}
        else
        {
            downSample = _downSampleRange[0] + 1;
        }
        float blurSpread = _blurSpreadRange[0] + _blurSpreadOffset * value;
        RenderTexture.ReleaseTemporary(_renderTexture);
        _renderTexture = RenderTexture.GetTemporary(weight, hight);
        _renderTexture.filterMode = FilterMode.Bilinear;
        GaussianBlurHelp.GetGaussianBlur(
            _texture,
            _renderTexture,
            _blurM,
            blurSpread,
            iterations,
            downSample);
        _photoImage.texture = _renderTexture;
    }

    static void SetScaleByPivot(RectTransform rect, Vector2 pivot, Vector2 size)
    {
        Vector2 orgPivot = rect.pivot;
        Vector2 oldPos = rect.localPosition;
        Vector2 newPos = oldPos - (orgPivot - pivot) * rect.sizeDelta;
        rect.pivot = pivot;
        rect.SetPositionOfPivot(newPos);
        rect.sizeDelta = size;

    }

    static void ChangePivotNotChangePos(RectTransform rect, Vector2 pivot)
    {
        Vector2 orgPivot = rect.pivot;
        Vector2 oldPos = rect.localPosition;
        Vector2 newPos = oldPos - (orgPivot - pivot) * rect.sizeDelta;
        rect.pivot = pivot;
        rect.SetPositionOfPivot(newPos);
    }

    static void SetPhotoOffset(RectTransform rect, Vector2 dir, float time, 
        float speed,float _scaleFactor)
    {
        ChangePivotNotChangePos(rect, new Vector2(0.5f, 0.5f));
        Vector2 newPos = rect.GetLocalCenterPos() + dir * time * speed;

        if(Mathf.Abs(newPos.x)> (1440 * _scaleFactor - 1080)  * 0.5f)
        {
            newPos.x = Mathf.Sign(newPos.x) * (1440 * _scaleFactor - 1080) * 0.5f;
        }
        if (Mathf.Abs(newPos.y) > (1920 * _scaleFactor - 1546)  * 0.5f)
        {
            newPos.y = Mathf.Sign(newPos.y) * (1920 * _scaleFactor - 1546)  * 0.5f;
        }

        rect.SetPositionOfPivot(newPos);
    }



    public void UpdateScore(int score)
    {
        _isPause = false;

        string text = I18NManager.Get("TakePhotosGame_GainScore", score);
       
        transform.Find("Score/Text").GetText().text = text.ToString();
    }
    private void Update()
    {
        if(_isPause)
        {
            return;
        }
        if (_curShowState==TakePhotosGameShowState.Blur)
        {
            curBlurValue += blurSpeed * Time.deltaTime;
            if (curBlurValue > 1)
            {
                curBlurValue = 2 - curBlurValue;
                blurSpeed = blurSpeed * -1;
            }
            else if (curBlurValue < 0)
            {
                curBlurValue = -curBlurValue;
                blurSpeed = blurSpeed * -1;
            }
            OnBlurChange(curBlurValue);
        }

        if (_curShowState == TakePhotosGameShowState.Move)
        {
            if (_joystick.Dir == Vector2.zero)
            {
                return;
            }
            SetPhotoOffset(_photo, -_joystick.Dir, Time.deltaTime, _photoSpeed,_scaleFactor);
        }
    }
}
