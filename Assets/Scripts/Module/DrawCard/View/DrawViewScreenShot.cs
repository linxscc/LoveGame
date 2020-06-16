using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class DrawViewScreenShot : MonoBehaviour {

    RawImage cardImage;
    Text _name;
    Image _credit;
    private void Awake()
    {
        cardImage = transform.Find("07/ImageCard/RawImage").GetComponent<RawImage>();
        _name = transform.Find("07/name").GetComponent<Text>();
        _credit = transform.Find("07/07-SR").GetComponent<Image>();
    }

    public void SetData(DrawCardResultVo _drawCardResultVo)
    {
        Debug.Log("DrawViewScreenShot "+ _drawCardResultVo.CardId +" name "+ _drawCardResultVo.Name);
        cardImage.texture  = ResourceManager.Load<Texture>(CardUtil.GetBigCardPath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
        _name.text = _drawCardResultVo.Name;

        _credit.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_DrawCard_" + _drawCardResultVo.Credit.ToString());
        //_credit.SetNativeSize();
    }

}
