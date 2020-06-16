using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using game.main.Live2d;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityMainViewScreenShot : MonoBehaviour {
    private RawImage _bgImage;
    private Live2dGraphic _live2DGraphic;
    private void Awake()
    {
        _live2DGraphic = transform.Find("CharacterContainer/Live2dGraphic").GetComponent<Live2dGraphic>();

        if ((float)Screen.height / Screen.width > 1.8f)
        {

            _live2DGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        _bgImage = transform.Find("BG").GetComponent<RawImage>();
    }

    public void SetData(int background,int cloth)
    {
        _live2DGraphic.AddDonotUnloadIds(cloth.ToString());
        _live2DGraphic.LoadAnimationById(cloth.ToString());

        BGTexture(GlobalData.FavorabilityMainModel.GetBgImageName(background));
    }
    private void BGTexture(string image)
    {
        _bgImage.texture = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(image));
    }
}
