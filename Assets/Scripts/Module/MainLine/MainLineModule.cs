using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using game.main;

public class MainLineModule : ModuleBase 
{
    private MainLinePanel _panel;
    private LevelModel _model;

    public override void Init()
    {
        GuideManager.RegisterModule(this);
        
        AssetManager.Instance.LoadAtlas("UIAtlas_Battle");
        
        _panel = new MainLinePanel();
        _panel.Init(this);
        _panel.Show(0);
    }
    
    public override void SetData(params object[] paramsObjects)
    {
        _model = RegisterModel<LevelModel>(GlobalData.LevelModel);
        if (paramsObjects != null && paramsObjects.Length > 0 && paramsObjects[0] is JumpData)
        {
            _model.JumpData = paramsObjects[0] as JumpData;
        }
        else
        {
            _model.JumpData = null;
        }
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
//            case MessageConst.CMD_CITY_GET_MYLEVEL_DATA:
//                GuideManager.OpenGuide(this);
//                break;
             case MessageConst.CMD_MIANLINE_GUIDE_SHOW_TOPBAR_AND_BACKBTN:
                 var isShow = (bool) body[0];
                 _panel.GuideIsShowBackBtnAndTopBar(isShow);
              
                 break;
        }
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _panel.OnShow();
        GuideManager.OpenGuide(this);
    }

}
