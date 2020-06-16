using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class DrawCardResultItem : MonoBehaviour
    {
        public int Index;
        private Image _cardQualityImage;
        private DrawCardResultVo _data;
        private void Awake()
        {
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
        }

        public void SetData(DrawCardResultVo vo)
        {
            _data = vo;

            if (vo.IsNew)
            {
                transform.Find("NewImage").gameObject.Show();
            }
            else
            {
                transform.Find("NewImage").gameObject.Hide();
            }
            //todo
            bool isPuzzle;
            isPuzzle = vo.Resource == ResourcePB.Puzzle ? true : false;
            transform.Find("NameText").GetComponent<Text>().text =isPuzzle?$"{vo.Name}({I18NManager.Get("Card_PuzzleTap")})": vo.Name;
            transform.Find("PuzzleRawImage").gameObject.SetActive(isPuzzle);
            transform.Find("RawImage").gameObject.SetActive(isPuzzle);
            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.Credit));
           // _cardQualityImage.SetNativeSize();

            RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            Texture texture = ResourceManager.Load<Texture>(vo.CardPath, ModuleConfig.MODULE_DRAWCARD);

            if (texture == null)
                texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/1000", ModuleConfig.MODULE_DRAWCARD);
            cardImage.texture = texture;
        }
    }
}
