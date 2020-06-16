using Assets.Scripts.Framework.GalaSports.Core;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public partial class StoryView
{
    private void CutLeftFadeOutBlack()
    {
        _isFade = true;
        IsWait = true;
        _isDelayDialog = true;
        _mask.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_black");
        _mask.gameObject.Show();
        _mask.DOLocalMoveX(-300, 0.8f).SetEase(Ease.OutQuart).onComplete = () =>
        {
            _mask.GetComponent<Image>().DOFade(0, 0.7f).onComplete = () =>
            {
                _mask.gameObject.Hide();
                IsWait = false;
            };
        };

        ClientTimer.Instance.DelayCall(ShowPageStep2, 0.5f);
    }

    private void ResetMask()
    {
        _mask.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        _mask.anchoredPosition = new Vector2(2200, 0);
    }

    private void FadeOutBlack()
    {
        _isFade = true;
        IsWait = true;
        _isDelayDialog = true;
        _mask.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_black");
        _mask.gameObject.Show();
        _mask.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        
        _mask.anchoredPosition = new Vector2(0, _mask.anchoredPosition.y);

        _mask.GetComponent<Image>().DOFade(1, 1.0f).onComplete = () =>
        {
            _mask.gameObject.Hide();
            IsWait = false;
        };

        ClientTimer.Instance.DelayCall(ShowPageStep2, 0.8f);
    }

    private void Right2LeftAnimation(string spriteName)
    {
        _isFade = true;
        IsWait = true;
        _isDelayDialog = true;
        _mask.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(spriteName);
        _mask.gameObject.Show();
        _mask.DOLocalMoveX(-2200, 2.6f).SetEase(Ease.OutQuart).onComplete = () => IsWait = false;

        ClientTimer.Instance.DelayCall(()=> { IsWait = false; }, 1.5f);
        
        ClientTimer.Instance.DelayCall(ShowPageStep2, 0.5f);
    }

    
}