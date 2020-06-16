
public interface IPayAgent
{
    void Pay(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None);
    void PayMonthCard(PayAgent.PayType payType = PayAgent.PayType.None);
    void PayGrowthFund(PayAgent.PayType payType = PayAgent.PayType.None);
    void PayGift(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None);
    void CheckPayWhenLogin();
    void InitPay();
}

