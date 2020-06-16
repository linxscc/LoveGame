using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;

public class VisitLevelCardItem : VisitLevelItem
{
    public override void SetData(VisitLevelVo vo)
    {
        base.SetData(vo);
        int index = 2;//奖励展示写死
        string path = "Card/Image/MiddleCard/" + vo.Awards[index].ResourceId.ToString();
        Debug.Log("VisitLevelCardItem   " + path);
        RawImage propImage = transform.Find("CardMask/Card").GetComponent<RawImage>();
        Texture tx = ResourceManager.Load<Texture>(path, ModuleConfig.MODULE_VISIT);
        if(tx==null)
        {
            tx = ResourceManager.Load<Texture>("Card/Image/MiddleCard/1420", ModuleConfig.MODULE_VISIT);
        }
        propImage.texture = tx;

        var card = GlobalData.CardModel.GetCardBase(vo.Awards[index].ResourceId);
        var credit = transform.GetImage("Credit");
        credit.sprite =AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(card.Credit));


        //propImage.SetNativeSize();

        if (vo.IsPass)
        {
            ExtraAni();
        }
        
    }
    
    private void ExtraAni()
    {
        var extraRect = transform.GetRectTransform("Extra");
        var tween = extraRect.DORotate(new Vector3(0,0,20),0.5f);
        tween.SetAutoKill(false);
        tween.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }
}
