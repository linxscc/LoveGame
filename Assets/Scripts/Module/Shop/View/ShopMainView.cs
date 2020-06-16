using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Common;
using DataModel;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class ShopMainView : Window
    {
        private Transform _tabBar;
        private LoopVerticalScrollRect _giftPageList;
        private LoopVerticalScrollRect _newPlayerPageList;
        private LoopVerticalScrollRect _vipPageList;
        private LoopVerticalScrollRect _cardshopPageList;

        private LoopVerticalScrollRect _gemPageList;

        //private LoopVerticalScrollRect _goldPageList;
        private LoopVerticalScrollRect _buyGemPageList;

        private RectTransform _giftcontentbg;
        private RectTransform _newPlayercontentbg;
        private RectTransform _vipcontendbg;
        private RectTransform _cardcontentbg;

        private RectTransform _gemcontentbg;

        //private RectTransform _goldcontentbg;
        private RectTransform _buyGemcontendbg;

        private Text _bottomTips;
        private Text _bottomLabel;
        private Text _refreshcost;
        private Button _refreshBtn;
        private Button bakcbtn;
        private Transform _refreshTran;

        private List<UserBuyRmbMallVo> _giftpackMallList;
        private List<UserBuyRmbMallVo> _newPlayerMallList;
        private List<UserBuyRmbMallVo> _vipMallList;
        private List<UserBuyRmbMallVo> _cardShopMallList;

        private List<UserBuyGameMallVo> _gemMallList;

        //private List<UserBuyGameMallVo> _goldGameMalllist;
        private List<UserBuyRmbMallVo> _buyGemMalllist;


        private Text _leftDay;

//    private Text _tasteCardNumt;
//    private Text _MonthCardNum;

        private GameObject _hasreceive;

        private int _curpage = 0;
        private string GoodPath = "Shop/Prefab/GoodsItem/GoodsItem";
        private TimerHandler _handler;
        private ShopModel _shopModel;
        private int _costGem = 0;

        private Transform _maxrefresh;

        //private bool canReceive=true;
        private Transform _lastMask;
        private Transform _jumpToMonthCard;
        private Button _gotoMonthCard;
        private GameObject _jumpredpoint;
        private List<Toggle> toggles;
        private GameObject vipTran;

        private int _totalHigh = 1688;
        private float _cellRate = 0.33f;
        private float _bgHeight = 422f; //背景高度！
        private float _toggleCellsWidth = -84;


        private Transform _topBar;
        private Transform _powerBar;
        private Transform _powerIcon;
        private Transform _powerAddIcon;
        private Transform _powerAddImg;
        private Transform _goldBar;
        private Transform _goldAddIcon;
        private Transform _gemBar;
        private Image _powerIconImage;

        private Text _powerTxt;
        private Text _goldTxt;
        private Text _gemTxt;
        private Text _visitPowerTxt;


        private Transform _recolletionBar;

        private GameObject _reddot;


        private Button _payProblemBtn; //充值问题Btn
        
        private void Awake()
        {
            _payProblemBtn = transform.GetButton("PayProblemBtn");
            _payProblemBtn.gameObject.SetActive(Channel.IsTencent);
            _payProblemBtn.onClick.AddListener(ShowTencentBalance);
            

            _recolletionBar = transform.Find("TopBar/RecolletionBar");
            _topBar = transform.Find("TopBar");
            //_topBar.GetRectTransform().anchoredPosition=new Vector2(_topBar.GetRectTransform().anchoredPosition.x,_topBar.GetRectTransform().anchoredPosition.y-ModuleManager.OffY/2);
            _topBar.GetRectTransform().offsetMax=new Vector2(0,-ModuleManager.OffY*2);
            _powerBar = _topBar.Find("PowerBar");

            _powerIconImage = _powerBar.Find("powerIcon").GetComponent<Image>();
            _powerIcon = _powerBar.Find("powerIcon/OnClick");
            _powerAddIcon = _powerBar.Find("addIcon/OnClick");
            _powerAddImg = _powerBar.Find("addIcon");

            _goldBar = _topBar.Find("GoldBar");
            _goldAddIcon = _goldBar.Find("addIcon/OnClick");

            _gemBar = _topBar.Find("GemBar");

            _powerTxt = _topBar.Find("PowerBar/Text").GetComponent<Text>();
            _goldTxt = _topBar.Find("GoldBar/Text").GetComponent<Text>();
            _gemTxt = _topBar.Find("GemBar/Text").GetComponent<Text>();
            _visitPowerTxt = _topBar.Find("VisitPowerBar/Text").GetComponent<Text>();

            PointerClickListener.Get(_powerAddIcon.gameObject).onClick = BuyPower;

            PointerClickListener.Get(_goldAddIcon.gameObject).onClick = BuyGold;


            _giftPageList = transform.Find("ContentList/GiftPackPageList").GetComponent<LoopVerticalScrollRect>();
            _newPlayerPageList = transform.Find("ContentList/NewPlayerPageList").GetComponent<LoopVerticalScrollRect>();
            _vipPageList = transform.Find("ContentList/VIPPageList").GetComponent<LoopVerticalScrollRect>();
            _cardshopPageList = transform.Find("ContentList/CardShopPageList").GetComponent<LoopVerticalScrollRect>();
            _gemPageList = transform.Find("ContentList/GemPageList").GetComponent<LoopVerticalScrollRect>();
            //_goldPageList = transform.Find("ContentList/GoldPageList").GetComponent<LoopVerticalScrollRect>();
            _buyGemPageList = transform.Find("ContentList/BuyGemPageList").GetComponent<LoopVerticalScrollRect>();
            _giftcontentbg = transform.Find("ContentList/GiftPackPageList/ContentBg").GetRectTransform();
            _newPlayercontentbg = transform.Find("ContentList/NewPlayerPageList/ContentBg").GetRectTransform();
            _cardcontentbg = transform.Find("ContentList/CardShopPageList/ContentBg").GetRectTransform();
            _gemcontentbg = transform.Find("ContentList/GemPageList/ContentBg").GetRectTransform();
            //_goldcontentbg = transform.Find("ContentList/GoldPageList/ContentBg").GetRectTransform();
            _vipcontendbg = transform.Find("ContentList/VIPPageList/ContentBg").GetRectTransform();
            _buyGemcontendbg = transform.Find("ContentList/BuyGemPageList/ContentBg").GetRectTransform();
            _lastMask = transform.Find("LastGoodsMask");
            vipTran = transform.Find("TabLine/Viewport/TabBar/VIP").gameObject;
            _bottomTips = transform.Find("BottomTips/Text").GetComponent<Text>();
            _tabBar = transform.Find("TabLine/Viewport/TabBar");
            _toggleCellsWidth = (1080 - _tabBar.GetRectTransform().GetWidth()) / _tabBar.childCount;

            _jumpToMonthCard = transform.Find("JumpToMonthCard");
            _gotoMonthCard = transform.Find("JumpToMonthCard/gotoBtn").GetButton();
            _gotoMonthCard.onClick.AddListener(() =>
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, true, GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthCard).JumpId);
//            SendMessage(new Message(MessageConst.CMD_MALL_BACKTOMAIN));
                ClientTimer.Instance.DelayCall(() => { gameObject.Hide(); }, 0.5f);
//            DoClose();
            });
            _jumpredpoint = transform.Find("JumpToMonthCard/gotoBtn/redpoint").gameObject;

            toggles = new List<Toggle>();
            for (int i = 0; i < _tabBar.childCount; i++)
            {
                Toggle toogle = _tabBar.GetChild(i).GetComponent<Toggle>();
                toggles.Add(toogle);
                toogle.onValueChanged.AddListener(OnTabChange);
            }

            bakcbtn = transform.Find("BackBtn").GetButton();
            bakcbtn.transform.GetRectTransform().anchoredPosition =
                new Vector2(bakcbtn.transform.GetRectTransform().anchoredPosition.x, ReturnablePanel.BackBtnY);
            //bakcbtn.gameObject.SetActive(false);
            bakcbtn.onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.CMD_MALL_BACKTOMAIN));
                DoClose();

