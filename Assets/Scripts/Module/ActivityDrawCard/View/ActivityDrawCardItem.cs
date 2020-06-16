using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDrawCardItem : MonoBehaviour
{

    private Transform _finish;
    private Transform _unfinish;

    private Transform _rewardItem;
    private Transform _state;
    private Transform _frameBg;
    private Text _desc1;
    private Text _desc2;
    private void Awake()
    {
        _finish = transform.Find("Finished");
        _unfinish = transform.Find("Unfinished");
        _rewardItem = transform.Find("RewardItem");
        _state = transform.Find("State");
        _frameBg = transform.Find("FrameBg");
        _desc1 = transform.GetText("Desc1");
        _desc2 = transform.GetText("Desc2");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(ActivityDrawCardVo vo)
    {
        _desc1.text = vo.MissionName;
        _desc2.text = vo.MissionDesc;
        UIEventListener.Get(_frameBg.gameObject).onClick = null;

        switch (vo.MissionStatusPB)
        {
            case MissionStatusPB.StatusUnfinished:
                _finish.gameObject.Hide();
                _unfinish.gameObject.Show();
                _state.gameObject.Hide();
                SetFinshText(_unfinish, vo.LimitValue.ToString(), vo.MissionName);
                if (vo.Awards != null)
                {
                    _frameBg.gameObject.Show();
                    _frameBg.Find("RedDot").gameObject.Hide();
                    SetAward(vo.Awards);
                    UIEventListener.Get(_frameBg.gameObject).onClick = (go) =>
                    {
                        OnShowGiftDesc(vo.Awards);
                    };
                }
                else
                {
                    _frameBg.gameObject.Hide();
                    _frameBg.Find("RedDot").gameObject.Hide();
                }
                break;
            case MissionStatusPB.StatusUnclaimed:
      
                if (vo.Awards != null)
                {
                    _finish.gameObject.Hide();
                    _unfinish.gameObject.Show();
                    _state.gameObject.Hide();
                    _frameBg.gameObject.Show();
                    UIEventListener.Get(_frameBg.gameObject).onClick =(go)=> {
                        OnGetGift(vo.ActivityId,vo.activity_mission_id);
                    } ;
                    _frameBg.Find("RedDot").gameObject.Show();
                    SetAward(vo.Awards);
                    SetFinshText(_unfinish, vo.LimitValue.ToString(), vo.MissionName);
                }
                else
                {
                    _finish.gameObject.Show();
                    _unfinish.gameObject.Hide();
                    _state.gameObject.Show();
                    _frameBg.gameObject.Hide();
                    _frameBg.Find("RedDot").gameObject.Hide();
                    SetFinshText(_finish, vo.LimitValue.ToString(), vo.MissionName);
                    _state.GetImage().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_DrawCard_finished");
                }
           
                break;
            case MissionStatusPB.StatusBeRewardedWith:
                _finish.gameObject.Show();
                _unfinish.gameObject.Hide();
                _state.gameObject.Show();
             
                _frameBg.gameObject.Hide();
                if (vo.Awards != null)
                {
                    _state.GetImage().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_DrawCard_got");
                }
                else
                {
                    _state.GetImage().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_DrawCard_finished");
                }

                SetFinshText(_finish, vo.LimitValue.ToString(), vo.MissionName);
                break;
        }      
    }

    void OnGetGift(int activity_id,int activity_mission_id)
    {
        EventDispatcher.TriggerEvent<int,int>(EventConst.ActivityGetDrawCardAward
            , activity_id,
          activity_mission_id);
        Debug.LogError("OnGetGift.......");
    }
    void OnShowGiftDesc(List<AwardPB> award)
    {
        var window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");

        window.SetData(award, true);
        //FlowText.ShowMessage(ClientData.GetItemDescById(pb.ResourceId, pb.Resource).ItemDesc);
    }


    void SetFinshText(Transform parent,string text1,string text2)
    {
        parent.GetText("Text1").text = text1;
        parent.GetText("Text2").text = text2;
    }

    void SetAward(List<AwardPB> awards)
    {
        var frameBg = transform.Find("FrameBg");
        var icon = frameBg.GetRawImage("Icon");
        var redDot = frameBg.Find("RedDot");
        var iconNum = frameBg.GetText("IconNum");
        //PointerClickListener.Get(icon.gameObject).onClick = null;

        if (awards.Count>1) 
        {
            string path = "Prop/GiftPack/tongyong2";
            icon.texture = ResourceManager.Load<Texture>(path);
            iconNum.text = 1.ToString();
            icon.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            RewardVo vo = new RewardVo(awards[0]);
            icon.texture = ResourceManager.Load<Texture>(vo.IconPath);
       
            icon.transform.GetChild(0).gameObject.SetActive(vo.Resource == ResourcePB.Puzzle);
            iconNum.text = vo.Num.ToString();
        }

        icon.gameObject.Show();


    }
}
