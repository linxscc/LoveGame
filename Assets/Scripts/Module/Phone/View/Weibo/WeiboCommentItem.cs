using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class WeiboCommentItem : MonoBehaviour {

    private Text _contentTxt;
    private RectTransform _conRectTransform;
    private RectTransform _bgRectTransform;

    private void Awake()
    {
        _contentTxt = transform.Find("Content").GetComponent<Text>();      
        //_conRectTransform = transform.GetComponent<RectTransform>();
        //_bgRectTransform=transform.Find("BgImg").GetComponent<RectTransform>();
    }

    public void SetData(string name, string str)
    {
        //string pattern = @"\#\{.*?\}\#";//匹配模式
        //Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        //MatchCollection matches = regex.Matches(str);
        //string showStr = "";
        ////存放匹配结果
        //if(matches.Count==1)
        //{
        //    string name = matches[0].Value.Substring(3, matches[0].Value.Length - 5);
        //    string content = str.Substring(matches[0].Value.Length);         
        //    showStr = "<color=#808080>" + name + "</color>:" + content;
        //}
        //else
        //{
        //    showStr = str;
        //}
        _contentTxt.text = name+":"+str;
        //float h = 59 + _contentTxt.preferredHeight;
        //_conRectTransform.SetHeight(h);
        //_bgRectTransform.SetHeight(h);
    }
}