//            ModuleManager.Instance.Remove(ModuleConfig.MODULE_SHOP);
            });


            _reddot = transform.Find("TabLine/Viewport/TabBar/Gem/RedDot").gameObject;
            _bottomLabel = transform.Find("Refresh/Tips").GetText();
            _refreshcost = transform.Find("Refresh/ChangeBtn/Icon/Num").GetText();
            _refreshBtn = transform.Find("Refresh/ChangeBtn").GetButton();
            _maxrefresh = transform.Find("Refresh/Max");
            _refreshBtn.onClick.AddListener(() =>
            {
                if (_costGem > GlobalData.PlayerModel.PlayerVo.Gem)
                {
                    FlowText.ShowMessage(I18NManager.Get("Shop_NotEnoughGem"));
                    return;
                }

                //次数达到上限不可以继续发消息！
                if (_shopModel?.UserBuyMallInfoPb.GoldRefreshNum >= _shopModel?.MallRefreshGoldRulePbs.Count)
                {
                    FlowText.ShowMessage(I18NManager.Get("Shop_RefreshCountMax"));
                }
                else
                {
                    SendMessage(new Message(MessageConst.CMD_REFRESHGOLDMALLITEM));
                }
            });
            _refreshTran = transform.Find("Refresh");

            _giftPageList.prefabName = GoodPath;
            _gemPageList.prefabName = GoodPath;
            _newPlayerPageList.prefabName = GoodPath;
            _cardshopPageList.prefabName = GoodPath;
            //_goldPageList.prefabName = GoodPath;
            _vipPageList.prefabName = GoodPath;
            _buyGemPageList.prefabName = GoodPath;

            _giftPageList.poolSize = 8;
            _vipPageList.poolSize = 8;
            _gemPageList.poolSize = 8;
            //_goldPageList.poolSize = 8;
            _buyGemPageList.poolSize = 8;
            _cardshopPageList.poolSize = 8;
            _newPlayerPageList.poolSize = 8;
            _handler = ClientTimer.Instance.AddCountDown("UpdateAutoChange", Int64.MaxValue, 1f, UpdateAutoChange,
                null);

            if (Channel.IsTencent)
            {
                Transform tencentText = transform.Find("BottomTips/TencentText");
                tencentText.gameObject.Hide();
                _jumpToMonthCard.Find("Tips").gameObject.Hide();
            
            }
            
        }

        private void ShowTencentBalance()
        {
            PopupManager.ShowWindow<TencentBalanceWindow>("Shop/Prefab/TencentBalanceWindow");
        }


        private void UpdateAutoChange(int time)
        {
            if (_shopModel != null)
            {
                var nexttimerefresh = _shopModel.UserBuyMallInfoPb.RefreshTime -
                                      ClientTimer.Instance.GetCurrentTimeStamp();
                if (nexttimerefresh > 0)
                {
                    _bottomLabel.text = DateUtil.GetTimeFormat4(nexttimerefresh);
//                    I18NManager.Get("Shop_RefreshCountTimer",
//                        DateUtil.GetTimeFormat4(nexttimerefresh)); //DateUtil.GetTimeFormat4(nexttimerefresh) + "后刷新";  
                }
                else
                {
                    Debug.LogError("send msg");
                    SendMessage(new Message(MessageConst.CMD_REGETSHOPRULE));
                }
            }
        }

        private void OnTabChange(bool isOn)
        {
            if (isOn == false)
                return;

            string name = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log("OnTabChange===>" + name);

            //ChangeColor(name);
            int idx = 0;
            switch (name)
            {
                case "GiftPack":
                    idx = 0;
                    break;
                case "NewPlayer":
                    idx = 1;
                    break;
                case "VIP":
                    idx = 2;
                    break;
                case "Star":
                    idx = 3;
                    break;
                case "Gem":
                    idx = 4;
                    break;
                case "BuyGem":
                    idx = 5;
                    break;
                default:
                    idx = 5;
                    break;
            }

            if (idx != _curpage)
            {
                _curpage = idx;
                RefreshPageData(_curpage);
            }
        }


        private void RefreshPageData(int idx)
        {
//            Debug.LogError(idx);
            switch (idx)
            {
                case 0:
                    SetPackGiftPage(_giftpackMallList);
                    break;
                case 1:
//                Debug.LogError(_vipMallList.Count);
                    SetNewPlayerMallPage(_newPlayerMallList);
                    break;
                case 2:
                    SetVIPMallPage(_vipMallList);
                    break;
                case 3:
                    //SetGoldMallPage(_goldGameMalllist);
                    SetCardMallPage(_cardShopMallList);
                    break;
                case 4:
                    SetGemMallPage(_gemMallList);
                    break;
                case 5:
                    SetbuyGemMallPage(_buyGemMalllist);
                    break;
            }

//            _bottomTips.text = _shopModel.MallPageDesc[idx];
            ShowPageList(idx);

            if (idx > 3)
            {
                _tabBar.GetRectTransform().DOAnchorPos(new Vector2((idx + 2) * _toggleCellsWidth,
                    _tabBar.GetRectTransform().anchoredPosition.y),0.5f);
            }
            else
            {
                _tabBar.GetRectTransform().DOAnchorPos( new Vector2((idx) * _toggleCellsWidth,
                    _tabBar.GetRectTransform().anchoredPosition.y),0.5f);
            }
        }

        private void ShowPageList(int pageIdx)
        {
            _giftPageList.gameObject.SetActive(pageIdx == 0);
            _newPlayerPageList.gameObject.SetActive(pageIdx == 1);
            _vipPageList.gameObject.SetActive(pageIdx == 2);
            _cardshopPageList.gameObject.SetActive(pageIdx == 3);
            _gemPageList.gameObject.SetActive(pageIdx == 4);
            //_goldPageList.gameObject.SetActive(pageIdx==3);
            _buyGemPageList.gameObject.SetActive(pageIdx == 5);
            _bottomTips.gameObject.SetActive(pageIdx != 2 ); //&&pageIdx!=1
            _jumpToMonthCard.gameObject.SetActive(pageIdx == 2);
            _refreshTran.gameObject.SetActive(false);//pageIdx == 4
            _refreshBtn.gameObject.SetActive(!_shopModel.Refresh);
            _maxrefresh.gameObject.SetActive(_shopModel.Refresh);
            _lastMask.gameObject.SetActive(false); //pageIdx!=1&&pageIdx!=3&&pageIdx!=4
//        Debug.LogError(GlobalData.PlayerModel.PlayerVo.UserMonthCard);
        }

        private void SetToggleShow(int idx)
        {
            foreach (var v in toggles)
            {
                v.onValueChanged.RemoveAllListeners();
            }

            for (int i = 0; i < _tabBar.childCount; i++)
            {
                toggles[i].isOn = i == idx;
            }

            foreach (var v in toggles)
            {
                v.onValueChanged.AddListener(OnTabChange);
            }
        }

        public void SetData(ShopModel shopModel, int jumppage = 0)
        {
            _shopModel = shopModel;
            vipTran.SetActive(AppConfig.Instance.SwitchControl.Recharge);
            _giftpackMallList =
                shopModel.GetTargetRmbMallList(MallLabelPB.LabelBestSellers); //(int)ShopModel.PageIndex.GiftPage
            _vipMallList = shopModel.GetTargetRmbMallList(MallLabelPB.LabelVip); //(int)ShopModel.PageIndex.VipPage
            _gemMallList =
                shopModel.GetTargetGameMallList(MallLabelPB.LabelResources); //(int)ShopModel.PageIndex.GemPage
            _newPlayerMallList = shopModel.GetTargetRmbMallList(MallLabelPB.LabelNovice);
            _cardShopMallList = shopModel.GetTargetRmbMallList(MallLabelPB.LabelStar);
            //_goldGameMalllist = shopModel.GetTargetGameMallList(MallLabelPB.LabelGold);//(int)ShopModel.PageIndex.GoldPage
            _buyGemMalllist = shopModel.GetBuyGemRmbMallList;
//            Debug.LogError(_giftpackMallList.Count + " " + _vipMallList.Count + " " + _newPlayerMallList.Count + " " +
//                           _cardShopMallList.Count);
            _costGem = shopModel.GetMallRefreshGoldCost(shopModel.UserBuyMallInfoPb.GoldRefreshNum + 1);
            _refreshcost.text = _costGem.ToString(); //I18NManager.Get("Shop_CostGem",_costGem);
            if (jumppage != 0)
            {
                _curpage = jumppage;
                RefreshPageData(jumppage);
                SetToggleShow(jumppage);
            }
            else
            {
                RefreshPageData(_curpage != 0 ? _curpage : 0);
            }

            _reddot.SetActive(shopModel.HasFreeGemMall());
            SetVIPState();
        }

        private void SetVIPState()
        {
            if (GlobalData.PlayerModel.PlayerVo.UserMonthCard != null)
            {
                _jumpredpoint.SetActive(ClientTimer.Instance.GetCurrentTimeStamp() -
                                        GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime >= 0);
            }
            else
            {
                _jumpredpoint.SetActive(true);
            }
        }


        public void SetPackGiftPage(List<UserBuyRmbMallVo> packMallVos)
        {
//        Debug.LogError(packMallVos.Count);
//        _giftPageList.RefillCells();
            _giftPageList.UpdateCallback = PackGiftItemCallBack;
            _giftPageList.totalCount = packMallVos.Count;
            _giftPageList.RefreshCells();
            //Debug.LogError(_giftPageList.transform.Find("Content").GetComponent<RectTransform>().GetHeight());
            _giftcontentbg.anchoredPosition = Vector2.zero;
            var scroll = _giftPageList.transform.GetComponent<ScrollRect>();
//        if ( packMallVos.Count<=9)
//        {
//            _giftcontentbg.SetHeight(_totalHigh);
//            scroll.enabled = false;
////            _giftPageList.movementType = LoopScrollRect.MovementType.Clamped;
//        }
//        else if(packMallVos.Count>=10&&packMallVos.Count<=12)
//        {
            _giftcontentbg.SetHeight(_bgHeight * ((float) Math.Ceiling(packMallVos.Count * _cellRate)));
            _giftPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            _giftPageList.movementType = LoopScrollRect.MovementType.Clamped;
            scroll.enabled = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
//        }
//
//        else
//        {
//            _giftcontentbg.SetHeight(_bgHeight*((float)Math.Ceiling(packMallVos.Count*_cellRate)));                
//            scroll.enabled = true;
//        }
        }

        private void PackGiftItemCallBack(GameObject go, int index)
        {
            if (index >= _giftpackMallList.Count && index < 0)
            {
                Debug.LogError(index);
            }
            else
            {
                go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_giftpackMallList[index].MallId],
                    _giftpackMallList[index]);
            }
        }

        public void SetNewPlayerMallPage(List<UserBuyRmbMallVo> newplayerMallVos)
        {
            //        Debug.LogError(vipMallVos.Count);
//        _vipPageList.RefillCells();
            _newPlayerPageList.UpdateCallback = SetNewPlayerItemCallBack;
            _newPlayerPageList.totalCount = newplayerMallVos.Count; //gemMallVos.Count;
            _newPlayerPageList.RefreshCells();
            _newPlayercontentbg.anchoredPosition = Vector2.zero;
            var scroll = _newPlayerPageList.transform.GetComponent<ScrollRect>();
            _newPlayercontentbg.SetHeight(_bgHeight * ((float) Math.Ceiling(newplayerMallVos.Count * _cellRate)));
            _newPlayerPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            scroll.enabled = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
        }

        private void SetNewPlayerItemCallBack(GameObject go, int index)
        {
            if (index >= _newPlayerMallList.Count && index < 0)
            {
                Debug.LogError(index);
            }
            else
            {
                go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_newPlayerMallList[index].MallId],
                    _newPlayerMallList[index]);
            }
        }

        public void SetCardMallPage(List<UserBuyRmbMallVo> cardMallVos)
        {
            //        Debug.LogError(vipMallVos.Count);
//        _vipPageList.RefillCells();
            _cardshopPageList.UpdateCallback = SetCardItemCallBack;
            _cardshopPageList.totalCount = cardMallVos.Count; //gemMallVos.Count;
            _cardshopPageList.RefreshCells();
            _cardcontentbg.anchoredPosition = Vector2.zero;
            var scroll = _cardshopPageList.transform.GetComponent<ScrollRect>();
            _cardcontentbg.SetHeight(_bgHeight * ((float) Math.Ceiling(cardMallVos.Count * _cellRate)));
            _cardshopPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            scroll.enabled = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
        }

        private void SetCardItemCallBack(GameObject go, int index)
        {
            if (index >= _cardShopMallList.Count && index < 0)
            {
                Debug.LogError(index);
            }
            else
            {
                go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_cardShopMallList[index].MallId],
                    _cardShopMallList[index]);
            }
        }


        public void SetVIPMallPage(List<UserBuyRmbMallVo> vipMallVos)
        {
//        Debug.LogError(vipMallVos.Count);
//        _vipPageList.RefillCells();
            _vipPageList.UpdateCallback = VIPPageListCallBack;
            _vipPageList.totalCount = vipMallVos.Count; //gemMallVos.Count;
            _vipPageList.RefreshCells();
            _vipcontendbg.anchoredPosition = Vector2.zero;
            _vipPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            var scroll = _vipPageList.transform.GetComponent<ScrollRect>();
//        if ( _vipMallList.Count<=9)
//        {
//            _vipcontendbg.SetHeight(_totalHigh);
//            scroll.enabled = false;
////            _vipPageList.movementType = LoopScrollRect.MovementType.Clamped;
//        }
//        else if(_vipMallList.Count>=10&&_vipMallList.Count<=12)
//        {
            _vipcontendbg.SetHeight(_bgHeight * ((float) Math.Ceiling(_vipMallList.Count * _cellRate)));
//            _vipPageList.movementType = LoopScrollRect.MovementType.Clamped;
            scroll.enabled = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
//        }
//
//        else
//        {
//            _vipcontendbg.SetHeight(_bgHeight*((float)Math.Ceiling(_vipMallList.Count*_cellRate)));                
////            _vipPageList.movementType = LoopScrollRect.MovementType.Elastic;
//            scroll.enabled = true;
//
//        }
        }

        private void VIPPageListCallBack(GameObject go, int index)
        {
            if (index >= _vipMallList.Count && index < 0)
            {
                Debug.LogError(index);
            }
            else
            {
                go.GetComponent<GoodsItem>()
                    .SetData(_shopModel.RmbMallDic[_vipMallList[index].MallId], _vipMallList[index]);
            }

//        go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_vipMallList[index].MallId],_vipMallList[index]);   
        }

        public void SetGemMallPage(List<UserBuyGameMallVo> gemMallVos)
        {
//        Debug.LogError(gemMallVos.Count);
            _gemPageList.UpdateCallback = GemPageListCallBack;
            _gemPageList.totalCount = gemMallVos.Count;
            _gemPageList.RefreshCells();
            _gemPageList.RefillCells();
            _gemcontentbg.anchoredPosition = Vector2.zero;
            _gemPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            var scroll = _gemPageList.transform.GetComponent<ScrollRect>();
//        if ( gemMallVos.Count<=9)
//        {
//            _gemcontentbg.SetHeight(_totalHigh);
//            scroll.enabled = false;
////            _gemPageList.movementType = LoopScrollRect.MovementType.Clamped;
//        }
//        else if(gemMallVos.Count>=10&&gemMallVos.Count<=12)
//        {
            _gemcontentbg.SetHeight(_bgHeight * ((float) Math.Ceiling(gemMallVos.Count * _cellRate)));
//            _gemPageList.movementType = LoopScrollRect.MovementType.Clamped;
            scroll.enabled = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
//        }
//        else
//        {
//            _gemcontentbg.SetHeight(_bgHeight*((float)Math.Ceiling(gemMallVos.Count*_cellRate)));                
////            _gemPageList.movementType = LoopScrollRect.MovementType.Elastic;
//            scroll.enabled = true;
//
//        }
        }

        private void GemPageListCallBack(GameObject go, int index)
        {
            go.GetComponent<GoodsItem>().SetData(_shopModel.GameMallDic[_gemMallList[index].MallId],
                _gemMallList[index], _shopModel.UserBuyMallInfoPb.RefreshTime);
        }


        public void SetbuyGemMallPage(List<UserBuyRmbMallVo> goldMallVos)
        {
//        Debug.LogError(goldMallVos.Count);

//        _buyGemPageList.RefillCells();
            _buyGemPageList.UpdateCallback = buyGemPageListCallBack;
            _buyGemPageList.totalCount = goldMallVos.Count;
            _buyGemPageList.RefreshCells();
            _buyGemcontendbg.anchoredPosition = Vector2.zero;
            var scroll = _buyGemPageList.transform.GetComponent<ScrollRect>();
            _buyGemcontendbg.SetHeight(_bgHeight * (float) Math.Ceiling(goldMallVos.Count * _cellRate)); //())); 
            _buyGemPageList.content.GetRectTransform().anchoredPosition = Vector2.zero;
            scroll.enabled = false;
            _buyGemPageList.movementType = LoopScrollRect.MovementType.Clamped;
//        if ( goldMallVos.Count<=9)
//        {
//            _buyGemcontendbg.SetHeight(_totalHigh+_bgHeight);
//            scroll.enabled = false;
//            _buyGemPageList.movementType = LoopScrollRect.MovementType.Clamped;
//        }
//        else if(goldMallVos.Count>=10&&goldMallVos.Count<=12)
//        {
//            _buyGemcontendbg.SetHeight(_bgHeight*(float)Math.Ceiling(goldMallVos.Count*_cellRate));//()));    
//            _buyGemPageList.movementType = LoopScrollRect.MovementType.Clamped;
//            
//            //还是要代码来判断是否长屏来选择是否可以移动！
//            scroll.enabled = true;
//            scroll.movementType=ScrollRect.MovementType.Clamped;
//        }
//        else
//        {
//            _buyGemcontendbg.SetHeight(_bgHeight*((float)Math.Ceiling(goldMallVos.Count*_cellRate)));                
////            _goldPageList.movementType = LoopScrollRect.MovementType.Elastic;
//            scroll.enabled = true;
//
//        }
        }

        private void buyGemPageListCallBack(GameObject go, int index)
        {
            if (index >= _buyGemMalllist.Count && index < 0)
            {
                Debug.LogError(index);
            }
            else
            {
                go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_buyGemMalllist[index].MallId],
                    _buyGemMalllist[index],_shopModel);
            }

            //go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_buyGemMalllist[index].MallId],_buyGemMalllist[index],_shopModel);  
        }


        public void SetTopbarData(PlayerVo vo)
        {
            //_uiv.InitData(vo);

            _goldTxt.text = vo.Gold + "";
            _gemTxt.text = vo.Gem + "";

            _visitPowerTxt.text = vo.EncourageEnergy + "/" + vo.MaxEncourageEnergy;

            _powerTxt.text = vo.Energy + "/" + vo.MaxEnergy;

            _recolletionBar.GetText("Text").text = vo.RecollectionEnergy + "/" +
                                                   GlobalData.ConfigModel.GetConfigByKey(GameConfigKey
                                                       .RESTORE_MEMORIES_POWER_MAX_SIZE); //90;
        }

