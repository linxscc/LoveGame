using game.main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameIntroductionWindow : Window
{

    protected override void OnInit()
    {
        base.OnInit();
      //  MaskAlpha = 0.7f;
        MaskColor = new Color(0, 0, 0 ,0.8f);
    }
}
