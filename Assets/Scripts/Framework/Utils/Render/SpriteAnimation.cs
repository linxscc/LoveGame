using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// sprite动画控制器
/// 需image组件，各个sprite拖进Sprites数组里
/// zouyu
/// </summary>
public class SpriteAnimation : MonoBehaviour
{

    public Sprite[] Sprites;

    private Image _image;

    private void CheckInit()
    {
        if (_image == null)
        {
            _image = gameObject.GetComponent<Image>();
        }
    }
    
    public void PlayAndStop(int frame)
    {
        CheckInit();
        if (frame < Sprites.Length)
        {
            _image.sprite = Sprites[frame];
        }
    }
}
