using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Module;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//public abstract class VisitLevelItem : MonoBehaviour, IPointerClickHandler
public abstract class VisitLevelItem : MonoBehaviour
{
    VisitLevelVo _visitLevelVo;
    private void Awake()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => {
            if(_visitLevelVo.IsCanPass)
            {
                OnPointerClick111(null);
            }
        });

        transform.Find("Extra").GetComponent<Button>().onClick.AddListener(() =>
        {
            OnExtraPointerClick(_visitLevelVo);
            //if (_visitLevelVo.IsPass)
            //{
            //    OnExtraPointerClick(null);
            //}
        });

       

    }


  

    public virtual void SetData(VisitLevelVo visitLevelVo)
    {
        Debug.Log("VisitLevelItem");
        _visitLevelVo = visitLevelVo;
        //transform.Find("Text").GetComponent<Text>().text = visitLevelVo.ChapterGroup + "-" + (visitLevelVo.LevelId % 100).ToString();
        //transform.Find("Lock").gameObject.SetActive(!visitLevelVo.IsCanPass);


        //bool isShowExtra = false;
        //if (visitLevelVo.levelFirstPassPB != null )
        //{
        //    if(!visitLevelVo.IsPass)
        //    {
        //        isShowExtra = true;
        //    }
        //    else if(visitLevelVo.IsPass&&!visitLevelVo.MyVisitLevel.IsGetFirst)
        //    {
        //        isShowExtra = true;
        //    }         
        //}
        //transform.Find("Extra").gameObject.SetActive(isShowExtra);
        UpdateItem();
    }

    public void UpdateItem()
    {
        transform.Find("Text").GetComponent<Text>().text = _visitLevelVo.ChapterGroup + "-" + (_visitLevelVo.LevelId % 100).ToString();
        transform.Find("Lock").gameObject.SetActive(!_visitLevelVo.IsCanPass);
        bool isShowExtra = false;
        if (_visitLevelVo.levelFirstPassPB != null)
        {
            if (!_visitLevelVo.IsPass)
            {
                isShowExtra = true;
            }
            else if (_visitLevelVo.IsPass && !_visitLevelVo.MyVisitLevel.IsGetFirst)
            {
                isShowExtra = true;
            }
        }
        transform.Find("Extra").gameObject.SetActive(isShowExtra);
    }

    public void OnPointerClick111(PointerEventData eventData)
    {
        if (!_visitLevelVo.IsCanPass)
        {
            return;
        }
        Debug.Log("OnPointerClick");
        EventDispatcher.TriggerEvent<int>(EventConst.VisitLevelItemClick, _visitLevelVo.LevelId);
    }
    public void OnExtraPointerClick(VisitLevelVo vo)
    {

        Debug.Log("OnExtraPointerClick");
        EventDispatcher.TriggerEvent<VisitLevelVo>(EventConst.VisitLevelItemExtraClick, vo);
    }
}
