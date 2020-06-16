

using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCallView : View
{
    private GameObject _hangupBtn;
    private GameObject _answerBtn;
    private GameObject _npcMsg;
    private GameObject _selections;

    private bool _isFinishedShowMsg;
    private MySmsOrCallVo _data;
    private int _curTalkId;
    private bool _isShowSelect;

    private Transform _contain;

    private RawImage _bg;

    private void Awake()
    {
        _isShowSelect = false;
        _isFinishedShowMsg = false;
        _hangupBtn = transform.Find("ControlBtns/HangupBtn").gameObject;
        _answerBtn = transform.Find("ControlBtns/AnswerBtn").gameObject;
        _contain = transform.Find("Select/Selections");
        _bg = transform.Find("Bg").GetComponent<RawImage>();
        _hangupBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                AudioManager.Instance.StopDubbing();
            }
            SendMessage(new Message(MessageConst.CMD_PHONE_CALL_HANGUP));
            Debug.Log("CMD_PHONE_SHOW_TELE");
            //SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_TELE));
        });

        _answerBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            ShowTalkingStatus();
            SendMessage(new Message(MessageConst.CMD_PHONE_CALL_ANSWER));
        });

        _npcMsg = transform.Find("Msg").gameObject;

        PointerClickListener.Get(_npcMsg).onClick = delegate (GameObject go)
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                AudioManager.Instance.StopDubbing();
            }
            //点击一次显示下一条
            if (_isFinishedShowMsg)
            {
                Debug.LogError("_isFinished");
                return;
            }
            ShowNextMsg();
        };

        PointerClickListener.Get(transform.Find("Image").gameObject).onClick = delegate (GameObject go)
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                AudioManager.Instance.StopDubbing();
            }
            //点击一次显示下一条
            if (_isFinishedShowMsg)
            {
                Debug.LogError("_isFinished");
                return;
            }
            ShowNextMsg();
        };
        PointerClickListener.Get(transform.Find("Bg").gameObject).onClick = delegate (GameObject go)
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                AudioManager.Instance.StopDubbing();
            }
            //点击一次显示下一条
            if (_isFinishedShowMsg)
            {
                Debug.LogError("_isFinished");
                return;
            }
            ShowNextMsg();
        };
        _selections = transform.Find("Select").gameObject;
    }

    private void ShowNextMsg()
    {
        Debug.Log("ShowNextMsg scene "+_data.SceneId+"  talkId "+ _curTalkId);
        if (_isShowSelect)
        {
            ShowSelects(true);
            return;
        }
        int talkId = _curTalkId;
        var rulePb = _data.SmsRuleInfo.smsTalkInfos.Find((item) => { return item.TalkId == talkId; });
        if (rulePb == null)
        {
            Debug.LogError("Don't find rulePb for talkId is " + talkId);
            return;
        }
        if(rulePb.NpcId==0)
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                AudioManager.Instance.StopDubbing();
            }
            ShowMyMsg(rulePb.TalkContent);
        }
        else
        {
            ShowNpcMsg(rulePb.TalkContent);
        }

        if(rulePb.MusicID!="0")
        {
            OnPlayVoice(rulePb.MusicID);
        }

        if (rulePb.Selects.Count==0)
        {
            SetTalkingEndStatus();
            _isFinishedShowMsg = true;
            if(!_data.IsReaded)
            {
                //send message to server;
                SendMessage(new Message(MessageConst.CMD_PHONE_TELE_CHOOSE, Message.MessageReciverType.CONTROLLER, _data.SceneId, _data.selectIds, _isFinishedShowMsg));
            }
            //todo   finished
        }
        else if(rulePb.Selects.Count==1)
        {
            _curTalkId = rulePb.Selects[0];
        }
        else
        {
            SetSelects();
            _isShowSelect = true;
        }
    }


    private void OnPlayVoice( string musicId)
    {
        if (musicId == "" || musicId == "0")
            return;

        Debug.Log("OnPlayVoice sceneId  "+ " musicId " + musicId + " " + AssetLoader.GetPhoneDialogById(musicId));
        if (AudioManager.Instance.IsPlayingDubbing)
        {
            AudioManager.Instance.StopDubbing();
        }
        new AssetLoader().LoadAudio(AssetLoader.GetPhoneDialogById(musicId),
            (clip, loader) =>
            {
                Debug.Log(clip.length);
                AudioManager.Instance.PlayDubbing(clip);
            });

    }
    private void ShowMyMsg(string str)
    {
        _npcMsg.transform.Find("LeftHeadIcon").gameObject.Show();
        _npcMsg.transform.Find("ImageMask").gameObject.Show();
        _npcMsg.transform.Find("MsgText").GetComponent<Text>().text = str;
    }

    private void ShowNpcMsg(string str)
    {
        _npcMsg.transform.Find("LeftHeadIcon").gameObject.Hide();
        _npcMsg.transform.Find("ImageMask").gameObject.Hide();

       // string newStr = Util.RegPlayerNameString(str, GlobalData.PlayerModel.PlayerVo.UserName);
        _npcMsg.transform.Find("MsgText").GetComponent<Text>().text = str;
    }

    private void SetSelects()
    {
        //暂时在prefab写死三个 以后改动再换成scroll view
        //for(var i=_contain.childCount-1;i>=0;i--)
        //{
        //    Destroy(_contain.GetChild(i));
        //}

        for(var i =0;i<_contain.childCount;i++)
        {
            _contain.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            _contain.GetChild(i).gameObject.Hide();
        }

        int talkId = _curTalkId;
        var rulePb = _data.SmsRuleInfo.smsTalkInfos.Find((item) => { return item.TalkId == talkId; });
        var selectIds = rulePb.Selects;
        int idx = 0; 
        foreach(var v in selectIds)
        {
            var rulePb1 = _data.SmsRuleInfo.smsTalkInfos.Find((item) => { return item.TalkId == v; });
            _contain.GetChild(idx).GetComponent<PhoneSelectItem>().SetData(rulePb1.TalkContent);
            _contain.GetChild(idx).gameObject.Show();
            _contain.GetChild(idx).GetComponent<Button>().onClick.AddListener(() => 
            {
                //todo 
                Debug.LogError(rulePb1.TalkId);
                _curTalkId = rulePb1.TalkId;
                ShowNextMsg();
                ShowSelects(false);
            });
            idx++;
        }
    }
    private void ShowSelects(bool b)
    {
        transform.Find("Select").gameObject.SetActive(b);
        _isShowSelect = false;
       // _contain.gameObject.SetActive(b);
    }

    public void SetData(MySmsOrCallVo data)
    {
        _data = data;
        transform.Find("SenderName").GetComponent<Text>().text = GlobalData.NpcModel.GetNpcById(data.NpcId).NpcName;
        _npcMsg.transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture= ResourceManager.Load<Texture>(PhoneData.GetHeadPath(0),ModuleName);

        string bgPath = "Background/PhoneCallBg" + data.NpcId;
        _bg.texture= ResourceManager.Load<Texture>(bgPath, ModuleName);
        //if (data.selectIds == null || data.selectIds.Count == 0)
        //{
        //    ShowCallingStatus();
        //}
        //else
        //{
        //    ShowTalkingStatus();
        //}
        _curTalkId = data.FirstTalkInfo.TalkId;
        _isFinishedShowMsg = false;
        ShowSelects(false);
        ShowTalkingStatus();
       

    }

    public void ShowCallingStatus()
    {
        _hangupBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("Phone_Call_Refuse"); 
        _answerBtn.Show();
        _npcMsg.Hide();
        _selections.Hide();
    }

    public void ShowTalkingStatus()
    {
        _hangupBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("Phone_Call_Hangup");
        _hangupBtn.transform.Find("EndEffect").gameObject.Hide();
        _answerBtn.Hide();
        _npcMsg.Show();
        _selections.Hide();
        transform.GetText("StatusText").text = I18NManager.Get("Story_ToRingUp");
        ShowNextMsg();
    }
    void SetTalkingEndStatus()
    {
        transform.GetText("StatusText").text= I18NManager.Get("Story_ToRingDown");
        _hangupBtn.transform.Find("EndEffect").gameObject.Show();
        //Story_ToRingUp

        //Story_ToRingDown
    }
}
