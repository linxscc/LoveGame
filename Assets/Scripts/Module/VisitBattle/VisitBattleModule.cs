using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using Google.Protobuf.Collections;
using Module.VisitBattle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitBattleModule : ModuleBase
{
    private VisitBattlePanel _visitBattlePanel;
    private VisitSupporterPanel _visitSupporterPanel;
    private VisitCallSuperStarPanel _visitCallSuperStarPanel;

    private VisitSuperStarPanel _visitSuperStarPanel;
    private GameObject _battleCommon;
    private GameObject _battleFinalBg;

    public float Power = 0;
    public float MaxPower = 0;
    private VisitBattleModel _model;

    public override void Init()
    {
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
      (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
        Debug.LogError("Init");
        AutoUnloadAtlas = false;

        _visitBattlePanel = new VisitBattlePanel();
        _visitBattlePanel.Init(this);
        _visitBattlePanel.Show(0);

        _visitBattlePanel.LoadFans(true);

        _battleCommon = InstantiateView("VisitBattle/Prefabs/Panels/VisitBattleCommon");
        _battleCommon.transform.SetParent(Parent.transform, false);

        _battleFinalBg = InstantiateView("Battle/FinalEstimate/FinalEstimateViewBg");
        _battleFinalBg.transform.SetParent(Parent.transform, false);
        _battleFinalBg.transform.SetSiblingIndex(0);
        _battleFinalBg.Hide();
        SetProgress(0, MaxPower);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_VISITBATTLE_SET_POWER:
                SetProgress((int)body[0], MaxPower);
                _battleCommon.Show();
                _model.IsGetBattleResult = true;
                ClientTimer.Instance.DelayCall(OnBattleResult, 1f);
                break;
            case MessageConst.CMD_VISITBATTLE_CHANGE_POWER:
                _battleCommon.Show();
                Power += (int)body[0];
                SetProgress(Power, MaxPower);
                break;
            case MessageConst.CMD_VISITBATTLE_SHOW_SUPPORTER_VIEW:
                _visitSupporterPanel = new VisitSupporterPanel();
                _visitSupporterPanel.Init(this);
                _visitSupporterPanel.Show(0);
                _currentPanel = _visitSupporterPanel;
                break;
            case MessageConst.CMD_VISITBATTLE_NEXT:
                if (_currentPanel == _visitSupporterPanel)
                {
                    //显示广场和粉丝动画
                    _currentPanel.Destroy();
                    _visitBattlePanel.ShowFans((bool)body[0], (bool)body[1]);

                    _currentPanel = _visitBattlePanel;
                }else if (_currentPanel == _visitBattlePanel)
                {
                    _visitCallSuperStarPanel = new VisitCallSuperStarPanel();
                    _visitCallSuperStarPanel.Init(this);
                    _visitCallSuperStarPanel.Show(0);
                    _currentPanel = _visitCallSuperStarPanel;
                }
                else if (_currentPanel == _visitCallSuperStarPanel)
                {
                    _currentPanel.Destroy();

                    _visitSuperStarPanel = new VisitSuperStarPanel();
                    _visitSuperStarPanel.Init(this);
                    _visitSuperStarPanel.Show(0);
                    _currentPanel = _visitSuperStarPanel;
                    _battleCommon.Hide();
                }
                break;
            case MessageConst.CMD_VISITBATTLE_SUPERSTAR_CONFIRM:
                _model.IsShowingAnimation = true;
                _visitSuperStarPanel.Hide();
                _battleCommon.Show();

                _visitBattlePanel.DoNext();
                break;
            case MessageConst.CMD_VISITBATTLE_FINISH:
                ModuleManager.Instance.GoBack();
                break;

            case MessageConst.CMD_VISITBATTLE_RESTART:
                //todo 检查探班次数
                //if (_model.LevelVo.CostEnergy > GlobalData.PlayerModel.PlayerVo.Energy)
                //{
                //    FlowText.ShowMessage("体力不足");
                //}
                //else
                //{
                    Reset();
                //}
                break;
            case MessageConst.CMD_VISITBATTLE_FANS_CALL_ANIMATION_FINISH:
                _model.IsShowingAnimation = false;
                OnBattleResult();
                break;
            case MessageConst.CMD_VISITBATTLE_SHOW_REWARD:
                _currentPanel.Destroy();
                _currentPanel = new VisitFinalEstimateRewardPanel();
                _currentPanel.Init(this);
                _currentPanel.Show(0);

                _battleFinalBg.Show();
                _battleCommon.Hide();
                _battleFinalBg.transform.SetSiblingIndex(0);
                break;
        }

    }

    private void Reset()
    {
        Power = 0;

        if (_currentPanel != null)
            _currentPanel.Destroy();

        _visitSuperStarPanel = null;

        SetProgress(0, MaxPower);

        _model.Reset();
        _visitBattlePanel.Restart();

        _battleFinalBg.Hide();
        _battleCommon.Show();

        //_battleCommon.transform.SetSiblingIndex(0);

        _battleCommon.transform.Find("PowerBar").gameObject.Show();
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("battleBgm"),
            (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
    }
    private void OnBattleResult()
    {
        if (_model.IsShowingAnimation)
            return;

        if (_model.IsGetBattleResult == false)
            return;

        VisitBattleResultData model = GetData<VisitBattleResultData>();

        if (model.Star > 0)
        {
            ShowBattleComment();
        }
        else
        {
            _currentPanel.Destroy();
            _currentPanel = new VisitFinalEstimateFailPanel();
            _currentPanel.Init(this);
            _currentPanel.Show(0);
            _battleFinalBg.Show();
            _battleFinalBg.transform.SetSiblingIndex(0);
        }
    }
    private void ShowBattleComment()
    {
        _currentPanel.Destroy();
        _currentPanel = new VisitFinalEstimateCommentPanel();
        _currentPanel.Init(this);
        _currentPanel.Show(0);

        _battleFinalBg.Show();
        _battleCommon.Hide();
        _battleFinalBg.transform.SetSiblingIndex(0);
    }

    public override void SetData(params object[] paramsObjects)
    {
        //先SetData 然后Init
        Debug.LogError("SetData");
        if (paramsObjects != null && paramsObjects.Length > 0)
        {
            _model = RegisterModel<VisitBattleModel>();
            RegisterModel<VisitBattleResultData>();
            _model.LevelVo = (VisitLevelVo)paramsObjects[0];

            MaxPower = _model.LevelVo.MaxPower;
            GetData<VisitBattleResultData>().Comments = (RepeatedField<VisitingLevelCommentRulePB>)paramsObjects[1];
            _model.CardNumRules = (List<ChallengeCardNumRulePB>)paramsObjects[2];
        }
    }

    private void SetProgress(float current, float max)
    {
        float percent = 0;
        if (max!=0)
        {
            percent = current / max;
        }

        Text text = _battleCommon.transform.Find("PowerBar/Text").GetComponent<Text>();
        text.text = "0/" + max;

        Util.TweenTextNum(text, 0.8f, (int)current, "", "/" + max);

        if (percent > 1)
            percent = 1;

        var percentWidth = percent * 605f;
        var bar = _battleCommon.transform.Find("PowerBar/ProgressBar/Bar").GetComponent<RectTransform>();
        var height = bar.sizeDelta.y;

        bar.DOSizeDelta(new Vector2(percentWidth, height), 0.8f)
            .SetEase(DG.Tweening.Ease.OutExpo)
            .onComplete = () =>
            {
            };

    }

    public override void Remove(float delay)
    {
        base.Remove(delay);
        AudioManager.Instance.PlayDefaultBgMusic();
    }
}

