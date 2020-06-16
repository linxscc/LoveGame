using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class RecollectionView : View
{

    private UserCardVo _userCardVo;

    public UserCardVo UserCardVo
    {
        get { return _userCardVo; }
    }

    //重构
    private Transform _choosePhotoOnClickTra;
    private Transform _cardContent;
    private Transform _btnContent;


    private Button _choosePhotoBtn;
    private RawImage _cardImg;
    private Text _cardNameTxt;
    private GameObject _propObj;  //炸鸡桶obj
    private Image _propImg;       //炸鸡桶Img
    private Text _consumeTxt;          //剩余可玩次数
    private Button _addConsumeNumBtn;  //增加可玩次数
    private Button _changeBtn;          //换一换
    private RawImage _signatureImg;    //签名

    private Button _leftPlayBtn;
    private Button _rightPlayBtn;

    private Text _leftPlayTxt;
    private Text _leftPropCostTxt;
    private Text _leftGoldCostTxt;

    private Text _rightPlayTxt;
    private Text _rightPropCostTxt;
    private Text _rightGoldCostTxt;
    private GameObject _notAvailable;//右边按钮置灰

    private Transform _maskAniContent;
    private RawImage _maskBlack;
    private RawImage _maskWhite;


   
    
    private int _propPrice; //花费星缘回忆体力道具的单价
    private int _goldPrice; // 金币花费的单价
    private int _playNumMax;//该星缘今日剩余冲洗次数/Max 的值

    private int _mayPlayNum; //可玩次数 上限是3次，下限是2次，变为1次按钮置灰不可点

    private Transform _mask;
        
    
    private void Awake()
    {
        _mask = transform.Find("Mask");
        _mask.gameObject.Hide();
        #region 重构

        _propPrice = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_POEWR);
        _goldPrice = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_GOLD);
        _playNumMax = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_NUM_MAX);
        
        _choosePhotoOnClickTra = transform.Find("Content/PhotoBg/ChoosePhotoOnClick");        
        _cardContent = transform.Find("Content/PhotoBg/CardContent");
        _btnContent = transform.Find("Content/BtnContent");

        _cardContent.gameObject.Hide();
        _btnContent.gameObject.Hide();
        
       

        _choosePhotoBtn = _choosePhotoOnClickTra.GetButton();
        _cardImg = _cardContent.GetRawImage("CardImg");
        _cardNameTxt = _cardContent.GetText("CardNameTxt");
        _propObj = _cardContent.Find("CardHint/PropIcon").gameObject;
        _propImg = _propObj.transform.GetImage();
        _consumeTxt = _cardContent.GetText("Num");
        _addConsumeNumBtn = _cardContent.GetButton("Num/AddIcon/AddBtn");
        _changeBtn = _cardContent.GetButton("ChangeBtn");
        _signatureImg = _cardContent.GetRawImage("Signature");
      
        _signatureImg.gameObject.Hide();

        _leftPlayBtn = _btnContent.GetButton("LeftBtn");
        _rightPlayBtn = _btnContent.GetButton("RightBtn");

        _leftPlayTxt = _leftPlayBtn.transform.GetText("Text");
        _leftPropCostTxt = _leftPlayBtn.transform.GetText("LeftCostBg/CostPropNum");
        _leftGoldCostTxt = _leftPlayBtn.transform.GetText("LeftCostBg/CostPropNum/GoldIcon/CostGoldNum");

        _rightPlayTxt = _rightPlayBtn.transform.GetText("Text");
        _rightPropCostTxt = _rightPlayBtn.transform.GetText("RightCostBg/CostPropNum");
        _rightGoldCostTxt = _rightPlayBtn.transform.GetText("RightCostBg/CostPropNum/GoldIcon/CostGoldNum");
        _notAvailable = _rightPlayBtn.transform.Find("NotAvailable").gameObject;
        _notAvailable.Hide();

        _maskAniContent = _cardContent.Find("MaskAniContent");
        _maskBlack = _maskAniContent.GetRawImage("MaskBlack");
        _maskWhite = _maskAniContent.GetRawImage("MaskWhite");
 
        //选择照片Btn事件
        _choosePhotoBtn.onClick.AddListener(() => { SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD));});
        //换一换Btn事件
        _changeBtn.onClick.AddListener(() => {SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD)); });
        //玩1次
        _leftPlayBtn.onClick.AddListener(OnClickPlayOneNumBtn);
        
        //玩未知次(次数可变)
        _rightPlayBtn.onClick.AddListener(OnClickPlayThreeNumBtn);
        
        //炸鸡桶点击事件
        PointerClickListener.Get(_propObj).onClick = go => { OnShowCardDropProp();};
        
        //主动点增加该卡回忆次数按钮
        _addConsumeNumBtn.onClick.AddListener(
            ()=>{ SendMessage(new Message(MessageConst.CMD_RECOLLECTION_BUY_COUNT, Message.MessageReciverType.CONTROLLER,
            _userCardVo.CardId));});
        
        _leftPlayTxt.text = I18NManager.Get("Recollection_LeftBtn",1);
        _leftPropCostTxt.text = "x "+_propPrice;
        _leftGoldCostTxt.text = "x " + _goldPrice;
        
        _rightPlayTxt.text = I18NManager.Get("Recollection_RightBtn",_playNumMax);
        _rightPropCostTxt.text = "x " + (_propPrice * _playNumMax);
        _rightGoldCostTxt.text = "x " + (_goldPrice * _playNumMax);
        
        PropImageAni();
        
        #endregion
    }

    /// <summary>
    /// 点击玩未知次按钮
    /// </summary>
    private void OnClickPlayThreeNumBtn()
    {
      Play(_playNumMax);  
    }


    /// <summary>
    /// 点击玩1次按钮
    /// </summary>
    private void OnClickPlayOneNumBtn()
    {
        Play(1);
    }


    /// <summary>
    /// 炸鸡桶左右摇摆动画
    /// </summary>
    private void PropImageAni()
    {
       
        var tween =  _propObj.transform.GetRectTransform().DOLocalRotate(new Vector3(0, 0, 10), 0.3f);
        tween.SetAutoKill(false);
        tween.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }

    public void SetRewardBtn(MissionStatusPB status)
    {
       
//        //string desc = "回忆奖励";
//        if (status == MissionStatusPB.StatusUnclaimed)   //未领取
//        {
//
//            // desc = "回忆奖励\n可领取";
//            _redPoint.SetActive(true);
//            _rewardBtn.onClick.RemoveAllListeners();
//            _rewardBtn.onClick.AddListener(delegate () {
//                _redPoint.SetActive(false);
//                ShowReward();
//            });
//        }
//        else if (status == MissionStatusPB.StatusBeRewardedWith)   //已领取
//        {
//            // desc = "回忆奖励\n(已领取)";
//            _redPoint.SetActive(false);
//            _rewardBtn.onClick.RemoveAllListeners();
//            _rewardBtn.onClick.AddListener(delegate () {
//                FlowText.ShowMessage(I18NManager.Get("Recollection_Hint1"));//("今日奖励已领取!");
//            });
//
//
//        }
//    
//       // _rewardBtn.transform.GetText("Text").text = desc;
//       
//        if (status == MissionStatusPB.StatusUnfinished)    //未完成
//        {
//            _rewardBtn.onClick.RemoveAllListeners();
//            _rewardBtn.onClick.AddListener(ShowReward);
//        }
     
    }

    public void SetEnergy(int energy, int max)
    {
        Text text = transform.GetText("PowerBar/Text");
        text.text = energy + "/" + max;
    }

    private void BuyCount()
    {
        SendMessage(new Message(MessageConst.CMD_RECOLLECTION_BUY_COUNT, Message.MessageReciverType.CONTROLLER,
            _userCardVo.CardId));
    }

    private void OnShowCardDropProp()
    {
        SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CARD_DROP_PROP,
            Message.MessageReciverType.DEFAULT, _userCardVo.CardVo.RecollectionDropItemId));
    }

    //private void BuyEnergy(GameObject go)
    //{
    //   // SendMessage(new Message(MessageConst.CMD_RECOLLECTION_BUY_ENERGY, Message.MessageReciverType.CONTROLLER));
    //}

    private void PlayThrice()
    {
        Play(3);
    }

    private void PlayOnce()
    {
        Play(1);
    }

    private void Play(int times)
    {
        if (_userCardVo == null)
        {
            FlowText.ShowMessage(I18NManager.Get("Recollection_Hint2"));//("请选择将回忆的星缘");
            return;
        }

     
         SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_PLAY, Message.MessageReciverType.CONTROLLER, times,
         _userCardVo));
    }

    private void ShowReward()
    {
        SendMessage(new Message(MessageConst.CMD_RECOLLECTION_SHOW_REWARD));
    }

    private void ShowChooseCard(GameObject go = null)
    {
       
        SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD));
    }

    public void SelectedCard(UserCardVo vo)
    {
        _userCardVo = vo;

   
        _choosePhotoOnClickTra.gameObject.Show();
        _cardContent.gameObject.Show();;
        _btnContent.gameObject.Show();
        

        SetCard(vo);

        _cardImg.texture =ResourceManager.Load<Texture>(vo.CardVo.BigCardPath(vo.UserNeedShowEvoCard()),ModuleName);
     
    }

    public void UpdateUserCard(UserCardVo vo)
    {
        SetCard(vo);
    }


    
    /// <summary>
    /// 特殊处理右边的Btn
    /// </summary>
    /// <param name="num"></param>
    private void SpecialDisposeRightBtn(int num)
    {
        if (num<_playNumMax)
        {
           _rightPlayBtn.enabled = false;  
           _notAvailable.Show();
        }
        else
        {
            _rightPlayBtn.enabled = true;
            _notAvailable.Hide();
        }
        
//        if (num==0||(num==1))
//        {
//          //右边按钮置灰，不可用
//          _rightPlayBtn.enabled = false;
//          _notAvailable.Show();
//
//          _rightPlayTxt.text = I18NManager.Get("Recollection_RightBtn",2);
//          _rightPropCostTxt.text = "x " + _propPrice * 2;
//          _rightGoldCostTxt.text = "x " + _goldPrice * 2;
//        }                      
//        else
//        {
//            _rightPlayBtn.enabled = true;
//            _notAvailable.Hide();
//        }        
    }

    /// <summary>
    /// 设置签名
    /// </summary>
    /// <param name="isSignature">是否有签名</param>
    private void SetSignature(bool isSignature)
    {
        if (isSignature)
        {
            _signatureImg.texture = ResourceManager.Load<Texture>("Prop/Signature/sign"+_userCardVo.CardId/1000);
            var width = _signatureImg.texture.width;
            var height = _signatureImg.texture.height;
            _signatureImg.transform.GetRectTransform().sizeDelta = new Vector2(width,height);  
            _signatureImg.gameObject.Show();
        }
        else
        {
            _signatureImg.gameObject.Hide();
        }
    }

    public void ShowMaskBlack()
    {
        _maskBlack.color =new Color(_maskBlack.color.r,_maskBlack.color.g,_maskBlack.color.b,0.6f);
    }
    
    public void MaskAni(float tweenSpeed1,float tweenSpeed2,Action aniOnComplete)
    {


        var blackAni1 = _maskBlack.DOColor(new Color(_maskBlack.color.r,_maskBlack.color.g,_maskBlack.color.b,0),tweenSpeed1).SetEase(Ease.Unset);
        var whiteAni1 = _maskWhite.DOColor(new Color(_maskWhite.color.r,_maskWhite.color.g,_maskWhite.color.b,1), tweenSpeed2).SetEase(Ease.Unset);
        var whiteAni2 = _maskWhite.DOColor(new Color(_maskWhite.color.r,_maskWhite.color.g,_maskWhite.color.b,0), tweenSpeed2).SetEase(Ease.Unset);

        Sequence sequences = DOTween.Sequence()
            .Append(blackAni1)
            .Append(whiteAni1)
            .Append(whiteAni2);
         sequences.onComplete = () =>
        {
            aniOnComplete();
        };
    }

    /// <summary>
    /// SetMask防止回忆时连续点击玩的按钮发请求
    /// </summary>
    /// <param name="isShow"></param>
    public void SetMask(bool isShow)
    {
        _mask.gameObject.SetActive(isShow);
            
    }
       
       
    
    private void SetCard(UserCardVo vo)
    {
        _userCardVo = vo;

        _cardNameTxt.text = vo.CardVo.CardName;
        _mayPlayNum = vo.RecollectionCount;

        if (_mayPlayNum>_playNumMax)
        {
            _mayPlayNum = _playNumMax;
        }
        _consumeTxt.text = I18NManager.Get("Recollection_ResidueNum",vo.RecollectionCount,_playNumMax);
        
//
//        _rightPlayTxt.text = I18NManager.Get("Recollection_RightBtn",_mayPlayNum);
//        _rightPropCostTxt.text = "x " + (_propPrice * _mayPlayNum);
//        _rightGoldCostTxt.text = "x " + (_goldPrice * _mayPlayNum);
        SpecialDisposeRightBtn(vo.RecollectionCount);
        
        _propImg.sprite =ResourceManager.Load<Sprite>("Prop/" + vo.CardVo.RecollectionDropItemId,ModuleName);

        var isSignature = _userCardVo.SignatureCard != null;
        SetSignature(isSignature);
        
//        
//        transform.Find("BgImage/WhiteBottomBg/Panel/PropImage/Tips/Text").GetComponent<Text>().text = I18NManager.Get("Recollection_RecollectionViewResidueNum", 3 - vo.RecollectionCount);
//           // "今日该星缘剩余次数" + (3 - vo.RecollectionCount) + "/3";
//
//            _buyBtn.gameObject.SetActive(vo.RecollectionCount>=3);
//            transform.Find("BgImage/WhiteBottomBg/Panel/PropImage/Tips/BuyDesc").gameObject.SetActive(vo.RecollectionCount>=3);  
//            if (vo.RecollectionCount>=3)
//            {
//                ChangeBuyIcon();
//            }    
//            
//            
//        Image propImage = transform.Find("BgImage/WhiteBottomBg/Panel/PropImage/Image").GetComponent<Image>();
//        propImage.sprite = ResourceManager.Load<Sprite>("Prop/" + vo.CardVo.RecollectionDropItemId,ModuleName);

        //PointerClickListener.Get(propImage.gameObject).onClick = go =>
        //{
        //    FlowText.ShowMessage(PropConst.GetTips(_userCardVo.CardVo.RecollectionDropItemId));
        //};

//        var hassignature = _userCardVo.SignatureCard != null;
//        _signature.gameObject.SetActive(hassignature);
//        if (hassignature)
//        {
//            _signature.texture = ResourceManager.Load<Texture>("Prop/Signature/sign"+_userCardVo.CardId/1000);
//
//        }
//
//        _propImage.gameObject.Show();

      
    }

    public void SetCost(int energyCost, int goldCost)
    {
//        transform.GetText("BottomTips/CostEnergy/Text").text = "x" + energyCost;
//        transform.GetText("BottomTips/CostGlod/Text").text = "x" + goldCost;
    }


    private void ChangeBuyIcon()
    {
//        var gemIcon = transform.Find("BgImage/WhiteBottomBg/Panel/PropImage/Tips/BuyBtn/GemIcon").gameObject;
//        var resetIcon =transform.Find("BgImage/WhiteBottomBg/Panel/PropImage/Tips/BuyBtn/ResetIcon").gameObject;
//        
//        var resetPropId = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_MEMORIES_RESET_ITEM_ID);
//        var resetPropNum = GlobalData.PropModel.GetUserProp(resetPropId).Num;
//        if (resetPropNum==0)
//        {
//            gemIcon.Show();
//            resetIcon.Hide();
//           
//        }
//        else
//        {
//            gemIcon.Hide();
//            resetIcon.Show();
//        }
//        SendMessage(new Message(MessageConst.CMD_RECOLLECTION_SHOW_BUY_DESC));
    }

    public void ShowBuyDesc(int costGem,int lastCostGem)
    {
//        
//        var text = transform.GetText("BgImage/WhiteBottomBg/Panel/PropImage/Tips/BuyDesc/Text");
//        if (costGem==-1)
//        {
//            costGem = lastCostGem;
//        }
//       
//        var resetPropId =GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_MEMORIES_RESET_ITEM_ID);
//        
//        var resetPropNum = GlobalData.PropModel.GetUserProp(resetPropId).Num;
//        text.text = resetPropNum==0 ? I18NManager.Get("Recollection_Hint16",costGem) : I18NManager.Get("Recollection_Hint17",resetPropNum);
        
    }
    
   
    
 
}