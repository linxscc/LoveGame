using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Security.Cryptography;
using Assets.Scripts.Framework.GalaSports.Language;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using Framework.Utils;
using DataModel;

public class Util
{
    
    /// <summary>
    /// 屏蔽emoji
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RegexString(string str)
    {
        Regex reg = new Regex(
            "(\uD83C[\uDDE8-\uDDFF]\uD83C[\uDDE7-\uDDFF])|[\uD800-\uDBFF][\uDC00-\uDFFF]|[\u2600-\u27ff][\uFE0F]|[\u2600-\u27ff]");
        var regStr = reg.Replace(str, "");
        return regStr;
    }

    public static string RegexString1(string strText)
    {
        string result = Regex.Replace(strText, @"\p{Cs}", "");
        return result;
    }



    /// <summary>
    /// InputField组件过滤掉无效输入的Char，这个方法可以过滤掉emoji的输入
    /// </summary>
    /// <param name="input"></param>
    /// <param name="charIndex"></param>
    /// <param name="addedChar"></param>
    /// <returns></returns>
    public static char Inputdelegate(string input, int charIndex, char addedChar)
    {
        return IsEmojiCharacter(addedChar) ? '\0' : addedChar;
    }

    public static bool IsEmojiCharacter(int codePoint)
    {
        return (codePoint >= 0x2600 && codePoint <= 0x27BF) // 杂项符号与符号字体
               || codePoint == 0x303D
               || codePoint == 0x2049
               || codePoint == 0x203C
               || (codePoint >= 0x2000 && codePoint <= 0x2005) //0x2006，IOS输入法所需要的字符，不屏蔽
               || (codePoint >= 0x2007 && codePoint <= 0x200F) //
               || (codePoint >= 0x2028 && codePoint <= 0x202F) //
               || codePoint == 0x205F //
               || (codePoint >= 0x2065 && codePoint <= 0x206F) //
               /* 标点符号占用区域 */
               || (codePoint >= 0x2100 && codePoint <= 0x214F) // 字母符号
               || (codePoint >= 0x2300 && codePoint <= 0x23FF) // 各种技术符号
               || (codePoint >= 0x2B00 && codePoint <= 0x2BFF) // 箭头A
               || (codePoint >= 0x2900 && codePoint <= 0x297F) // 箭头B
               || (codePoint >= 0x3200 && codePoint <= 0x32FF) // 中文符号
               || (codePoint >= 0xD800 && codePoint <= 0xDFFF) // 高低位替代符保留区域
               || (codePoint >= 0xE000 && codePoint <= 0xF8FF) // 私有保留区域
               || (codePoint >= 0xFE00 && codePoint <= 0xFE0F) // 变异选择器
               || codePoint >= 0x10000; // Plane在第二平面以上的，char都不可以存，全部都转
    }

