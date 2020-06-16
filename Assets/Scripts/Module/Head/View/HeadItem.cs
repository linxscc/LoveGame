using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using UnityEngine;
using UnityEngine.UI;


public class HeadItem : MonoBehaviour
{
  private Button _onClickBtn;
  private RawImage _head;
  private GameObject _bottom;
  private HeadVo _data;
  

  private void Awake()
  {
    _onClickBtn = transform.GetButton();
    _head = transform.GetRawImage("Head");
    _bottom = transform.Find("Bottom").gameObject;       
    _onClickBtn.onClick.AddListener(OnClickHead);
  }

  private void OnClickHead()
  {   
    EventDispatcher.TriggerEvent(EventConst.OnClickHead,_data);
  }
  
  public void SetData(HeadVo vo)
  {
      _data = vo;
      _head.texture = ResourceManager.Load<Texture>(vo.Path);
  
      
  }
}
 