//    public void SetSupporterPowerInfo(bool isSupporter)
//    {
//        _powerTxt.gameObject.SetActive(!isSupporter);
//        _powerIconImage.gameObject.SetActive(!isSupporter);
//        _powerAddImg.gameObject.SetActive(!isSupporter);
//        _powerBar.GetComponent<RawImage>().enabled = !isSupporter;
//    }


        protected override void OpenAnimation()
        {
        }


        protected override void AddBgMask()
        {
        }

        private void BuyGold(GameObject go)
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            if (! GuideManager.IsPass1_9())
            {
                FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
                return;
            }

            SendMessage(new Message(MessageConst.CMD_MAIN_SHOW_BUY_GOLD));
        }

        private void BuyPower(GameObject go)
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
            if (! GuideManager.IsPass1_9())
            {
                FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
                return;
            }


            SendMessage(new Message(MessageConst.CMD_MAIN_SHOW_BUY_POWER));
        }


        private void OnDestroy()
        {
            ClientTimer.Instance.RemoveCountDown(_handler);
        }


        //    public void SetGoldMallPage(List<UserBuyGameMallVo> goldMallVos)
//    {
////        _goldPageList.RefillCells();
//        _goldPageList.UpdateCallback = GoldPageListCallBack;
//        _goldPageList.totalCount = goldMallVos.Count;
//        _goldPageList.RefreshCells(); 
//        _goldcontentbg.anchoredPosition =  Vector2.zero;
//        var scroll = _goldPageList.transform.GetComponent<ScrollRect>();
//        _goldcontentbg.SetHeight(_bgHeight * (float) Math.Ceiling(goldMallVos.Count * _cellRate)); //()));    
//        _goldPageList.movementType = LoopScrollRect.MovementType.Clamped;
//
//        //还是要代码来判断是否长屏来选择是否可以移动！
//        scroll.enabled = true;
//        scroll.movementType = ScrollRect.MovementType.Clamped;
////        if ( goldMallVos.Count<=9)
////        {
////            _goldcontentbg.SetHeight(_totalHigh+_bgHeight);
////            scroll.enabled = false;
////            _goldPageList.movementType = LoopScrollRect.MovementType.Clamped;
////        }
////        else if(goldMallVos.Count>=10&&goldMallVos.Count<=12)
////        {
////            _goldcontentbg.SetHeight(_bgHeight*(float)Math.Ceiling(goldMallVos.Count*_cellRate));//()));    
////            _goldPageList.movementType = LoopScrollRect.MovementType.Clamped;
////            
////            //还是要代码来判断是否长屏来选择是否可以移动！
////            scroll.enabled = true;
////            scroll.movementType=ScrollRect.MovementType.Clamped;
////        }
////        else
////        {
////            _goldcontentbg.SetHeight(_bgHeight*((float)Math.Ceiling(goldMallVos.Count*_cellRate)));                
////            scroll.enabled = true;
////
////        }
//
//              
//    }
//
//    private void GoldPageListCallBack(GameObject go, int index)
//    {
//        
//        go.GetComponent<GoodsItem>().SetData(_shopModel.GameMallDic[_goldGameMalllist[index].MallId],_goldGameMalllist[index],_shopModel.UserBuyMallInfoPb.RefreshTime);  
// 
//    }


        //private Transform _showPic;
