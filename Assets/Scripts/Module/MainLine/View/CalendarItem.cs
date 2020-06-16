using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.MainLine.View
{
    public class CalendarItem : MonoBehaviour
    {
        public LevelVo LevelVo;
        
        public void SetData(LevelVo levelVo, bool doAnimation)
        {
            LevelVo = levelVo;

            Text text = transform.GetText("Text");
            int index = (levelVo.ChapterGroup - 1) % 4 + 1;

            if (levelVo.LevelType == LevelTypePB.Value)
            {
                text.text = levelVo.LevelMark;
                Transform starContainer = transform.Find("Stars");
                for (int i = 0; i < 3; i++)
                {
                    Image image = starContainer.GetChild(i).GetImage();
                    if (levelVo.CurrentStar > i)
                    {
                        image.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_star");
                    }
                    else
                    {
                        image.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_starEmpty");
                    }
                }
                transform.GetImage("Frame").sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_battleFrame" + index);
            }
            else
            {
                text.text = levelVo.LevelMark;
                transform.GetImage("Frame").sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_storyFrame" + index);
//                transform.GetImage("Icon").sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_storyItem" + index);
            }

            text.color = MainLineView.Colors[index-1];

            if (doAnimation)
            {
                transform.localScale = Vector3.zero;
                transform.DOScale(new Vector3(1, 1, 1), 0.13f).SetDelay(0.3f);
            }
        }
    }
}