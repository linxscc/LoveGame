using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;
using Ease = DG.Tweening.Ease;

public class BattleModule : ModuleBase
{
    private Panel _currentPanel;

    private SupportStrengthPanel _supportStrengthPanel;
    private SupporterPanel _supporterPanel;
    private CallSuperStarPanel _callSuperStarPanel;
    private SuperStarPanel _superStarPanel;

    private GameObject _battleCommon;
    private GameObject _battleFinalBg;

    private int _defaultPage = 0;

    private RawImage _animImage;
    private int _aniIndex = 0;

    public float Power = 0;
    public float MaxPower = 0;
    private bool _showSignUpAnimation;
//    private SignUpAnimationPanel _signUpAndimationPanel;
    private BattlePanel _battlePanel;

    private BattleModel _model;
    private Queue<int> roleIds;
   
    public override void Init()
    {
        DelayUnloadAtlas = 0.2f;
        
        GuideManager.RegisterModule(this);
        
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
            (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
        
        AutoUnloadAtlas = false;

        _battlePanel = new BattlePanel();
        _battlePanel.Init(this);
        _battlePanel.Show(0);

        _battleCommon = InstantiateView("Battle/Prefabs/Panels/BattleCommon");
        _battleCommon.transform.SetParent(Parent.transform, false);

        _battleFinalBg = InstantiateView("Battle/FinalEstimate/FinalEstimateViewBg");
        _battleFinalBg.transform.SetParent(Parent.transform, false);
        _battleFinalBg.transform.SetSiblingIndex(0);
        _battleFinalBg.Hide();

        SetProgress(0, MaxPower);
    }

    private void Reset()
    {
        Power = 0;

        if (_currentPanel != null)
            _currentPanel.Destroy();

        _superStarPanel = null;

      
        
        SetProgress(0, MaxPower,true);

        _model.Reset();
        _battlePanel.Restart();
        
        _battleFinalBg.Hide();
        _battleCommon.Show();

//        _battleCommon.transform.SetSiblingIndex(0);
        _battleCommon.transform.Find("PowerBar").gameObject.Show();
        
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
            (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_BATTLE_SET_POWER:
                SetProgress((int) body[0], MaxPower);
                _battleCommon.Show();
                _model.IsGetBattleResult = true;
                ClientTimer.Instance.DelayCall(OnBattleResult, 1f);
                break;
            case MessageConst.CMD_BATTLE_CHANGE_POWER:
                _battleCommon.Show();
                Power += (int) body[0];
//                Debug.LogError("进度条增加的值==="+Power);
                SetProgress(Power, MaxPower);
                break;
            case MessageConst.CMD_BATTLE_SHOW_SUPPORTER_VIEW:
                _supporterPanel = new SupporterPanel();
                _supporterPanel.Init(this);
                _supporterPanel.Show(0);
                _currentPanel = _supporterPanel;
                break;
            case MessageConst.CMD_BATTLE_NEXT:
                if (_currentPanel == _supporterPanel)
                {                    
                  //  _battleCommon.Hide();
                    //显示广场和粉丝动画
                    _currentPanel.Destroy();

                    Queue<string> fansInfo = (Queue<string>) body[0];
                    int tempPower = (int) body[1];
                    _battlePanel.GetFansInfo(fansInfo);
                    _battlePanel.GetPower(tempPower);                                 
                    _currentPanel = _battlePanel;
                }
                else if(_currentPanel == _battlePanel)
                {
                    _superStarPanel = new SuperStarPanel();
                    _superStarPanel.Init(this);
                    _superStarPanel.Show(0);
                    _currentPanel = _superStarPanel;
                    _battleCommon.Hide();
                }                
                break;

            
            case MessageConst.CMD_BATTLE_SUPERSTAR_CONFIRM:
                _model.IsShowingAnimation = true;
                _superStarPanel.Hide();
                _battleCommon.Show();
                Power += (int) body[0];
                SetProgress(Power, MaxPower);
               
                break;
            
            case MessageConst.CMD_BATTLE_SHOW_REWARD:
                _currentPanel.Destroy();
                _currentPanel = new FinalEstimateRewardPanel();
                _currentPanel.Init(this);

                _battleFinalBg.Show();
                _battleCommon.Hide();
                _battleFinalBg.transform.SetSiblingIndex(0);
                break;

            case MessageConst.CMD_BATTLE_FINISH:
//                if(body != null && body.Length > 0)
//                {
//                    string moduleName = (string) body[0];
//                    ModuleManager.Instance.EnterModule(moduleName);
//                }
//                else
//                {                 
//                    ModuleManager.Instance.GoBack(); 
//                }
                ClientTimer.Instance.DelayCall(() =>
                {
                    if(body != null && body.Length > 0)
                    {
                        string moduleName = (string) body[0];
                        ModuleManager.Instance.EnterModule(moduleName);
                    }
                    else
                    {
                        // 
                        ModuleManager.Instance.GoBack(); 
                    }     
                }, 0.3f);
                          
                break;
           
            case MessageConst.CMD_BATTLE_RESTART:
                if (_model.LevelVo.CostEnergy > GlobalData.PlayerModel.PlayerVo.Energy)
                {
                    FlowText.ShowMessage(I18NManager.Get("MainLine_BattleIntroductionPopupHint1"));
                }
                else
                {
                    Reset();
                }
                break;
            case MessageConst.CMD_BATTLE_FANS_CALL_ANIMATION_FINISH:
                _model.IsShowingAnimation = false;
                OnBattleResult();
                break;
            
            case MessageConst.CMD_BATTLE_SEND_ROLE_ID:

                roleIds = (Queue<int>) body[0];
                
                                       
                break;
            case MessageConst.CMD_BATTLE_GET_RES:
                _battleCommon.Hide();
                _battlePanel.HideBackBtn();
                _battlePanel.GetRolesId(roleIds);
                break;
                                  
            case MessageConst.CMD_BATTLE_HIDE_PROGRESS:
                _battleCommon.Hide();              
                break;
           
        }
    }

   
    private void OnBattleResult()
    {
        if(_model.IsShowingAnimation)
            return;
        
        if(_model.IsGetBattleResult == false)
            return;
        
        BattleResultData model = GetData<BattleResultData>();

        if (model.Star > 0)
        {
            ShowBattleComment();
        }
        else
        {           
            SendMessage(new Message(MessageConst.MODULE_BATTLE_SHOW_FAIL));
            _currentPanel.Destroy();
            _currentPanel = new FinalEstimateFailPanel();
            _currentPanel.Init(this);
           
            _battleFinalBg.Show();
            _battleFinalBg.transform.SetSiblingIndex(0);
        }
    }

    private void ShowBattleComment()
    {
        _currentPanel.Destroy();
        _currentPanel = new FinalEstimateCommentPanel();
        _currentPanel.Init(this);

        _battleFinalBg.Show();
        _battleCommon.Hide();
        _battleFinalBg.transform.SetSiblingIndex(0);
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects != null && paramsObjects.Length > 0)
        {
            _model = RegisterModel<BattleModel>();
            RegisterModel<BattleResultData>();
            
            _model.LevelVo = (LevelVo) paramsObjects[0];

            MaxPower = _model.LevelVo.MaxPower;

            GetData<BattleResultData>().Comments = (RepeatedField<LevelCommentRulePB>) paramsObjects[1];
            _model.CardNumRules = (RepeatedField<ChallengeCardNumRulePB>) paramsObjects[2];
            
            SdkHelper.StatisticsAgent.OnMissionBegin(_model.LevelVo.LevelMark);
        }
    }

