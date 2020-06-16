using Framework.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotRender : MonoBehaviour {

    Camera _camera;
    private void Awake()
    {
        _camera=GetComponent<Camera>();
    }


    public void DestorySelf()
    {
        DestroyImmediate(gameObject);
    }
}
