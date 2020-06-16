using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using UnityEngine;

public class FavorabilityNpcInfoController : Controller
{
    public FavorabilityNpcInfoView View;
       
  
    public override void Start()
    {        
        View.SetData(GlobalData.FavorabilityMainModel.GetNpcInfo(GlobalData.FavorabilityMainModel.CurrentRoleVo.Player)); 
    }


   

}
