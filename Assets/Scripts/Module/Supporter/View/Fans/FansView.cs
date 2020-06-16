using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Supporter;
using Module.Supporter.Data;
using UnityEngine;

namespace game.main
{
	public class FansView : View 
	{
		private Transform _content;

		private void Awake()
		{
		
		}

		public void SetData(List<FansVo> list)
		{
			_content = transform.Find("FansBg/List/Content");
			if (_content.childCount > 0)
			{
				for (int i = 0; i < _content.childCount; i++)
				{
					Destroy(_content.GetChild(i).gameObject);
				}
			}

			for (int i = 0; i < list.Count/2; i++)
			{		
//			if (i * 2 < list.Count)
//			{
//				if (list[i * 2].Num <= 0)
//				{
//					Debug.LogError(list[i * 2].Name);
//					continue;
//				}
//			}

				GameObject item = InstantiatePrefab("Supporter/Prefabs/FansItem");
//			GameObject item = Instantiate(InstantiatePrefab("Supporter/Prefabs/FansItem"));
				item.transform.SetParent(_content, false);
				FansVo vo2 = null;
				if ( i*2 + 1 < list.Count)
				{
					vo2 = list[i*2+1];
				}
			
				item.GetComponent<FansItem>().SetData(list[i*2], vo2);
			}
		
		
		}

	
	}

}




