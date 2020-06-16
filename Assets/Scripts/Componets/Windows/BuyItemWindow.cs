

using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class BuyItemWindow : AlertWindow
{
    [SerializeField] private Text _haveNumText;                     //当前拥有数量
    [SerializeField] private Text _itemNumText;                    //道具数量

    [SerializeField] private RawImage _itemRawIamge;           //道具图片
    [SerializeField] private RawImage _costIconRawIamge;           //道具图片
    [SerializeField] private LongPressButton _addLongPressBtn;
    [SerializeField] private LongPressButton _reduceLongPressBtn;

  
     private int _itemNum;          //道具数量      
     private int _costNum;           //花费数量
     private int _mallId;      //商品ID
     private int _itemId;      //道具ID
   


    protected override void OnInit()
    {
        base.OnInit();
        _addLongPressBtn.Interval = 0.25f;
        _reduceLongPressBtn.Interval = 0.25f;
        AddBtnEvent();
        ReduceBtnEvent();

 
    }

    /// <summary>
    /// 增加按钮事件
    /// </summary>
    private void AddBtnEvent()
    {
        //按下
        _addLongPressBtn.OnDown = (() => {

            if (_itemNum == 100) { return; }
            _itemNum++;
            _itemNumText.text = _itemNum.ToString();
            //  Content = "是否花费  " + (_itemNum * _costNum).ToString() + "   " + "    购买"; 
            Content = I18NManager.Get("Common_BuyStarContent", (_itemNum * _costNum).ToString());
        });
             
    }


    /// <summary>
    /// 减少按钮事件
    /// </summary>
    private void ReduceBtnEvent()
    {
        //按下
        _reduceLongPressBtn.OnDown = (() => {
            if (_itemNum == 1) { return; }
            _itemNum--;
            _itemNumText.text = _itemNum.ToString();
            //Content = "是否花费  " + (_itemNum * _costNum).ToString() + "   " + "    购买";
            Content = I18NManager.Get("Common_BuyStarContent", (_itemNum * _costNum).ToString());
        });
 
    }

    protected override void OnOkBtn()
    {
      
        WindowEvent = WindowEvent.Ok;             
//        BuyItemReq req = new BuyItemReq
//        {
//            MallId = _mallId,
//            Num= _itemNum,
//        };
//
//        byte[] data = NetWorkManager.GetByteData(req);
//        NetWorkManager.Instance.Send<BuyItemRes>(CMD.ITEMC_BUYITEM, data, res =>
//        {
//            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
//            GlobalData.PropModel.UpdateProps(new UserItemPB[] { res.UserItem });
//         
//            var num = GlobalData.PropModel.GetUserProp(_itemId).Num;         
//            
//            
//            CloseAnimation();
//        });

    }

    private string imagePath = "Prop/particular/";       //图片路径

   
    public void InitWindowInfo(int buyItemId,int costItemId)
    {
        switch (buyItemId)
        {
            case PropConst.DrawCardByGem:
                BuyStarCard(buyItemId,costItemId);
                break;
        }
    }


    private void BuyStarCard(int buyItemId,int costItemId)
    {
        var _curHaveNum = GlobalData.PropModel.GetUserProp(buyItemId).Num;
        var _mallItem = GlobalData.PropModel.GetMallItemInfo(buyItemId);

        Title = I18NManager.Get("Common_StarCardBuy");//"购买星卡";        
        //_haveNumText.text = "当前拥有   " + _curHaveNum.ToString() + "   张";
        _haveNumText.text = I18NManager.Get("Common_HaveStarCardNum", _curHaveNum.ToString());
        _itemNum = _mallItem.Num;
        _costNum = _mallItem.Price;
        _itemNumText.text = _mallItem.Num.ToString();
        // Content = "是否花费  " + _mallItem.Price.ToString() + "   " + "    购买";
        Content = I18NManager.Get("Common_BuyStarContent", _mallItem.Price.ToString());
        _itemRawIamge.texture = ResourceManager.Load<Texture>(ImageName(buyItemId), null, true);
        _costIconRawIamge.texture = ResourceManager.Load<Texture>(CostImageName(costItemId), null, true);
        _mallId = _mallItem.MallId;
        _itemId = _mallItem.ItemId;
       // OkText = "购  买";
    }
    

    private string ImageName(int buyItemId)
    {
        string temp = "";
        switch (buyItemId)
        {
            case PropConst.DrawCardByGem:           //钻石抽卡卷
                temp = imagePath + PropConst.DrawCardByGem.ToString();
                break;          
        }
        return temp;
    }

    private string CostImageName(int costItemId)
    {
        string temp = "";
        switch (costItemId)
        {
            case PropConst.GemIconId:
                temp = imagePath + PropConst.GemIconId.ToString();
                break;
        }
        return temp;
    }
}
