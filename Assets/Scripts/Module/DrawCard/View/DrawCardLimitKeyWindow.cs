using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class DrawCardLimitKeyWindow : Window
{
    private NumBarWidget _numWidget;


    private void Awake()
    {
        _numWidget = transform.Find("NumBarWidget").GetComponent<NumBarWidget>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _numWidget.Init(0, 200, 100);
    }

}
