using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class TaskDailyView : View
    {
//    private Transform _tabBar;
        private LoopVerticalScrollRect _dailytaskList;
        private LoopVerticalScrollRect _weektaskList;

        private Transform _dailyTask;
        private Transform _weekTask;

        private Transform _taskTips;
        //private Text _TipsContend;


//        private List<UserMissionVo> _originaleMissionList;
        private List<UserMissionVo> _data;
        private List<UserMissionVo> _weekData;

        private PlayerPB _curPlayerPb = PlayerPB.None;

//    private Text _playstarCourseNum;

        private Transform rewardList;
        private Transform weekRewardList;

        private ProgressBar _activityReward;
        private ProgressBar _weekactivityReward;

        private MissionModel _missionModel;

        private Toggle _dailyToggle;
        private Toggle _weekToggle;

        private GameObject _dailyredPiont;
        private GameObject _weekredPiont;


        private void Awake()
        {
            //transform.Find("ListContent/DailyTask/TabArrow").GetComponent<Button>().onClick.AddListener(OnDailyTaskClick);
//        transform.Find("ListContent/SupporterTaskList/TabArrow").GetComponent<Button>().onClick
//            .AddListener(OnSupporterTaskClick);

            _dailyToggle = transform.Find("Tap/ToggleGroup/Daily").GetComponent<Toggle>();
            _weekToggle = transform.Find("Tap/ToggleGroup/Week").GetComponent<Toggle>();
            _dailyredPiont = transform.Find("RedPoint/Daily").gameObject;
            _weekredPiont = transform.Find("RedPoint/Week").gameObject;

            _dailyToggle.onValueChanged.AddListener(ChangePanel);
            _weekToggle.onValueChanged.AddListener(ChangePanel);

            _taskTips = transform.Find("BottomTips");

            //先用来做任务进度吧
            //_TipsContend = _taskTips.Find("Text").GetComponent<Text>();


            _dailyTask = transform.Find("ListContent/DailyTask");
            _weekTask = transform.Find("ListContent/WeekTask");
            _activityReward = _dailyTask.Find("Progress/ProgressBar").GetComponent<ProgressBar>();
            _weekactivityReward = _weekTask.Find("Progress/ProgressBar").GetComponent<ProgressBar>();

            rewardList = transform.Find("ListContent/DailyTask/TabBar");
            weekRewardList = transform.Find("ListContent/WeekTask/TabBar");
//        _playstarCourseNum = transform.Find("ListContent/SupporterTaskList/Text").GetComponent<Text>();

            _dailytaskList = transform.Find("ListContent/DailyTask/DailyTaskList")
                .GetComponent<LoopVerticalScrollRect>();
            _dailytaskList.prefabName = "Task/Prefabs/DailyTaskItem/DailyTaskItem";
            _dailytaskList.poolSize = 8;
            _weektaskList = transform.Find("ListContent/WeekTask/WeekTaskList").GetComponent<LoopVerticalScrollRect>();
            _weektaskList.prefabName = "Task/Prefabs/WeekTaskItem/WeekTaskItem";
            _weektaskList.poolSize = 8;
        }

        private void ChangePanel(bool isOn)
        {
            if (isOn == false)
                return;

            string name = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log("OnTabChange===>" + name);

            switch (name)
            {
                case "Daily":
                    OnDailyTaskClick();
                    break;
                case "Week":
                    OnWeekTaskClick();
                    break;
            }
        }


        public void ChangeViewState(MissionTypePB state)
        {
//        Debug.LogError(state);
            if (state == MissionTypePB.Daily)
            {
                _dailyTask.gameObject.Show();
                _weekTask.gameObject.Hide();
                SetDailyMissionInfo();
            }
            else
            {
                _dailyTask.gameObject.Hide();
                _weekTask.gameObject.Show();
                SetWeekMissionInfo();
            }
            SendMessage(new Message(MessageConst.CMD_RECOREDSTATE,state));
            //_taskTips.gameObject.SetActive(state == MissionTypePB.Daily);
        }


        /// <summary>
        /// 用户任务数据主通道
        /// </summary>
        /// <param name="missionModel"></param>
        public void SetMissionItemData(MissionModel missionModel)
        {
            _missionModel = missionModel;
            _data = _missionModel.GetMissionListByType(MissionTypePB.Daily);
            _weekData = _missionModel.GetMissionListByType(MissionTypePB.WeekDaily);
            
            //在此处设置红点！
            _dailyredPiont.SetActive(_missionModel.HasDailyActivityAward(_data));
            _weekredPiont.SetActive(_missionModel.HasWeekActivityAward(_weekData));
        }

        public void SetDailyMissionInfo()
        {
            SetDailyMissionItemData();
            SetDailyMissionActivity();
        }

        public void SetWeekMissionInfo()
        {
            SetWeekTaskItemData();
            SetWeekMissionActivity();
        }

        public void SetDailyMissionActivity()
        {
            _activityReward.DeltaX = 0;
            _activityReward.Progress = (int) ((float) _missionModel.DailyMissionActivityInfoPb.Progress / 120 * 100f);
            for (int i = 0; i < rewardList.childCount; i++)
            {
                int weight = _missionModel.GetReceiveWeight(i);
                var reward = rewardList.GetChild(i).Find("Background/Checkmark").GetComponent<Image>();
                var point = rewardList.GetChild(i).Find("Point").gameObject;
                var arrow = rewardList.GetChild(i).Find("Background/Checkmark/Arrow").gameObject;
                var numText = rewardList.GetChild(i).Find("Background/Checkmark/Num").GetComponent<Text>();
                arrow.SetActive(false);
                PointerClickListener.Get(reward.gameObject).onClick = null;
                var rewardlist = _missionModel.GetMissionawardByWeight(weight);
                int gemNum = 0;
                foreach (var v in rewardlist)
                {
                    if (v.Resource == ResourcePB.Gem)
                    {
                        gemNum = v.Num;
                    }
                }

                numText.text = gemNum.ToString();
                numText.gameObject.SetActive(false);
                if (_missionModel.DailyMissionActivityInfoPb.List.Contains(weight))
                {
                    numText.gameObject.SetActive(false);

                    reward.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_gemDoneIcon");
                    point.GetComponent<Image>().sprite =
                        AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_Showpoint");
                    PointerClickListener.Get(reward.gameObject).onClick = go =>
                    {
                        FlowText.ShowMessage(I18NManager.Get("Task_HasReceive"));
                    };
                }
                else
                {
                    if (weight > _missionModel.DailyMissionActivityInfoPb.Progress)
                    {
                        numText.gameObject.SetActive(true);
                        reward.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_gemOpenIcon");
                        point.GetComponent<Image>().sprite =
                            AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_whitePoint");
                    }
                    else
                    {
                        //待领取状态
                        numText.gameObject.SetActive(true);
                        reward.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_gemIcon");
                        point.GetComponent<Image>().sprite =
                            AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_whitePoint");
                        arrow.SetActive(true);
                    }

                    PointerClickListener.Get(reward.gameObject).onClick = go =>
                    {
                        if (weight > _missionModel.DailyMissionActivityInfoPb.Progress)
                        {
                            FlowText.ShowMessage(I18NManager.Get("Task_NoEnoughActive"));
                        }
                        else
                        {
                            SendMessage(new Message(MessageConst.CMD_TASK_RECEIVE_ACTREWARD,
                                Message.MessageReciverType.CONTROLLER, MissionTypePB.Daily, weight));
                        }
                    };
                }

                reward.SetNativeSize();
            }
        }

        public void SetWeekMissionActivity()
        {
            _weekactivityReward.DeltaX = 0;
            if (_missionModel.WeekMissionActivityInfoPb == null)
            {
                _weekactivityReward.Progress = 0;
            }
            else
            {
                _weekactivityReward.Progress =
                    (int) ((float) _missionModel.WeekMissionActivityInfoPb.Progress / 120 * 100f);
            }

            for (int i = 0; i < weekRewardList.childCount; i++)
            {
                int weight = _missionModel.GetReceiveWeight(i);
                var reward = weekRewardList.GetChild(i).Find("Background/Checkmark").GetComponent<RawImage>();
                var point = weekRewardList.GetChild(i).Find("Point").GetImage();
                var arrow = weekRewardList.GetChild(i).Find("Background/Checkmark/Arrow").gameObject;
                var numText = weekRewardList.GetChild(i).Find("Background/Checkmark/Num").GetComponent<Text>();
                var rewardmask=weekRewardList.GetChild(i).Find("Background/Checkmark/Mask");
                arrow.SetActive(false);
                rewardmask.gameObject.SetActive(false);
                PointerClickListener.Get(reward.gameObject).onClick = null;
                var rewardlist = _missionModel.GetWeekMissionRewardPBByCount(weight);
                int gemNum = 0;
                RewardVo rewardVo=null;
                foreach (var v in rewardlist)
                {
                    rewardVo=new RewardVo(v);
                    gemNum = rewardVo.Num;
                }

                numText.text = gemNum.ToString();
                numText.gameObject.SetActive(false);
                if (_missionModel.WeekMissionActivityInfoPb == null)
                {
                    numText.gameObject.SetActive(true);
                    reward.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
                    point.sprite =
                        AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_whitePoint");
                }
                else
                {
                    if (_missionModel.WeekMissionActivityInfoPb.List.Contains(weight))
                    {
                        numText.gameObject.SetActive(false);

                        reward.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
                        point.sprite =
                            AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_Showpoint");
                        rewardmask.gameObject.SetActive(true);
                        PointerClickListener.Get(reward.gameObject).onClick = go =>
                        {
                            FlowText.ShowMessage(I18NManager.Get("Task_HasReceive"));
                        };
                    }
                    else
                    {
                        if (weight > _missionModel.WeekMissionActivityInfoPb.Progress)
                        {
                            numText.gameObject.SetActive(true);
                            reward.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
                            point.sprite =
                                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_whitePoint");
                        }
                        else
                        {
                            //待领取状态
                            numText.gameObject.SetActive(true);
                            reward.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
                            point.sprite =
                                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Task_whitePoint");
                            arrow.SetActive(true);
                        }

                        PointerClickListener.Get(reward.gameObject).onClick = go =>
                        {
                            if (weight > _missionModel.WeekMissionActivityInfoPb.Progress)
                            {
                                FlowText.ShowMessage(I18NManager.Get("Task_NoEnoughActive"));
                            }
                            else
                            {
                                SendMessage(new Message(MessageConst.CMD_TASK_RECEIVE_ACTREWARD,
                                    Message.MessageReciverType.CONTROLLER, MissionTypePB.WeekDaily, weight));
                            }
                        };
                    }

                    //reward.SetNativeSize();
                }
            }
        }

        public void SetDailyMissionItemData()
        {
//            _dailytaskList.RefillCells();
            _dailytaskList.UpdateCallback = ListUpdateCallBack;
//            _TipsContend.text = I18NManager.Get("Task_ProgressValue") + _missionModel.GetHasFinishTaskCount() + "/" +
//                                _data.Count;
            _dailytaskList.totalCount = _data.Count;
            _dailytaskList.RefreshCells();
        }


        private void OnEnable()
        {
            if (_dailytaskList != null)
            {
                _dailytaskList.RefreshCells();
            }
        }

        private void ListUpdateCallBack(GameObject go, int weight)
        {
            go.GetComponent<DailyTaskItem>().SetData(_data[weight], _missionModel.GetMissionById(_data[weight].MissionId));
        }

        public void SetWeekTaskItemData()
        {
//            _weektaskList.RefillCells();
            _weektaskList.UpdateCallback = WeekListUpdateCallBack;
            _weektaskList.totalCount = _weekData.Count;
            _weektaskList.RefreshCells();
        }

        private void WeekListUpdateCallBack(GameObject go, int weight)
        {
            //星缘列表出现过的对象池的问题,切换的时候要重新
            go.GetComponent<WeekTaskItem>().SetData(_weekData[weight], _missionModel.GetMissionById(_weekData[weight].MissionId));
        }


        private void OnDailyTaskClick()
        {
            ChangeViewState(MissionTypePB.Daily);
        }

        private void OnWeekTaskClick()
        {
            ChangeViewState(MissionTypePB.WeekDaily);
        }


        #region 星路历程参考代码

        //    private void OnTabChange(bool isOn)
//    { 
//        if(isOn == false)
//            return;
//		
//        string name = EventSystem.current.currentSelectedGameObject.name;
//        Debug.Log("OnTabChange===>" + name);
//
//        PlayerPB pb = PlayerPB.None;;
//        switch (name)
//        {
//            case "All" :
//                pb = PlayerPB.None;
//                break;
//            case "Fang" :
//                pb = PlayerPB.ChiYu;
//                break;
//            case "Tang" :
//                pb = PlayerPB.YanJi;
//                break;
//            case "Lin" :
//                pb = PlayerPB.TangYiChen;
//                break;
//            case "Li" :
//                pb = PlayerPB.QinYuZhe;
//                break;
//        }
//
//        if (_curPlayerPb!=pb)
//        {
//            _curPlayerPb = pb;
//            RefreshStarData(_curPlayerPb);
//        }
//        
//
//    }

//    public void SetStarRoadItemData(List<UserMissionVo> missions)
//    {
//        _supportertaskList.prefabName = "Task/Prefabs/StarRoadItem/StarRoadItem";
//        _supportertaskList.poolSize = 8;
//        _supportertaskList.UpdateCallback = StarListUpdateCallBack;
//        _originableStarMissionList = missions;
//        if (_curPlayerPb==PlayerPB.None)
//        {
//            _curPlayerPb = PlayerPB.ChiYu;
//        }
//
//
//        
//        RefreshStarData(_curPlayerPb);
//
//    }



        #endregion
    }
}