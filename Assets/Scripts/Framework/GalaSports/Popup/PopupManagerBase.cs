using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;

public class PopupManagerBase : MonoBehaviour
{
    protected GameObject _container;

    protected IPopup _currentPopup;
    public IPopup CurrentPopup
    {
        get { return _currentPopup; }
        set { _currentPopup = value; }
    }

    private static PopupManagerBase _instance;
    public static PopupManagerBase Instance
    {
        get
        {
            return _instance;
        }
    }

    
}
