using System;
using Assets.Scripts.Module;
using UnityEngine;

namespace DataModel
{
	/// <summary>
	/// 物品描述
	/// </summary>
	public class DescData
	{

		public int ItemType;
		public string ItemName;
		public string ItemDesc;

		public void ParseData(string data)
		{
			string[] arr = data.Split(',');
			if (arr.Length>1)
			{
				ItemName = arr[1];
				ItemDesc = arr[2];
			}
			else
			{
				Debug.LogError("Error"+data);
			}

		}

		public void ParseSpecial(string data)
		{
			string[] arr = data.Split(',');
			if (arr.Length>1)
			{
				ItemType=Int32.Parse(arr[1]);
				ItemName =arr[2] ;
				ItemDesc = arr[3];
			}
			else
			{
				Debug.LogError("Error"+data);
			}
			
		}
		
	}
	
	
	
	
}