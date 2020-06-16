using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityNpcInfoView : View
{
    private CanvasGroup _canvasGroup;
    private Transform _content;
    private RawImage _rawImage;
    

    private Text _cvTxt;
    private Text _birthdayTxt;
    private Text _bloodTypeTxt;
    private Text _constellationTxt;
    private Text _heightTxt;
    private Text _weightTxt;
    private Text _interestTxt;

    private Text _hobbyTxt;
    private Text _likeFoodTxt;

    private Button _moreInfoBtn;

    private string _moreInfo;


    private Transform _npcInfoBgTra;
    private RawImage _bg;
    private Image _moreInfoImg;


    private void Awake()
    {
        _rawImage = transform.GetRawImage("Image");
        _canvasGroup = transform.Find("NpcInfoBg/Content").GetComponent<CanvasGroup>();
        _content = transform.Find("NpcInfoBg/Content");
        _cvTxt = _content.GetText("CV");
        _birthdayTxt = _content.GetText("Birthday");
        _bloodTypeTxt = _content.GetText("BloodType");
        _constellationTxt = _content.GetText("Constellation");
        _heightTxt = _content.GetText("Height");
        _weightTxt = _content.GetText("Weight");
        _interestTxt = _content.GetText("Interest");
        _hobbyTxt = _content.GetText("Hobby");
        _likeFoodTxt = _content.GetText("LikeFood");


        _moreInfoBtn = transform.GetButton("NpcInfoBg/MoreInfoBtn");
        _moreInfoBtn.onClick.AddListener(OnClickMoreInfoBtn);

        _npcInfoBgTra = transform.Find("NpcInfoBg");
        _bg = _npcInfoBgTra.GetRawImage();
        _moreInfoImg = _npcInfoBgTra.GetImage("MoreInfoBtn");
    }

    private void OnClickMoreInfoBtn()
    {

        var window = PopupManager.ShowWindow<FavorabilityNpcInfoWindwo>("FavorabilityMain/Prefabs/FavorabilityNpcInfoWindwo");
        window.SetData(I18NManager.Get("Favorability_NpcInfoTitle"),_moreInfo);
        
    }


    public void SetData(FavorabilityNpcInfo info)
    {
//        _cvTxt.text = "CV:" + info.CV;
//        _birthdayTxt.text = "生日:" + info.Birthday;
//        _constellationTxt.text = "星座:" + info.Constellation;
//        _heightTxt.text = "身高:" + info.Height;
//        _weightTxt.text = "体重:" + info.Weight;
//        _bloodTypeTxt.text = "血型:" + info.BloodType;
//        _interestTxt.text = "兴趣:" + info.Interest;
//        _hobbyTxt.text = "爱好:" + info.Hobby;
//        _likeFoodTxt.text = "喜欢的食物:" + info.LikeFood;

        _cvTxt.text = I18NManager.Get("Favorability_NpcInfoCV",info.CV);
        _birthdayTxt.text = I18NManager.Get("Favorability_NpcInfoBirthday",info.Birthday);
        _constellationTxt.text = I18NManager.Get("Favorability_NpcInfoConstellation",info.Constellation);
        _heightTxt.text = I18NManager.Get("Favorability_NpcInfoHeight",info.Height);
        _weightTxt.text = I18NManager.Get("Favorability_NpcInfoWeight",info.Weight);
        _bloodTypeTxt.text = I18NManager.Get("Favorability_NpcInfoBloodType",info.BloodType);
        _interestTxt.text = I18NManager.Get("Favorability_NpcInfoInterest",info.Interest);
        _hobbyTxt.text = I18NManager.Get("Favorability_NpcInfoHobby",info.Hobby);
        _likeFoodTxt.text = I18NManager.Get("Favorability_NpcInfoLikeFood",info.LikeFood);
        
        _moreInfo = info.MoreInfo;

       
        _bg.texture = ResourceManager.Load<Texture>(info.BgPath);
        _moreInfoImg.sprite = ResourceManager.Load<Sprite>(info.BtnPath);
        _rawImage.texture = ResourceManager.Load<Texture>("Favorability/"+info.NpcId);
        
        //SendMessage(new Message(MessageConst.CMD_FACORABLILITY_PLAT_NPC_INFO_VOICE,
      //      Message.MessageReciverType.CONTROLLER, info.VoiceId));
        
        SetInfoBgAni();
    }

    private void SetInfoBgAni()
    {
        ClientTimer.Instance.DelayCall(() => { _canvasGroup.alpha = 1.0f; }, 1.0f);
    }
}