    private void SetProgress(float current, float max,bool isReset=false)
    {
        var percent = current / max;

        Text text = _battleCommon.transform.Find("PowerBar/Text").GetComponent<Text>();
        text.text = "0/" + max;

        float speed = 0.8f;
        
        if (isReset)
        {
            speed = 0;
        }
       
        Util.TweenTextNum(text, speed, (int) current, "", "/" + max);

        if (percent > 1)
            percent = 1;

        var percentWidth = percent * 605f;
        var bar = _battleCommon.transform.Find("PowerBar/ProgressBar/Bar").GetComponent<RectTransform>();
        
        Debug.LogError("StarInit===>"+bar.sizeDelta.x);
        var height = bar.sizeDelta.y;

     var sizeDeltaX= bar.DOSizeDelta(new Vector2(percentWidth, height), speed)
            .SetEase(DG.Tweening.Ease.OutExpo);

//     sizeDeltaX.onUpdate = () =>
//     {
//         Debug.LogError("bar.sizeDelta.x===>"+bar.sizeDelta.x+";bar.sizeDelta.y==>"+bar.sizeDelta.y+";percentWidth===>"+percentWidth);
//        
//     };
//     sizeDeltaX.onComplete = () =>
//         {
//            if (_showSignUpAnimation)
//            {
//                _showSignUpAnimation = false;
//                _currentPanel.Destroy();
//                _signUpAndimationPanel = new SignUpAnimationPanel();
//                _signUpAndimationPanel.Init(this);
//                _signUpAndimationPanel.Show(0);
//                _currentPanel = _signUpAndimationPanel;
//                _battleCommon.Hide();
//            }
//        };           
    }


    
  


    public override void Remove(float delay)
    {
        base.Remove(delay);
        AudioManager.Instance.PlayDefaultBgMusic();
        
    }
}