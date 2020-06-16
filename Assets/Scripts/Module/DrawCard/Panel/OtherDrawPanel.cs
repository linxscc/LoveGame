using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;

public class OtherDrawPanel : Panel
{
    OtherDrawController _control;

    bool _isHasDrawData = false;
    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<DrawView2>("DrawCard/Prefabs/DrawView2");
        _control = new OtherDrawController();
        RegisterController(_control);
        _control.View = (DrawView2)viewScript;
        if (!_isHasDrawData)
        {
            RegisterModel<DrawData>();
        }
      
        _control.Start(); 
    }

    public override void Show(float delay)
    {
        base.Show(delay);
       // HideBackBtn();
    }
    
    public void SetData(object[] _paramsObjects ,bool isHasDrawData = false)
    {
        _isHasDrawData = isHasDrawData;
        _control.SetData(_paramsObjects);
    }

    

    //public void IsShowBottomBg(bool isShow)
    //{
    //   //_control.View.IsShowBottomBg(isShow); 
    //}
}
