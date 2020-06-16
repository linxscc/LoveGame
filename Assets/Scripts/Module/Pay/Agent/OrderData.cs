using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[Serializable]
public class OrderData
{
	public enum CheckStatus
	{
		ON_CHECK,
		NORMAL
	}
	
	public string userId;

	public string serverId;

	public PayAgent.PayType payType;

//	public int type;

	public DateTime CreatOrderTime;

	public string orderId;

	public ProductVo productVo;
}