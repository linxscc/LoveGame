using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using game.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitLevelPreUnlockCardWindow : Window
{
    Transform _puzzle;
    Transform _props;
    private Image _cardQualityImage;
    private void Awake()
    {
        _puzzle = transform.Find("Puzzle");
        _props = transform.Find("Props");
    }
    public void Init(VisitLevelVo vo)
    {
        SetShowPuzzle(vo.LevelExtra.CardId);
    }

    private void SetShowPuzzle(int cardId)
    {
        var card = GlobalData.CardModel.GetCardBase(cardId);
        _cardQualityImage = transform.Find("Puzzle/CardQualityImage").GetComponent<Image>();
        transform.Find("Puzzle/NameText").GetComponent<Text>().text = card.CardName;
        bool isPuzzle = false;
        transform.Find("Puzzle/PuzzleRawImage").gameObject.SetActive(isPuzzle);
        transform.Find("Puzzle/RawImage").gameObject.SetActive(isPuzzle);
        _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(card.Credit));
        _cardQualityImage.SetNativeSize();

        RawImage cardImage = transform.Find("Puzzle/Mask/CardImage").GetComponent<RawImage>();
        Texture texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/" + cardId, ModuleConfig.MODULE_VISIT);
        if (texture == null)
            texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/1420", ModuleConfig.MODULE_VISIT);
        cardImage.texture = texture;
    }

}
