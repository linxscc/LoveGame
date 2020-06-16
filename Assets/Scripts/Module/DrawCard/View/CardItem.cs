using game.tools;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using UnityEngine;
using UnityEngine.UI;
using game.main;

public class CardItem : MonoBehaviour
{

    private Text _cardName;
    //卡牌的
    private Image _cardId;
    private Image _cardLevel;
    private Image _isGet;
    private Text _cardRole;

    private void Awake()
    {
        //_cardId = transform.Find("ShowCardItem/CardId").GetComponent<Image>();
        //_cardName = transform.Find("ShowCardItem/CardName").GetComponent<Text>();
        //_cardLevel = transform.Find("ShowCardItem/CardLevel").GetComponent<Image>();
        //_isGet = transform.Find("ShowCardItem/IsGet").GetComponent<Image>();
        //_cardRole= transform.Find("ShowCardItem/CardRole").GetComponent<Text>();

    }

    public void SetData(ShowCardModel data)
    {
        //Debug.Log("CardId  " + data.CardId + "  CardName " + data.CardName + "  Credit " + data.Credit + "  IsNew  " + data.IsNew);
        transform.Find("NameText").GetComponent<Text>().text  = CardVo.SpliceCardName(data.CardName, data.Player); 
        transform.Find("PuzzleImg").gameObject.SetActive(data.Resource == SortResouce.Puzzle);
        
        transform.Find("CardQualityImage").GetComponent<Image>().sprite
             = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(data.Credit));
        transform.Find("CardQualityImage").GetComponent<Image>().SetNativeSize();

        RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
        Texture texture = ResourceManager.Load<Texture>(data.MiddleCardPath);

        if (texture == null)
        {
    
            texture = ResourceManager.Load<Texture>("Card/Image/MiddleCard/1000",ModuleConfig.MODULE_CARD);
            Debug.LogError("no find texture   CardId  " + data.CardId + " Resource " + data.Resource + "  CardName " + data.CardName + "  Credit " + data.Credit + "  IsNew  " + data.IsNew);
        }
        cardImage.texture = texture;
       // Debug.LogError("no find texture   CardId  " + data.CardId + " Resource " + data.Resource + "  CardName " + data.CardName + "  Credit " + data.Credit + "  IsNew  " + data.IsNew);
        transform.Find("GetImg").gameObject.SetActive(data.IsNew);
        

    }
}
