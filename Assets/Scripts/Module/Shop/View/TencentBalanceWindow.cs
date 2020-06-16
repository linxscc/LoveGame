using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Pay;
using Com.Proto;
using game.tools;
using UnityEngine;

namespace game.main
{
    public class TencentBalanceWindow : Window
    {
        private RemoteService<TxBalanceRes> _service;

        private void Awake()
        {
            transform.GetText("BalanceText").text = I18NManager.Get("Shop_TencentBalanceCheck");
            
            PointerClickListener.Get(transform.Find("CustomerServiceBtn").gameObject).onClick = GotoCustomerService;

            _service = new TencentBalanceService().SetCallback(OnGetBalance);
            _service.Execute();
        }

        private void OnGetBalance(TxBalanceRes res)
        {
            int num = (int) (res.Balance / 10);
            transform.GetText("BalanceText").text = I18NManager.Get("Shop_TencentBalance", num);
        }

        private void GotoCustomerService(GameObject go)
        {
            SdkHelper.CustomServiceAgent.Show();
        }

        private void OnDestroy()
        {
            _service?.Dispose();
        }
    }
}