    /// <summary>
    /// 去除list里面的空元素
    /// </summary>
    /// <returns></returns>
    public static List<T> RemoveAllNullElement<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list[i] == null)
            {
                int newCount = i++;
                for (; i < count; i++)
                {
                    if (list[i] != null)
                    {
                        list[newCount++] = list[i];
                    }
                }

                list.RemoveRange(newCount, count - newCount);
                break;
            }
        }

        return list;
    }

    


    /// <summary>
    /// 交换两个子物体在父物体中的层次位置
    /// </summary>
    /// <param name="tran1"></param>
    /// <param name="tran2"></param>
    public static void ExchangeSiblingIndex(Transform tran1, Transform tran2)
    {
        var index1 = tran1.GetSiblingIndex();
        var index2 = tran2.GetSiblingIndex();
        var tempIndex = index1;
        tran1.SetSiblingIndex(index2);
        tran2.SetSiblingIndex(tempIndex);
    }

    public static void SetMaxSiblingIndex(Transform tran)
    {
        Transform parent = tran.parent;
        int max = -1;
        foreach (Transform child in parent)
        {
            int siblingIndex = child.GetSiblingIndex();
            if (siblingIndex > max)
                max = siblingIndex;
        }

        tran.SetSiblingIndex(max + 1);
    }


    /// <summary>
    /// 判断字符串是否包含中文
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool HasChinese(string content)
    {
        //判断是不是中文
        string regexstr = @"[\u4e00-\u9fa5]";

        if (Regex.IsMatch(content, regexstr))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsOnlyEnglishAndNumber(string content)
    {
        Regex NumandEG = new Regex("[^A-Za-z0-9]");
        return !NumandEG.IsMatch(content);
    }

    public static bool IsOnlyNumber(string content)
    {
        return Regex.IsMatch(content, @"^[+-]?\d*$");
    }

    /// <summary>
    /// 文本框数字动态变化
    /// </summary>
    /// <param name="txt">文本框</param>
    /// <param name="time">变化耗时</param>
    /// <param name="toNum">最终值</param>
    /// <param name="prefix">前缀</param>
    /// <param name="suffix">后缀</param>
    public static void TweenTextNum(Text txt, float time, int toNum, string prefix = "", string suffix = "", Action onComplete=null)
    {
        string numStr = txt.text;
        if (prefix != "")
            numStr = numStr.Replace(prefix, "");

        if (suffix != "")
            numStr = numStr.Replace(suffix, "");

        int num = int.Parse(numStr);

        Tweener tween = DOTween.To(() => num, x => num = x, toNum, time);
        tween.onUpdate = () => { txt.text = prefix + num + suffix; };
        tween.onComplete = () =>
        {
            onComplete?.Invoke();
        };
    }

    /// <summary>
    /// 物体闪烁
    /// </summary>
    /// <param name="obj">GameObject</param>
    /// <param name="times">闪烁总时间</param>
    /// <param name="count">闪烁次数</param>
    public static void BlinkGameObject(GameObject obj, float totalTime, int count, Action completeCallback)
    {
        if (count <= 0)
            return;
        int i = 0;
        float blinkTime = totalTime / (float) count;
        Action action = null;
        action = () =>
        {
            ClientTimer.Instance.DelayCall(() =>
            {
                if (obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                }
                else
                {
                    obj.SetActive(true);
                }

                i++;
                if (i >= count * 2)
                {
                    if (completeCallback != null)
                    {
                        completeCallback();
                    }

                    return;
                }
                else
                {
                    action();
                }
            }, blinkTime);
        };
        action();
    }

    /// <summary>
    /// 屏蔽{player}字符替换
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string RegPlayerNameString(string oldStr,string name)
    {
        return GetNoBreakingString(oldStr.Replace("{$player}", name));
    }

    public static Vector3 randomRotation(float dist)
    {
        return new Vector3(UnityEngine.Random.Range(0, dist), UnityEngine.Random.Range(0, dist),
            UnityEngine.Random.Range(0, dist));
    }

    public static Sprite TextureToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);
    }

    public static bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null)
            return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static float GetAdapteFov(float designFov)
    {
        float ratio = Screen.height / (float) Screen.width;
        //float resultFov = designFov * (1 + ratio - 750 / 1334.0f);
        //return resultFov;

        float ratio_ori = 750 / 1334.0f;
        float resultFov = 2 * Mathf.Atan((ratio / ratio_ori) * Mathf.Tan(designFov / 2 * Mathf.Deg2Rad)) *
                          Mathf.Rad2Deg;
        //Debug.LogWarning("ratio_ori:"+ratio_ori + " ratio:"+ratio + " tan Fov:"+ Mathf.Tan(designFov / 2*Mathf.Deg2Rad));
        //Debug.LogWarning("resultFov:"+resultFov  +" designFov:"+designFov);
        return resultFov;
    }

    public static string ToUnderlineName(string str)
    {
        string newStr = "";
        for (int i = 0; i < str.Length; i++)
        {
            var ch = str[i];
            if (ch > 64 && ch < 91)
            {
                newStr += "_" + (char) (ch + 32);
            }
            else
            {
                newStr += ch;
            }
        }

        return newStr;
    }


    /// <summary>
    /// 合理化路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string PathNormalize(string path)
    {
        return new Regex("[/]{1,100}").Replace(path.Replace("\\", "/"), "/");
    }


    /// <summary>
    /// 如果文件存在则删除
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static bool ExistsDeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 生成GUID
    /// </summary>
    /// <returns></returns>
    public static string Guid()
    {
        return System.Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 删除字符串左边相同的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="left"></param>
    /// <returns></returns>
    public static string RemoveLeft(string str, string left)
    {
        if (str.Substring(0, left.Length) == left)
        {
            return str.Substring(left.Length);
        }

        return str;
    }


    public static string RemoveStr(string str,string choose)
    {
        string numStr = str;
        if (choose != "")
            numStr = numStr.Replace(choose, "");

        return numStr;
    }
    
    /// <summary>
    /// 逐个访问所有子项
    /// </summary>
    /// <param name="parent">父项</param>
    /// <param name="handle">处理方法</param>
    /// <param name="deep">是否深度访问</param>
    public static void EachChildren(Transform parent, Action<GameObject, GameObject> handle, bool deep = false)
    {
        foreach (Transform child in parent.transform)
        {
            handle(parent.gameObject, child.gameObject);
            if (deep) EachChildren(child, handle, true);
        }
    }

    /// <summary>
    /// 删除字符串右边相同的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string RemoveRight(string str, string right)
    {
        if (str.Substring(str.Length - right.Length) == right)
        {
            return str.Substring(0, str.Length - right.Length);
        }

        return str;
    }


    /// <summary>
    /// 获取字符串长度,非英文或数字字符一个字符长度为2
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int GetWordCount(String s)
    {
        int length = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int ascii = s[i];
            if (ascii >= 0 && ascii <= 255)
                length++;
            else
                length += 2;
        }

        return length;
    }

    public static string GetSizeUnit(double size)
    {
        String[] units = new String[] {"B", "KB", "MB", "GB", "TB", "PB"};
        double mod = 1024.0;
        int i = 0;
        while (size >= mod)
        {
            size /= mod;
            i++;
        }

        return Math.Round(size) + units[i];
    }

    //手机震动
    public static void Vibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
    
    public static bool IsMail(string str)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(str,
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    public static void CreateLinkTextBtn(Text text, UnityEngine.Events.UnityAction onClickBtn)
    {
        if (text == null)
            return;

        //克隆Text，获得相同的属性  
        Text underline = GameObject.Instantiate(text) as Text;
        underline.name = "Underline";

        underline.transform.SetParent(text.transform, false);
        RectTransform rt = underline.rectTransform;

        //设置下划线坐标和位置  
        rt.anchoredPosition3D = Vector3.zero;
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchorMin = Vector2.zero;

        underline.text = "_";
        float perlineWidth = underline.preferredWidth; //单个下划线宽度  
        Debug.Log(perlineWidth);

        float width = text.preferredWidth;
        Debug.Log(width);
        int lineCount = (int) Mathf.Round(width / perlineWidth);
        Debug.Log(lineCount);
        for (int i = 1; i < lineCount; i++)
        {
            underline.text += "_";
        }

        var btn = text.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(onClickBtn);
    }
    
    /// <summary>
    /// 替换不换行的空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetNoBreakingString(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Replace(" ", "\u00A0");
    }

    public static Color GetColorFormRichText(string str)
    {
        Color nowColor = Color.black;
        //var reg1 = "<color.*?>";
        //Regex reg = new Regex(reg1);
        //Match matchs = reg.Match(str);//<color=#0000FFFF>
        //Debug.Log(matchs.Value);
        string newstr = str.Substring(7, 9);//#0000FFFF
        //string color1 = matchs.Value.Substring(7, 9);
        Debug.Log(newstr);
        ColorUtility.TryParseHtmlString(newstr, out nowColor);
        return nowColor;
    }

    public static string SetColorFormRichText(string str,Color color)
    {
        string c = ColorUtility.ToHtmlStringRGBA(color);
        string strnew = "<color=#" + c + ">" + GetStringFormRichText(str) + "</color>";
        return strnew;
    }

    public static string GetStringFormRichText(string str)
    {
        Debug.Log(str);
        string newstr = str.Substring(17, str.Length-25);
        Debug.Log(newstr);
        return newstr;
    }
    public static string SetStringFormRichText(string str,string content)
    {
        string newstr = str.Substring(0, 17)+content+ "</color>";
        Debug.Log(newstr);
        return newstr;
    }
    public static string GetPlayerName(PlayerPB playerPb)
    {
        switch (playerPb)
        {
            case PlayerPB.ChiYu:

                return I18NManager.Get("Common_Role4");
            case PlayerPB.YanJi:

                return I18NManager.Get("Common_Role3");

            case PlayerPB.QinYuZhe:

                return I18NManager.Get("Common_Role2");

            case PlayerPB.TangYiChen:

                return I18NManager.Get("Common_Role1");
            default:
                return I18NManager.Get("Common_Invalid");

        }

    }
    public static bool GetIsRedPoint(string constStr)
    {
        string strKey = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + constStr;
        return PlayerPrefs.GetInt(strKey, 0) == 1;
    }

    public static void SetIsRedPoint(string constStr, bool b)
    {
        string strKey = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + constStr;
        PlayerPrefs.SetInt(strKey, b == false ? 0 : 1);
    }

    public static void DeleteRedPoint(string constStr, bool b)
    {
        string strKey = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + constStr;
        PlayerPrefs.DeleteKey(strKey);
    }

}