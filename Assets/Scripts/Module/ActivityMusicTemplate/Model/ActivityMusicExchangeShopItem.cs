using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicExchangeShopItem : MonoBehaviour
{
    private Frame _frame;
    private Text _name;
    private Text _price;
    private RawImage _icon;
    private Button _clickBtn;
    private GameObject _mask;
    private ActivityExchangeShopVo _data;
    
    private void Awake()
    {
        _frame = transform.Find("BigFrame").GetComponent<Frame>();
        _name = transform.GetText("Name");
        _price = transform.GetText("ExchangeNumBg/Text");
        _icon = transform.GetRawImage("ExchangeNumBg/Icon");
        _clickBtn = transform.GetButton("OnClick");
        _mask = transform.Find("Mask").gameObject;
        _mask.gameObject.SetActive(false);
        _clickBtn.onClick.AddListener(OnClickItem);
    }

    private void OnClickItem()
    {
        EventDispatcher.TriggerEvent(EventConst.OnClickExchangeItem,_data);
    }


    public void SetData(ActivityExchangeShopVo vo)
    {
        _data = vo;
        _frame.SetData(vo.Rewards[0]);
        _name.text = vo.Rewards[0].Name;
        _price.text = vo.Price.ToString();
        _icon.texture = ResourceManager.Load<Texture>(vo.ExchangeIconPath);
        _mask.gameObject.SetActive(vo.RemainBuyNum==0);
    }
}
