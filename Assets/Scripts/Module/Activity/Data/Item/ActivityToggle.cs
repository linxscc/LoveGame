using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ActivityToggle : MonoBehaviour
{
    private Text _name;
    private Text _text;
    private Image _nameBg;
    private Image _frameBg;

    private Toggle _toggle;
    
    
    private GameObject _redDot;
   
    private void Awake()
    {
        _name = transform.GetText("Text");
        _text = transform.GetText("BludBg/Checkmark/Frame/Text");
        _nameBg = transform.GetImage("BludBg/Checkmark/NameBg");
        _frameBg = transform.GetImage("BludBg/Checkmark/Frame");
        
       
        _redDot = transform.Find("RedDot").gameObject;


        _toggle = GetComponent<Toggle>();
        
        _toggle.onValueChanged.AddListener(OnClick);
    }

    private void OnClick(bool isOn)
    {
        if (isOn)
        {
            _name.gameObject.SetActive(false);  
        }
    }

    public void SetData(ActivityVo vo)
    {
        
        _name.text = vo.Name;
        _text.text = vo.Name;

  
        _nameBg.sprite =  ResourceManager.Load<Sprite>(vo.ActivityToggleBgPath);
        _frameBg.sprite = ResourceManager.Load<Sprite>(vo.ActivityToggleFramePath);

        
        _nameBg.GetComponent<RectTransform>().sizeDelta =new Vector2(_nameBg.sprite.rect.width,_nameBg.sprite.rect.height);  

        ActivityToggleVO vO = new ActivityToggleVO(vo.ActivityType);
        _redDot.SetActive(vO.IsShowRedDot);    
    }


    
   
  
}
