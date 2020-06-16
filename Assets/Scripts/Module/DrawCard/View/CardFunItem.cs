using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFunItem : MonoBehaviour
{
    public int Index;
    //private DrawCardResultVo _data;

    private void Awake()
    {
    }
    public void SetData(ShowCardModel vo)
    {
        //_data = vo;
        transform.Find("NameText").GetComponent<Text>().text = vo.CardName;

        RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
        Texture texture = ResourceManager.Load<Texture>(vo.SmallCardFunPath, ModuleConfig.MODULE_DRAWCARD);

        if (texture == null)
        {
            Debug.LogError("don't hava fans texture");
        }
            //texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/1000");
        cardImage.texture = texture;
    }
}

