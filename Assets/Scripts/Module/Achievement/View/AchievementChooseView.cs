using System;
using System.Collections.Generic;

using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;

namespace game.main
{
	/// <summary>
	/// 好感度选择角色View
	/// </summary>
	public class AchievementChooseView : View
	{
		private Transform _btnGroup;
    
		private void Awake()
		{
			_btnGroup = transform.Find("BtnGroup");
		}

//	public void SetView()
//	{

//	}

		public void SetData(MissionModel missionModel)
		{
			for (int i = 0; i < _btnGroup.childCount; i++)
			{
				Transform roleStory = _btnGroup.GetChild(i);
				Text nameText = roleStory.Find("NumBottom/Text").GetComponent<Text>();
				var roleId = (PlayerPB)(i);
				//var vo = missionModel.GetPlayerName(roleId);
				//RedPoint
				roleStory.Find("RedPoint").gameObject.SetActive(missionModel.HasReceiveChievement(roleId));
				
				if (missionModel.StarCourseSchedule.ContainsKey(roleId))
				{
					nameText.text = I18NManager.Get("Achievement_LongKM",
						missionModel.StarCourseSchedule[roleId]
							.Progress); //"里程:"+missionModel.StarCourseSchedule[roleId].Progress+"km";				
				}
				else
				{
					nameText.text = I18NManager.Get("Achievement_LongKM",0);
				}

				var role = i;
				PointerClickListener.Get(_btnGroup.GetChild(i).gameObject).onClick = go =>
				{
					SendMessage(new Message(MessageConst.CMD_CHOOSEROLE, role ));
				};
			} 	
		
		
		}




	}

}


