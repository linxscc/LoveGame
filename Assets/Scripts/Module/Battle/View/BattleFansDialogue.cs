using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Com.Proto;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleFansDialogue : MonoBehaviour
{
    private TextI18N _contentTxt;

    private string _dialogStr;


    private float _normalSpeed = 0.08f;
    private float _fastSpeed = 0.01f;

    public bool IsFastSpeed = false;

    public Action OnStepEnd;


    private bool _isTweenText = false;
    private float _lineHeight;
    private float _lineSpacing;
    private int _lastLineCount;

    private Vector3 _pos;

    private int _contentTxtY;
    private float _moveY;


    private void Awake()
    {
        _contentTxt = transform.Find("ContextMask/ContentTxt").GetComponent<TextI18N>();

        _lineHeight = _contentTxt.preferredHeight;
        _lineSpacing = (_contentTxt.lineSpacing - 1) * _contentTxt.fontSize;

        RectTransform rect = _contentTxt.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, _lineHeight * 3);
    }


    private void Update()
    {
        if (_dialogStr == null)
        {
            return;
        }

        float h = _contentTxt.preferredHeight;

        int lineCount = (int) Math.Ceiling(h / (_lineHeight + _lineSpacing));

        if (lineCount > 3 && lineCount > _lastLineCount && _isTweenText == false)
        {
            _lastLineCount = lineCount;
            RectTransform rect = _contentTxt.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, h);
            _moveY = rect.anchoredPosition.y + _lineHeight * (lineCount - 3) - _lineSpacing / 2 * lineCount;
            rect.DOAnchorPosY(_moveY, 0.2f).onComplete = () => { _isTweenText = false; };
        }
    }


    public void SetData(string dialogStr)
    {
        IsFastSpeed = false;

        _dialogStr = dialogStr;
        _dialogStr = Util.GetNoBreakingString(_dialogStr);

        _contentTxt.color = new Color(1,1,1,0);
        _contentTxt.DOColor(Color.white, 0.5f);

        _lastLineCount = 0;

        if (_contentTxt != null)
        {
            _contentTxt.text = "";
        }

        StartCoroutine(Typewriter(_contentTxt, _dialogStr, _normalSpeed));
    }

    IEnumerator Typewriter(Text textMesh, string str, float speed)
    {
        textMesh.text = str;

        yield return new WaitForSeconds(0.3f);
        OnStepEnd?.Invoke();

        int length = str.Length;
//		int index = 0;
//		textMesh.text = string.Empty;
//		while (index < length)
//		{
//			if (IsFastSpeed)
//			{
//				speed = _fastSpeed;
//			}
//			
//			if (str[index].Equals('<'))
//			{
//				int left = index;
//				int right = index;
//				while (!str[++right].Equals('>'))
//				{
//				}
//
//				++right;
//				int middleIndex = 0;
//				int middleLength = 0;
//				middleIndex = right;
//				string leftRichText = str.Substring(left, right - left);
//
//				left = middleIndex;
//				while (!str[++left].Equals('<'))
//				{
//					middleLength++;
//				}
//
//				right = left;
//				while (!str[++right].Equals('>'))
//				{
//				}
//
//				++right;
//				string rightRichText = str.Substring(left, right - left);
//				string middleStr = str.Substring(middleIndex, middleLength+1);
//				yield return Typewriter(textMesh, middleStr, string.Format("{0}{{0}}{1}", leftRichText, rightRichText),speed);
//				index = right;
//			}
//			else
//			{
//				textMesh.text += str[index];
//				index++;
//			}
//			
//			yield return new WaitForSeconds(speed);
//		}
//
//		
//		if (index==length)
//		{
//			yield return new WaitForSeconds(0.5f);		  
//	       OnStepEnd?.Invoke();          
//		}
    }

    IEnumerator Typewriter(Text textMesh, string str, string format, float speed)
    {
        int length = str.Length;
        int index = 0;
        while (index < length)
        {
            if (IsFastSpeed)
            {
                speed = 0.05f;
            }

            textMesh.text += string.Format(format, str[index]);
            index++;
            yield return new WaitForSeconds(speed);
        }
    }
}