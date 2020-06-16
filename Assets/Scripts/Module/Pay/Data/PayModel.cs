using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using Google.Protobuf.Collections;

namespace Assets.Scripts.Module.Pay.Data
{
    public class PayModel : Model
    {
        private List<ProductVo> _productList;

        public void InitProductInfo(CommodityInfoRes res)
        {
            _productList = new List<ProductVo>();
            foreach (var pb in res.CommodityInfo)
            {
                ProductVo vo = new ProductVo(pb);
                _productList.Add(vo);
            }
        }

        public void AddOn(RepeatedField<RmbMallRulePB> rules)
        {
            foreach (var productVo in _productList)
            {
                foreach (var rule in rules)
                {
                    if (productVo.CommodityId == rule.MallId)
                    {
                        productVo.Name = rule.MallName;
                        productVo.Description = rule.MallDesc;
                    }
                }
            }
        }
        
        public string[] GetProductIds()
        {
            string[] list = new string[_productList.Count];

            for (int i = 0; i < list.Length; i++)
            {
                list[i] = _productList[i].ProductId;
            }

            return list;
        }

        public List<ProductVo> GetGemProducts()
        {
            List<ProductVo> list = new List<ProductVo>();

            foreach (var productVo in _productList)
            {
                if (productVo.ProductType == CommodityTypePB.Recharge)
                {
                    list.Add(productVo);
                }
            }

            return list;
        }
        
        public List<ProductVo> GetGiftProducts()
        {
            List<ProductVo> list = new List<ProductVo>();

            foreach (var productVo in _productList)
            {
                if (productVo.ProductType == CommodityTypePB.Gift)
                {
                    list.Add(productVo);
                }
            }

            return list;
        }
        
        public ProductVo GetMonthCardProduct()
        {
            if (_productList==null)
            {
                return null;
            }
            
            foreach (var productVo in _productList)
            {
                if (productVo.ProductType == CommodityTypePB.MonthCard)
                {
                    return productVo;
                }
            }

            return null;
        }
        
        public ProductVo GetGrowthCapitalProduct()
        {
            foreach (var productVo in _productList)
            {
                if (productVo.ProductType == CommodityTypePB.GrowthFund)
                {
                    return productVo;
                }
            }

            return null;
        }
        
        /// <summary>
        /// 通过商品ID获取ProductVo
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns></returns>
        public ProductVo GetProduct(int id)
        {
            foreach (var productVo in _productList)
            {
                if (productVo.CommodityId == id)
                {
                    return productVo;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 通过计费点获得ProductVo
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductVo GetProductById(string productId)
        {
            foreach (var productVo in _productList)
            {
                if (productVo.ProductId == productId)
                {
                    return productVo;
                }
            }
            return null;
        }
        
        public void SetAreaPrice(JSONObject strList)
        {
            string curreny = null;
            bool hasCurreny = false;
            if (strList.keys.Contains("currency"))
            {
                curreny = strList["currency"].str;
                hasCurreny = true;
            }

            string pattern = @"(([1-9][0-9]*)|(([0]\.\d{0,2}|[1-9][0-9]*\.\d{0,2})))$";//匹配模式
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            foreach (var vo in _productList)
            {
                foreach (var str in strList.keys)
                {
                    if (str==vo.ProductId)
                    {
                        string price = strList[str].str;
                        if(hasCurreny == false)
                        {
                            MatchCollection matches = regex.Matches(price);
                            foreach (Match match in matches)
                            {
                                string temp = price.Substring(match.Index, match.Length);

                                curreny = price.Replace(temp, "").Trim();
                                price = temp.Trim();
                            }
                        }

        
                        vo.Curreny = curreny;

                        if (vo.Curreny==Constants.CHINACURRENCY)
                        {
                            vo.AreaPrice = price+"元";
                        }
                        else
                        {
                            vo.AreaPrice = price; 
                        }
                    }
                }
            }          
        }

       
    }
}