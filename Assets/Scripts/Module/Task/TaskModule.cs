using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using game.main;

public class TaskModule : ModuleBase
{
    private TaskDailyPanel _taskDailyPanel;


    public override void Init()
    {
        _taskDailyPanel=new TaskDailyPanel();
        _taskDailyPanel.Init(this);
        _taskDailyPanel.TaskController._missionModel = RegisterModel<MissionModel>();                                    
        _taskDailyPanel.Show(0);
        
        GuideManager.RegisterModule(this);
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        //只显示顶部栏，不显示头像！
        _taskDailyPanel.Show(0);
        _taskDailyPanel.StartController();
        //刷新任务界面！

    }
    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_TASK_SHOW_SUPPORTERTASK:
                break;
        }
    }

    
    public void Destroy()
    {
    }
} 

