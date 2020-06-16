using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyText : MonoBehaviour
{

    public static void ShowFlyText(Transform tf,
        float duration = 1.5f)
    {
        Transform idle = tf;
        idle.gameObject.Show();
        Vector3 originPos = tf.position;
        Sequence sequence = DOTween.Sequence();                                          //创建顺序列
        Tweener move1 = tf.DOLocalMoveY(tf.localPosition.y + 50, 0.5f);                   //
        Tweener move2 = tf.DOLocalMoveY(tf.localPosition.y + 100, 0.5f);
        sequence.Append(move1);
        Image childTf0 = idle.GetChild(0).GetComponent<Image>();
        Image childTf1 = idle.GetChild(1).GetComponent<Image>();
        Image childTf2 = idle.GetChild(2).GetComponent<Image>();
        Image childTf3 = idle.GetChild(3).GetComponent<Image>();
        Tweener alpha10 = DOTween.To(() => childTf0.color, x => childTf0.color = x, new Color(childTf0.color.r, childTf0.color.g, childTf0.color.b, 1), 1f);
        sequence.Insert(0.2f, alpha10);
        Tweener alpha11 = DOTween.To(() => childTf1.color, x => childTf1.color = x, new Color(childTf1.color.r, childTf1.color.g, childTf1.color.b, 1), 1f);    
        sequence.Insert(0.2f, alpha11);
        Tweener alpha12 = DOTween.To(() => childTf2.color, x => childTf2.color = x, new Color(childTf2.color.r, childTf2.color.g, childTf2.color.b, 1), 1f);
        sequence.Insert(0.2f, alpha12);
        Tweener alpha13 = DOTween.To(() => childTf3.color, x => childTf3.color = x, new Color(childTf3.color.r, childTf3.color.g, childTf3.color.b, 1), 1f);
        sequence.Insert(0.2f, alpha13);
        sequence.AppendInterval(0.2f);
        sequence.Append(move2);
        Tweener alpha20 = DOTween.To(() => childTf0.color, x => childTf0.color = x, new Color(childTf0.color.r, childTf0.color.g, childTf0.color.b, 0), 1f);
        sequence.Insert(1.2f, alpha20);
        Tweener alpha21 = DOTween.To(() => childTf1.color, x => childTf1.color = x, new Color(childTf1.color.r, childTf1.color.g, childTf1.color.b, 0), 1f);
        sequence.Insert(1.2f, alpha21);
        Tweener alpha22 = DOTween.To(() => childTf2.color, x => childTf2.color = x, new Color(childTf2.color.r, childTf2.color.g, childTf2.color.b, 0), 1f);
        sequence.Insert(1.2f, alpha22);
        Tweener alpha23 = DOTween.To(() => childTf3.color, x => childTf3.color = x, new Color(childTf3.color.r, childTf3.color.g, childTf3.color.b, 0), 1f);
        sequence.Insert(1.2f, alpha23);
        //    sequence.Join(alpha2);
        sequence.AppendInterval(0.2f);
        sequence.OnComplete(()=> { OnComplete1(idle.gameObject,originPos); });
    }

    private static void OnComplete1(GameObject obj,Vector3 pos)
    {
        Debug.Log("OnComplete");
        obj.transform.position = pos;
        obj.Hide();     
    }

}
