using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MailWinodw : Window {
	
	private GameObject _noMail;
	
	private Button _aKeyToGetBtn;
	private Button _aKeyToDeleteBtn;
	private Transform _parent;
	
	private void Awake()
	{

		_parent = transform.Find("Bg/UserMailList/Content");
		
		_noMail = transform.Find("Bg/NoMail").gameObject;
		

		_aKeyToGetBtn = transform.GetButton("Bg/AKeyToGetBtn");
		_aKeyToDeleteBtn = transform.GetButton("Bg/AKeyToDeleteBtn");
		
		
		_aKeyToGetBtn.onClick.AddListener(AKeyToGetBtn);
		_aKeyToDeleteBtn.onClick.AddListener(AKeyToDeleteBtn);
	}
	

	
	


	public void SetData(UserMailState state,List<UserMailVO> list)
	{
		IsHaveUserMail(state);
		CreateUserMailItem(list);
	}

	public void IsHaveUserMail(UserMailState state)
	{
	    switch (state)
		{
			case UserMailState.NoMail:
				_noMail.SetActive(true);
				_aKeyToGetBtn.gameObject.SetActive(false);
				_aKeyToDeleteBtn.gameObject.SetActive(false);
				break;
			case UserMailState.HaveAttachment:
			case UserMailState.NoAttachment:
				_noMail.SetActive(false);
				_aKeyToGetBtn.gameObject.SetActive(true);
				_aKeyToDeleteBtn.gameObject.SetActive(true);
				break;	         
		}
	}
	
	private void CreateUserMailItem(List<UserMailVO> list)
	{
		for (int i = _parent.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(_parent.GetChild(i).gameObject);
		}
		var prefab = GetPrefab("Mail/Prefabs/MailItem");
		foreach (var vo in list)
		{
			var item = Instantiate(prefab, _parent, false);
			item.GetComponent<UserMailItem>().SetData(vo);
			item.name = vo.Id.ToString();
		}
	}

	public void DestroyOneMailItem(int id)
	{
		for (int i = _parent.childCount - 1; i >= 0; i--)
		{
			if (id.ToString() == _parent.GetChild(i).gameObject.name)
			{
				DestroyImmediate(_parent.GetChild(i).gameObject);
				break;
			}
			
		}
	}

	
	private void AKeyToGetBtn()
	{
		SendMessage(new Message(MessageConst.CMD_MAIL_A_KEY_TO_GET));
	}
	
	private void AKeyToDeleteBtn()
	{
		SendMessage(new Message(MessageConst.CMD_MAIL_A_KEY_TO_DELETE));
	}
	
	public override void Close()
	{     
		CloseAnimation();
		ModuleManager.Instance.GoBack();
	}
}
