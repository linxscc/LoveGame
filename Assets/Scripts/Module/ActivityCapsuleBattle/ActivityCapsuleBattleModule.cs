using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class ActivityCapsuleBattleModule : ModuleBase
{
    private Panel _currentPanel;
    public float Power = 0;
    public float MaxPower = 0;
    private CapsuleBattlePanel _capsuleBattlePanel;
    
    private CapsuleBattleModel _model;

    private GameObject _battleCommon;
    private GameObject _battleFinalBg;
    
    private CapsuleSupporterPanel _capsuleSupporterPanel;
    private CapsuleSuperStarPanel _capsuleSuperStarPanel;
    private Queue<int> roleIds;
    public override void Init()
    {
        DelayUnloadAtlas = 0.2f;
        
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
            (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
        
        AutoUnloadAtlas = false;
        
        _capsuleBattlePanel = new CapsuleBattlePanel();
        _capsuleBattlePanel.Init(this);
        _capsuleBattlePanel.Show(0);
        
        _battleCommon = InstantiateView("ActivityCapsuleBattle/Prefabs/CapsuleBattleCommon");
        _battleCommon.transform.SetParent(Parent.transform, false);
        
        _battleFinalBg = InstantiateView("ActivityCapsuleBattle/Prefabs/FinalEstimateViewBg");
        _battleFinalBg.transform.SetParent(Parent.transform, false);
        _battleFinalBg.transform.SetSiblingIndex(0);
        _battleFinalBg.Hide();
        
        SetProgress(0, MaxPower);
    }

    private void SetProgress(float current, float max, bool isReset = false)
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
        var height = bar.sizeDelta.y;
        var sizeDeltaX= bar.DOSizeDelta(new Vector2(percentWidth, height), speed)
            .SetEase(DG.Tweening.Ease.OutExpo);
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects != null && paramsObjects.Length > 0)
        {
            _model = RegisterModel<CapsuleBattleModel>();
            RegisterModel<CapsuleBattleResultData>();
            _model.LevelVo = (CapsuleLevelVo) paramsObjects[0];
            MaxPower = _model.LevelVo.MaxPower;
            GetData<CapsuleBattleResultData>().Comments = (RepeatedField<LevelCommentRulePB>) paramsObjects[1];
            _model.CardNumRules = (RepeatedField<ChallengeCardNumRulePB>) paramsObjects[2];
            SdkHelper.StatisticsAgent.OnMissionBegin(_model.LevelVo.LevelMark);
        }
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_CAPSULEBATTLE_SET_POWER:
                SetProgress((int) body[0], MaxPower);
                _battleCommon.Show();
                _model.IsGetBattleResult = true;
                ClientTimer.Instance.DelayCall(OnBattleResult, 1f);
                break;
            case MessageConst.CMD_CAPSULEBATTLE_CHANGE_POWER:
                _battleCommon.Show();
                Power += (int) body[0];
                SetProgress(Power, MaxPower); 
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SHOW_SUPPORTER_VIEW:
                _capsuleSupporterPanel =new CapsuleSupporterPanel();
                _capsuleSupporterPanel.Init(this);
                _capsuleSupporterPanel.Show(0);
                _currentPanel = _capsuleSupporterPanel;
                break;
            case MessageConst.CMD_CAPSULEBATTLE_NEXT:
                if (_currentPanel == _capsuleSupporterPanel)
                {
                    _currentPanel.Destroy();
                    Queue<string> fansInfo = (Queue<string>) body[0];
                    int tempPower = (int) body[1];
                    _capsuleBattlePanel.GetFansInfo(fansInfo);
                    _capsuleBattlePanel.GetPower(tempPower); 
                    _currentPanel = _capsuleBattlePanel;
                }
                else if(_currentPanel == _capsuleBattlePanel)
                {
                    _capsuleSuperStarPanel =new CapsuleSuperStarPanel();
                    _capsuleSuperStarPanel.Init(this);
                    _capsuleSuperStarPanel.Show(0);
                    _currentPanel = _capsuleSuperStarPanel;
                    _battleCommon.Hide();
                }
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SUPERSTAR_CONFIRM:
                _model.IsShowingAnimation = true;
                _capsuleSuperStarPanel.Hide();
                _battleCommon.Show();
                Power += (int) body[0];
                SetProgress(Power, MaxPower);              
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SHOW_REWARD:
                _currentPanel.Destroy();
                _currentPanel = new CapsuleFinalEstimateRewardPanel();
                _currentPanel.Init(this);

                _battleFinalBg.Show();
                _battleCommon.Hide();
                _battleFinalBg.transform.SetSiblingIndex(0);
                break;
            case MessageConst.CMD_CAPSULEBATTLE_FINISH:
                ClientTimer.Instance.DelayCall(() =>
                {
                    if(body != null && body.Length > 0)
                    {
                        string moduleName = (string) body[0];
                        ModuleManager.Instance.EnterModule(moduleName);
                    }
                    else
                    {                     
                        ModuleManager.Instance.GoBack(); 
                    }     
                }, 0.3f);
                break;
//            case MessageConst.CMD_BATTLE_RESTART:
//                if (_model.LevelVo.CostEnergy > GlobalData.PlayerModel.PlayerVo.Energy)
//                {
//                    FlowText.ShowMessage(I18NManager.Get("MainLine_BattleIntroductionPopupHint1"));
//                }
//                else
//                {
//                    Reset();
//                }
//                break;
            case MessageConst.CMD_CAPSULEBATTLE_FANS_CALL_ANIMATION_FINISH:
                _model.IsShowingAnimation = false;
                OnBattleResult();
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SEND_ROLE_ID:
                roleIds = (Queue<int>) body[0];
                break;
            case MessageConst.CMD_CAPSULEBATTLE_GET_RES:
                _battleCommon.Hide();
                _capsuleBattlePanel.HideBackBtn();
                _capsuleBattlePanel.GetRolesId(roleIds);
                break;
            case MessageConst.CMD_CAPSULEBATTLE_HIDE_PROGRESS:
                _battleCommon.Hide();              
                break;
        }
    }
    
    private void Reset()
    {
        Power = 0;

        if (_currentPanel != null)
            _currentPanel.Destroy();

        _capsuleSuperStarPanel = null;
    
        SetProgress(0, MaxPower,true);

        _model.Reset();
        _capsuleBattlePanel.Restart();
        
        _battleFinalBg.Hide();
        _battleCommon.Show();

        _battleCommon.transform.Find("PowerBar").gameObject.Show();
        
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
            (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
    }
    
    private void OnBattleResult()
    {
        if(_model.IsShowingAnimation)
            return;
        
        if(_model.IsGetBattleResult == false)
            return;
        
        CapsuleBattleResultData model = GetData<CapsuleBattleResultData>();

        if (model.Star > 0)
        {
            ShowBattleComment();
        }
        else
        {           
            _currentPanel.Destroy();
            _currentPanel = new CapsuleFinalEstimateFailPanel();
            _currentPanel.Init(this);
           
            _battleFinalBg.Show();
            _battleFinalBg.transform.SetSiblingIndex(0);
        }
    }
    
    
    private void ShowBattleComment()
    {
        _currentPanel.Destroy();
        _currentPanel = new CapsuleFinalEstimateCommentPanel();
        _currentPanel.Init(this);

        _battleFinalBg.Show();
        _battleCommon.Hide();
        _battleFinalBg.transform.SetSiblingIndex(0);
    }
    
    
    
    public override void Remove(float delay)
    {
        base.Remove(delay);
        AudioManager.Instance.PlayDefaultBgMusic();
        
    }
   
}
