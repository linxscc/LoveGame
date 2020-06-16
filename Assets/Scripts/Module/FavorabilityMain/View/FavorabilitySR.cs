using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavorabilitySR : MonoBehaviour,IDragHandler//,IEndDragHandler
{


	private ScrollRect _scrollRect;	
	private Transform _bottomBG;
	private FavorabilityGiftView _view;
	
	
	private void Awake()
	{
		_scrollRect = GetComponent<ScrollRect>();
		_bottomBG = transform.parent;		
	}


	private void Start()
	{
		_view = _bottomBG.parent.gameObject.GetComponent<FavorabilityGiftView>();		
	}

	public void OnDrag(PointerEventData eventData)
	{
		_scrollRect.OnDrag(eventData);	 
		_view.HideHint();
	}



}
