using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using Componets;
using DataModel;
using game.main;
using game.main.Live2d;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DG.Tweening;
using GalaAccountSystem;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavorabilityMainView : View
{
    private RawImage _bgImage;
    private Live2dGraphic _live2DGraphic;
    private Transform _btn;
    private Button _showinMain;
    private Button _cloth;
    private Button _npcInfoBtn;
    private Button _sendGift;
    private Button _voice;    
    private Transform _bottomBG;
    private Transform _redHeart;
    private GameObject _onClick;
    private Transform _head;
    private Transform _body;    
    bool _isClick = false;
    private int _npcId;
    private Text _level;
    private int _maxLevel;
    private ProgressBar _progressBar;
    private Text _progressText;
    private void Awake()
    {
        
        _maxLevel = GlobalData.FavorabilityMainModel.GetLatsFavorabilityLevelRulePB().Level;
        _redHeart = transform.Find("Redheart");
        _level = _redHeart.GetText("Image/LevelText");
        _progressBar = _redHeart.Find("ProgressBar").GetComponent<ProgressBar>();
        _progressText = _redHeart.GetText("ProgressText");
        
        _live2DGraphic = transform.Find("CharacterContainer/Live2dGraphic").GetComponent<Live2dGraphic>();

        if ((float) Screen.height / Screen.width > 1.8f)
        {

            _live2DGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        _bgImage = transform.Find("BG").GetComponent<RawImage>();      
        _btn = transform.Find("Btn");
        _showinMain = transform.Find("Btn/ShowInMain").GetComponent<Button>();
        _cloth = transform.Find("Btn/Cloth").GetComponent<Button>();
        _npcInfoBtn = transform.Find("Btn/NpcInfo").GetComponent<Button>();
        _sendGift = transform.Find("Btn/SendGift").GetComponent<Button>();
        _showinMain.onClick.AddListener(ShowInMain);
        _cloth.onClick.AddListener(OnCloth);
        _npcInfoBtn.onClick.AddListener(PhoneEvent);
        _sendGift.onClick.AddListener(SendGift);
        _onClick = transform.Find("OnClick").gameObject;
        _onClick.Hide();


        GuideSet();

        _head = transform.Find("CharacterContainer/Live2dGraphic/Head");
        _body = transform.Find("CharacterContainer/Live2dGraphic/Body");

        PointerClickListener.Get(_head.gameObject).onClick = Live2dClickTigger;
        PointerClickListener.Get(_body.gameObject).onClick = Live2dClickTigger;
        
        _voice = transform.GetButton("Btn/VoiceCollect");
        _voice.onClick.AddListener(VoiceBtn);
       
      
    }

    private void VoiceBtn()
    { 
        SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_VOICE_WINDOW));
    }

  

    private void Live2dClickTigger(GameObject obj)
    {
        EXPRESSIONTRIGERTYPE eType = EXPRESSIONTRIGERTYPE.NO;
        if (obj.name == "Head")
        {
            eType = EXPRESSIONTRIGERTYPE.HEAD;
        }
        else if (obj.name == "Body")
        {
            eType = EXPRESSIONTRIGERTYPE.BODY;
        }
        else
        {
            eType = EXPRESSIONTRIGERTYPE.NORMAL;
        }
        if (_isClick)
            return;
        if (AudioManager.Instance.IsPlayingDubbing)
            return;
        
        _isClick = true;
        ExpressionInfo expressionInfo = ClientData.GetRandomExpression(_npcId, eType);
        Live2dTigger(eType);
    }

    /// <summary>
    /// 触发好感度主机界面摸摸乐语音
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="labelId"></param>
    /// <param name="isSendClick"></param>
    private void Live2dTigger(EXPRESSIONTRIGERTYPE eType, int labelId = -1, bool isSendClick = true)
    {
        L2DModel model = _live2DGraphic.GetMainLive2DView.Model;
        ExpressionInfo expressionInfo = ClientData.GetRandomExpression(_npcId, eType, labelId);
        if (expressionInfo==null)
        {
           Debug.Log("expressionInfo == null");
           _isClick = false;
           return;  
        }

        if (!model.IsIdle)
        {
           _isClick = false;
           Debug.LogError("model  is busy  ");
           return;  
        }
        
        _live2DGraphic.GetMainLive2DView.LipSync = true;


        if (expressionInfo.Dialog == "")
        {
            Debug.Log("expressionInfo.Dialog == null");
            model.SetExpression(model.ExpressionList[expressionInfo.Id], 2);
            _isClick = false;
            return;
        }
        
        new AssetLoader().LoadAudio(AssetLoader.GetMainPanleDialogById(expressionInfo.Dialog), //expressionInfo.Dialog),
            (clip, loader) =>
            {
                AudioManager.Instance.PlayDubbing(clip);
                Debug.Log("AudioManager.Instance.PlayDubbing");
                model.SetExpression(model.ExpressionList[expressionInfo.Id], clip.length + 1);
                _isClick = false;
            });
    }
    
    /// <summary>
    /// 新手引导隐藏按钮设置
    /// </summary>
    private void GuideSet()
    {
        transform.Find("Redheart").gameObject
            .SetActive(GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts));
    }
    
    private void SendGift()
    {
        if (!GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts))
        {
            string desc = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts);
            FlowText.ShowMessage(desc);
            return;
        }
        _btn.gameObject.SetActive(false);
        //发送进入送礼界面的消息
        SendMessage(new Message(MessageConst.CMD_FACORABLILITY_ENTER_SEND_GIVE_GIFTS, Message.MessageReciverType.DEFAULT,_redHeart.gameObject,false));     
    }

    public void JumpTo(string name)
    {
        switch (name)
        {
           case  "SendGift":
               if (!GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts))
               {
                   string desc = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts);
                   FlowText.ShowMessage(desc);
                   return;                 
               }
               _btn.gameObject.SetActive(false);                               
               SendMessage(new Message(MessageConst.CMD_FACORABLILITY_ENTER_SEND_GIVE_GIFTS, Message.MessageReciverType.DEFAULT,_redHeart.gameObject,true)); 
               break;
           case "Voice":
               VoiceBtn();
               break;
        }

    }

    public void OnShowMainBtn()
    {
        _btn.gameObject.SetActive(true);
        _redHeart.gameObject.Show();
    }
    
       
    private void PhoneEvent()
    {
        _redHeart.gameObject.Hide();
        _btn.gameObject.SetActive(false);
        SendMessage(new Message(MessageConst.MODULE_FACORABLILITY_ON_SHOW_NPC_INFO));
    }

    private void OnCloth()
    {
        if (!GuideManager.IsOpen( ModulePB.Favorability, FunctionIDPB.FavorabilityClothes))
        {
           string desc = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityClothes);                    
           FlowText.ShowMessage(desc);
            return; 
        }
        
        SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_CLOTHPANEL_BTN));
    }

    private void ShowInMain()
    {
        SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_MAINVIEW));

    }
    UserFavorabilityVo _userFavorabilityVo;
    public void SetData(UserFavorabilityVo vo)
    {
        _userFavorabilityVo = vo;
        _npcId = (int) vo.Player;               
        _level.text = vo.Level.ToString();
        var curNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelExpNeed(vo.Level);
        var curShowNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelRule(vo.Level).Exp;
        if (vo.Level == _maxLevel)
        {
            _progressText.text = GlobalData.FavorabilityMainModel.GetLastExp().ToString();
            _progressBar.DeltaX = 0;
            _progressBar.ProgressY = 0;
        }
        else
        {
            _progressText.text = vo.ShowExp + "/" + curShowNeedExp;
            _progressBar.DeltaX = 0;
            _progressBar.ProgressY = (int) ((float) vo.Exp / curNeedExp * 100);
        }
        _live2DGraphic.LoadAnimationById(vo.Apparel[0].ToString());     
        BGTexture(GlobalData.FavorabilityMainModel.GetBgImageName(vo.Apparel[1]));     
    }





    private void BGTexture(string image)
    {
        _bgImage.texture = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(image), ModuleName);
    }

    /// <summary>
    /// 触发送礼语音
    /// </summary>
    /// <param name="pbId"></param>
    /// <param name="itemId"></param>
    public void Live2dTiggerGiftVoice(int pbId, int itemId)
    {
        L2DModel model = _live2DGraphic.GetMainLive2DView.Model;
        ExpressionInfo expressionInfo = ClientData.GetRandomGiftExpression(pbId, itemId);
        if (expressionInfo == null)
            return;
        _live2DGraphic.GetMainLive2DView.LipSync = true;
        model.SetExpression(model.ExpressionList[expressionInfo.Id]);
        if (expressionInfo.Dialog == "")
            return;
        new AssetLoader().LoadAudio(
            AssetLoader.GetMainPanleDialogById(expressionInfo.Dialog), //expressionInfo.Dialog),
            (clip, loader) => { AudioManager.Instance.PlayDubbing(clip); });
    }


    /// <summary>
    /// 触发语音Item语音
    /// </summary>
    /// <param name="info"></param>
    public void Live2dTiggerVoice(CardAwardPreInfo info)
    {
        L2DModel model = _live2DGraphic.GetMainLive2DView.Model;
        _live2DGraphic.GetMainLive2DView.LipSync = true;

        if (info.dialogId == "")
        {
            model.SetExpression(model.ExpressionList[info.expressionId], 2);
            return;
        }
        new AssetLoader().LoadAudio(
            AssetLoader.GetMainPanleDialogById(info.dialogId), //expressionInfo.Dialog),
            (clip, loader) => {
                model.SetExpression(model.ExpressionList[info.expressionId], clip.length+1);
                AudioManager.Instance.PlayDubbing(clip);
            });
        
    }

    /// <summary>
    /// 触发男主信息默认第一句语音
    /// </summary>
    /// <param name="dialogId"></param>
    public void Live2DTiggerNpcInfoFirstVoice(string dialogId)
    {
      
        L2DModel model = _live2DGraphic.GetMainLive2DView.Model;
        _live2DGraphic.GetMainLive2DView.LipSync = true;
        
        new AssetLoader().LoadAudio(
            AssetLoader.GetMainPanleDialogById(dialogId), 
            (clip, loader) => { AudioManager.Instance.PlayDubbing(clip); });
 
       
        
    }

    public void HideL2D()
    {
       ClientTimer.Instance.DelayCall(() => { _live2DGraphic.Hide();},0.5f);        
    }


    public override void Show(float delay = 0)
    {   
        _live2DGraphic.Show();
    }
}



  


