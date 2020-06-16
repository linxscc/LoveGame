using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class DailyTaskItem : MonoBehaviour
    {
        private Text _titleName;
        //private Text _activityTime;
        private Text _gold;
        private Text _rewardName;
        private Text _exp;
        private Text _activity;
        private ProgressBar _taskProgressBar;
        private Button _taskBtn;
        private Text _progressText;
        private Text _taskState;
        private UserMissionVo _userMissionVo;
        private MissionRulePB _missionPb;
        private Image _task;
        private GameObject _finished;

        void Awake()
        {
            _titleName = transform.Find("TitleName").GetComponent<Text>();
            //_activityTime=transform.Find("ActivityTimes/TimesNumText").GetComponent<Text>();
            _gold=transform.Find("Reward/GoldTag/GoldText").GetComponent<Text>();
            _activity=transform.Find("Reward/Activity/ExpText").GetComponent<Text>();
            _rewardName = transform.Find("Reward/ExpTag/ExpText").GetComponent<Text>();
            _exp=transform.Find("Reward/ExpTag/ExpText").GetComponent<Text>();
            _taskProgressBar=transform.Find("ExpSlider/ProgressBar").GetComponent<ProgressBar>();
            _taskBtn=transform.Find("TaskBtn").GetComponent<Button>();
            _taskState = transform.Find("TaskBtn/Text").GetComponent<Text>();
            _progressText = transform.Find("ProgressText").GetComponent<Text>();
            _task = transform.Find("TaskBtn/BG").GetComponent<Image>();
            _finished = transform.Find("FinishTask").gameObject;
        }


        public void SetData(UserMissionVo vo,MissionRulePB missionPb)
        {
            _userMissionVo = vo;
            _missionPb = missionPb;//missionModel.GetMissionById(vo.MissionId);
            _titleName.text = _missionPb.MissionDesc;//任务描述
//            _activityTime.text ="X"+missionPb.Award.Num.ToString();//这个应该是活力字段获取应该错误了。
            _activity.text = _missionPb.Weight.ToString();
            
//            Debug.LogError(_missionPb.MissionDesc);
            _taskBtn.onClick.RemoveAllListeners();
            switch (vo.Status)
            {
                case    MissionStatusPB.StatusUnfinished:
                    //未完成的状态
                    _taskState.text = I18NManager.Get("Common_Goto");
                    _taskBtn.onClick.AddListener(JumpToOnClick);
                    _task.sprite=AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_gotoBottom");
                    _taskBtn.gameObject.Show();
                    _finished.Hide();
                    break;
                case    MissionStatusPB.StatusUnclaimed:
                    //未领取
                    _taskState.text = I18NManager.Get("Common_GetReward");
                    _taskBtn.onClick.AddListener(ReceiveGifts);  
                    _task.sprite=AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_receiveBottom");
                    _taskBtn.gameObject.Show();
                    _finished.Hide();
                    break;                
                case    MissionStatusPB.StatusBeRewardedWith:
                    //已完成
                    _taskBtn.gameObject.Hide();
                    _finished.Show();
                    break;
                
                
            }
            
            //Debug.LogError(missionPb.Award.Count);
            for (int i = 0; i < _missionPb.Award.Count; i++)
            {   
                RewardVo rewardVo=new RewardVo(_missionPb.Award[i]);
                if (rewardVo.Resource==ResourcePB.Gold)
                {
                    _gold.text =_missionPb.Award[i].Num.ToString();  
                }
                else if(rewardVo.Resource==ResourcePB.Exp)
                {
                    _rewardName.text = rewardVo.Name;
                    _exp.text = rewardVo.Num.ToString();
                }
                
                
            }

            _progressText.text = (vo.Progress>vo.Finish?vo.Finish:vo.Progress) + "/" + vo.Finish;
            _taskProgressBar.DeltaX = 0;
            _taskProgressBar.Progress = (int) ((float)vo.Progress / vo.Finish*100);
            
            //根据Vo的状态来修改按钮的显示。
            
        }

        private void ReceiveGifts()
        {
            //领取奖励
            EventDispatcher.TriggerEvent(EventConst.RecieveTaskReward, _userMissionVo);
        }

        private void JumpToOnClick()
        {
            //跳转到对应模块
            Debug.LogError(_missionPb.JumpTo);
            //string类型moduleconfig里的字段
            

//            var openLevel = GuideManager.GetOpenUserLevel(ModulePB.EncourageAct, FunctionIDPB.EncourageActEntry);
//            if (GlobalData.PlayerModel.PlayerVo.Level < openLevel&&_missionPb.JumpTo=="SupporterActivity")
//            {
//                FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
//                return;
//            }
//
//            openLevel=GuideManager.GetOpenUserLevel(ModulePB.CardMemories, FunctionIDPB.CardMemoriesEntry);
//            if (GlobalData.PlayerModel.PlayerVo.Level < openLevel&&_missionPb.JumpTo=="Recollection")
//            {
//                FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
//                return; 
//            }
//            
//            openLevel=GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);
//            if (GlobalData.PlayerModel.PlayerVo.Level < openLevel&&_missionPb.JumpTo==ModuleConfig.MODULE_VISIT)
//            {
//                FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
//                return; 
//            }
            
            
            EventDispatcher.TriggerEvent(EventConst.JumpToCMD,_missionPb.JumpTo);

            
        }


    }

}
