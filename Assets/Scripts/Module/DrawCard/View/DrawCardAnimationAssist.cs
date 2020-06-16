using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;
public class DrawCardAnimationAssist : MonoBehaviour {

    RawImage _showCard;
    DrawCardResultVo _drawCardResultVo;
    private GameObject _Ui;
    private RawImage _signatureImg;//签名

    GameObject _frontPuzzleObj;//碎片前置遮罩
    Text _cardName;//卡牌名称

    Image _ssr;//卡牌级别
    Image _ssr_light;//卡牌刷光效果


    private void Awake()
    {
        _Ui = transform.parent.Find("UI").gameObject;
        _Ui.Hide();
        _frontPuzzleObj = transform.Find("FrontPuzzleImage").gameObject;
        _cardName = _Ui.transform.GetText("ui_02/CardName");
        _showCard = transform.GetRawImage("role1");
        animator = transform.GetComponent<Animator>();

         _ssr_light = _Ui.transform.GetImage("ui_01_SSR/ui_01_SSR_light");
        _ssr = _Ui.transform.GetImage("ui_01_SSR");

        _isRepeat = false;
    }
    bool _isRepeat;
    public void SetResultVo(DrawCardResultVo vo)
    {
        _Ui.Hide();
        _drawCardResultVo = vo;
        _isAnimationPlaying = true;
        if(_isRepeat)
        {
            animator.SetTrigger("RepeatTrigger");
            AudioManager.Instance.PlayEffect("draw_showcard", 1);
        }
        else
        {
            AudioManager.Instance.PlayEffect("draw_heart", 1);

            _isRepeat = true;
        }
             
    }
    /// <summary>
    /// animation event  在result animation中调用
    /// </summary>
    public void SetShowInfo()
    {
        Debug.Log("SetShowInfo");
        if (_drawCardResultVo == null)
        {
            Debug.LogError("_drawCardResultVo is null");
            return;
        }

        if (_drawCardResultVo.Resource == ResourcePB.Signature)
        {
            _signatureImg = transform.GetRawImage("01/role1/Signature");
            _signatureImg.gameObject.SetActive(true);
        }

        Texture texture = null;
        if (_drawCardResultVo.Resource == ResourcePB.Fans)
        {
            texture = ResourceManager.Load<Texture>(CardUtil.GetBigFunsCardPath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
            _ssr.gameObject.Hide();
            ShowLapiao(false);
        }
        else if (_drawCardResultVo.Resource == ResourcePB.Card ||
             _drawCardResultVo.Resource == ResourcePB.Puzzle ||
             _drawCardResultVo.Resource == ResourcePB.Signature)
        {
            texture = ResourceManager.Load<Texture>(CardUtil.GetBigCardPath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);


            _ssr.gameObject.Show();
            _ssr.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_DrawCard_" + _drawCardResultVo.Credit.ToString());
            _ssr.SetNativeSize();
            _ssr_light.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_DrawCard_" + _drawCardResultVo.Credit.ToString());
            _ssr_light.SetNativeSize();

            if (_signatureImg != null)
            {
                _signatureImg.texture = ResourceManager.Load<Texture>(CardUtil.GetCardSignaturePath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
                _signatureImg.GetComponent<RectTransform>().sizeDelta = new Vector2(_signatureImg.texture.width, _signatureImg.texture.height);
            }
            if (isCanShowLapiao && (_drawCardResultVo.Resource == ResourcePB.Card || _drawCardResultVo.Resource == ResourcePB.Signature))
            {
                ShowLapiao(true);
            }
            else
            {
                ShowLapiao(false);
            }

        }

        _frontPuzzleObj.SetActive(_drawCardResultVo.Resource == ResourcePB.Puzzle);
        _cardName.text = _drawCardResultVo.Resource == ResourcePB.Puzzle ? $"{_drawCardResultVo.Name}({I18NManager.Get("Card_PuzzleTap")})" : _drawCardResultVo.Name;
        _showCard.texture = texture;



        if (_drawCardResultVo.Resource == ResourcePB.Fans)
        {
            _showCard.transform.GetComponent<RectTransform>().SetSize(new Vector2(1080, 1920));
          //  _showCard.GetComponent<BackgroundSizeFitter>().Reset();
          //  _showCard.GetComponent<BackgroundSizeFitter>().DoFit();         

        }
        else
        {
            _showCard.transform.GetComponent<RectTransform>().SetSize(new Vector2(1440, 1920));
           // _showCard.GetComponent<BackgroundSizeFitter>().Reset();
           // _showCard.GetComponent<BackgroundSizeFitter>().DoFit();

        }
    }
    public void ShowLapiao(bool isShow)
    {
        _Ui.transform.Find("ui_02+").gameObject.SetActive(isShow);
    }
    public void AnimationClipEnd()
    {
        Debug.Log("AnimationClipEnd");
    
        _isAnimationPlaying = false;
       // PlayAnimationEffect();
    }

    public void SetUiShow()
    {
        _Ui.Show();
    }

    public bool _isAnimationPlaying;
    public bool isAnimationPlaying
    {
        get
        {
            return _isAnimationPlaying;
        }
    }


    Animator animator;

    public bool isCanShowLapiao;//是否显示拉票  主要是用于开关


    public void PlayAnimationEffect()
    {
        if (_drawCardResultVo.Resource == ResourcePB.Fans)
            return;
        string name = "";
        switch (_drawCardResultVo.Credit)
        {
            case CreditPB.Ssr:
                name = "draw_r";
                if (_drawCardResultVo.Resource == ResourcePB.Card)
                {
                    PlayDialog(_drawCardResultVo.Dialog);
                    JumpToStoreScore();
                }
                break;
            case CreditPB.Sr:
                name = "draw_r";
                break;
            case CreditPB.R:
                name = "draw_r";
                break;
            default:
                return;
        }
        AudioManager.Instance.PlayEffect(name, 1);
        // PlayDialog("cy_draw3");
    }
    private void JumpToStoreScore()
    {
        EventDispatcher.TriggerEvent(EventConst.ShowStoreScore);
    }
    private void PlayDialog(string name)
    {
        Debug.LogError("PlayDialog  " + name);
        new game.main.AssetLoader().LoadAudio(game.main.AssetLoader.GetDrawCardDialogById(name),
            (clip, loader) =>
            {
                AudioManager.Instance.PlayDubbing(clip);
            });
    }

}