//    private Button _tasteBtn;
//    private Button _buymonthBtn;
//    private Button _enterBuyGem;

//    private Text _vipcontent;
//    private Button _reveiveVipAward;
//    private Text _receiveNum;

        //        _enterBuyGem = transform.Find("Enter/EnterGem").GetButton();
//        _enterBuyGem.onClick.AddListener(() =>
//        {
//            Debug.LogError("Come");
//            SendMessage(new Message(MessageConst.CMD_MALL_ENTERTOBUYGEM));
//        });
        //_showPic = transform.Find("ShowPic");
//        _tasteBtn = transform.Find("ShowPic/TasteCard").GetButton();
//        _tasteCardNumt = transform.Find("ShowPic/TasteCard/Text").GetText();
//        _buymonthBtn = transform.Find("ShowPic/BuyMonthCard").GetButton();
//        _MonthCardNum = transform.Find("ShowPic/BuyMonthCard/Text").GetText();
//        ClientTimer.Instance.DelayCall(() =>
//        {
//            var height = _giftPageList.GetComponent<RectTransform>().GetHeight();
//            //Debug.LogError(height);
//            if (height<=1730)
//            {
//
//            }
//
//        }, 1f);

        //_showPic.gameObject.SetActive(false);
//        _tasteCardNumt.text = I18NManager.Get("Shop_TasteCard") + GlobalData.PropModel.GetUserProp(PropConst.TasteCardId).Num;//GlobalData.PlayerModel.PlayerVo.UserMonthCard?.BuyCount;
//        _MonthCardNum.text = I18NManager.Get("Shop_BuyGemCost", _shopModel.GetMonthCardMall?.RealPrice);//$"星钻￥{_shopModel.GetMonthCardMall?.RealPrice}购买";
//        _receiveNum.text = I18NManager.Get("Shop_CanReceiveGem",GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_GEM_NUM));//每天可以领取的奖励！_shopModel.GetMonthCardMall.Award[0].Num
//        
        //开通月卡后才可以领钻石

        //这个表示已经领取了
        //        _vipcontent = transform.Find("ShowPic/Content").GetText();
