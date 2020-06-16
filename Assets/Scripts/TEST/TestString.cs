using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TestString : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
//		TextAsset txt = Resources.Load<TextAsset>("TEST/1");
//		string str = txt.text;
////		str = str.Replace("#{$heroLi}#", "<color=#ff0000>李赫</color>");
//		str = str.Replace("#{$heroLi}#", "#{"+"李赫"+"}#");
//		
//		
//		string pattern = @"\#\{.*?\}\#";//匹配模式
//		Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
//		MatchCollection matches = regex.Matches(str);
//		//存放匹配结果
// 
//		foreach (Match match in matches)
//		{
//			string value = match.Value.Substring(2, match.Value.Length - 4);
//			
//			StringBuilder sb = new StringBuilder();
//			sb.Append("<color=#ff0000>");
//			sb.Append(value);
//			sb.Append("</color>");
//
//			str = str.Replace(match.Value, sb.ToString());
//		}
//
//		transform.Find("Text").GetComponent<Text>().text = str;

        string str =
            "{\"gift_33\":\"MYR 12.90\",\"gift_35\":\"MYR 39.90\",\"gift_34\":\"MYR 19.90\",\"gift_32\":\"MYR 4.99\",\"gift_36\":\"MYR 79.90\",\"gift_37\":\"MYR 199.90\",\"gift_8\":\"MYR 399.90\",\"gift_30\":\"MYR 1.90\",\"gift_31\":\"MYR 1.90\"}";

        JSONObject jsonObject = new JSONObject(str);

        SetAreaPrice(jsonObject);

//		str.IndexOf(\"#{", StringComparison.Ordinal)
    }

    public void SetAreaPrice(JSONObject strList)
    {
        string curreny = null;
        bool hasCurreny = false;
        if (strList.keys.Contains("currency"))
        {
            curreny = strList["currency"].str;
            hasCurreny = true;
        }

        string pattern = @"(([1-9][0-9]*)|(([0]\.\d{0,2}|[1-9][0-9]*\.\d{0,2})))$"; //匹配模式
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        foreach (var str in strList.keys)
        {
            string price = strList[str].str;
            if (hasCurreny == false)
            {
                MatchCollection matches = regex.Matches(price);
                foreach (Match match in matches)
                {
                    string value = match.Value.Substring(2, match.Value.Length - 4);
                    string temp = price.Substring(match.Index, match.Length);

                    curreny = price.Replace(temp, "").Trim();
                    price = temp.Trim();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}