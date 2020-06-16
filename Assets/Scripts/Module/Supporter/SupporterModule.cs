using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using System.Collections.Generic;

public class SupporterModule : ModuleBase {
    
    private SupporterViewPanel _supporterViewPanel;
    
    private FansPanel _fansPanel;


    public override void Init()
    {
        _supporterViewPanel = new SupporterViewPanel();
        _supporterViewPanel.Init(this);
        _supporterViewPanel.Show(0);
        
        GuideManager.RegisterModule(this);
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _supporterViewPanel.RefreshRedPoint();
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        switch (name)
        {
            case MessageConst.MODULE_SUPPOTER_GOTO_FRIENDS:
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FRIENDS,false);   
                break;
            case MessageConst.MODULE_SUPPOTER_GOTO_AIRBORNEGAME:
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_AIRBORNEGAME, false);
                break;
            case MessageConst.MODULE_SUPPOTER_GOTO_TAKEPHOTO:
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_TAKEPHOTOSGAME, false);
                break;
            case MessageConst.MODULE_SUPPOTER_GOTO_MORN_FANS:
                if (_fansPanel == null)
                {
                    _fansPanel = new FansPanel();
                    _fansPanel.Init(this);
                }
                _fansPanel.Show(0);
                _supporterViewPanel.Hide();
                break;
            case MessageConst.MODULE_SUPPOTER_BACK_SUPPOTER:
                _fansPanel.Hide();
                _supporterViewPanel.Show(0);
                break;
        }
    }

  
    public override void Remove(float delay)
    {
        base.Remove(delay);
        _supporterViewPanel.Destroy();
        
    }

    
}
