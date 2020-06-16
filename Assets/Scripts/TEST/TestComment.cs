#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             TestComment.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           #CreateTime#
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace game.main
{
	public class TestComment : MonoBehaviour
	{
		
		private Transform _commentItem;
		private Text _text;
		private RectTransform _bg;

		void Start () {

			_commentItem = transform.Find("CommentItem1");
			_text = transform.Find("CommentItem1/Bg/CommentText").GetComponent<Text>();

			_text.text = "肯定撒打算放过的苟富贵地方萨芬的规范的挂号费的很干净获国家航空打开打算打算大V从吧我打算的克里斯今飞凯达跨世纪的框架房地方撒可减肥咖啡奥斯卡房间打开沙发加快暗室逢灯接口暗示法立刻接口";

			_bg = transform.Find("CommentItem1/Bg").GetComponent<RectTransform>();

			float height = _text.preferredHeight + 150;
			_bg.sizeDelta = new Vector2(_bg.sizeDelta.x, height);

			transform.Find("CommentItem1").GetComponent<LayoutElement>().preferredHeight = height;

		}
		
	}
}
