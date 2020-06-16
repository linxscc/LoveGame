
using Assets.Scripts.Framework.GalaSports.Core;


public class ActivityFirstRechargeModule : ModuleBase 
{

    private ActivityFirstRechargePanel _activityFirstRechargePanel;


    public override void Init()
    {
        base.Init();
        _activityFirstRechargePanel = new ActivityFirstRechargePanel();
        _activityFirstRechargePanel.Init(this);
        _activityFirstRechargePanel.Show(0);
    }


    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
             case  MessageConst.CMD_FIRSTRECHARGE_HIDE_BACK_BTN:
                 _activityFirstRechargePanel.HideBackBtnAndTop();                 
                 break;
        }
    }
}
