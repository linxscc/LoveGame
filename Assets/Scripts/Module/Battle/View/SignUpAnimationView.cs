//using System.Collections.Generic;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Framework.GalaSports.Service;
//using DataModel;
//using game.tools;
//using Module.Supporter.Data;
//using UnityEngine;
//using UnityEngine.UI;
//
//public class SignUpAnimationView : View 
//{
//	private Transform _signUpItems;
//
//	private string[] _timeText = new[] {"刚刚", "1小时前", "昨天", "3小时前"};
//
//	private string[] _contents = new[]
//	{
//		"听闻你们应援团很久啦！想报名试试看，希望可以被选取！",
//		"是哥哥的资深粉丝，希望可以加入应援团贡献自己的一份力量！",
//		"选我选我选我！我是真爱粉！",
//		"我可以提供资源和资金！只求加入应援团！",
//		"加入应援团一直是我的梦想，特别是加入这么优秀的应援团。",
//		"希望应援团的哥哥姐姐们可以看我一眼，我是真心想加入应援团的。",
//		"听说应援团招新啦，我终于等到招新的这一天了！想加入很久了，求看我！",
//		"我是资深粉！什么问题都知道，随意考我只求一入应援团！",
//		"刀山火海都不怕，但求一入应援团！",
//		"想加入应援团！用我的真爱来为哥哥们应援！希望应援团可以批准我的加入！加入之后一定全心全意服从管理指哪走哪绝不二心！"
//	};
//
//	private void Awake()
//	{
//		ClientTimer.Instance.DelayCall(() =>
//		{
//			PointerClickListener.Get(gameObject).onClick = go =>
//			{
//				SendMessage(new Message(MessageConst.CMD_BATTLE_NEXT));
//			};
//		},2);
//		
//		_signUpItems = transform.Find("SignUpItems");
//
//		List<FansVo> fansList = GlobalData.DepartmentData.Fanss;
//		for (int i = 0; i < 10; i++)
//		{
//			Transform item = _signUpItems.GetChild(i);
//			item.Find("Text").GetComponent<Text>().text = _contents[i];
//			item.Find("Mask/RawImage").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>("FansTexture/Head/"+fansList[i].FansId, ModuleName);
//			item.Find("NameText").GetComponent<Text>().text = fansList[i].Name;
//			item.Find("TimeText").GetComponent<Text>().text = _timeText[Random.Range(0,_timeText.Length)];
//		}
//	}
//	
//	
//}
//
//
