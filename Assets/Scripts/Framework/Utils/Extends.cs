using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Common;
using game.main;
using UnityEngine.UI;
using XLua;
using Object = UnityEngine.Object;

//扩展类
[LuaCallCSharp]
[ReflectionUse]
public static class Extends
{
    public static bool IsNull(this UnityEngine.Object o) // 或者名字叫IsDestroyed等等
    {
        return o == null;
    }

    public static void Show(this GameObject self)
    {
        if (self != null && !self.activeSelf)
        {
            self.SetActive(true);
        }
    }

    public static void Hide(this GameObject self)
    {
        if (self != null && self.activeSelf)
        {
            self.SetActive(false);
        }
    }

    public static T AddScriptComponent<T>(this GameObject self) where T : Component
    {
        if (self.GetComponent<T>() == null)
            return self.AddComponent<T>();
        return self.GetComponent<T>();
    }

    public static void DelayCall(this MonoBehaviour self, Action action, float time)
    {
        if (ClientTimer.Instance != null)
        {
            ClientTimer.Instance.DelayCall(() =>
            {
                if (self != null && action != null)
                    action();
            }, time);
        }
    }

    /// 移除字符串左边匹配字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="match">匹配的字符串</param>
    /// <returns></returns>
    public static string RemoveLeft(this string input, string match)
    {
        if (input.IndexOf(match) == 0)
        {
            return input.Substring(match.Length);
        }

        return input;
    }


    public static void RemoveComponent<T>(this Component src) where T : Component
    {
        T component = src.GetComponent<T>();
        if (component != null)
        {
            Object.DestroyImmediate(component);
        }
    }

    public static void RemoveChildren(this Transform self)
    {
        for (int i = self.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(self.GetChild(i).gameObject);
        }
    }


    public static Text GetText(this Transform transform, string path = null)
    {
        if (path == null)
        {
            return transform.GetComponent<Text>();
        }

        return transform.Find(path).GetComponent<Text>();
    }

    public static Button GetButton(this Transform transform, string path = null)
    {
        if (path == null)
        {
            return transform.GetComponent<Button>();
        }

        return transform.Find(path).GetComponent<Button>();
    }

    public static RectTransform GetRectTransform(this Transform transform, string path = null)
    {
        if (path == null)
        {
            return transform.GetComponent<RectTransform>();
        }

        return transform.Find(path).GetComponent<RectTransform>();
    }

    public static RawImage GetRawImage(this Transform transform, string path = null)
    {
        if (path == null)
        {
            return transform.GetComponent<RawImage>();
        }

        return transform.Find(path).GetComponent<RawImage>();
    }

    public static Image GetImage(this Transform transform, string path = null)
    {
        if (path == null)
        {
            return transform.GetComponent<Image>();
        }

        return transform.Find(path).GetComponent<Image>();
    }

    public static Toggle GetToggle(this Transform transform, string path)
    {
        return transform.Find(path).GetComponent<Toggle>();
    }
    public static ProgressBar GetProgressBar(this Transform transform, string path)
    {
         return transform.Find(path).GetComponent<ProgressBar>();
    }

    public static void PlayButtonEffect(this Button self, string soundName = null)
    {
        if (soundName == null)
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        }
        else
        {
            new AssetLoader().LoadAudio(AssetLoader.GetSoundEffectById(soundName),
                (clip, loader) => { AudioManager.Instance.PlayEffect(clip); });
        }
    }
}