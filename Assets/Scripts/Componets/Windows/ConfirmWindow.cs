#region 模块信息

// **********************************************************************
// Copyright (C) 2018 The 望尘体育科技
//
// 文件名(File Name):             ConfirmWindow.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/2/12 9:45:57
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************

#endregion

using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class ConfirmWindow : AlertWindow
    {
        [SerializeField] private Button _cancelBtn;
        
        public string CancelText
        {
            get
            {
                return _cancelBtn.GetComponentInChildren<Text>().text;
            }
            set
            {
                _cancelBtn.GetComponentInChildren<Text>().text = value;
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            _cancelBtn.onClick.AddListener(OnCancelBtn);
        }

        protected void OnCancelBtn()
        {
            WindowEvent = WindowEvent.Cancel;
            Close();
        }
    }
}