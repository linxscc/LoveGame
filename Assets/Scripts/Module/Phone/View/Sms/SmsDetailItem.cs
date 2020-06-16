using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
//using Assets.Scripts.Module.Phone.Data;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class SmsDetailItem : MonoBehaviour
{
    Image _voice;
    Image _line;
    Text _msg;
    Button _transBtn;
    Button _playVoice;//播放语音
    Text _voiceLenth;
    SmsTalkInfo _data;

    GameObject _redPoint;


    bool _isVoice;//是否是语音
    bool _isTransVoice;//是否转文字


    private void Awake()
    {
        _isTransVoice = false;
        _line = transform.Find("Msg/LineImage").GetComponent<Image>();
        _msg = transform.Find("Msg/MsgText").GetComponent<Text>();
        _transBtn = transform.Find("Msg/TransButton").GetComponent<Button>();
        //_transBtn.onClick.AddListener(OnTransClick);
        _voice = transform.Find("Msg/VoiceBtn/VoiceImage").GetComponent<Image>();
        _playVoice = transform.Find("Msg/VoiceBtn").GetComponent<Button>();
        //_playVoice.onClick.AddListener(OnPlayVoice);
        _voiceLenth = transform.Find("Msg/VoiceBtn/VoiceLenthTxt").GetComponent<Text>();
        _playVoice.gameObject.SetActive(false);
        _line.gameObject.SetActive(false);
        _msg.gameObject.SetActive(false);

        _redPoint = transform.Find("Msg/RedPoint").gameObject;
    }

    public void OnTransClick()
    {
        transform.Find("Msg/MsgText").GetComponent<Text>().text = "\n\n" + _data.TalkContent;
        _msg.gameObject.SetActive(true);
        _redPoint.SetActive(false);
        _isTransVoice = true;
        _transBtn.gameObject.SetActive(!_isTransVoice);
        AjustSize();
    }



    public void SetRedPoint(bool b = false)
    {
        _redPoint.SetActive(b);
    }

    public void SetData(SmsTalkInfo data, bool isPlayed = false)
    {
        if (transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture == null)
        {
            string headPath = PhoneData.GetHeadPath(data.NpcId);
            transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(headPath, ModuleConfig.MODULE_PHONE);
        }
        _data = data;
        if (data.MusicID == "0"|| data.MusicID=="")
        {
            _msg.text = data.TalkContent;
            _playVoice.gameObject.SetActive(false);
            _msg.gameObject.SetActive(true);
            _redPoint.SetActive(false);
            _isVoice = false;
            _transBtn.gameObject.SetActive(false);
        }
        else
        {
            _isVoice = true;
            _playVoice.gameObject.SetActive(true);
            _msg.gameObject.SetActive(false);
            _redPoint.SetActive(!isPlayed);
            _transBtn.gameObject.SetActive(true);
            _voiceLenth.text = data.MusicLen.ToString()+"\"";
        }
        //_data.IsPlayed
        AjustSize();
    }
    
    public void SetMsg(string msg,string headPath)
    {
        Debug.Log("SetMsg  " + msg);
        transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(headPath, ModuleConfig.MODULE_PHONE);
        transform.Find("Msg/MsgText").GetComponent<Text>().text = msg;
        _msg.gameObject.SetActive(true);
        _playVoice.gameObject.SetActive(false);
        _transBtn.gameObject.SetActive(false);
        AjustSize();
    }

    private void AjustSize()
    {
        if(_msg.preferredWidth>560)
        {
            transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(560+60, _msg.preferredHeight + 70));
        }
        else
        {
            if(_isVoice)
            {
                // transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(560 + 60, _msg.preferredHeight + 70));
                int musicLen = int.Parse(_data.MusicLen==""?"0": _data.MusicLen);
                float offset = musicLen / 10;
                float width = 0;
                if (musicLen < 1)
                {
                    width = Math.Max(_msg.preferredWidth + 60, 160 + 60);
                    //transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(160 + 60, _msg.preferredHeight + 70));
                }
                else if(musicLen < 10)
                {
                    width = Math.Max(_msg.preferredWidth + 60, 160 + musicLen * 40 + 60);
                    //transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(160+ _data.MusicLen * 40 + 60, _msg.preferredHeight + 70));
                }
                else
                {
                    width = Math.Max(_msg.preferredWidth + 60, 560 + 60);
                    //transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(560 + 60, _msg.preferredHeight + 70));
                }
                transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(width, _msg.preferredHeight + 70));
            }
            else
            {
                transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(_msg.preferredWidth + 60, _msg.preferredHeight + 70));
            }
        }
        _line.gameObject.SetActive(_isTransVoice);
        GetComponent<RectTransform>().SetHeight(transform.Find("Msg").GetComponent<RectTransform>().GetHeight()+30);
    }
    
}
