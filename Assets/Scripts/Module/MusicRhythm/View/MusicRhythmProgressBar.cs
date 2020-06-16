using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmProgressBar : MonoBehaviour
{
    float width = 0;
    // Start is called before the first frame update
    RectTransform _bar;
    RectTransform _self;
    private void Awake()
    {
        _bar = transform.GetRectTransform("Mask/Bar");
    }
    void Start()
    {
 
        width = transform.GetRectTransform().GetSize().x;
    }

    public void SetPercent(float per)
    {
        _bar.SetSize(new Vector2(width * per, _bar.sizeDelta.y));
    }
}
