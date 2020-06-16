using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Componets;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Common
{
    public class RandowGiftWindow : Window
    {
        private List<TriggerGiftVo> _list;
        private Text _titleText;
        private Button _buyBtn;
        private Text _buyBtnText;
        private Transform _content;
        private Button _leftBtn;
        private Button _rightBtn;
        private Text _desc;

        private int _currentIndex;
        private Text _timeText;
        private string _countName = "RandowGiftWindowCountDown";
        private TriggerGiftVo _triggerGiftVo;
       private Text _originalPriceText;

        private Text _freeBtnText;

        private void Awake()
        {
            _titleText = transform.GetText("window/TitleText");
            _buyBtn = transform.GetButton("window/BuyBtn");
            _buyBtnText = transform.GetText("window/BuyBtn/Text");
            _desc = transform.GetText("window/Desc");
            _timeText = transform.GetText("window/TimeText");

            _content = transform.Find("window/Content");
            _leftBtn = transform.GetButton("window/LeftBtn");
            _rightBtn = transform.GetButton("window/RightBtn");

           

            _originalPriceText = transform.GetText("window/OriginalPriceText");

            _leftBtn.onClick.AddListener(ShowPrev);
            _rightBtn.onClick.AddListener(ShowNext);
            _buyBtn.onClick.AddListener(OnBuyGift);

            _freeBtnText = transform.GetText("window/BuyBtn/FreeText");
        }

        private void GetGiftSuccess(ReceiveFreeTriggerGiftRes res)
        {
            LoadingOverlay.Instance.Hide();
            RewardUtil.AddReward(res.Award);

            AwardWindow awardWindow = PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
            awardWindow.SetData(res.Award);
            awardWindow.WindowActionCallback = evt => { RandomEventManager.ShowGiftWindow(_currentIndex); };

            GlobalData.RandomEventModel.UpdateDate(res.UserTriggerGift);

            Close();
        }

        private void OnBuyGift()
        {
            if (_triggerGiftVo.IsFree)
            {
                LoadingOverlay.Instance.Show();
                byte[] data = NetWorkManager.GetByteData(new ReceiveFreeTriggerGiftReq() {Id = _triggerGiftVo.Id});
                NetWorkManager.Instance.Send<ReceiveFreeTriggerGiftRes>(CMD.MALLC_RECEIVEFREETRIGGERGIFT, data,
                    GetGiftSuccess);
            }
            else
            {
                long result;
                long time = ClientTimer.Instance.GetCurrentTimeStamp();
                if (GlobalData.RandomEventModel.ClickBuyDict.TryGetValue(_triggerGiftVo.Id, out result))
                {
                    if (time - result < 60 * 2 * 1000)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Activity_LimitBuy"));
                        return;
                    }
                    else
                    {
                        GlobalData.RandomEventModel.ClickBuyDict[_triggerGiftVo.Id] = time;
                    }
                }
                else
                {
                    GlobalData.RandomEventModel.ClickBuyDict.Add(_triggerGiftVo.Id, time);
                }

                ProductVo vo = _triggerGiftVo.GetProduct();
                vo.ExtInt = _triggerGiftVo.Id;
                SdkHelper.PayAgent.PayGift(vo);
                Close();
            }
        }

        private void ShowNext()
        {
            if (_currentIndex < _list.Count - 1)
            {
                _currentIndex++;
            }
            else
            {
                _currentIndex = 0;
            }

            ShowGift();
        }

        private void ShowPrev()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
            }
            else
            {
                _currentIndex = _list.Count - 1;
            }

            ShowGift();
        }

        public void SetData(List<TriggerGiftVo> list, int index = 0)
        {
            _list = list;

            if (index >= _list.Count)
                index -= 1;

            _currentIndex = index;
            ShowGift();
        }

        private void ShowGift()
        {
            ClientData.LoadItemDescData(null);
            ClientData.LoadSpecialItemDescData(null);

            var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
            var npcImg = transform.GetRawImage("window/RoleImage"+npcId);
            npcImg.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId, null, true);
            npcImg.gameObject.Show();
          
   

            _triggerGiftVo = _list[_currentIndex];

            _titleText.text = _triggerGiftVo.Rule.MallName;
            _desc.text = _triggerGiftVo.Rule.MallDesc;

            List<RewardVo> rewardList = _triggerGiftVo.GetRewardList();
            for (int i = 0; i < _content.childCount; i++)
            {
                Transform child = _content.GetChild(i);
                if (i < rewardList.Count)
                {
                    child.GetRawImage("Image").texture = ResourceManager.Load<Texture>(rewardList[i].IconPath, null, true);
                    child.GetText("NumText").text = rewardList[i].Num.ToString();
                }

                if (i < rewardList.Count)
                {
                    RewardVo vo = rewardList[i];
                    PointerClickListener.Get(child.gameObject).onClick = go =>
                    {
                        FlowText.ShowMessage(ClientData.GetItemDescById(vo.Id, vo.Resource).ItemDesc);
                    };
                }
                child.gameObject.SetActive(i < rewardList.Count);
            }

            // RectTransform rect = _buyBtnText.transform.GetRectTransform();

            if (_triggerGiftVo.IsFree)
            {
                _buyBtnText.gameObject.Hide();
                _freeBtnText.gameObject.Show();
                _freeBtnText.text = I18NManager.Get("RandowEventWindow_Free");


                _originalPriceText.gameObject.Hide();
                // rect.sizeDelta = new Vector2(920, rect.sizeDelta.y);
            }
            else
            {
                ProductVo product = GlobalData.PayModel.GetProduct(_triggerGiftVo.MallId);
                _buyBtnText.text = product.AreaPrice;//product.Curreny + " " + 

                _buyBtnText.gameObject.Show();
                _freeBtnText.gameObject.Hide();
                _originalPriceText.gameObject.Show();

                if (AppConfig.Instance.isChinese=="true"||product?.Curreny==Constants.CHINACURRENCY)
                {
                    _originalPriceText.text = I18NManager.Get("RandowEventWindow_OriginalPrice")  +
                                              _triggerGiftVo.Rule.OriginalPrice+"元";
                }
                else
                {
                    _originalPriceText.text = I18NManager.Get("RandowEventWindow_OriginalPrice") + product.Curreny + " " +
                                              product.GetOriginalPrice(_triggerGiftVo.Rule.OriginalPrice);
                }


                // rect.sizeDelta = new Vector2(920-100, rect.sizeDelta.y);
            }

            ClientTimer.Instance.RemoveCountDown(_countName);

            _timeText.text = I18NManager.Get("RandowEventWindow_Time",
                                 DateUtil.GetTimeFormat4(_triggerGiftVo.MaturityTime -
                                                         ClientTimer.Instance.GetCurrentTimeStamp()))
                             + I18NManager.Get("RandowEventWindow_Later");

            ClientTimer.Instance.AddCountDown(_countName, long.MaxValue, 1, tick =>
            {
                if (_triggerGiftVo.MaturityTime - ClientTimer.Instance.GetCurrentTimeStamp() <= 0)
                {
                    Close();
                    GlobalData.RandomEventModel.Delete(new RepeatedField<long>() {_triggerGiftVo.Id});
                    RandomEventManager.ShowGiftWindow(_currentIndex);
                    return;
                }

                _timeText.text =
                    I18NManager.Get("RandowEventWindow_Time",
                        DateUtil.GetTimeFormat4(
                            _triggerGiftVo.MaturityTime - ClientTimer.Instance.GetCurrentTimeStamp())) +
                    I18NManager.Get("RandowEventWindow_Later");
            }, null);

            _leftBtn.gameObject.SetActive(_list.Count > 1);
            _rightBtn.gameObject.SetActive(_list.Count > 1);
        }

        private void OnDestroy()
        {
            ClientTimer.Instance.RemoveCountDown(_countName);

//            ClientData.Clear();
        }
    }
}