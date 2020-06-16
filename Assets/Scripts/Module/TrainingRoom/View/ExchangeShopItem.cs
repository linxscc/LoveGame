using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExchangeShopItem : MonoBehaviour
{
    private RawImage _icon;
    private Text _num;
    private Text _price;
    private GameObject _sellOut;
    private ExchangeVO _data;
    private Frame _frame;
    private Text _name;
    private Button _clickBtn;
    private GameObject _mask;

    private void Awake()
    {
        _frame = transform.Find("BigFrame").GetComponent<Frame>();
        _name = transform.GetText("Name");
        _price = transform.GetText("ExchangeNumBg/Text");
        _icon = transform.GetRawImage("ExchangeNumBg/Icon");
        _clickBtn = transform.GetButton("OnClick");
        _mask = transform.Find("Mask").gameObject;
        _mask.gameObject.SetActive(false);

        _num = transform.GetText("Num");
        
        _clickBtn.onClick.AddListener(OnClickItem);
        
        // _sellOut = transform.Find("SellOut").gameObject;
    }

    private void OnClickItem()
    {
        if(_data.IsBuy == false)
            EventDispatcher.TriggerEvent(EventConst.BuyExchangeItem, _data);
    }

    public void SetData(ExchangeVO vo)
    {
        // _icon.texture = ResourceManager.Load<Texture>(vo.IconPath);
        _num.text = vo.Num.ToString();
        _price.text = vo.Price.ToString();
        _data = vo;
        // IsBuy(vo.IsBuy);
        
        _frame.SetData(vo.Rewards[0]);
        _name.text = vo.Rewards[0].Name;
        _mask.gameObject.SetActive(vo.IsBuy);
    }

    // private void IsBuy(bool isBuy)
    // {
    //     if (isBuy)
    //     {
    //         _sellOut.Show();
    //     }
    //     else
    //     {
    //         _clickBtn.gameObject.Show();
    //         
    //     }
    // }
}
