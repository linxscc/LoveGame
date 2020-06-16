using DataModel;
using game.main;
using UnityEngine;

public class PayAgentEditor : IPayAgent
{
    public void Pay(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ShowProduct(product);
    }

    public void PayMonthCard(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ShowProduct(GlobalData.PayModel.GetMonthCardProduct());
    }

    public void PayGrowthFund(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ShowProduct(GlobalData.PayModel.GetGrowthCapitalProduct());
    }

    public void PayGift(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ShowProduct(product);
    }

    public void CheckPayWhenLogin()
    {
    }

    public void InitPay()
    {
        string str =
            "{\"CN_gift_25\":\"88.98\",\"CN_gift_09\":\"148.98\",\"storeCountry\":\"CN\",\"currency\":\"CNY\",\"CN_gift_22\":\"16.98\",\"CN_gift_03\":\"1.48\",\"CN_gift_16\":\"44.98\",\"CN_gift_05\":\"6.98\",\"CN_gift_11\":\"8.98\",\"CN_gift_13\":\"17.98\",\"CN_gift_07\":\"28.98\",\"CN_gift_21\":\"10.98\",\"CN_gift_02\":\"1.48\",\"CN_gift_15\":\"36.98\",\"CN_gift_23\":\"22.98\",\"CN_gift_04\":\"4.48\",\"CN_gift_10\":\"2.98\",\"CN_gift_19\":\"5.98\",\"CN_gift_06\":\"14.98\",\"CN_gift_12\":\"1.48\",\"CN_gift_17\":\"60.98\",\"CN_gift_20\":\"5.98\",\"CN_gift_01\":\"1.48\",\"CN_gift_14\":\"21.98\",\"CN_gift_24\":\"64.98\",\"CN_gift_18\":\"65.98\",\"CN_gift_08\":\"68.98\"}";
            
            //"{\"gift_33\":\"MYR 12.90\",\"gift_35\":\"MYR 39.90\",\"gift_34\":\"MYR 19.90\",\"gift_32\":\"MYR 4.99\",\"gift_36\":\"MYR 79.90\",\"gift_37\":\"MYR 199.90\",\"gift_8\":\"MYR 399.90\",\"gift_30\":\"MYR 1.90\",\"gift_31\":\"MYR 1.90\"}";
//        string str =
//            "{\"CN_gift_01\":\"CNY 12.90\",\"CN_gift_02\":\"CNY 39.90\",\"CN_gift_03\":\"CNY 19.90\",\"gift_32\":\"MYR 4.99\",\"gift_04\":\"MYR 79.90\",\"gift_37\":\"MYR 199.90\",\"gift_8\":\"MYR 399.90\",\"gift_30\":\"MYR 1.90\",\"gift_31\":\"MYR 1.90\"}";
        JSONObject jsonObject = new JSONObject(str);
        GlobalData.PayModel.SetAreaPrice(jsonObject);
    }

    private void ShowProduct(ProductVo vo)
    {
        Debug.LogError("商品信息："+vo.ToString());

        IconSelectWindow win = PopupManager.ShowWindow<IconSelectWindow>(Constants.IconSelectWindowPath);
        win.SetData("",IconType.Alipay, IconType.WeChatFriend);
        win.clickCallback = (m) =>
        {
            win.Close();
        };
    }
}