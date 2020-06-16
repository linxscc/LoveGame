using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module;
using uTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Com.Proto;
using DataModel;
using game.main;

public class ActivityPopupWindowDataItem : MonoBehaviour,IPointerClickHandler
{


	private RawImage _image;
	private ActivityPopupWindowData _data;
	private Text _text;
	private PopupWindow _window;
	private void Awake()
	{
		_image = GetComponent<RawImage>();
		_text = transform.GetText("Text");
		_window = transform.parent.parent.GetComponent<PopupWindow>();
	}


	public void SetData(ActivityPopupWindowData data)
	{		
        _image.texture = ResourceManager.Load<Texture>(data.ImgPath);	
		_data = data;
	}
	
	
	
	
	
	
	public void OnPointerClick(PointerEventData eventData)
	{

		if (!_data.IsCanJumpTo)
		{
			FlowText.ShowMessage(_data.PromptDesc);
			return;
		}
		else
		{
			switch (_data.PopupType)
			{
			   case	"CapsuleTemplate":
			   case "DrawTemplate":
			   case "FirstRecharge":
			   case "MusicTemplate":
			   case "DrawCard":  
				   ModuleManager.Instance.EnterModule(_data.ModuleName, false, true);
				   break;
			   case "SevenSigninTemplate":							 			  
			   case "SevenSignin":			
			   case "MonthCard":			
			   case "GrowthFund":
				   ModuleManager.Instance.EnterModule(_data.ModuleName, false, true,_data.ActivityJumpId);	
				   break;
			   case "StarActivity":
				   ModuleManager.Instance.EnterModule(_data.ModuleName, false, true, GlobalData.MissionModel.GetOpenDay());
				   break;
			}
		}


		_window.Close();
	}
}
