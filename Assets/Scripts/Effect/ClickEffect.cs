using System;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffect : MonoBehaviour 
{
	private ParticleSystem _ps;

	private void Awake()
	{
		GameObject go = Instantiate(ResourceManager.Load<GameObject>("UIEffect/ClickEffect/ClickEffect"));
		go.transform.SetParent(transform, false);
		go.name = "ParticleSystem";
		go.transform.SetAsLastSibling();
	}

	void Start ()
	{
		_ps = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
		Button btn = GetComponent<Button>();
		if(btn == null)
		{
			throw new Exception("Button Component not found!");
		}
		
		btn.onClick.AddListener(PlayParticleSystem);
	}

	private void PlayParticleSystem()
	{
		_ps.Play();
	}

}
