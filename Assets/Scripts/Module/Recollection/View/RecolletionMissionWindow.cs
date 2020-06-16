using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;
using game.tools;

namespace Assets.Scripts.Module.Recollection.View
{
    public class RecolletionMissionWindow : Window
    {
        Button getBtn;   //领取按钮
        bool isShowGetBtn = false;
        int itemIdMin = 11001;   //B类道具范围最小值
        int itemIdMax = 11106;   //B类道具范围最大值
        private Text _titleText;
        

        private void Awake()
        {
            getBtn = transform.Find("Bg/GetBtn").GetComponent<Button>();
            getBtn.gameObject.SetActive(false);

            _titleText = transform.GetText("Bg/TitelText");
            
        }

        public void SetData(CardMemoriesMissionPB missionPb, UserMemoriesMissionPB userMission)
        {


            _titleText.text = I18NManager.Get("Recollection_AwardHint1");
            isShowGetBtn = false;
            var _contentText = transform.Find("Bg/ContentText").GetComponent<Text>();

            //  _contentText.text = missionPb.MissionDesc + "\n（已完成" + userMission.Progress + "/" + userMission.Finish + "）";
            _contentText.text = I18NManager.Get("Recollection_RecolletionMissionWindowContentText", missionPb.MissionDesc, userMission.Progress, userMission.Finish);
             var frameImage = transform.Find("Bg/FrameImage").gameObject;




            if (missionPb.ItemRandom != 1)
            {
                RewardVo vo = new RewardVo(missionPb.Award[0]);

                var _propImage = transform.Find("Bg/FrameImage/PropImage").GetComponent<RawImage>();
                var _numText = _propImage.transform.Find("PropNumText").GetComponent<Text>();

                _numText.text = vo.Num.ToString();           
                _propImage.texture = ResourceManager.Load<Texture>(vo.IconPath, ModuleConfig.MODULE_RECOLLECTION);
                
                var _nameText = transform.Find("Bg/FrameImage/PropImage/PropNameText").GetComponent<Text>();
                _nameText.text = vo.Name;

                if (itemIdMin<=vo.Id  && vo.Id<=itemIdMax)
                {
                    PointerClickListener.Get(frameImage).onClick = go =>
                    {
                        FlowText.ShowMessage(I18NManager.Get("Recollection_Hint5"));
                    };
                }
            }

            
           
        }

        public void ShowReward(RepeatedField<AwardPB> awardPbs)
        {
            
            _titleText.text = I18NManager.Get("Recollection_GetAward");
            transform.GetText("Bg/ContentText").gameObject.Hide();
            transform.GetText("Bg/TipText").gameObject.Hide();
            var frameImage = transform.Find("Bg/FrameImage").gameObject;

            frameImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(28, -223);

            RewardVo vo = new RewardVo(awardPbs[0]);

            var _propImage = transform.Find("Bg/FrameImage/PropImage").GetComponent<RawImage>();
            var _numText = _propImage.transform.Find("PropNumText").GetComponent<Text>();
            var _nameText = _propImage.transform.Find("PropNameText").GetComponent<Text>();
            _numText.text = vo.Num.ToString();
            _nameText.text = vo.Name;
            _propImage.texture = ResourceManager.Load<Texture>(vo.IconPath, ModuleConfig.MODULE_RECOLLECTION);


            if (itemIdMin <= vo.Id && vo.Id <= itemIdMax)
            {
                PointerClickListener.Get(frameImage).onClick = go =>
                {
                    FlowText.ShowMessage(I18NManager.Get("Recollection_Hint5"));
                   // FlowText.ShowMessage("提升星缘好感度的道具");
                };
            }
            getBtn.gameObject.SetActive(true);
            isShowGetBtn = true;
            if (isShowGetBtn)
            {
               // Debug.LogError("我通过领取按钮关闭");
                getBtn.onClick.AddListener(() => {
                    base.Close();
                    isShowGetBtn = false;
                });
            }
           

        }


        protected override void OnClickOutside(GameObject go)
        {

            if (isShowGetBtn==false)
            {
                //Debug.LogError("我没有通过领取按钮关闭");
                base.OnClickOutside(go);
            }
                
        }
    }
}