//        _reveiveVipAward = transform.Find("ShowPic/ReceiveBtn").GetButton();
//        _receiveNum = transform.Find("ShowPic/ReceiveBtn/Text").GetText();
//        _leftDay = transform.Find("ShowPic/LeftDay").GetText();
//        _hasreceive = transform.Find("ShowPic/HasReveive").gameObject;

//        _reveiveVipAward.onClick.AddListener(() =>
//        {
//            if (canReceive)
//            {
//                SendMessage(new Message(MessageConst.CMD_MALL_DAILYGEMREWARD)); 
//            }
//
//        });

//        _tasteBtn.onClick.AddListener(() =>
//        {
//            SendMessage(new Message(MessageConst.CMD_USETASTECARD));
//        });

//        _buymonthBtn.onClick.AddListener(() =>
//        {
//            SendMessage(new Message(MessageConst.CMD_BUYMONTHCARD));
//        });

        //    private void ChangeColor(string name)
//    {
//        for (int i = 0; i < _tabBar.childCount; i++)
//        {
//            var choose = _tabBar.GetChild(i).Find("ChooseLabel").gameObject;
//            var nochoose = _tabBar.GetChild(i).Find("NoChooseLabel").gameObject;  
//            choose.SetActive(_tabBar.GetChild(i).name==name);
//            nochoose.SetActive(_tabBar.GetChild(i).name!=name);
//        }
//        
//    }

