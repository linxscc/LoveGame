using System;
using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.main
{

	/// <summary>
	/// 用户约会数据
	/// </summary>
	public class UserAppointmentVo
	{
		public int UserId;
		public int AppointmentId;
		public int ActiveState;
		public List<int> ActiveGateInfos;
		public List<int> FinishGateInfos;
//		public int ClearState;
//		public int NailUpState;
		public long CreateTime;
		public long LastModifyTime;

		public UserAppointmentVo(UserAppointmentPB res)
		{
			UserId = res.UserId;
			AppointmentId = res.AppointmentId;	
			ActiveState = res.ActiveState;
			ActiveGateInfos=new List<int>();
			FinishGateInfos=new List<int>();
			foreach (var v in res.ActiveGates)
			{
				ActiveGateInfos.Add(v);
			}

			foreach (var v in res.FinishGates)
			{
				FinishGateInfos.Add(v);
			}
			
//			ClearState = res.ClearState;
//			NailUpState = res.NailUpState;
			CreateTime = res.CreateTime;
			LastModifyTime = res.LastModifyTime;
		}

	}

	public enum GateState
	{
		NotAcive,
		Active,
		Finish,
	}

    
}