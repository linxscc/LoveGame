using Assets.Scripts.Framework.GalaSports.Core;
using Common;

public class VisitModule : ModuleBase
{

    private VisitPanel   _visitPanel;
    private WeatherPanel _weatherPanel;
    private VisitLevelPanel _visitLevelPanel;

    public override void Init()
    {
        _visitPanel = new VisitPanel();
        _visitPanel.Init(this);
        _visitPanel.Show(0.5f);

        GuideManager.RegisterModule(this);

    }

    public override void OnMessage(Message message)
    {

        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_VISIT_SHOW_VISIT_PANEL:
                HideAll();
                if (_visitPanel == null)
                {
                    _visitPanel = new VisitPanel();
                    _visitPanel.Init(this);
                }else
                {
                    _visitPanel.Refeash();
                }
                _visitPanel.Show(0.5f);
                break;
            case MessageConst.MODULE_VISIT_SHOW_WEATHER_PANEL:
                HideAll();
                if(_weatherPanel==null)
                {
                    _weatherPanel = new WeatherPanel();
                    _weatherPanel.Init(this);
                }
                PlayerPB  npcId=(PlayerPB)body[0];
                _weatherPanel.SetData(npcId);
                _weatherPanel.Show(0.5f);
                break;
            case MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL:
                HideAll();
                if (_visitLevelPanel == null)
                {
                    _visitLevelPanel = new VisitLevelPanel();
                    _visitLevelPanel.Init(this);
                }
                _visitLevelPanel.SetData((PlayerPB)body[0]);
                _visitLevelPanel.Show(0.5f);
                break;
            case MessageConst.MODULE_VISIT_WEATHER_SET_BACKBTNSHOWORHIDE:
                if (_weatherPanel != null)
                {
                    bool isShow = (bool)body[0];
                    if(!isShow)
                    {
                        _weatherPanel.HideBackBtn();
                    }
                    else
                    {
                        _weatherPanel.ShowBackBtn();
                    }
              
                }
                break;
            default:
                break;
        }
    }
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        if (_visitLevelPanel != null)
        { 
            _visitLevelPanel.Show(delay);
            _visitLevelPanel.Refresh();
        }
    }

    private void HideAll()
    {
        if (_visitPanel != null)
            _visitPanel.Hide();
        if (_weatherPanel != null)
        {
            _weatherPanel.Hide();
           // _weatherPanel.Destroy();
           // _weatherPanel = null;
        }
        if (_visitLevelPanel != null)
            _visitLevelPanel.Hide();
    }

    public override void Remove(float delay)
    {
        base.Remove(delay);
        _visitPanel.Destroy();
        //ToDo...卸载图集
    }
}
