
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto.Server;
using Google.Protobuf.Collections;
using UnityEngine;


namespace DataModel
{
	public class NoticeData: Model
	{

		private List<NoticePB> _notices;


		public void InitData(RepeatedField<NoticePB> pbs)
		{
			if (_notices==null)
			{
				_notices =new List<NoticePB>();
			}

			_notices = pbs.ToList();				
		}

		public NoticePB GetNoticeInfo()
		{
			NoticePB pb = null;
			if (IsStopService())      //停服
			{
				foreach (var t in _notices)
				{
					if (t.Type == (int) NoticeType.SHOP_SERVICE && t.Use == 1)
					{
						pb = t;
						break;
					}
				}
			}
			else                      //没停服
			{
				foreach (var t in _notices)
				{
					if (t.Type == (int) NoticeType.NORMAL && t.Use == 1)
					{
						pb = t;
						break;
					}
				}
			}

			return pb;
		}
		
		public NoticePB GetForceUpdateNotice()
		{
			foreach (var t in _notices)
			{
				if (t.Type == (int) NoticeType.FORCE_UPDATE && t.Use == 1)
				{
					return t;
				}
			}
			return null;
		}
		
		public NoticePB GetHotfixNotice()
		{
			foreach (var t in _notices)
			{
				if (t.Type == (int) NoticeType.HOT_UPDATE && t.Use == 1)
				{
					return t;
				}
			}
			return new NoticePB();
		}
		 


		/// <summary>
		/// 是否停服
		/// ture为停服，false为正常
		/// </summary>
		/// <returns></returns>
		public bool IsStopService()
		{
			bool temp = false;
			foreach (var t in _notices)
			{
				if (t.Type==0 && t.Use == 1)
				{
					temp = true;
					break;
				}
			}

			return temp;
		}
		
	}
}


