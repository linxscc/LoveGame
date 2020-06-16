using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;

public class ReloadingModule : ModuleBase
{  
    private ReloadingPanel _reloadingPanel;      
    public override void Init()
    {
        _reloadingPanel = new ReloadingPanel();
        _reloadingPanel.Init(this);
        _reloadingPanel.Show(0.5f);                       
    }


    public override void OnShow(float delay)
    {
        base.OnShow(delay);       
    }


    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {          
            case MessageConst.MODULE_FACORABLILITY_SHOW_SAVEANDSHARE_VIEW:
              //  _saveAndSharePanel = new SaveAndSharePanel();
             //   _saveAndSharePanel.Init(this);
              
                if (_reloadingPanel!=null)
                {
                    _reloadingPanel.Hide();
                }

              //  _saveAndSharePanel.Show(0);

                break;
            case MessageConst.MODULE_FACORABLILITY_Close_SAVEANDSHARE_VIEW:
             //   _saveAndSharePanel.Destroy();
                _reloadingPanel.BackFormSaveAndShareView();
                break;

        }
    }

    public override void Remove(float delay)
    {
        base.Remove(delay);
        _reloadingPanel.Destroy();
        AssetManager.Instance.UnloadAtlas("UIAtlas_ReloadingBackGround");
        AssetManager.Instance.UnloadAtlas("UIAtlas_ReloadingClothing");
    }




}
