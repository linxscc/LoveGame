using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace game.main
{
    public class TaskDailyPanel : ReturnablePanel
    {
        private TaskDailyView _taskDailyView;
        public TaskPanelController TaskController;

        private PlayerPB _currentTab = PlayerPB.None;
    
        public override void Init(IModule module)
        {
            SetComplexPanel();//待研究这个方法
            base.Init(module);

            _taskDailyView = (TaskDailyView) InstantiateView<TaskDailyView>("Task/Prefabs/TaskView");
            TaskController=new TaskPanelController();
            RegisterController(TaskController);
            TaskController.View = _taskDailyView;
            //StartController();
            TaskController.Start();
        }

    
    
        public override void Show(float delay)
        {
            base.Show(0);
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
            ShowBackBtn();
        }

        public void StartController()
        {
            TaskController.GetData();
        }
    
        
        //参考CardCollectionPanel
        public override void Hide()
        {
        }

    }  

}

