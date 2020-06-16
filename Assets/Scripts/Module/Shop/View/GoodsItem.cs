using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class GoodsItem : MonoBehaviour, IPointerClickHandler
    {
        private Text _goodsName;
        private RawImage _goodsimg;
        private Image _tag;

        private Text _tagContend;
        //private GameObject _malllabelobj;
        private RawImage _monenyIcon;
        private Text _rmbIcon;
        private Text _curcost;
        private Text _primecost;
        private Text _desc;
        private Text _limitText;
        private GameObject _tips;
        private Text _tiprefreshLabe;
        private GameMallVo _itemvo;
        private UserBuyGameMallVo _userBuyGameMallVo;
        private Transform _limitTran;
        private GameObject nodiscountBg;
        private GameObject discountBg;
        private TimerHandler _handler;
        private Text _goodsNum;

        private GameObject _gemIcon;
        private GameObject _redPoint;
        private Text _selloutTips;

        //private Image _numIconTran;
        //private Button _clickBtn;
        //private Transform _rawBg;

        private RmbMallVo _itemRmbVo;
        private UserBuyRmbMallVo _userBuyRmbMallVo;
        private MallSortPB _curmallSortPB;
        private bool _canEnter = false;
        private long _endtime = 0;
        private int mallid = 0;
        private bool _isfirstPrice=false;
        private bool _isBuyGem = false;

        private string _areaprice="";

        private void Awake()
        {
            _gemIcon = transform.Find("NameText/Gem").gameObject;
            _goodsName = transform.Find("NameText").GetText();
            _goodsimg = transform.Find("GoodsItem/Goods").GetRawImage();
            _tag = transform.Find("CardQualityImage/Detail").GetImage();
            _tagContend = transform.Find("CardQualityImage/Detail/Text").GetText();
            _curcost = transform.Find("Cost/Current").GetText();
            _primecost = transform.Find("Cost/Prime").GetText();
            _desc = transform.Find("Des/Text").GetText();
            _tips = transform.Find("GoodsItem/Goods/Tips").gameObject;
            _tiprefreshLabe = transform.Find("GoodsItem/Goods/Tips/Label").GetText();
            _limitTran = transform.Find("LimitDes");
            _limitText = transform.Find("LimitDes/Text").GetText();
            _monenyIcon = transform.Find("Cost/Icon").GetRawImage();
            _rmbIcon = transform.Find("Cost/Current/RMBIcon").GetText();
            _goodsNum = transform.Find("GoodsItem/Goods/GoodsNum").GetText();
            //_numIconTran = transform.Find("CardQualityImage/Detail/DiscountNum").GetComponent<Image>();

            nodiscountBg = transform.Find("Cost/BgDesc").gameObject;
            discountBg = transform.Find("Cost/DiscountBg").gameObject;
            _redPoint = transform.Find("RedPoint").gameObject;
            _selloutTips = transform.Find("GoodsItem/Goods/Tips/SellOutTip").GetText();

        }


        public void SetData(GameMallVo vo, UserBuyGameMallVo uservo,long refreshTime)
        {
            _rmbIcon.gameObject.SetActive(false);
            _itemvo = vo;
            mallid = vo.MallId;
            _userBuyGameMallVo = uservo;
            _curmallSortPB = vo.MallSortPb;
            if (vo.OriginalPrice>0)
            {
                _primecost.text = vo.OriginalPrice+""; 
            }
            SetCommonUIData(vo.MallName, vo.MallDesc, vo.RealPrice, vo.OriginalPrice,
                DateUtil.GetDay(vo.EndTime-ClientTimer.Instance.GetCurrentTimeStamp()), vo.MallLabelPb, vo.BuyMax,
                uservo != null && (uservo.BuyNum >= vo.BuyMax && vo.BuyMax != 0),
                (vo.MoneyTypePb == MoneyTypePB.MoGem ? PropConst.GemIconId : PropConst.GoldIconId), vo.Award,
                vo.GiftImage,refreshTime,_itemvo.LabelImage);
        }

        public void SetData(RmbMallVo vo, UserBuyRmbMallVo uservo,ShopModel buyGemModel=null)
        {
            _rmbIcon.gameObject.SetActive(true);
            _itemRmbVo = vo;
            mallid = vo.MallId;
            _userBuyRmbMallVo = uservo;
            _curmallSortPB = vo.MallSortPb;
            _isBuyGem = buyGemModel != null;
            if (vo.MallSortPb==MallSortPB.MallOrdinary&&buyGemModel!=null&&!buyGemModel.HasDoublePrice(vo.RealPrice))
            {
                _isfirstPrice = true;
            }
            else
            {
                _isfirstPrice = false;
            }


//            Debug.LogError(vo.MallName+"_isfirstPrice"+_isfirstPrice);
            var payvo = GlobalData.PayModel.GetProduct(vo.MallId);

            if (vo.OriginalPrice>0)
            {
                _primecost.text = payvo?.GetOriginalPrice(vo.OriginalPrice); 
            }
            var realrmbpoint = payvo != null ? payvo.AmountRmb : vo.RealPrice;
   
            _rmbIcon.text = payvo?.Curreny;
            if (AppConfig.Instance.isChinese=="true"||payvo?.Curreny==Constants.CHINACURRENCY)
            {
                _rmbIcon.text = "";
                _primecost.text = vo.OriginalPrice+"元";
            }
            
            _areaprice = payvo?.AreaPrice;
            if (_isBuyGem)
            {
                realrmbpoint = vo.RealPrice*10;
                if (vo.RealPrice<=0)
                {
                    Debug.LogError("数据异常："+vo.MallName+" "+vo.OriginalPrice);
                }

            }
                       
            if (uservo != null)
                SetCommonUIData(vo.MallName, vo.MallDesc, realrmbpoint, vo.OriginalPrice,
                    DateUtil.GetDay(vo.EndTime - ClientTimer.Instance.GetCurrentTimeStamp()), vo.MallLabelPb, vo.BuyMax,
                    (uservo.BuyNum >= vo.BuyMax && vo.BuyMax != 0), PropConst.GemIconId, vo.Award,
                    vo.GiftImage, uservo.RefreshTime,_itemRmbVo.LabelImage); //(vo.BuyMax - uservo?.BuyNum ?? 0) + "/" + vo.BuyMax
        }

        private void SetCommonUIData(string goodsNameText, string desc, double realprice, double primecost, string limitText,
            MallLabelPB mallLabelPb, int buyMax, bool finishedtipsShow, int propIconId, RepeatedField<AwardPB> awardPbs,
            string giftImgae,long endtime,string imageLabel="")
        {
            _goodsNum.text = "";
            _desc.text = desc;
            _curcost.text = realprice.ToString();
            
            string spName = "";
            _tag.gameObject.SetActive(true);
            switch (imageLabel)
            {
                case "0":
                    _tag.gameObject.SetActive(false);
                    break;
                case "1":
                    spName = I18NManager.Get("Shop_DiscountTips");//"折扣"; 
                    break;
                case "2":
                    spName = I18NManager.Get("Shop_LimitCount");
                    break;
                case "3":
                    spName = "VIP";
                    break;
                case "4":
                    spName = "Hot";
                    break;
                default:
                    Debug.LogError("Notype:"+imageLabel);
                    break;
                
            }
            bool discount = primecost > 0 ;//&& primecost > realprice折扣判断有改变，primecost>0就表示有折扣
            _primecost.gameObject.SetActive(discount);
            if (_isfirstPrice)
            {
                //_tag.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Shop_mallSign"); 
//                Debug.LogError(_isfirstPrice);
                _tag.gameObject.SetActive(true);
                _tagContend.text = I18NManager.Get("Shop_FirstPrice");;
            }
            else
            {
                if (!String.IsNullOrEmpty(spName))
                {
                    //_tag.sprite = AssetManager.Instance.GetSpriteAtlas(spName); 
                    _tagContend.text = spName;
                }

            }
            

            
            if (_curmallSortPB == MallSortPB.MallGift||_curmallSortPB == MallSortPB.MallOrdinary)
            {
                _monenyIcon.gameObject.SetActive(false);
//                _rmbIcon.SetActive(true);
                _curcost.text = realprice.ToString();
            }
            else
            {
//                _rmbIcon.SetActive(false);
                _monenyIcon.gameObject.SetActive(true);
                _monenyIcon.texture =
                    ResourceManager.Load<Texture>("Prop/particular/" + propIconId, ModuleConfig.MODULE_SHOP,true);
            }

            _limitTran.gameObject.SetActive(false);//mallLabelPb==MallLabelPB.LimitDiscount
            _limitText.text = I18NManager.Get("Shop_LeftTimeToEnd", limitText);//$"剩余{limitText}下架";

            _tips.SetActive(finishedtipsShow);
            _selloutTips.text = realprice > 0 ? "已售罄" : "已领取";
            
            if (finishedtipsShow&&_curmallSortPB==MallSortPB.MallGift)
            {
                //Debug.LogError(endtime);
                _tiprefreshLabe.text = "";
                _endtime = endtime;
                _handler=ClientTimer.Instance.AddCountDown(mallid.ToString(), Int64.MaxValue, 1f, RefreshTime, null);//"RefreshTime"
            }
            else
            {
                _tiprefreshLabe.text = "";
            }

            
            if (_curmallSortPB == MallSortPB.MallGift||_curmallSortPB == MallSortPB.MallOrdinary||_curmallSortPB==MallSortPB.MallRebateGift)
            {
//                Debug.LogError(giftImgae);
                _goodsimg.texture = ResourceManager.Load<Texture>(GlobalData.PropModel.GetGiftPropPath(giftImgae));

            }
            else
            {
                foreach (var awardPb in awardPbs)
                {
                    RewardVo vo=new RewardVo(awardPb);
                    _goodsimg.texture =
                        ResourceManager.Load<Texture>(vo.IconPath);
                    //顺便加上描述！
                    _goodsNum.text = _curmallSortPB == MallSortPB.MallBatchItem ? awardPb.Num.ToString() : "";
                }
            }

            _gemIcon.SetActive(_isBuyGem);
            if (_isBuyGem)
            {
                if (_isfirstPrice)
                {
                    _goodsName.text = SetRechargeName(realprice, realprice);
                }
                else
                {
                    if (awardPbs.Count>0)
                    {
                        foreach (var awardPb in awardPbs)
                        {
                            if (awardPb.Resource==ResourcePB.Gem)
                            {
                                _goodsName.text = SetRechargeName(realprice,awardPb.Num);   
                            }
                        }
                    }
                    else
                    {
                        _goodsName.text = SetRechargeName(realprice,0);   
                    }
                }
            }
            else
            {
                _goodsName.text = goodsNameText;  
            }

            if (String.IsNullOrEmpty(_areaprice)&&(_curmallSortPB == MallSortPB.MallGift||_curmallSortPB == MallSortPB.MallOrdinary))
            {
                _curcost.text = ""; 
            }
            else if(!String.IsNullOrEmpty(_areaprice))
            {
                _curcost.text =_areaprice;
            }
            else
            {
                _curcost.text =realprice>0? realprice.ToString():"免费";
            }
            _redPoint.gameObject.SetActive(realprice <= 0 && !finishedtipsShow);
        }

        private string SetRechargeName(double amount,double awardnum)
        {
            if (awardnum>0)
            {

                return I18NManager.Get("Shop_SendGem", amount, awardnum); //amount + "送" + awardnum; 
            }

            return amount+"";

        }
        
        private void RefreshTime(int time)
        {
            if (_endtime>ClientTimer.Instance.GetCurrentTimeStamp())
            {
                _tiprefreshLabe.text =  I18NManager.Get("Shop_RefreshCountTimer",
                    DateUtil.GetTimeFormat4(_endtime-ClientTimer.Instance.GetCurrentTimeStamp())); 
            }
            else
            {
                _tiprefreshLabe.text = "";//重进该界面可刷新

            }
            

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //要根据_itemvo.MallSortPb来打开不同的窗口！
//            Debug.LogError("OnPointerClick");
            if (_canEnter)
            {
                FlowText.ShowMessage(I18NManager.Get("Shop_BuyMax"));
                return;
            }
            
            switch (_curmallSortPB)
            {
                case MallSortPB.MallGift:
                    if (_itemvo != null)
                    {
                        EventDispatcher.TriggerEvent(EventConst.BuyMallGift, _userBuyGameMallVo);
                    }
                    else
                    {
                        EventDispatcher.TriggerEvent(EventConst.BuyRmbMallGift, _userBuyRmbMallVo);
                    }

                    break;
                case MallSortPB.MallItem:
                    EventDispatcher.TriggerEvent(EventConst.BuyMallItem, _itemvo, _userBuyGameMallVo);
                    break;
                case MallSortPB.MallBatchItem:
                    EventDispatcher.TriggerEvent(EventConst.BuyMallBatchItem, _itemvo, _userBuyGameMallVo);
                    break;
                case MallSortPB.MallRebateGift:
                    FlowText.ShowMessage(I18NManager.Get("Shop_Hint3"));// ("暂无返利礼包");
                    break;
                case MallSortPB.MallOrdinary:
                    if (_itemvo != null)
                    {
                        EventDispatcher.TriggerEvent(EventConst.BuyMallGift, _itemvo);
                    }
                    else
                    {
                        EventDispatcher.TriggerEvent(EventConst.BuyGemMall, _itemRmbVo);
                    }
                    break;
                default:
                    if (_itemvo!=null)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Shop_Hint4", _itemvo.MallSortPb));// ("枚举错误？" + _itemvo.MallSortPb); 
                    }
                    else
                    {
                        FlowText.ShowMessage(I18NManager.Get("Shop_Hint4", _itemRmbVo.MallSortPb));// ("枚举错误？" + _itemvo.MallSortPb);  
                    }


                    break;
            }
        }

        private void OnDestroy()
        {
            if (_handler!=null)
            {
                ClientTimer.Instance.RemoveCountDown(_handler);
            }
        }
    }
}