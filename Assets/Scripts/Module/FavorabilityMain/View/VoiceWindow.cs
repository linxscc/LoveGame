using System.Collections.Generic;
using game.main;
using Module.FavorabilityMain.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Module.FavorabilityMain.View
{
	public class VoiceWindow : Window
	{
		private Text _name;
		private Transform _parent;
		
		
		
		
		
		private void Awake()
		{
			_name = transform.GetText("Bg/Name");
			_parent = transform.Find("Bg/ScrollRect/Content");
		}


		public void SetData(string name,List<CardAwardPreInfo> list)
		{
			_name.text = name;

			var prefab = GetPrefab("FavorabilityMain/Prefabs/VoiceItem");

			foreach (var t in list)
			{			
				var item = Instantiate(prefab, _parent, false);
				item.GetComponent<VoiceItem>().SetData(t);				
			}
		}

	
		
		
		
		
		public void OnClickClose()
		{
			base.Close();
		}
	}
}
