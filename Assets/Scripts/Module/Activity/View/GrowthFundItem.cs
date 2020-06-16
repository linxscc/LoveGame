using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class GrowthFundItem : MonoBehaviour
    {
//        private Text _titleName;
//
//        //private Text _activityTime;
//        private Text _reward;
//        private Text _rewardName;
//
//        private RawImage _rewardimg;
////        private Text _activity;
//
//        private Button _taskBtn;
//
        private GameObject _finished;
//        private Text Condiction;

        
        //下面是新的
        private Text _tagTxt;
        private RawImage _iconImg;
        private Text _numTxt;
        private Button _getBtn;
        private Text _finishTxt;
        
        
        
        
        
        void Awake()
        {
//            _titleName = transform.Find("TitleName").GetComponent<Text>();
//            _rewardimg = transform.Find("Reward/Item").GetRawImage();
//            _reward = transform.Find("Reward/Item/Num").GetComponent<Text>();
////            _activity=transform.Find("Reward/Activity/ExpText").GetComponent<Text>();
//            _rewardName = transform.Find("Reward/Item/Name").GetComponent<Text>();
//            _taskBtn = transform.Find("TaskBtn").GetComponent<Button>();

//            Condiction = transform.Find("Condiction").GetText();

            _tagTxt = transform.GetText("Tag/Text");
            _iconImg = transform.GetRawImage("Icon");
            _numTxt = transform.GetText("Num");
            _getBtn = transform.GetButton("GetBtn");
            _finished = transform.Find("FinishBtn").gameObject;
            _finishTxt = transform.Find("FinishBtn/Text").GetText();
        }


        public void SetData(GrowthFunVo vo)
        {
            _tagTxt.text = I18NManager.Get("Activity_GrowthFundHint1", vo.DepartmentLevel); //{0}级可领取

            foreach (var t in vo.AwardPbs)
            {
                RewardVo rewardVo = new RewardVo(t); 
                _numTxt.text =rewardVo.Num.ToString();
               _iconImg.texture =  ResourceManager.Load<Texture>(rewardVo.IconPath);
            }
            _getBtn.onClick.RemoveAllListeners();
            switch (vo.Weight)
            {
                case 0:
                    _getBtn.gameObject.Hide();
                    _finishTxt.text = I18NManager.Get("Common_AlreadyGet");
                    _finished.Show();
                    break;
                case 1:
                    _getBtn.gameObject.Hide();
                    _finishTxt.text = I18NManager.Get("Common_GetReward");
                    _finished.Show();
                    break;
                case 2:
                    _finished.Hide();
                    _getBtn.gameObject.Show();
                    _getBtn.onClick.AddListener(() =>
                    {
                        if (GlobalData.PlayerModel.PlayerVo.ExtInfo.GrowthFund == 0)
                        {
                            FlowText.ShowMessage(I18NManager.Get("Activity_GrowthFundHint3"));
                        }
                        else
                        {
                            EventDispatcher.TriggerEvent(EventConst.GetGrowthFundAward, vo.Id);
                        }
                    });
                    break;   
                
            }
            
            //领取按钮的逻辑没写 （没到等级不可领取的是那个淡色，到等级可领取是红色，这个要问下伟滔）
        }
        
//        public void SetData(GrowthFunVo vo)
//        {
//            _titleName.text = I18NManager.Get("Activity_GrowthFundHint1",vo.DepartmentLevel);//$"应援会{vo.DepartmentLevel}级可领取";
//            for (int i = 0; i < vo.AwardPbs.Count; i++)
//            {
//                RewardVo rewardVo = new RewardVo(vo.AwardPbs[i]);
//                _reward.text = rewardVo.Num.ToString();
//                _rewardName.text = rewardVo.Name;
//                Debug.LogError(rewardVo.IconPath);
//                _rewardimg.texture = ResourceManager.Load<Texture>(rewardVo.IconPath);
//            }
//
////           Debug.LogError(vo.Weight); 
//            _taskBtn.onClick.RemoveAllListeners();
//            switch (vo.Weight)
//            {
//                case 0:
//                    _taskBtn.gameObject.Hide();
//                    _finished.Show();
//                    break;
//                case 1:
//                    _taskBtn.gameObject.Hide();
//                    _finished.Hide();
//                    Condiction.gameObject.Show();
//                    Condiction.text=I18NManager.Get("Activity_GrowthFundHint2",vo.DepartmentLevel); //$"应援会\nLv.{vo.DepartmentLevel}级可领取";
//                    break;
//                case 2:
//                    _finished.Hide();
//                    _taskBtn.gameObject.Show();
//                    _taskBtn.onClick.AddListener(() =>
//                    {
//                        if (GlobalData.PlayerModel.PlayerVo.ExtInfo.GrowthFund==0)
//                        {
//                             FlowText.ShowMessage(I18NManager.Get("Activity_GrowthFundHint3"));
//                        }
//                        else
//                        {
//                            EventDispatcher.TriggerEvent(EventConst.GetGrowthFundAward, vo.Id); 
//                        }
//
//                    });
//                    break;
//            }
//
//            //根据Vo的状态来修改按钮的显示。
//        }


    }
}