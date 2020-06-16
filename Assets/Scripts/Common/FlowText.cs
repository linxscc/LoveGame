#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 望尘体育科技
//
// 文件名(File Name):             FlowTextCtrl.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/3/5 17:18:21
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

namespace game.main
{
	public class FlowText : MonoBehaviour {

	    private static FlowText _instance;

	    private Queue<string> _messageQueue = new Queue<string>();

	    private bool _isPlaying = false;
	    
	    void Start ()
		{
		    _instance = this;
            this.gameObject.SetActive(false);
		}

//		public static void ShowMessage(string msg, float duration = 1.5f)
//	    {
//		    		   	   		    
//	        Text msgTxt = _instance.transform.Find("Text").GetComponent<Text>();
//	        Image img = _instance.transform.GetComponent<Image>();
//		    //canvas = instance.transform.Find("Canvas").GetComponent<CanvasGroup>();
//		    Image star1 = _instance.transform.Find("Text/StarImage").GetComponent<Image>();
//		    Image star2=_instance.transform.Find("Text/StarImage2").GetComponent<Image>();
//
//	        msgTxt.color = new Color(msgTxt.color.r, msgTxt.color.g, msgTxt.color.b, 1);
//	        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
//		    star1.color = new Color(star1.color.r, star1.color.g, star1.color.b, 1);
//		    star2.color=new Color(star2.color.r,star2.color.g,star2.color.b,1);
//            msgTxt.text = msg;
//
//	        _instance.transform.DOKill();
//            _instance.gameObject.SetActive(true);
//
//            _instance.transform.localPosition = new Vector3(_instance.transform.localPosition.x, 50);
//	        
//	        Tweener move1 = _instance.transform.DOLocalMoveY(100, 0.3f).SetEase(DG.Tweening.Ease.OutSine);
//	        Tweener move2 = _instance.transform.DOLocalMoveY(200, 0.3f).SetEase(DG.Tweening.Ease.OutSine);
//
//	        Tweener alpha1 = img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 0), 0.3f);
//	        Tweener alpha2 = msgTxt.DOColor(new Color(msgTxt.color.r, msgTxt.color.g, msgTxt.color.b, 0), 0.3f);
//		    Tweener alpha3 = star1.DOFade(0f, 0.3f);
//		    Tweener alpha4 = star2.DOFade(0f, 0.3f);
//
//	        DOTween.Sequence()
//	            .Append(move1)
//	            .AppendInterval(duration)
//	            .Append(move2)
//	            .Join(alpha1)
//	            .Join(alpha2)
//			    .Join(alpha3)
//			    .Join(alpha4);
//	    }


		public static void ShowMessage(string msg,float duration = 0.5f)
		{
			if (_instance._messageQueue.Contains(msg))
			{
				return;
			}
		
			_instance._messageQueue.Enqueue(msg);   //入队
			
			if (!_instance._isPlaying)
			{
				_instance.ShowTextAni(duration);
			}
		}
	    
		

		private void ShowTextAni(float duration = 0.5f)
		{
			_isPlaying = true;
						
			Text msgTxt = _instance.transform.Find("Text").GetComponent<Text>();
			Image img = _instance.transform.GetComponent<Image>();
			//canvas = instance.transform.Find("Canvas").GetComponent<CanvasGroup>();
			Image star1 = _instance.transform.Find("Text/StarImage").GetComponent<Image>();
			Image star2=_instance.transform.Find("Text/StarImage2").GetComponent<Image>();

			msgTxt.color = new Color(msgTxt.color.r, msgTxt.color.g, msgTxt.color.b, 1);
			img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
			star1.color = new Color(star1.color.r, star1.color.g, star1.color.b, 1);
			star2.color=new Color(star2.color.r,star2.color.g,star2.color.b,1);
			
			msgTxt.text = _instance._messageQueue.Dequeue();      //拿到队列首个元素，并且移除出列
			
			_instance.gameObject.SetActive(true);
					
			_instance.transform.localPosition = new Vector3(_instance.transform.localPosition.x, 50);
	        
			Tweener move1 = _instance.transform.DOLocalMoveY(100, 0.3f).SetEase(DG.Tweening.Ease.OutSine);
			Tweener move2 = _instance.transform.DOLocalMoveY(200, 0.3f).SetEase(DG.Tweening.Ease.OutSine);

			Tweener alpha1 = img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 0), 0.3f);
			Tweener alpha2 = msgTxt.DOColor(new Color(msgTxt.color.r, msgTxt.color.g, msgTxt.color.b, 0), 0.3f);
			Tweener alpha3 = star1.DOFade(0f, 0.3f);
			Tweener alpha4 = star2.DOFade(0f, 0.3f);

			var tween = DOTween.Sequence()
				      .Append(move1)
				      .AppendInterval(duration)
				      .Append(move2)
				      .Join(alpha1)
				      .Join(alpha2)
				      .Join(alpha3)
				      .Join(alpha4);
			tween.OnComplete(_instance.TweenOver);//动画完成的回调
		}
		
		private void TweenOver()
		{
			_instance.transform.DOKill();
			_isPlaying = false;
			if (_instance._messageQueue.Count!=0)
			{
				ShowTextAni();
			}
			
		}
	}
	
	

}
