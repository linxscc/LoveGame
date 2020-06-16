using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class Frame : MonoBehaviour
{


	private RawImage _icon;
	private RawImage _puzzleIcon;
	private Transform _puzzleTra;

	
	
	private void Awake()
	{
		
		_puzzleTra = transform.Find("Mask/Puzzle");
		_icon = transform.Find("Mask/Icon").GetComponent<RawImage>();
		_puzzleIcon = transform.Find("Mask/Puzzle/Icon").GetComponent<RawImage>();
	}

	

	public void SetData(RewardVo vo, string module = null, bool unloadLater = false)
	{

		transform.Find("Mask/Icon").GetRawImage().material = ResourceManager.Load<Material>("Prefabs/CommonPrefabs/BigFrameMask");
		transform.Find("Mask/Puzzle").GetImage().material = ResourceManager.Load<Material>("Prefabs/CommonPrefabs/PuzzleBottomMask");
		transform.Find("Mask/Puzzle/Icon").GetRawImage().material = ResourceManager.Load<Material>("Prefabs/CommonPrefabs/PuzzleMask");
		
		var isPuzzle = vo.Resource == ResourcePB.Puzzle;
		

		if (isPuzzle)
		{
			_icon.gameObject.Hide();
			_puzzleTra.gameObject.Show();
			_puzzleIcon.texture =ResourceManager.Load<Texture>(vo.IconPath, module, unloadLater);
		}
		else
		{
			_icon.gameObject.Show();
			_puzzleTra.gameObject.Hide();
			_icon.texture =ResourceManager.Load<Texture>(vo.IconPath, module, unloadLater);
		}
		
		PointerClickListener.Get(gameObject).onClick = go =>
		{
			var desc = ClientData.GetItemDescById(vo.Id,vo.Resource);
			if (desc!=null)
			{
				FlowText.ShowMessage(desc.ItemDesc); 			
			}
		};
	}
	
}
