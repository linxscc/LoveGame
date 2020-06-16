using Com.Proto;
using Componets;
using Framework.GalaSports.Service;
using game.main;

namespace Assets.Scripts.Module.Pay.Agent
{
    public class PayAgentTencent : PayAgent
    {
        public double Balance = -1;
        
        /// <summary>
        /// 应用宝充值补单
        /// </summary>
        public bool CompensateOrder = false;
        
        protected override void CreateOrder(ProductVo product)
        {
            _product = product;
            LoadingOverlay.Instance.Show();
            new TencentBalanceService().SetCallback(OnGetBalance, OnGetBalanceFail).Execute();
        }

        private void OnGetBalanceFail(HttpErrorVo obj)
        {
            LoadingOverlay.Instance.Hide();
        }

        private void OnGetBalance(TxBalanceRes obj)
        {
            LoadingOverlay.Instance.Hide();
            
            if(Balance >= _product.Price)
            {
                CompensateOrder = true;
            }
            else
            {
                CompensateOrder = false;
            }
            base.CreateOrder(_product);
        }

        protected override void OnGetOrderSuccess(CreateOrderRes res)
        {
            if (CompensateOrder)
            {
                LoadingOverlay.Instance.Hide();
                OnPaySuccess(res.OrderId);
            }
            else
            {
                base.OnGetOrderSuccess(res);
            }
        }

        public override void OnCheckOrdersSuccess(CheckOrderRess resList)
        {
            base.OnCheckOrdersSuccess(resList);
            if (Balance > 0)
            {
                FlowText.ShowMessage(I18NManager.Get("Pay_TencentBalance"));

                foreach (var res in resList.CheckOrderRess_)
                {
                    Balance -= res.Amount;
                }
            }
        }
    }
}