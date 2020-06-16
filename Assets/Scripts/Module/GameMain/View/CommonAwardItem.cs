using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Module;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

public class CommonAwardItem : MonoBehaviour {

	private Text _numTxt;
	private Text _nameTxt;
	private Frame _bigFrame;


	private void Awake()
	{
		_numTxt = transform.GetText("Num");
		_nameTxt = transform.GetText("Name");
		_bigFrame = transform.Find("BigFrame").GetComponent<Frame>();
	}

	public void SetData(RewardVo vo,string module=null, bool unloadLater=false)
	{
		_numTxt.text = "x"+vo.Num;
		_nameTxt.text = vo.Name;			
		_bigFrame.SetData(vo,module,unloadLater);
		
	}
}
