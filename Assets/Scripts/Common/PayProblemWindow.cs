using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class PayProblemWindow : Window
{

	private Text _curRemainingBalance; //当前余额
	private Button _customerServicesBtn; //联系客服按钮
	
	private void Awake()
	{
		_curRemainingBalance = transform.GetText("RemainingBalance");
		_customerServicesBtn = transform.GetButton("CustomerServicesBtn");
		_customerServicesBtn.onClick.AddListener((() =>
		{
			//调客服窗口
			Debug.LogError("调客服窗口");
			SdkHelper.CustomServiceAgent.Show();
			Close();
		}));
	}
	
	
	public void  SetData()
	{
		_curRemainingBalance.text = "当前余额：" + 0;
	}
}
