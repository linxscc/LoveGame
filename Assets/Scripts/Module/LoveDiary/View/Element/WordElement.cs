using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordElement : DiaryElementBase
{
    float MaxWidth;
    float MinWidth;
    float MaxHeight;
    float MinHeight;

    protected override void Init()
    {
        _isDelete = true;
        _isEdit = false;
        _isRotation = true;
        _isScale = true;
        _isMove = true;
        _isSelectAndUniformScale = false;
        _isOutline = true;
        _isNeedtoHideInput = true;
        base.Init();
    }
    protected override void UpdateView()
    {
        _rect.localPosition = new Vector3(pb.XPos, pb.YPos, _rect.localPosition.z);
        _rect.eulerAngles = new Vector3(0, 0, pb.Rotation);
        transform.GetComponent<Text>().text = pb.Content;
        if (pb.Size<20)
        {
            transform.GetComponent<Text>().fontSize = 50;  
        }
        else
        {
            transform.GetComponent<Text>().fontSize = pb.Size; 
        }

        _originalSize = _rect.GetSize();
        Debug.LogError(_originalSize.x * pb.ScaleX+" "+_originalSize.y * pb.ScaleY);
//        Debug.LogError(pb.ScaleX+" "+pb.ScaleY);
        _rect.SetSize(new Vector2(_originalSize.x * pb.ScaleX, _originalSize.y * pb.ScaleY));
        //_rect.SetSize(new Vector2(500, 500));
        //_rect.SetSize(new Vector2(pb.ScaleX, pb.ScaleY));//用ScaleX和ScaleY作为长度存储
    }

    protected override void OnScaleDrag(PointerEventData eventData)
    {
        base.OnScaleDrag(eventData);
    }

    public void SetText(string str)
    {
        pb.Content = str;
        transform.GetComponent<Text>().text = pb.Content;
//        Debug.LogError("pb.Size"+pb.Size);
        if (pb.Size<20)
        {
            transform.GetComponent<Text>().fontSize = 50; 
        }
        else
        {
            transform.GetComponent<Text>().fontSize = pb.Size; 
        }

    }
    public string GetText()
    {
        //需要提出颜色

        return pb.Content;
    }
    
    public void OnSizeAdd(int size)
    {
        var localsize=transform.GetComponent<Text>().fontSize;
        if (localsize<200)
        {
            transform.GetComponent<Text>().fontSize+= size;
            pb.Size = transform.GetComponent<Text>().fontSize;
        }
    }
    
    public void OnSizeReduce(int size)
    {
        var localsize=transform.GetComponent<Text>().fontSize;
        if (localsize>50)
        {
            transform.GetComponent<Text>().fontSize-= size;
            pb.Size = transform.GetComponent<Text>().fontSize;
        }
    }

    public void SetColor(Color color)
    {
        transform.GetComponent<Text>().color = color;
    }
    
}
