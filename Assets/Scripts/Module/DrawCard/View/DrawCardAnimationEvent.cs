using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class DrawCardAnimationEvent : MonoBehaviour
{

    private Image _credit;
    //  private Image _cardImg;
    private Text _name;
    private RawImage _cardImg;
    private RawImage _signatureImg;//签名
    private Transform _cardShadowImg;

    private Transform _cardFrontImg;
    private DrawCardResultVo _drawCardResultVo;

    public CURANIMALSTATE CurDrawCardState;

    private Animator _animator;


    public bool isCanShowLapiao;

    public enum CURANIMALSTATE
    {
        NONE,
        PRESS,
        RESULATING,
        RESULATED,
        REPEATING,
        REPEATED,
    }

    private void Awake()
    {
        CurDrawCardState = CURANIMALSTATE.PRESS;
        _credit = transform.Find("07-SR").GetComponent<Image>();
        //_cardImg = transform.Find("Image").GetComponent<Image>();
        _name = transform.Find("name").GetComponent<Text>();
        _cardImg = transform.Find("ImageCard/RawImage").GetComponent<RawImage>();
        
        _cardFrontImg = transform.Find("ImageCard/FrontImage");
        _cardShadowImg = transform.Find("ShadowImage");
        _cardImg.gameObject.SetActive(false);
        _animator = GetComponent<Animator>();
        isCanShowLapiao = false;
    }

    public void SetResultVo(DrawCardResultVo vo)
    {

        AudioManager.Instance.StopDubbing();
        //Debug.LogError("SetResultVo " + CurDrawCardState);
        if (CurDrawCardState == CURANIMALSTATE.PRESS)
        {
            CurDrawCardState = CURANIMALSTATE.RESULATING;

            _animator.SetBool("IsResult", true);
        }
        else if (CurDrawCardState == CURANIMALSTATE.RESULATED)
        {
            CurDrawCardState = CURANIMALSTATE.REPEATING;
            _animator.SetBool("IsRepeat", true);
        }
        else if (CurDrawCardState == CURANIMALSTATE.REPEATED)
        {
            CurDrawCardState = CURANIMALSTATE.REPEATING;
            //  _animator.SetBool("IsRepeat", true);
            _animator.SetBool("IsEndRepeat", false);
        }
        else
        {
            return;
        }

        ParticleSystemRenderer ps = transform.Find("Particle System/SR").GetComponent<ParticleSystemRenderer>();
        var resMat = ResourceManager.Load<Material>(vo.GetShowMatPath(), ModuleConfig.MODULE_DRAWCARD);
        ps.material = resMat;

        _drawCardResultVo = vo;

    }

    public void SetEndResult()
    {
        //Debug.LogError("SetEndResult " + CurDrawCardState);

        if (CurDrawCardState == CURANIMALSTATE.PRESS)
        {
        }
        else if (CurDrawCardState == CURANIMALSTATE.RESULATING)
        {
            CurDrawCardState = CURANIMALSTATE.RESULATED;
        }
        else if (CurDrawCardState == CURANIMALSTATE.REPEATING)
        {
            _animator.SetBool("IsEndRepeat", true);
            CurDrawCardState = CURANIMALSTATE.REPEATED;

        }
        else
        {
            return;
        }
    }


    public void ShowLapiao(bool isShow)
    {
     //   isShow = false;//临时去掉
        transform.Find("07-lapiao").gameObject.GetComponent<Image>().enabled = isShow;
        transform.Find("07-lapiao/Button").gameObject.SetActive(isShow);
        transform.Find("07-suikuai").gameObject.SetActive(isShow);
        transform.Find("07-suikuai2").gameObject.SetActive(isShow);

    }

    public void SetShowInfo()
    {
        Transform chouka = transform.parent.Find("chouka");
        if(chouka != null)
        {
            chouka.gameObject.SetActive(false);
        }

        if (_drawCardResultVo.Resource == ResourcePB.Signature )
        {
            _signatureImg = transform.GetRawImage("ImageCard/RawImage/Signature");
            _signatureImg.gameObject.SetActive(true);
        }
        
        Texture texture = null;
        string markRes = "";
        if (_drawCardResultVo.Resource == ResourcePB.Fans)
        {
            _credit.gameObject.SetActive(false);
            _cardShadowImg.gameObject.SetActive(false);
            markRes = "UIAtlas_DrawCard2_Star";

            texture = ResourceManager.Load<Texture>(CardUtil.GetBigFunsCardPath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
            ShowLapiao(false);

        }
        else if (_drawCardResultVo.Resource == ResourcePB.Card ||
            _drawCardResultVo.Resource == ResourcePB.Puzzle ||
            _drawCardResultVo.Resource == ResourcePB.Signature)            
        {
            _credit.gameObject.SetActive(true);
            _cardShadowImg.gameObject.SetActive(true);
            markRes = _drawCardResultVo.Resource == ResourcePB.Card ? "UIAtlas_DrawCard2_Star" : "UIAtlas_DrawCard2_Puzzle";
            texture = ResourceManager.Load<Texture>(CardUtil.GetBigCardPath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
  
            _credit.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_DrawCard2_07-" + _drawCardResultVo.Credit.ToString());
            _credit.SetNativeSize();
            if (_signatureImg!=null)
            {
                _signatureImg.texture = ResourceManager.Load<Texture>(CardUtil.GetCardSignaturePath(_drawCardResultVo.CardId), ModuleConfig.MODULE_DRAWCARD);
                _signatureImg.GetComponent<RectTransform>().sizeDelta =new Vector2(_signatureImg.texture.width,_signatureImg.texture.height);
            }
      
            if (isCanShowLapiao&&( _drawCardResultVo.Resource == ResourcePB.Card|| _drawCardResultVo.Resource == ResourcePB.Signature))
            {
                ShowLapiao(true);
            }
        }       
        else
        {
            return;
        }

        transform.Find("suipian").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(markRes);
        transform.Find("suipian/Image (1)").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(markRes);
        _cardFrontImg.gameObject.SetActive(_drawCardResultVo.Resource == ResourcePB.Puzzle ? true : false);
        
        _name.text = _drawCardResultVo.Resource == ResourcePB.Puzzle ?$"{_drawCardResultVo.Name}({I18NManager.Get("Card_PuzzleTap")})":_drawCardResultVo.Name;
        if (texture == null)
        {
            Debug.LogError("SetShowInfo sprite is null");
            _cardImg.texture = ResourceManager.Load<Texture>("Card/Image/1000", ModuleConfig.MODULE_DRAWCARD);
            _cardImg.gameObject.SetActive(true);
            return;
        }

        _cardImg.texture = texture;
        if (_drawCardResultVo.Resource == ResourcePB.Fans)
        {
            transform.Find("ImageCard").GetComponent<Canvas>().sortingOrder = 786;
            _cardImg.transform.GetComponent<RectTransform>().SetSize(new Vector2(1080, 1920));
        }
        else
        {
            transform.Find("ImageCard").GetComponent<Canvas>().sortingOrder = 789;
            _cardImg.transform.GetComponent<RectTransform>().SetSize(new Vector2(1440, 1920));
        }
        _cardImg.gameObject.SetActive(true);
        //暂时放这里
        //  AudioManager.Instance.PlayEffect("draw_ssr", 1);
    }

    public void PlayAnimationEffect()
    {
        if (_drawCardResultVo.Resource == ResourcePB.Fans)
            return;
        string name = "";
        switch (_drawCardResultVo.Credit)
        {
            case CreditPB.Ssr:
                name = "draw_ssr";
                if(_drawCardResultVo.Resource == ResourcePB.Card)
                {
                    PlayDialog(_drawCardResultVo.Dialog);
                    JumpToStoreScore();
                }
                break;
            case CreditPB.Sr:
                name = "draw_sr";
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

    private void PlayDialog(string name)
    {
        Debug.LogError("PlayDialog  " + name);
        new game.main.AssetLoader().LoadAudio(game.main.AssetLoader.GetDrawCardDialogById(name),
            (clip, loader) =>    
             {
                 AudioManager.Instance.PlayDubbing(clip);
             });
    }

    private void JumpToStoreScore()
    {
        EventDispatcher.TriggerEvent(EventConst.ShowStoreScore);
    }
    
    public void PlayAnimationStarEffect()
    {
        AudioManager.Instance.PlayEffect("draw_star_comeout", 1);
    }
}
