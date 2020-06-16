using System;
using Assets.Scripts.Componets;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Com.Proto;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ReturnablePanel:Panel
    {
        private static GameObject _backBtn;
        

        public enum BtnYPosition
        {
            BTN_POSITION_LOW=-210,
            BTN_POSITION_NORMAL=-30
        }
        public static float BackBtnY;

        public static void SetBackBtn(GameObject btn)
        {
            _backBtn = btn;
            BackBtnY = _backBtn.GetComponent<RectTransform>().anchoredPosition.y;
//            BackBtnY = _backBtn.transform.localPosition.y;
            _backBtn.Hide();
        }

        public override void Init(IModule module)
        {
            base.Init(module);
            ShowBackBtn();
        }

        public void ShowBackBtn(BtnYPosition offsetY = 0)
        {
            float offY = -ModuleManager.OffY/2  + (int)offsetY + (int)BtnYPosition.BTN_POSITION_NORMAL;

            if (_backBtn != null)
            {
                _backBtn.Show();
                RectTransform rect = _backBtn.GetComponent<RectTransform>();
                BackBtnY = offY;
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, offY);

                _backBtn.transform.SetSiblingIndex(0);
                _backBtn.GetComponent<BackBtnComponent>().OnBackClick = OnBackClick;
            }
            
        }
        
        public void HideBackBtn()
        {
            _backBtn.Hide();
        }

        public virtual void OnBackClick()
        {
            ModuleManager.Instance.GoBack();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (_backBtn != null)
                _backBtn.Hide();
        }
    }
}
