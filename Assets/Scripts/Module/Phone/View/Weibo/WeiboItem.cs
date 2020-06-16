using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class WeiboItem : MonoBehaviour {

    private Text _blogName;//微博主
    private Text _lastTimeTxt;//上一次登陆时间
    private Text _ContentTxt;//内容
    private Button _likeBtn;//点赞
    private RawImage _bgImg;
    private WeiboVo _data;
    private void Awake()
    {
        _blogName = transform.Find("Title/BlogNameTxt").GetComponent<Text>();
        _lastTimeTxt = transform.Find("Title/LastTimeTxt").GetComponent<Text>();
        _ContentTxt = transform.Find("ContentLayout/Text").GetComponent<Text>();
        _likeBtn = transform.Find("Bottom/LikeBtn").GetComponent<Button>();
        _bgImg = transform.Find("ContentLayout/Image/Image").GetComponent<RawImage>();
        _likeBtn.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        Image img = _likeBtn.image;
        img.sprite= AssetManager.Instance.GetSpriteAtlas("UIAtlas_Phone_WeiboLike2");
        EventDispatcher.TriggerEvent(EventConst.PhoneWeiboItemLikeClick, _data.SceneId);

        transform.Find("Bottom/LikeNumTxt").GetComponent<Text>().text =
            (int.Parse(_data.WeiboRuleInfo.weiboSceneInfo.LikeNum)+1).ToString();
        _likeBtn.interactable = false;
    }

    public void SetData(WeiboVo data)
    {
        _data = data;
        // _blogName.text = _data.Blogger;
        if (_data.WeiboRuleInfo.weiboSceneInfo.NpcId<5)
        {
            _blogName.text = PhoneData.GetNpcNameById(_data.WeiboRuleInfo.weiboSceneInfo.NpcId);
        }
        else
        {
            _blogName.text = _data.WeiboRuleInfo.weiboSceneInfo.NpcName;
        }

        _ContentTxt.text = _data.WeiboRuleInfo.weiboSceneInfo.TitleContent;

        string dateStr = "";
        long hasPassedStamp = ClientTimer.Instance.GetCurrentTimeStamp() - data.CreateTime;
        if (hasPassedStamp < 0)
        {
            dateStr = I18NManager.Get("Phone_Tips1");
        }
        else if (hasPassedStamp < 1000 * 60 * 60 * 24)
        {
            // long s = (smsVo.CreateTime / 1000) % 60;
            long m = (data.CreateTime / (60 * 1000)) % 60;
            long h = (data.CreateTime / (60 * 60 * 1000) + 8) % 24;
            // dateStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);    
            dateStr = string.Format("{0:D2}:{1:D2}", h, m);
        }
        else
        {
            dateStr = I18NManager.Get("Phone_Tips2");
        }
        _lastTimeTxt.text = dateStr;

        string bgId = _data.WeiboRuleInfo.weiboSceneInfo.BackgroundId;
        if(bgId == "0"|| bgId == "")
        {
            _bgImg.transform.parent.gameObject.Hide();
        }
        else
        {

            _bgImg.texture = ResourceManager.Load<Texture>("Phone/WeiboBackGround/" + bgId, ModuleConfig.MODULE_PHONE);          
          //  _bgImg.SetNativeSize();
            _bgImg.transform.parent.gameObject.Show();
        }

        string headPath = WeiboVo.GetHeadPath(_data.WeiboRuleInfo.weiboSceneInfo.NpcId);
        transform.Find("Title/HeadMask/HeadIcon").GetComponent<RawImage>().texture= ResourceManager.Load<Texture>(headPath, ModuleConfig.MODULE_PHONE);

        transform.Find("Bottom/ShareNumTxt").GetComponent<Text>().text = _data.WeiboRuleInfo.weiboSceneInfo.ReadNum;
        transform.Find("Bottom/CommentNumTxt").GetComponent<Text>().text = _data.WeiboRuleInfo.weiboSceneInfo.ConmentNum;
        int LikeNum = int.Parse(_data.WeiboRuleInfo.weiboSceneInfo.LikeNum);
        int likes = _data.IsLike ? LikeNum + 1 : LikeNum;
        transform.Find("Bottom/LikeNumTxt").GetComponent<Text>().text = likes.ToString();
        _likeBtn.interactable =! _data.IsLike;
    }
}
