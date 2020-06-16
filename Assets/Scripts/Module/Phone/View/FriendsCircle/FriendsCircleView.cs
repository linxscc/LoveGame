using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FriendsCircleView : View
{
    private Transform _container;
    private Transform _selectContainer;
    private List<FriendCircleVo> _data;
    private List<FriendCircleVo> _showData;
    private Button _publishBtn;
    private Transform _publishSelect;
    private Transform _publishContainer;
    private Transform _replySelect;
    private int _selectSceneId = 0;
    private GameObject _selectObj;
    private LoopVerticalScrollRect _loopRect;

    private void Awake()
    {
        _selectObj = null;
        _container = transform.Find("Scroll View/Viewport/Content");
        _selectContainer = transform.Find("ReplySelect/Scroll View/Viewport/Content");
        _publishBtn = transform.Find("PublishBtn").GetComponent<Button>();

        _replySelect = transform.Find("ReplySelect");
        _replySelect.gameObject.SetActive(false);
        _publishSelect = transform.Find("PublishSelect");
        _publishContainer = transform.Find("PublishSelect/Scroll View/Viewport/Content");

        _publishBtn.transform.Find("Tips").gameObject.SetActive(false);
        _publishBtn.interactable = false;
        _loopRect = transform.Find("Scroll View").GetComponent<LoopVerticalScrollRect>();
        _loopRect.prefabName = "Phone/Prefabs/Item/FriendsCircleItem";
        _loopRect.poolSize = 10;
        _loopRect.totalCount = 0;
        _loopRect.UpdateCallback = UpdateCallback;

        _replySelect.Find("BgClick").GetComponent<Button>().onClick.AddListener(delegate
        {
            _replySelect.gameObject.Hide();
        });
        
        _publishSelect.Find("BgClick").GetComponent<Button>().onClick.AddListener(delegate
        {
            _publishSelect.gameObject.Hide();
        });
        
        _publishBtn.onClick.AddListener(() =>
        {
            ShowPubish();
        });
    }

    /// <summary>
    /// 展示发布界面
    /// </summary>
    public void ShowPubish()
    {
        Debug.Log("ShowPubish  ");
        _publishSelect.gameObject.SetActive(true);

        for (int i = _publishContainer.childCount - 1; i >= 0; i--)
        {

            Destroy(_publishContainer.GetChild(i).gameObject);
        }

        List<FriendCircleVo> infoList;
        infoList = _data.FindAll(match => { return match.PublishState == false && match.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0; });
        if (infoList.Count > 0)
        {
            _publishBtn.interactable = true;
            _publishBtn.transform.Find("Tips").gameObject.SetActive(true);
            foreach (var v in infoList)
            {
                var FCSelectPrefab = GetPrefab("Phone/Prefabs/Item/FCSelectItem");
                var item = Instantiate(FCSelectPrefab) as GameObject;
                item.transform.SetParent(_publishContainer, false);
                item.transform.localScale = Vector3.one;
                item.GetComponent<FCSelectItem>().SetData(v.FriendCircleRuleInfo.friendCircleSceneInfo.TitleContent);
                item.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.LogError("SceneID  " + v.SceneId + "  Title  " + v.FriendCircleRuleInfo.friendCircleSceneInfo.TitleContent);
                    EventDispatcher.TriggerEvent<int>(EventConst.PhoneFriendCirclePublishClick, v.SceneId);
                    _publishSelect.gameObject.SetActive(false);
                });
            }
        }
    }
    /// <summary>
    /// 展示回复界面
    /// </summary>
    /// <param name="sceneId"></param>
    public void ShowReplay(int sceneId,GameObject gameObject,List<int> ids)
    {
        Debug.Log("ShowReplay  " + sceneId + "  ids " + ids.ToString());
        _selectSceneId = sceneId;
        _selectObj = gameObject;
        FriendCircleVo info = _data.Find(match => match.SceneId == sceneId);
        if (info == null)
        {
            Debug.LogError("ShowReplay info is null");
        }

        for (int i = _selectContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(_selectContainer.GetChild(i).gameObject);
        }

        foreach (var v in ids)
        {
            var vrule = info.GetCurSceneFCRuleByReplyId(v);
            var FCSelectPrefab = GetPrefab("Phone/Prefabs/Item/FCSelectItem");
                var item = Instantiate(FCSelectPrefab) as GameObject;
                item.transform.SetParent(_selectContainer, false);
                item.transform.localScale = Vector3.one;
                item.GetComponent<FCSelectItem>().SetData(vrule.Content);
                item.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.LogError("SceneID  " + sceneId + " replyID " + v);
                    EventDispatcher.TriggerEvent<int, int>(EventConst.ClickFriendCircleItemReplyClick, sceneId, v);
                    _replySelect.gameObject.SetActive(false);
                });
            }
      
        _replySelect.gameObject.SetActive(true);
    }

    private int CheckIsSameElement(FriendCircleRulePB rule, List<int> selectIds)
    {
        //int res = -1;
        foreach (var v in rule.SelectIds)
        {
            if (selectIds.Contains(v))
            {
                //res = v;
                return v;
            }
        }
        return -1;
    }

    private void ShowList()
    {
        //展示发布状态小红点
        List<FriendCircleVo> infoList;
        if(_data==null)
        {
            return;
        }
        infoList = _data.FindAll(match => { return match.PublishState == false && match.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0; });
        if (infoList.Count > 0)
        {
            _publishBtn.interactable = true;
            _publishBtn.transform.Find("Tips").gameObject.SetActive(true);
        }

        //var friendsCircleprefab = Resources.Load("Phone/Prefabs/Item/FriendsCircleItem");
        _loopRect.totalCount = _showData.Count;
        _loopRect.RefreshCells();
        //_loopRect.RefillCells();
        //_loopRect
        //foreach (var v in _data)
        //{
        //    if (v.PublishState == false && v.DefaultRule.Type == 0)
        //    {
        //        continue;
        //    }

        //    var item = Instantiate(friendsCircleprefab) as GameObject;
        //    item.transform.SetParent(_container, false);
        //    item.transform.localScale = Vector3.one;
        //    item.name = v.SceneId.ToString();
        //    item.GetComponent<FriendsCircleItem>().SetData(v);
        //}
    }

    private void UpdateCallback(GameObject go, int index)
    {    
        if (_showData.Count > index)
        {
            go.Hide();
            go.GetComponent<FriendsCircleItem>().SetData(_showData[index]);
            go.Show();
        }
    }

    public void SetData(List<FriendCircleVo> data)
    {
        _data = data;
        //_data 需要按时间排序
        _data.Sort();
        _showData = _data.FindAll(item => {
            return (item.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0&&item.PublishState)
            ||item.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId != 0;
        });
        _loopRect.ClearCells();
        ShowList();
        _loopRect.RefillCells();
    }

    public void UpdateData(List<FriendCircleVo> data)
    {
       // data.Sort();
        //_selectSceneId
        _data = data;
        FriendCircleVo frindData = _data.Find(item => { return item.SceneId == _selectSceneId; });
    //    _selectObj.GetComponent<FriendsCircleItem>().SetData(data[1]);
        _selectObj.GetComponent<FriendsCircleItem>().SetComment(frindData);
       // _loopRect.
       // _loopRect.content.localPosition = new Vector3(_loopRect.content.localPosition.x, _loopRect.content.localPosition.y+40, _loopRect.content.localPosition.z);
        // _loopRect.RefreshCells();
        //_loopRect.ClearCells();
        //ShowList();
        // _loopRect.Rebuild(CanvasUpdate.Prelayout);
    }

    /// <summary>
    /// 添加单个data
    /// </summary>
    /// <param name="data"></param>
    public void AddData(List<FriendCircleVo> data)
    {
        _data = data;     
        //_data 需要按时间排序
        _data.Sort();
        _showData = _data.FindAll(item => {
            return (item.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0&&item.PublishState)
            || item.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId != 0; });
        //_loopRect.ClearCells();
        _loopRect.totalCount = _showData.Count;
        _loopRect.RefillCells(0);
     
        var infoList = _data.FindAll(match => { return match.PublishState == false && match.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0; });
        if (infoList.Count == 0)
        {
            _publishBtn.interactable = false;
            _publishBtn.transform.Find("Tips").gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///更新单个data
    /// </summary>
    //public void UpData(FriendCircleVo data)
    //{
    //    FriendCircleVo oldData=_data.Find(match => match.SceneId == data.SceneId);
    //    oldData = data;

    //    Transform gameTrans = _container.transform.Find(data.SceneId.ToString());
    //    if(gameTrans==null)
    //    {
    //        Debug.LogError("gameTrans is null");
    //    }
    //    gameTrans.gameObject.GetComponent<FriendsCircleItem>().SetData(data);
    //}
    public override void Hide()
    {
        base.Hide();
        transform.gameObject.Hide();
    }

    public override void Show(float delay = 0)
    {
        base.Show(delay);
        transform.gameObject.Show();
        ShowList();
    }

    private void OnDestroy()
    {
        _data = null;
        _showData = null;
    }

    //private void Update()
    //{
    //    //可以的
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        Debug.LogError("111111111111111111112");
    //        FriendCircleVo test = _data[1];
    //        test.SelectIds.Clear();
    //        //_loopRect.ClearCells();
    //        //_loopRect.ClearCells();
    //        //ShowList();
    //        //_container.GetChild(1).GetComponent<FriendsCircleItem>().testSet("dffffffffffffffffffffffffffffffffffffffffffsssssssss");
    //        _container.GetChild(1).GetComponent<FriendsCircleItem>().SetData(test);
    //        _loopRect.RefillCells(1);
    //        //_loopRect.horizontalNormalizedPosition = _loopRect.horizontalNormalizedPosition + 1;
    //        //_loopRect.normalizedPosition = new Vector2(_loopRect.normalizedPosition.x, _loopRect.normalizedPosition.y+0.01f); 
    //        //_loopRect.content.transform.GetComponent<ContentSizeFitter>().SetLayoutVertical();
    //        //_container.GetComponentInChildren<FriendsCircleItem>().testSet("ssssssssssssssssss");
    //        //_loopRect.RefreshCells();
    //        // _loopRect.Rebuild(CanvasUpdate.Prelayout);
    //        //_loopRect.LayoutComplete();
    //        //_loopRect.content.GetComponent<ContentSizeFitter>()();
    //        // Debug.LogError("2222222222222");
    //        //go.GetComponent<FriendsCircleItem>().SetData(_data[3]);
    //    }
    //}
}
