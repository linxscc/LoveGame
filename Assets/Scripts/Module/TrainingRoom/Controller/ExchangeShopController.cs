using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Utils;

public class ExchangeShopController : Controller
{
    private ExchangeItemWindow _exchangeItemWindow;
    private ExchangeShopModel _model;
    private ExchangeVO _vo;

    public ExchangeShopView View;
    public override void Init()
    {
        base.Init();
    }

    public override void Start()
    {
        NetWorkManager.Instance.Send<MallRes>(CMD.MUSICGAMEC_MALL, null, GetExchangeShopDate);
        _model = GetData<ExchangeShopModel>();
        EventDispatcher.AddEventListener<ExchangeVO>(EventConst.BuyExchangeItem, OpenBuyWindow);
    }

    private void GetExchangeShopDate(MallRes res)
    {
        _model.InitOpenExchangeShop(res);
        View.SetData(_model.GetExchangeShops(), _model.GetCurRefreshRules(_model.GetRefreshMallNum()));
    }

    public override void OnMessage(Message message)
    {
        var name = message.Name;
        var body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_TRAININGROOM_EXCHANGESHOP_OPEN_REFRESH_NOTARIZE_WINDOW:
                OpenRefreshConfirmWindow();
                break;
        }
    }

    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.BuyExchangeItem);
    }

    private void OpenRefreshConfirmWindow()
    {
        var costGem = _model.GetCurRefreshRules(_model.GetRefreshMallNum()).ResourceNum;
        var refresh = _model.GetTodayMayRefreshNum() + 1;
        // var isMayRefresh = _model.IsMayRefresh();
        // var hint = _model.GetNoRefreshHint();

        var desc = "是否消耗" + costGem + "星钻刷新所有商品?\n今日还可刷新" + refresh + "次";

        var confirmWindow = PopupManager.ShowConfirmWindow(desc);
        confirmWindow.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok) SendRefreshReq();
        };
    }


    //打开购买窗口
    private void OpenBuyWindow(ExchangeVO vo)
    {
        if (_exchangeItemWindow == null)
        {
            _exchangeItemWindow =
                PopupManager.ShowWindow<ExchangeItemWindow>("TrainingRoom/Prefabs/ExchangeItemWindow");
            _exchangeItemWindow.SetData(vo);
            _exchangeItemWindow.WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok) SendBuyExchangeItemReq(vo);
            };
        }
    }


    //发送购买请求
    private void SendBuyExchangeItemReq(ExchangeVO vo)
    {
        LoadingOverlay.Instance.Show();
        _vo = null;
        _vo = vo;
        var req = new ShoppingReq {SlotId = vo.SlotId};
        var data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ShoppingRes>(CMD.MUSICGAMEC_SHOPPING, data, GetBuyExchangeItemRes);
    }

    //拿到购买回包
    private void GetBuyExchangeItemRes(ShoppingRes res)
    {
        LoadingOverlay.Instance.Hide();
        GlobalData.TrainingRoomModel.UpdateCurIntegral(res.Integral);
        foreach (var t in res.Awards) RewardUtil.AddReward(t);
        _vo.IsBuy = true;
        _model.BuyLaterUpdateExchangeShops(_vo);
        View.UpdateBuyLaterExchangeItemState(_vo);
    }


    //发送刷新请求
    private void SendRefreshReq()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<RefreshMallRes>(CMD.MUSICGAMEC_REFRESHMALL, null, GetRefreshRes);
    }

    //拿到刷新回包
    private void GetRefreshRes(RefreshMallRes res)
    {
        LoadingOverlay.Instance.Hide();
        _model.UpdateExchangeShops(res.Info);
        _model.UpdateRefreshMallNum(res.RefreshMallCount);
        GlobalData.PlayerModel.UpdateUserMoney(res.Money);
        View.SetData(_model.GetExchangeShops(), _model.GetCurRefreshRules(_model.GetRefreshMallNum()));
    }
}