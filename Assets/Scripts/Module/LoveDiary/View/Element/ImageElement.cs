using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ImageElement : DiaryElementBase
{
    float MaxWidth;
    float MinWidth;
    float MaxHeight;
    float MinHeight;
    Vector4 matrix4x1Size;//[MinWidth,MaxWidth,MinHeight,MaxHeight]
 


    protected override void Init()
    {
        _isScale = true;
        _isRotation = true;
        _isMove = true;
        _isDelete = true;
        _isSelectAndUniformScale = true;
        _isOutline = true;
        base.Init();
    }


    protected override void UpdateView()
    {
        _rect.localPosition = new Vector3(pb.XPos, pb.YPos, _rect.localPosition.z);
        _rect.eulerAngles = new Vector3(0, 0, pb.Rotation);
        RawImage rawImage = transform.GetComponent<RawImage>();
        rawImage.texture = ResourceManager.Load<Texture>("LoveDiary/Element/" + pb.ElementId.ToString(), ModuleConfig.MODULE_LOVEDIARY);   //Load没有传第二个参数。
        rawImage.SetNativeSize();

        _originalSize = _rect.GetSize();
        _rect.SetSize(new Vector2(_originalSize.x * pb.ScaleX, _originalSize.y * pb.ScaleY));
    }

}
