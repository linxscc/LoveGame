using System;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace game.tools
{
    public class BackgroundSizeFitter : MonoBehaviour
    {
        public FitType FitType = FitType.Background;
        public bool isOffY = true;
        private bool _isDoFit = false;
        
        public bool autoDotFix = true;

        private void Start()
        {
            if(autoDotFix)
                DoFit();
        }

        public void DoFit()
        {
            if(_isDoFit)
                return;
            
            RectTransform rect = transform.GetComponent<RectTransform>();

            float scaleFactor = Main.ScaleFactor;

            switch (FitType)
            {
                case FitType.Background:
                    rect.sizeDelta = new Vector2(Main.StageWidth / scaleFactor, Main.StageHeight / scaleFactor);
                    break;
                case FitType.CustomerWidthHeight:
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x / scaleFactor, rect.sizeDelta.y / scaleFactor);
                    break;
                case FitType.Width:
                    scaleFactor = Main.ScaleX * Main.CanvasScaleFactor;
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x / scaleFactor, rect.sizeDelta.y / scaleFactor);
                    break;
                case FitType.Heigth:
                    scaleFactor = Main.ScaleY * Main.CanvasScaleFactor;
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x / scaleFactor, rect.sizeDelta.y / scaleFactor);
                    break;
                case FitType.WidthScale:
                    scaleFactor = Main.ScaleX * Main.CanvasScaleFactor;
                    scaleFactor = 1 / scaleFactor;
                    rect.localScale = new Vector3(scaleFactor, scaleFactor, 1);
                    break;
                case FitType.HeightScale:
                    scaleFactor = Main.ScaleY * Main.CanvasScaleFactor;
                    scaleFactor = 1 / scaleFactor;
                    rect.localScale = new Vector3(scaleFactor, scaleFactor, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isOffY)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, ModuleManager.OffY / 2);
            }
            else
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);
            }

            _isDoFit = true;
        }

        public void Reset()
        {
            _isDoFit = false;
        }
    }

    public enum FitType
    {
        Background,
        CustomerWidthHeight,
        Width,
        Heigth,
        WidthScale,
        HeightScale
    }
}