using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Download;
//using Assets.Scripts.Module.Phone.Data;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class SmsDetailView : View
{
    private float normalPosY;
    private const int UPPOS_Y = -360;
    private Transform _inputBar;
    private Transform _selectionsContainer;
    private Transform _sceneIdsSelectionsContainer;
    private Transform _msgContainer;
    private Transform _msgContainerMask;
    private int _sceneId;
    private bool _isNoamlStatus;
    private List<SmsTalkInfo> _talkRules;
    private GameObject _inputingItem;
    private List<int> _selectIds;
    private List<int> _listenIds;
    private bool _isFinished;
    private string _headPath;
    private Button _sendBtn;
    private Text _inputTex;

    private Transform _ClickImage;

    private Transform _redPoint;

    private bool _isSelectionReady;
    public bool isSelectionReady
    {
        set
        {
            _isSelectionReady = value;
            if (_isSelectionReady)
            {
                if (_IsShowSelect && _redPoint != null)
                {
                    _redPoint.gameObject.Show();
                }
            }
        }
        get { return _isSelectionReady; }
    }


    private bool _IsShowSelect;
    public bool IsShowSelect
    {
        set
        {
            _IsShowSelect = value;
            if (_IsShowSelect)
            {
                if (_isSelectionReady && _redPoint != null)
                {
                    _redPoint.gameObject.Show();
                }
            }
        }
        get { return _IsShowSelect; }
    }

    private int _curSelectId;
    private int _curSceneSelectId;
    private float _height;

    MySmsOrCallVo _data;
    List<MySmsOrCallVo> _unfinishOfPlayerData;
    List<MySmsOrCallVo> _unfinishOfOtherData;

    List<Coroutine> _coroutines;

    private void Awake()
    {
        _isSelectionReady = false;
        _coroutines = new List<Coroutine>();
        _curSelectId = -1;
        _ClickImage = transform.Find("ClickImage");
        _listenIds = new List<int>();
        IsShowSelect = false;
        _redPoint = transform.Find("InputBar/RedPoint");
        _inputBar = transform.Find("InputBar");
        _sendBtn = transform.Find("InputBar/SendBtn").GetComponent<Button>();
        _sendBtn.onClick.AddListener(SendReplyMsg);
        _inputTex = transform.Find("InputBar/InputTxt").GetComponent<Text>();
        _selectionsContainer = transform.Find("Selections");
        _sceneIdsSelectionsContainer = transform.Find("SceneIdsSelections");
        _msgContainer = transform.Find("ListContainer/Contents");
        _msgContainerMask = transform.Find("ListContainer");
        _height = _msgContainerMask.GetComponent<RectTransform>().GetHeight();
        _selectionsContainer.gameObject.Hide();
        _sceneIdsSelectionsContainer.gameObject.Hide();
        normalPosY = _inputBar.localPosition.y;
        PointerClickListener.Get(_ClickImage.gameObject).onClick = delegate (GameObject go)
        {
            isSelectionReady = true;
            GotoNormalStatus();
        };
        PointerClickListener.Get(_inputBar.gameObject).onClick = delegate (GameObject go)
        {
            if (_isFinished)
            {
                if (!_isNoamlStatus)
                {
                    GotoNormalStatus();
                }
                else
                {
                    if (_unfinishOfOtherData.Count > 0)
                        return;
                    if (_unfinishOfPlayerData.Count > 0 && IsShowSelect)
                    {
                        GotoSelectStatus();
                    }
                }
                return;
            }

            if (!IsShowSelect)
            {
                return;
            }

            if (_isNoamlStatus)
            {
                GotoSelectStatus();
            }
            else
            {
                isSelectionReady = true;
                GotoNormalStatus();
            }
        };
        GotoNormalStatus();
    }
    /// <summary>
    /// 正常显示状态
    /// </summary>
    private void GotoNormalStatus()
    {
        Debug.LogError("GotoNormalStatus " + normalPosY);
        _isNoamlStatus = true;
        _ClickImage.gameObject.Hide();
        var tweener = _inputBar.DOLocalMoveY(normalPosY, 0.5f);
        tweener.SetEase(DG.Tweening.Ease.OutExpo);
        _selectionsContainer.gameObject.Hide();
        _sceneIdsSelectionsContainer.gameObject.Hide();
        _sendBtn.gameObject.Hide();
        _inputTex.text = "";
        _msgContainerMask.GetComponent<RectTransform>().SetHeight(_height);
    }


    public void GuideSelect()
    {
        GotoSelectStatus();
    }

    public void GuideSceneSelect(int sceneId)
    {
        SelectSceneId(sceneId);
    }

    /// <summary>
    /// 点击进入选择显示界面
    /// </summary>
    private void GotoSelectStatus()
    {
        _ClickImage.gameObject.Show();
        _isNoamlStatus = false;
        var tweener = _inputBar.DOLocalMoveY(normalPosY + 600, 0.5f);
        tweener.SetEase(DG.Tweening.Ease.OutExpo);
        tweener.onComplete = () =>
        {
            if (_isNoamlStatus == false)
            {
                if (_isFinished == true)
                {
                    _sceneIdsSelectionsContainer.gameObject.Show();
                }
                else
                {
                    _redPoint.gameObject.Hide();
                    _selectionsContainer.gameObject.Show();
                    _sendBtn.gameObject.Show();
                }
            }
            _inputTex.text = "";
        };
        _msgContainerMask.GetComponent<RectTransform>().SetHeight(_height - 600);
    }
    private void SendReplyMsg()
    {
        Debug.LogError("SendReplyMsg " + _curSelectId);
        if (_curSelectId < 0)
        {
            return;
        }
        GotoNormalStatus();
        IsShowSelect = false;
        SelectItem(_curSelectId);
        _curSelectId = -1;
    }
    /// <summary>
    /// setData
    /// </summary>
    /// <param cur is Running Scene="data"></param>
    /// <param name="unfinishOfPlayerData"></param>
    /// <param name="unfinishOfOtherData"></param>
    public void SetData(MySmsOrCallVo data, List<MySmsOrCallVo> unfinishOfPlayerData, List<MySmsOrCallVo> unfinishOfOtherData, MySmsOrCallVo lastData = null, string npcName = "")
    {
        if (lastData != null)
        {
            _data = lastData;
            foreach (var v in lastData.selectIds)
            {
                SmsTalkInfo talkItem = lastData.SmsRuleInfo.smsTalkInfos.Find((item) => { return item.TalkId == v; });
                if (talkItem.NpcId == 0)
                {
                    AddMyMsg(talkItem);
                }
                else
                {
                    AddNpcMsg(talkItem, lastData);
                }

                if (v == lastData.selectIds[lastData.selectIds.Count - 1])
                {
                    AddLineMsg();
                    _isFinished = true;
                }
            }
        }
        _data = data;
        _unfinishOfPlayerData = unfinishOfPlayerData;
        _unfinishOfOtherData = unfinishOfOtherData;
        _redPoint.gameObject.Hide();
        _isSelectionReady = false;

        transform.Find("Text").GetComponent<Text>().text = npcName;

        if (data == null)
        {
            //InitMsgs(null);
            if (_unfinishOfPlayerData.Count > 0)
            {
                _redPoint.gameObject.Show();
                IsShowSelect = true;
                _isFinished = true;
                SetSceneSelect(_unfinishOfPlayerData);
            }
            return;
        }

        SetShowView(_data);
    }

    public void SetShowView(MySmsOrCallVo data)
    {
        _sceneId = data.SceneId;
        _talkRules = data.SmsRuleInfo.smsTalkInfos;

        //transform.Find("Text").GetComponent<Text>().text = data.Sender;
        _selectIds = data.selectIds;
        _listenIds = data.listenIds;
        if (_selectIds == null)
        {
            _selectIds = new List<int>();
        }

        string s = "";
        foreach (var v in _selectIds)
        {
            s += v.ToString() + ",";
        }
        Debug.LogError("SetData  sceneId  before  " + _sceneId + "    selectIds " + s + "  IsReaded  " + data.IsReaded);

        _isFinished = false;
        if (_selectIds.Count == 0)//表示首次触发  没有记录
        {
            if (data.FirstTalkInfo.NpcId == 0)//表示玩家首先发言
            {
            }
            else
            {
                SmsTalkInfo nextTalkItem = null;
                if (data.FirstTalkInfo.Selects.Count > 0)
                {
                    nextTalkItem = _talkRules.Find((item) => { return item.TalkId == data.FirstTalkInfo.Selects[0]; });
                }

                SmsTalkInfo preTalkItem = nextTalkItem;
                List<int> unFinishedId = new List<int>();
                unFinishedId.Add(data.FirstTalkInfo.TalkId);
                while (nextTalkItem != null && nextTalkItem.NpcId != 0)
                {
                    preTalkItem = nextTalkItem;
                    unFinishedId.Add(nextTalkItem.TalkId);
                    if (nextTalkItem.Selects.Count > 0)
                    {
                        nextTalkItem = _talkRules.Find((item) => { return item.TalkId == nextTalkItem.Selects[0]; });
                    }
                    else
                    {
                        nextTalkItem = null;
                    }
                }
                if (nextTalkItem == null)
                {
                    _isFinished = true;
                }

                if (unFinishedId.Count > 0)
                {
                    _selectIds.AddRange(unFinishedId);
                    //发送读取读完时记录
                    //todo还需要判断是否完成 _isFinished
                    SendMessage(new Message(MessageConst.CMD_PHONE_SMS_CHOOSE, Message.MessageReciverType.CONTROLLER, _sceneId, _selectIds, _isFinished));
                }
            }
        }
        else
        {
            if (data.CurTalkInfo.Selects.Count > 0)
            {
                SmsTalkInfo nextTalkItem = _talkRules.Find((item) => { return item.TalkId == data.CurTalkInfo.Selects[0]; });
                SmsTalkInfo preTalkItem = nextTalkItem;
                List<int> unFinishedId = new List<int>();
                //unFinishedId.Add(data.CurrentTalk.TalkId);
                while (nextTalkItem != null && nextTalkItem.NpcId != 0)
                {
                    //preTalkItem = nextTalkItem;
                    unFinishedId.Add(nextTalkItem.TalkId);
                    if (nextTalkItem.Selects.Count > 0)
                    {
                        nextTalkItem = _talkRules.Find((item) => { return item.TalkId == nextTalkItem.Selects[0]; });
                    }
                    else
                    {
                        nextTalkItem = null;
                    }
                }

                if (nextTalkItem == null)
                {
                    _isFinished = true;
                }

                if (unFinishedId.Count > 0)
                {
                    _selectIds.AddRange(unFinishedId);
                    //发送读取读完时记录
                    //todo还需要判断是否完成 _isFinished
                    SendMessage(new Message(MessageConst.CMD_PHONE_SMS_CHOOSE, Message.MessageReciverType.CONTROLLER, _sceneId, _selectIds, _isFinished));
                }
            }
            else
            {
                _isFinished = true;
                SendMessage(new Message(MessageConst.CMD_PHONE_SMS_CHOOSE, Message.MessageReciverType.CONTROLLER, _sceneId, _selectIds, _isFinished));
            }
        }

        string st = "";
        foreach (var v in _selectIds)
        {
            st += v.ToString() + ",";
        }
        Debug.LogError("SetData  sceneId  after " + _sceneId + "    selectIds " + st + "  IsReaded  " + data.IsReaded);

        GotoNormalStatus();
        InitMsgs(data);

    }

    private void InitMsgs(MySmsOrCallVo data)
    {
        if (data == null)
        {
            return;
        }
        if (_inputingItem != null)
        {
            _inputingItem = null;
        }

        foreach (var v in _selectIds)
        {
            SmsTalkInfo talkItem = _talkRules.Find((item) => { return item.TalkId == v; });
            if (talkItem.NpcId == 0)
            {
                AddMyMsg(talkItem);
            }
            else
            {
                AddNpcMsg(talkItem);
            }
            if (_isFinished && v == _selectIds[_selectIds.Count - 1])
            {
                AddLineMsg();
            }
        }

        //判断是否显示select界面
        if (_selectIds.Count == 0)
        {
            List<int> temp = new List<int>();
            List<string> tempStr = new List<string>();
            temp.Add(data.CurTalkInfo.TalkId);
            tempStr.Add(data.CurTalkInfo.TalkContent);
            IsShowSelect = true;
            SetSelections(temp, tempStr);
        }
        else
        {
            SmsTalkInfo talkItem = _talkRules.Find((item) => { return item.TalkId == _selectIds[_selectIds.Count - 1]; });
            List<int> temp = new List<int>();
            List<string> tempStr = new List<string>();
            temp.AddRange(talkItem.Selects);
            tempStr.AddRange(talkItem.SelectsContent);
            IsShowSelect = true;
            SetSelections(temp, tempStr);
        }
    }


    private void OnPlayVoice(int sceneId, int talkId, string musicId)
    {
        if (musicId == "0" || musicId == "")
            return;
        Debug.LogError("OnPlayVoice sceneId  " + sceneId + "  talkId   " + talkId + " musicId " + musicId + " " + AssetLoader.GetPhoneDialogById(musicId));


        if (AudioManager.Instance.IsPlayingDubbing)
        {
            AudioManager.Instance.StopDubbing();
        }
        else
        {
            //new AssetLoader().LoadAudio(AssetLoader.GetPhoneDialogById(musicId),
            //    (clip, loader) =>
            //    {
            //        Debug.LogError(clip.length);
            //        ClientTimer.Instance.DelayCall(() => { AudioManager.Instance.PlayDubbing(clip); }, 0.5f);
            //        //  ClientTimer.Instance.DelayCall(() => { OnPlayVoiceFinished(); }, clip.length + 0.6f);
            //    }, AudioType.WAV);
            new AssetLoader().LoadAudio(AssetLoader.GetPhoneDialogById(musicId),
                (clip, loader) =>
                {
                    AudioManager.Instance.PlayDubbing(clip);
                });
            SendMessage(new Message(MessageConst.CMD_PHONE_SMS_LISTEN, Message.MessageReciverType.CONTROLLER, sceneId, talkId));
        }
    }

    private void OnPlayVoiceFinished()
    {
        Debug.LogError("OnPlayVoiceFinished");
    }

    private void SetSceneSelect(List<MySmsOrCallVo> data)
    {
        Transform SceneIdsSelections = transform.Find("SceneIdsSelections/Viewport/Content");
        for (int i = SceneIdsSelections.childCount - 1; i >= 0; i--)
        {
            Destroy(SceneIdsSelections.GetChild(i).gameObject);
        }
        var prefab = GetPrefab("Phone/Prefabs/Item/SmsSelectionItem");
        foreach (var v in data)
        {
            var item = Instantiate(prefab) as GameObject;
            item.transform.SetParent(SceneIdsSelections, false);
            item.transform.localScale = Vector3.one;

            item.transform.Find("Text").GetComponent<Text>().text = v.SmsRuleInfo.smsSceneInfo.SceneName;//待定
            int selectSceneId = v.SceneId;
            PointerClickListener.Get(item).onClick = delegate (GameObject go)
            {
                Debug.LogError("PointerClickListener " + selectSceneId);
                //IsShowSelect = false;
                //_curSceneSelectId = selectSceneId;
                //SelectSceneItem(selectSceneId);
                //GotoNormalStatus();
                //isSelectionReady = false;
                SelectSceneId(selectSceneId);
            };
        }
    }

    private void SelectSceneId(int selectSceneId)
    {
        Debug.LogError("PointerClickListener " + selectSceneId);
        IsShowSelect = false;
        _curSceneSelectId = selectSceneId;
        SelectSceneItem(selectSceneId);
        GotoNormalStatus();
        isSelectionReady = false;
    }

    private void SelectSceneItem(int sceneId)
    {
        Debug.Log("SelectSceneItem  " + sceneId);
        _redPoint.gameObject.Hide();
        MySmsOrCallVo selectItem = _unfinishOfPlayerData.Find((item) => { return item.SceneId == sceneId; });
        if (selectItem == null)
        {
            return;
        }
        else
        {
            _unfinishOfPlayerData.Remove(selectItem);
        }

        if (_selectIds == null)
        {
            _selectIds = new List<int>();
        }
        else
        {
            _selectIds.Clear();
        }

        _data = selectItem;

        _isFinished = false;
        int talkId = selectItem.FirstTalkInfo.TalkId;
        //_selectIds.Add(talkId);
        _sceneId = sceneId;
        _talkRules = selectItem.SmsRuleInfo.smsTalkInfos;
        SelectItem(talkId);
    }

    private void SetSelections(List<int> selectIds, List<string> contents)
    {
        Debug.LogError("sceneID " + _sceneId + " SetSelections " + selectIds.Count + "  " + contents.Count);

        RectTransform contentTrf = _selectionsContainer.GetComponent<ScrollRect>().content;

        for (int i = contentTrf.childCount-1; i >=0; i--)
        {
            Destroy(contentTrf.GetChild(i).gameObject);
        }

        //if (selectIds.Count != contents.Count) //没有配置content  自己添加
        //{
            contents.Clear();
            foreach (var id in selectIds)
            {
                var talkItem1 = _talkRules.Find((item) => { return item.TalkId == id; });
                contents.Add(talkItem1.TalkContent);
            }
      //  }
        var prefab = GetPrefab("Phone/Prefabs/Item/SmsSelectItem");
        for (int i = 0; i < selectIds.Count; i++)
        {
            var item = Instantiate(prefab) as GameObject;
            item.transform.SetParent(contentTrf, false);
            item.transform.localScale = Vector3.one;
            item.name = "Item_" + selectIds[i];
            int selectId = selectIds[i];
            //           Debug.Log(selectIds[i]);
            string str = contents[i];
            //if (str.Length > 15)
            //{
            //    str = str.Substring(0, 15);
            //    //  str += "\n...";
            //}
            Text tt =
            item.transform.Find("Text").GetComponent<Text>();
            tt.text = str;//待定
            //Debug.LogError("flexibleWidth  " + tt.flexibleWidth 
            //    + "  flexibleHeight  " + tt.flexibleHeight
            //    + "  minWidth  " + tt.minWidth
            //    + "  minHeight  " + tt.minHeight
            //    + "  preferredWidth  " + tt.preferredWidth
            //    + "  preferredHeight  " + tt.preferredHeight);

            item.transform.GetRectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tt.preferredHeight+50);


            PointerClickListener.Get(item).onClick = delegate (GameObject go)
            {
                _inputTex.text = str;
                Debug.LogError("PointerClickListener " + selectId);
                //  _isShowSelect = false;
                _curSelectId = selectId;
            };
        }

        if (selectIds.Count > 0)
        {
            isSelectionReady = true;
        }
    }

    /// <summary>
    /// 点击选择后逻辑处理
    /// </summary>
    /// <param name="selectId"></param>
    private void SelectItem(int selectId)
    {
        Debug.Log("SelectItem  " + selectId);
        bool isShowSceneSelect = false;
        if (selectId > 0)
        {
            var talkItem = _talkRules.Find((item) => { return item.TalkId == selectId; });
            if (talkItem.NpcId == 0)
            {
                AddMyMsg(talkItem);
                if (talkItem.Selects.Count > 0)//判断下一段对话
                {
                    var nextTalkItem = _talkRules.Find((item) => { return item.TalkId == talkItem.Selects[0]; });
                    if (nextTalkItem.NpcId != 0)//表示下一段对话为npc对话 自动播放
                    {
                        _coroutines.Add(ClientTimer.Instance.DelayCall(() => { SelectItem(talkItem.Selects[0]); }, 1f));
                    }
                    else
                    {
                        IsShowSelect = true;
                        SetSelections(talkItem.Selects, talkItem.SelectsContent);
                    }
                }
                else
                {
                    _isFinished = true;
                    AddLineMsg();
                }
            }
            else
            {
                _coroutines.Add(ClientTimer.Instance.DelayCall(() => { ShowNpcInputing(talkItem.NpcId); }, 0.5f));

                if (talkItem.Selects.Count > 0)//判断下一段对话
                {
                    _coroutines.Add(ClientTimer.Instance.DelayCall(() => { AddNpcMsg(talkItem); }, 3f));
                    var nextTalkItem = _talkRules.Find((item) => { return item.TalkId == talkItem.Selects[0]; });
                    if (nextTalkItem.NpcId != 0)//表示下一段对话为npc对话 自动播放
                    {
                        _coroutines.Add(ClientTimer.Instance.DelayCall(() => { SelectItem(talkItem.Selects[0]); }, 4f));
                    }
                    else
                    {
                        _coroutines.Add(ClientTimer.Instance.DelayCall(() => { IsShowSelect = true; }, 3f));
                        SetSelections(talkItem.Selects, talkItem.SelectsContent);
                    }
                }
                else
                {
                    _coroutines.Add(ClientTimer.Instance.DelayCall(() =>
                    {
                        AddNpcMsg(talkItem);
                        AddLineMsg();
                        if (_isFinished)
                        {
                            if (_unfinishOfOtherData.Count > 0)//如果下一条为NPC 点进去进入
                            {
                                IsShowSelect = false;
                                return;
                            }
                            if (_unfinishOfPlayerData.Count > 0)
                            {
                                IsShowSelect = true;
                                Debug.Log("Show redPoint for select player's scene");
                                if (talkItem.NpcId == 0)
                                {
                                    _redPoint.gameObject.Show();
                                }
                                else
                                {
                                    //_coroutines.Add(ClientTimer.Instance.DelayCall(() =>
                                    //{
                                    _redPoint.gameObject.Show();
                                    //  }, 2.8f));
                                }
                                SetSceneSelect(_unfinishOfPlayerData);
                            }
                        }

                    }, 3f));
                    _isFinished = true;
                    isShowSceneSelect = true;

                }
            }

            _selectIds.Add(selectId);
            //发送读取读完时记录
            SendMessage(new Message(MessageConst.CMD_PHONE_SMS_CHOOSE, Message.MessageReciverType.CONTROLLER, _sceneId, _selectIds, _isFinished));

            if (isShowSceneSelect)
            {
                IsShowSelect = false;

                return;
            }

            if (_isFinished)
            {
                 
                if( _data.SceneId == 101 && _data.IsReaded)          
                {                         
                    SendMessage(new Message(MessageConst.CMD_MIANLINE_GUIDE_PHONE_TO_MAINLINE));            
                }
                
                if (_unfinishOfOtherData.Count > 0)//如果下一条为NPC 点进去进入
                {
                    IsShowSelect = false;
                    return;
                }
                if (_unfinishOfPlayerData.Count > 0)
                {
                    Debug.Log("Show redPoint for select player's scene");
                    if (talkItem.NpcId == 0)
                    {
                        _redPoint.gameObject.Show();
                    }
                    //else
                    //{
                    //    _coroutines.Add(ClientTimer.Instance.DelayCall(() =>
                    //    {
                    //        _redPoint.gameObject.Show();
                    //    }, 2.8f));
                    //}
                    IsShowSelect = true;
                    SetSceneSelect(_unfinishOfPlayerData);
                }
            }
        }
    }

    private void SetNext()
    {

    }


    public void AddLineMsg()
    {
        Debug.Log("AddLineMsg");
        var prefab = GetPrefab("Phone/Prefabs/Item/SmsDetailLineItem");
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(_msgContainer, false);
        item.transform.localScale = Vector3.one;
        MoveToBottom();
    }

    public void AddMyMsg(SmsTalkInfo data,MySmsOrCallVo smsOrCallVo = null)
    {
      
        
        Debug.Log("AddMyMsg");
        var prefab = GetPrefab("Phone/Prefabs/Item/SmsDetailMyItem");
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(_msgContainer, false);
        item.transform.localScale = Vector3.one;
        item.GetComponent<SmsDetailMyItem>().SetData(data);
        MoveToBottom();
    }

    /// <summary>
    /// 添加NPC对话
    /// </summary>
    /// <param name="data"></param>
    public void AddNpcMsg(SmsTalkInfo data, MySmsOrCallVo smsOrCallVo = null)
    {

        MySmsOrCallVo mySmsOrCallVo = smsOrCallVo == null ? _data : smsOrCallVo;

        if (mySmsOrCallVo.SceneId == 101 && mySmsOrCallVo.IsReaded)          
        {                   
            SendMessage(new Message(MessageConst.CMD_MIANLINE_GUIDE_PHONE_TO_MAINLINE));            
        }

        bool isPlayed = mySmsOrCallVo.listenIds.Contains(data.TalkId);
        GameObject item = null;
        if (_inputingItem != null)
        {
            Debug.Log("AddNpcMsg  _inputingItem != null");
            item = _inputingItem;
            _inputingItem = null;
        }
        else
        {
            Debug.Log("AddNpcMsg  _inputingItem == null");
            var prefab = GetPrefab("Phone/Prefabs/Item/SmsDetailItem");
            item = Instantiate(prefab) as GameObject;
            item.transform.SetParent(_msgContainer, false);
            item.transform.localScale = Vector3.one;
        }
        item.GetComponent<SmsDetailItem>().SetData(data, isPlayed);

        item.transform.Find("Msg/VoiceBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            if(CacheManager.isGuideSmsBySceneId(mySmsOrCallVo.SceneId))
            {
                OnPlayVoice(mySmsOrCallVo.SceneId, data.TalkId, data.MusicID);
                item.GetComponent<SmsDetailItem>().SetRedPoint();
                return;
            }

            CacheVo cacheVo = CacheManager.CheckPhoneCache();
            if (cacheVo.ids.Contains(mySmsOrCallVo.NpcId))//新手引导 4512
            {
                item.GetComponent<SmsDetailItem>().OnTransClick();
                SendMessage(new Message(MessageConst.CMD_PHONE_SMS_LISTEN, Message.MessageReciverType.CONTROLLER, mySmsOrCallVo.SceneId, data.TalkId));
                //点击翻译按钮 需要移动到底部
                if (_data.selectIds.LastIndexOf(data.TalkId) == _data.selectIds.Count - 1)
                {
                    MoveToBottom();
                }
            }
            else
            {
                OnPlayVoice(mySmsOrCallVo.SceneId, data.TalkId, data.MusicID);
                item.GetComponent<SmsDetailItem>().SetRedPoint();
            }
        });
        item.transform.Find("Msg/TransButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            item.GetComponent<SmsDetailItem>().OnTransClick();
            SendMessage(new Message(MessageConst.CMD_PHONE_SMS_LISTEN, Message.MessageReciverType.CONTROLLER, mySmsOrCallVo.SceneId, data.TalkId));

            //点击翻译按钮 需要移动到底部
            if (_data.selectIds.LastIndexOf(data.TalkId) == _data.selectIds.Count - 1)
            {
                MoveToBottom();
            }
        });

        MoveToBottom();
    }



    private void ShowNpcInputing(int NpcID)
    {
        Debug.Log("ShowNpcInputing");
        var prefab = GetPrefab("Phone/Prefabs/Item/SmsDetailItem");
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(_msgContainer, false);
        item.transform.localScale = Vector3.one;

        item.GetComponent<SmsDetailItem>().SetMsg(I18NManager.Get("Phone_Hint2"), PhoneData.GetHeadPath(NpcID));
        item.GetComponent<SmsDetailItem>().SetRedPoint();
        _inputingItem = item;
        MoveToBottom();
    }
    private void MoveToBottom()
    {
        ClientTimer.Instance.DelayCall(() =>
        {
            float containHeight = _msgContainer.GetComponent<RectTransform>().GetHeight();
            if (containHeight <= _height + _msgContainer.localPosition.y)
            {
                return;
            }
            //_msgContainer.DOKill(true); 
            var tweener = _msgContainer.DOLocalMoveY(containHeight - _height, 0.3f);
            tweener.SetEase(DG.Tweening.Ease.OutExpo);
            //tweener.onComplete = () =>
            //{
            //}; }, 0.5f);
            Debug.Log("MoveToBottom");
        }, 0.2f);

    }

    public override void Show(float delay = 0)
    {
        Debug.Log("Show");
        base.Show(delay);
    }
    public override void Hide()
    {
        Debug.Log("Hide");
        if (AudioManager.Instance.IsPlayingDubbing)
        {
            AudioManager.Instance.StopDubbing();
        }
        for (var i = _coroutines.Count - 1; i >= 0; i--)
        {
            ClientTimer.Instance.CancelDelayCall(_coroutines[i]);
        }
        _coroutines.Clear();
        GotoNormalStatus();

        for (int i = 0; i < _msgContainer.childCount; i++)
        {
            Destroy(_msgContainer.GetChild(i).gameObject);
        }
        base.Hide();
    }



}