//    public void SetVIPDailyGemState()
//    {
//        _vipcontent.text = I18NManager.Get("Activity_MonthCardHint1",_shopModel.GetMonthCardMall?.Award[0].Num,GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_GEM_NUM)
//            ,GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_POWER_NUM),GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_LEVEL_EXP_NUM));
//        if (GlobalData.PlayerModel.PlayerVo.UserMonthCard!=null)
//        {
////            Debug.LogError(DateUtil.CheckIsToday(
////                DateUtil.GetDataTime(GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime))+" "+GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime);
//            //int isToday = DateUtil.CheckIsToday(receiveTime);
//            var day = DateUtil.GetLeftDay(GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime -
//                                          ClientTimer.Instance.GetCurrentTimeStamp());
//            _leftDay.text = I18NManager.Get("Activity_MonthCardHint6",day);
//            if (day>0)
//            {
//                _leftDay.gameObject.SetActive(true); 
//                _vipcontent.gameObject.SetActive(false);
//                _reveiveVipAward.gameObject.SetActive(true);
//            }
//            else
//            {
//                _leftDay.gameObject.SetActive(false); 
//                _vipcontent.gameObject.SetActive(true);
//                _reveiveVipAward.gameObject.SetActive(false);
//            }
//            
//            //GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_DAY_NUM) - 
//            
//            bool viprewardshow =(ClientTimer.Instance.GetCurrentTimeStamp()-GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime) >= 0;       
////            Debug.LogError(viprewardshow+" "+GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime);
//            _hasreceive.SetActive(!viprewardshow); 
//
//            
//        }
//        else
//        {
//            //为空的时候！
//            _leftDay.gameObject.SetActive(false);
//            _vipcontent.gameObject.SetActive(true);
//            _hasreceive.SetActive(false); 
//            _reveiveVipAward.gameObject.SetActive(false);
//            
//        }
//        
//    }
    }
}