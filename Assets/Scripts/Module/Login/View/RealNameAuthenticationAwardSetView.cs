using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine;
using UnityEngine.UI;

public class RealNameAuthenticationAwardSetView : MonoBehaviour
{

   

    private RawImage _icon;
    private Text _desc;

    private string _iconPath;
   
   
    
    private void Awake()
    {
        _icon = transform.GetRawImage("AwardContent/Icon");
        _desc =transform.GetText("AwardContent/Text");

        
        SetIconPath();
        SetAwardInfo();
    }

    private void SetIconPath()
    {
        _iconPath = "Update/AntiAddictionAwardIcon";
    }
    
    
    

    private void SetAwardInfo()
    {
        _icon.texture = ResourceManager.Load<Texture>(_iconPath);
        _desc.text = I18NManager.Get("Antiaddiction_AwardDesc");
    }
}
