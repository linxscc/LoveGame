using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class GamePlayModule : ModuleBase
{
    private GamePlayPanel _gamePlayPanel;
    private bool _isShow = false;
    private int showTargetPanel = 0;
    
    public override void Init()
    {
        GuideManager.RegisterModule(this);
        
        _gamePlayPanel = new GamePlayPanel();
        _gamePlayPanel.SetComplexPanel();
        switch (showTargetPanel)
        {
            case  0:
                _gamePlayPanel.Init(this);
                _gamePlayPanel.Show(0.6f);
                _gamePlayPanel.IsShowArrow(_isShow);
                break;
            case 1:
                _gamePlayPanel.TargetPanel = 1;
                _gamePlayPanel.Init(this);
                _gamePlayPanel.Show(0.6f);
                _gamePlayPanel.IsShowArrow(_isShow);
                break;
            
        }

    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _gamePlayPanel.Show(0.6f);
        _gamePlayPanel.IsShowArrow(false);
        GuideManager.OpenGuide(this);
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        int openLevel;
        switch (name)
        {
            case MessageConst.MODULE_GAMEPLAY_GOTO_MAIN_LINE:
                openLevel = GuideManager.GetOpenUserLevel(ModulePB.Career, FunctionIDPB.LevelEntry);
                if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
                }
                else
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE, false, false);
                }
                break;
            case MessageConst.MODULE_GAMEPLAY_GOTO_SUPPORTERACTIVITY:
                openLevel = GuideManager.GetOpenUserLevel(ModulePB.EncourageAct, FunctionIDPB.EncourageActEntry);
                if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
                }
                else
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SUPPORTERACTIVITY, false, true);
                }
                break;
            case MessageConst.MODULE_GAMEPLAY_GOTO_RECOLLECTION:
                openLevel = GuideManager.GetOpenUserLevel(ModulePB.CardMemories, FunctionIDPB.CardMemoriesEntry);
                if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
                }
                else
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_RECOLLECTION, false, true);
                }
                break;
            case MessageConst.MODULE_GAMEPLAY_GOTO_EXERCISE_ROOM:
                FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));  
                break;
            case MessageConst.MODULE_GAMEPLAY_GOTO_VISIT:
                openLevel = GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);
                if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
                }
                else
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_VISIT, false, true);
                }

                break;
            
            case MessageConst.MODULE_GAMEPLAY_GOTO_SHOPPING:
                openLevel = GuideManager.GetOpenUserLevel(ModulePB.Shopping, FunctionIDPB.FunctionShopping);
                if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
                }
                else
                {
                    _gamePlayPanel.ShowShopping();
                }
                break;
            
           case MessageConst.MODULE_GAMEPLAY_GOTO_TRAININGROOM:
               ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_TRAININGROOM, false, false);
               break;               
        }
        
        
        
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length > 0)
        {
            if (paramsObjects[0] is string)
            {
                var jumptarget = (string) paramsObjects[0];
                switch (jumptarget)
                {
                    case  "Shopping":
                        showTargetPanel = 1;
                        break;
                    default:
                        showTargetPanel = 0;
                        break;
                }
                
                
            }
            else
            {
                var isShow = (bool) paramsObjects[0];
                Debug.LogError("接首是否显示===>"+isShow);
                _isShow = isShow;  
            }

            
        }
    }
}