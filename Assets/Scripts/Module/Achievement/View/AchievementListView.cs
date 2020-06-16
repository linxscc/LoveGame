using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using BehaviorDesigner.Runtime.Tasks.Basic.UnityInput;
using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class AchievementListView : View
    {
//    private Transform _tabBar;
        private LoopVerticalScrollRect _achievementList;

        private Text _title;
        private List<UserMissionVo> _data;


        private ProgressBar _activityReward;
        private Text _roadnum;

        private MissionModel _missionModel;

        private Text _rewardName;
        private Text _rewardNum;
        private RawImage _rewardImage;
        private Transform _rewardTran;
        private GameObject _rewardRedpoint;
        private UserMissionActivityInfoPB usermissioninfo;

        private int weight = 0;
        private PlayerPB playepb =PlayerPB.None;


        private Button _awardPreviewBtn;  //奖励预览
        
        
        private void Awake()
        {
            //先用来做任务进度吧
            _title = transform.Find("Tap/Title/Text").GetComponent<Text>();
            _roadnum = transform.Find("ListContent/Achievement/RoadNum").GetText();
            _rewardTran = transform.Find("ListContent/Achievement/Award");
            _rewardRedpoint = transform.Find("ListContent/Achievement/Award/Redpoint").gameObject;
            _rewardName = transform.Find("ListContent/Achievement/Award/Text").GetText();
            _rewardNum = transform.Find("ListContent/Achievement/Award/Num").GetText();
            _rewardImage = transform.Find("ListContent/Achievement/Award/Item").GetRawImage();
            _activityReward = transform.Find("ListContent/Achievement/Progress/ProgressBar")
                .GetComponent<ProgressBar>();
            _achievementList = transform.Find("ListContent/Achievement/AchievementList")
                .GetComponent<LoopVerticalScrollRect>();
            _achievementList.prefabName = "Achievement/Prefabs/AchievementItem";
            _achievementList.poolSize = 8;

            _awardPreviewBtn = transform.GetButton("ListContent/Achievement/AwardPreviewBtn");
            
            _awardPreviewBtn.onClick.AddListener(OnAwardPreviewBtn);
            
            PointerClickListener.Get(_rewardTran.gameObject).onClick = go =>
            {
                if (usermissioninfo?.Progress>=weight)
                {
                    //Debug.LogError(weight);
                    SendMessage(new Message(MessageConst.CMD_TASK_RECEIVE_ACTREWARD,
                        Message.MessageReciverType.CONTROLLER, MissionTypePB.StarCourse, weight,playepb));
                }
                else
                {
                    FlowText.ShowMessage(I18NManager.Get("Achievement_NoEnough"));
                }


                    
            }; 
        }

        private void OnAwardPreviewBtn()
        {
          var  list = _missionModel.GetAwardPreviews(playepb);
          var window = PopupManager.ShowWindow<AchievementAwardPreviewWindow>("Achievement/Prefabs/AchievementAwardPreviewWindow");
          window.SetData(list);
        }


        public void SetData(MissionModel missionModel, int player)
        {
            _missionModel = missionModel;
            playepb = (PlayerPB) player;
            _title.text = missionModel.GetPlayerName(playepb);
            //PointerClickListener.Get(_rewardTran.gameObject).onClick = null;
            if (missionModel.StarCourseSchedule.ContainsKey(playepb))
            {
                usermissioninfo = missionModel.StarCourseSchedule[playepb];
                _roadnum.text = I18NManager.Get("Achievement_LongKM2",usermissioninfo.Progress);//"星路里程:" + usermissioninfo.Progress+"km";
                weight=0;
                
                var rewardList = missionModel.GetStarRoadRewardPBByCount(usermissioninfo.Progress,playepb,ref weight,usermissioninfo.List);
                _activityReward.DeltaX = 0;
                _activityReward.Progress= (int) ((float) usermissioninfo.Progress / weight * 100f);
                RewardVo rewardVo = null;
                foreach (var v in rewardList)
                {
                    rewardVo = new RewardVo(v);
                }

                _rewardImage.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
                _rewardName.text = weight+I18NManager.Get("Achievement_KMReward");
                _rewardNum.text = rewardVo?.Num.ToString();
                //可领取的状态！

                    _rewardRedpoint.SetActive(usermissioninfo.Progress>=weight&&usermissioninfo.Progress>0);
                


            }
            else
            {
                _activityReward.DeltaX = 0;
                _activityReward.Progress= 0;
                if (missionModel.StarCourseSchedule.ContainsKey(playepb))
                {
                    SetActReward(0, missionModel.StarCourseSchedule[playepb].Progress, null,missionModel.StarCourseSchedule[playepb].List,playepb); 
                }
                else
                {
                    SetActReward(0, 0, null,null,playepb); 
                }

            }

            _data = missionModel.GetMissionByPlayerPB(playepb);
            _data.Sort();
            SetStarRoadItemData();
        }

        private void SetActReward(int roadnum,int progress,RewardVo vo,RepeatedField<int> receiveList,PlayerPB playerPb)
        {
            _roadnum.text = I18NManager.Get("Achievement_LongKM2",roadnum);//"星路里程:" +roadnum+"km";
            weight = 0;
            var rewardList = _missionModel.GetStarRoadRewardPBByCount(progress,playerPb,ref weight,receiveList);
            RewardVo rewardVo = vo;
            foreach (var v in rewardList)
            {
                rewardVo = new RewardVo(v);
            }

            _rewardImage.texture = ResourceManager.Load<Texture>(rewardVo?.IconPath);
            if (rewardVo!=null)
            {
                _rewardName.text =weight+I18NManager.Get("Achievement_KMReward"); //rewardVo.Name + " X" + rewardVo.Num;
                _rewardNum.text = rewardVo?.Num.ToString();
            }
            else
            {
                _rewardName.text =weight+I18NManager.Get("Achievement_KMReward");
                _rewardNum.text = "";
            }

        }
        

        public void SetStarRoadItemData()
        {
            _achievementList.UpdateCallback = StarListUpdateCallBack;
            _achievementList.totalCount = _data.Count;
            _achievementList.RefreshCells();
            _achievementList.RefillCells();
        }

        private void StarListUpdateCallBack(GameObject gameObject, int index)
        {
            gameObject.GetComponent<AchievementItem>().SetData(_data[index], _missionModel.GetMissionById(_data[index].MissionId));
        }


    }
}