using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class FansDetailView : View
    {
        private Text _supporterName;
        private Transform _fansList;
        private Text _timeLabel;
        private Transform _propList;
        private Button _goBtn;
        private Text _leftEnerge;
        private EncourageActRuleVo _encourageActRuleVo;
        private UserEncourageActVo _userEncourageActVo;
        private Text _addperHour;
        private Text _costLabel;
        private Button _changeBtn;
        private Text _changeCost;
        private GameObject _dropwayBg;

        private void Awake()
        {
            _supporterName = transform.Find("TopLayout/Title/NameTxt").GetComponent<Text>();
            _fansList = transform.Find("TopLayout/FansList");
            _timeLabel = transform.Find("SandGlasss/TimeLabel").GetComponent<Text>();
            _propList = transform.Find("SupporterProps/SupporterPropList");
            _goBtn = transform.Find("GoBtn").GetComponent<Button>();
            _leftEnerge = transform.Find("SandGlasss/Energy/Text").GetComponent<Text>();
            _goBtn.onClick.AddListener(StartActivity);
            _addperHour = transform.Find("SupporterEnergy/AddPerHour").GetComponent<Text>();
            _costLabel = transform.Find("SupporterEnergy/CostNum/CostLabe").GetComponent<Text>();
            for (int k = 0; k < 3; k++)
            {
                PointerClickListener.Get(_fansList.GetChild(k).gameObject).onClick = go =>
                {
                    _dropwayBg.SetActive(true);
                };
            }

            _changeBtn = transform.Find("ChangeBtn").GetButton();
            _changeCost = transform.Find("Icon/Num").GetText();

            _dropwayBg = transform.Find("DropWay").gameObject;
            _dropwayBg.SetActive(false);

            PointerClickListener.Get(_dropwayBg).onClick = go => { _dropwayBg.SetActive(false); };

            _changeBtn.onClick.AddListener(GoBackAndChangeAct);

            var jumpToDrawcard = _dropwayBg.transform.Find("BtnGroup/GainWayBtn1").GetButton();
            jumpToDrawcard.onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.MODULE_SUPPORTERACTIVITY_JUMPTOOTHER));
            });


            var gobackBtn = _dropwayBg.transform.Find("BtnGroup/GainWayBtn2").GetButton();
            gobackBtn.onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GOBACK));
            });
        }

        private void GoBackAndChangeAct()
        {
            SendMessage(new Message(MessageConst.MODULE_SUPPORTERACTIVITY_GOBACKANDCHANGE, _userEncourageActVo));
        }

        private void StartActivity()
        {
            //要分几种情况
            SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_STARTACTIVITY,
                Message.MessageReciverType.CONTROLLER, _userEncourageActVo));
        }

        public void SetData(UserEncourageActVo vo, SupporterActivityModel supporterActivityModel)
        {
            _userEncourageActVo = vo;
            _encourageActRuleVo = supporterActivityModel.EncourageRuleDic[vo.ActId];
            _supporterName.text = _encourageActRuleVo.Title;
            _changeCost.text = supporterActivityModel.GetRefreshCost(supporterActivityModel.RefreshCount + 1).Gold
                .ToString();
            _timeLabel.text = I18NManager.Get("SupporterActivity_Time", _encourageActRuleVo.NeedTime / 60);//"时间" + + "小时";
            //_leftEnerge.text = "消耗：" + _encourageActRuleVo.Power;
            for (int k = 0; k < 3; k++)
            {
                _fansList.GetChild(k).gameObject.Hide();
                _propList.GetChild(k).gameObject.Hide();
            }


            var i = 0;
            foreach (var v in _encourageActRuleVo.Fans)
            {
                _fansList.GetChild(i).gameObject.Show();
                SetFansData(_fansList.GetChild(i), v.Key, v.Value);
                i++;
            }

            for (int k = 0; k < _fansList.childCount; k++)
            {
                if (!_fansList.GetChild(k).gameObject.activeInHierarchy)
                {
                    _fansList.GetChild(k).gameObject.Show();
                    SetFansData(_fansList.GetChild(k), 0, 0);
                }
            }


            var j = 0;
            foreach (var v in _encourageActRuleVo.Consume)
            {
                _propList.GetChild(j).gameObject.Show();
                PointerClickListener.Get(_propList.GetChild(j).gameObject).onClick = null;
                PointerClickListener.Get(_propList.GetChild(j).gameObject).onClick = go =>
                {
                    FlowText.ShowMessage(I18NManager.Get("SupporterActivity_ItemPath"));
                };
                SetPropData(_propList.GetChild(j), v.Key, v.Value);
                j++;
            }

            SetSupporterEnergy();
        }

        private void SetFansData(Transform tran, int fansid, int needNum)
        {
            var hasfans = tran.Find("HasFansLeft");
            var fansname = tran.Find("HasFansLeft/FansName/Text").GetComponent<Text>();
            var fansNum = tran.Find("HasFansLeft/NumBg/NumText").GetComponent<Text>();
            var fansimg = tran.Find("HasFansLeft/FansImg/Image").GetComponent<RawImage>();
            var nofans = tran.Find("NoFansLeft");

            var fansNumTips = tran.Find("NotEnough");
            tran.Find("HasFansLeft/FansName").GetComponent<Image>().sprite =
                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_fansNameTag" + fansid);


            if (fansid == 0)
            {
                hasfans.gameObject.Hide();
                nofans.gameObject.Show();
                fansNumTips.gameObject.SetActive(false);
            }
            else
            {
                nofans.gameObject.Hide();
                hasfans.gameObject.Show();
                fansimg.gameObject.Show();

                var fans = GlobalData.DepartmentData.GetFans(fansid);
                fansname.text = fans.Name;
                fansNum.text = fans.Num + "/" + needNum;
                fansimg.texture = ResourceManager.Load<Texture>(fans.FansBigTexturePath, ModuleName);
                fansNumTips.gameObject.SetActive(fans.Num < needNum);
            }

            //之后再另外看颜色需不需要做
        }

        private void SetPropData(Transform tran, int propid, int propCount)
        {
            var prop = tran.Find("PropImage").GetComponent<RawImage>();
            var propName = tran.Find("Text").GetComponent<Text>();
            var propNum = tran.Find("Owned/Text").GetComponent<Text>();

            if (propid == 0)
            {
                propName.text = "";
                propNum.text = "0";
            }
            else
            {
                var propvo = GlobalData.PropModel.GetUserProp(propid);
                propName.text = propvo.Name;
                propNum.text = propvo.Num + "/" + propCount;
                prop.texture = ResourceManager.Load<Texture>(propvo.GetTexturePath(), ModuleName);
            }
        }

        private void SetSupporterEnergy()
        {
//            _addperHour.text =
//                $"每{GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_ENCOURAGE_ACT_POWER_ONE_TIME)}分钟恢复1点体力";
//            _costLabel.text = GlobalData.PlayerModel.PlayerVo.EncourageEnergy + "/" +
//                              GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_ENCOURAGE_ACT_POWER_MAX_SIZE);
        }
    }
}