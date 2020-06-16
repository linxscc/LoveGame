using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Effect;
using game.main;
using UnityEngine;

public partial class StoryView
{
    private BackgroundBlurEffect _bgBlurEffect;

    private void AwakeEffect()
    {
        _isFade = true;
        IsWait = true;
        _isDelayDialog = true;

        _bgBlurEffect = EffectManager.CreateImageBlurEffect();

        _bgBlurEffect.StartRecord(ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(_currentDialogVo.BgImageId), ModuleName), tex =>
        {
            _bgImage.texture = tex;
        });
        
        _bgBlurEffect.SetAnimation("Awake",() =>
        {
            ClientTimer.Instance.DelayCall(() =>
            {
                EffectManager.DestroyBackgroundEffect();

//                _bgBlurEffect.gameObject.SetActive(false);
                _bgImage.texture = _storyLoader.BgImageCache[_currentDialogVo.BgImageId];
                ShowPageStep2();
            }, 0.005f);
        });
        
        ClientTimer.Instance.DelayCall(()=> { IsWait = false; }, 5.5f);
    }

    private void ToBlurEffect()
    {
        _isFade = true;
        IsWait = true;
        _isDelayDialog = true;

        _bgBlurEffect = EffectManager.CreateImageBlurEffect();

        _bgBlurEffect.StartRecord(ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(_currentDialogVo.BgImageId), ModuleName), tex =>
        {
            _bgImage.texture = tex;
        });
        
        _bgBlurEffect.SetAnimation("ToBlur",OnBlurEnd);
        
        ClientTimer.Instance.DelayCall(()=> { IsWait = false; }, 5.5f);
    }

    private void OnBlurEnd()
    {
        _dialogFrame.gameObject.SetActive(false);
        _bgBlurEffect.RecordCurrent();
        //新手引导专用
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CREATE_USER, false, false, true);
    }
}