using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftItem : MonoBehaviour {


    private Transform _propContainer;
    private Button _get;   //领取按钮
    private Text _des;
    private Text _price;
    private int mallId;

    private Transform _originalPrice;
    private Text _originalpriceTxt;
    private Text _freeTxt;
    //private GameObject _puzzleIcon;

    private void Awake()
    {
        _propContainer = transform.Find("PropContainer");
        _get = transform.GetButton("GetBtn");   
        _get.onClick.AddListener(Get);  
        _des = transform.GetText("DescBg/Desc");
        _price = transform.GetText("GetBtn/Text");
        _originalPrice = transform.Find("OriginalPrice");
        _originalpriceTxt = transform.GetText("OriginalPrice/Text");
        _freeTxt = transform.GetText("GetBtn/FreeText");
        //_puzzleIcon = transform.GetImage("");
    }

    public void SetData(RmbMallVo rmbMallVo,UserBuyRmbMallVo uservo)
    {
        mallId = rmbMallVo.MallId;
        _freeTxt.gameObject.SetActive(false);
        _price.gameObject.SetActive(true);
        for (int i = 0; i < rmbMallVo.Award.Count; i++)
        {
            var item=_propContainer.GetChild(i);
            item.gameObject.Show();
            RewardVo vo=new RewardVo(rmbMallVo.Award[i]);
//            PointerClickListener.Get(item.gameObject).onClick = go =>
//            {
//                var desc = ClientData.GetItemDescById(vo.Id, vo.Resource);
//                FlowText.ShowMessage(desc.ItemDesc);
//            };
//            item.Find("PropNameTxt").GetComponent<Text>().text = vo.Name;
            item.GetComponent<Frame>().SetData(vo);
            item.Find("ObtainText").GetComponent<Text>().text =vo.Num.ToString();//I18NManager.Get("Pay_Get")+vo.Num;
//            item.Find("PropImage").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(vo.IconPath);
//            item.Find("Image").gameObject.SetActive(vo.Resource==ResourcePB.Puzzle);
        }
        _des.text =  I18NManager.Get("Shop_DailyBuyLimit");
        //_get.image.color=uservo.BuyNum > 0 ? Color.grey : Color.white;
        var payvo = GlobalData.PayModel.GetProduct(uservo.MallId);
//        var realrmbpoint = payvo != null ? payvo.AmountRmb : rmbMallVo.RealPrice;
//        _get.interactable = uservo.BuyNum == 0 ;
        int imageType = uservo.BuyNum > 0 ? 2 : 1;
        _get.enabled = uservo.BuyNum == 0;
        _get.image.sprite=AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_Btn"+imageType);
        string _areaprice = "";
        string _originalPricetxt = "";
        if (AppConfig.Instance.isChinese=="true"||payvo?.Curreny==Constants.CHINACURRENCY)
        {
            _areaprice = payvo?.AreaPrice ;
            _originalPricetxt = rmbMallVo.OriginalPrice + "元";
        }
        else
        {
            _areaprice = payvo?.Curreny+payvo?.AreaPrice;  
            _originalPricetxt = payvo?.Curreny+payvo?.GetOriginalPrice(rmbMallVo.OriginalPrice);
        }

        _price.text =uservo.BuyNum > 0 ?I18NManager.Get("Common_AlreadyGet") :  _areaprice;
        _originalPrice.gameObject.SetActive(rmbMallVo.OriginalPrice>0);
        _originalpriceTxt.text = _originalPricetxt;//payvo?.Curreny 

    }
    
    public void SetFreeAward(GameMallVo gameMallVo,UserBuyGameMallVo uservo)
    {
        _originalPrice.gameObject.SetActive(false);
        _freeTxt.gameObject.SetActive(true);
        _price.gameObject.SetActive(false);
        mallId = gameMallVo.MallId;
        for (int i = 0; i < gameMallVo.Award.Count; i++)
        {
            var item=_propContainer.GetChild(i);
            item.gameObject.Show();
            RewardVo vo=new RewardVo(gameMallVo.Award[i]);
            item.GetComponent<Frame>().SetData(vo);
//            PointerClickListener.Get(item.gameObject).onClick = go =>
//            {
//                var desc = ClientData.GetItemDescById(vo.Id, vo.Resource);
//                FlowText.ShowMessage(desc.ItemDesc); 
//            };
//            item.Find("PropNameTxt").GetComponent<Text>().text = vo.Name;
            item.Find("ObtainText").GetComponent<Text>().text =vo.Num.ToString();//I18NManager.Get("Pay_Get")+vo.Num;
//            item.Find("PropImage").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(vo.IconPath);
//            item.Find("Image").gameObject.SetActive(vo.Resource==ResourcePB.Puzzle);
        }

        _des.text =  I18NManager.Get("Shop_DailyGetLimit");
//        _get.interactable = !GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus;
        int imageType = GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus ? 2 : 1;
        _get.image.sprite=AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_Btn"+imageType); 
        //_get.image.color = GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus ? Color.grey : Color.white;
        _get.enabled = !GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus;
        _freeTxt.text = GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus ? I18NManager.Get("Common_AlreadyGet") : I18NManager.Get("Common_Free");    
        
    }
    
    private void Get()
    {
//        Debug.LogError("调用SDK！");
        EventDispatcher.TriggerEvent(EventConst.PayforDaily,mallId);
        
    }
}
