using System;
using Com.Proto;
using UnityEngine;

[Serializable]
public class ProductVo 
{
    /// <summary>
    /// 计费点
    /// </summary>
    public string ProductId;
    
    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderId;
    
    public double AmountRmb;
    
   
    /// <summary>
    /// 扩展数据
    /// </summary>
    public string ExtString;

    /// <summary>
    /// 扩展数据(1.触发礼包ID 2.无)
    /// </summary>
    public long ExtInt;

    /// <summary>
    /// 货币符号 例如CNY
    /// </summary>
    public string Curreny;

    public string Name;
    
    public string Description;

    public override string ToString()
    {
        return $"{nameof(ProductId)}: {ProductId}, {nameof(OrderId)}: {OrderId}, {nameof(AmountRmb)}: {AmountRmb}, {nameof(ExtString)}: {ExtString}, {nameof(ExtInt)}: {ExtInt}, {nameof(Curreny)}: {Curreny}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(Price)}: {Price}, {nameof(AreaPrice)}: {AreaPrice}, {nameof(ProductType)}: {ProductType}, {nameof(CommodityId)}: {CommodityId}";
    }
    
    public double Price;

    public string AreaPrice="";
    
    /// <summary>
    /// 商品类型：0宝石，1月卡，2礼包，3成长基金
    /// </summary>
    public CommodityTypePB ProductType;
   
    /// <summary>
    /// 商品唯一标识
    /// </summary>
    public int CommodityId;

    public ProductVo(CommodityInfoPB pb)
    {
//        Debug.LogError(pb);
        ProductId = pb.ProductId;
        AmountRmb = pb.AmountRmb;
        Price = pb.Price;
        CommodityId = pb.CommodityId;
        ProductType = pb.CommodityType;
        if (AppConfig.Instance.isChinese=="true")
        {
            AreaPrice = pb.Price + "元";
        }
    }

    public string GetOriginalPrice(int originalPrice)
    {
        float result = 0;
        if (float.TryParse(AreaPrice, out result))
        {
            result = result / originalPrice * 100;
        }

        return result.ToString("F2");
    }
}