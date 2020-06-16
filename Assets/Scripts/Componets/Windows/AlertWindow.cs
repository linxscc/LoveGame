#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             AlertWindow.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/3/8 14:10:58
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion

using System;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class AlertWindow : Window
	{
	    [SerializeField] private Text _contenText;
	    [SerializeField] private Button _okBtn;
	    [SerializeField] private Text _titleText;

	    public string Title
	    {
	        get { return _titleText.text; }
	        set { _titleText.text = value; }
	    }

	    public string Content
	    {
	        get { return _contenText.text; }
	        set { _contenText.text = value; }
	    }

		public bool CanClickBGMask = true;

	    public string OkText
	    {
	        get
	        {
	            return _okBtn.GetComponentInChildren<Text>().text;
	        }
	        set
	        {
	            _okBtn.GetComponentInChildren<Text>().text = value;
	        }
	    }

		protected override void OnClickOutside(GameObject go)
		{
			if (CanClickBGMask)
			{
				base.OnClickOutside(go);
			}
		}

		protected override void OnInit()
	    {
            base.OnInit();

	        _titleText.text = "";
	        _contenText.text = "";
		    CanClickBGMask = true;
	        _okBtn.onClick.AddListener(OnOkBtn);
	    }

        protected virtual void OnOkBtn()
	    {
		    WindowEvent = WindowEvent.Ok;
	        CloseAnimation();
	    }

    }
}
