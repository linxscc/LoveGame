using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using game.main;
using GalaAccountSystem;
using UnityEditor;
using UnityEngine;

public static class I18NManager
{

///**英文*/
//	EN(0),
//	/**简体中文*/
//	CN(1),
//	/**繁体中文 香港*/
//	HK(2),
//	/**繁体中文 台湾*/
//	TW(3),
//	/**泰语*/
//	THAI(4),
//	/**越南语*/
//	VN(5),
//	/**日语*/
//	JP(6),
//	/**韩语*/
//	KO(7),
//	/**西班牙语*/
//	ES(8),
//	/**意大利语*/
//	IT(9),
//	/**法语*/
//	FR(10)
		
	public enum LanguageType
	{	 
		English,
		ChineseSimplified,
		ChineseTraditionalHk,
		ChineseTraditionalTw,
		Thailand,
		Vietnam,
		Japan,
		Korea,
	}
	
	private static Dictionary<string, string> _languageDict;

#if UNITY_EDITOR
	[InitializeOnLoadMethod]
	public static void LoadLanguageConfig()
	{
		LanguageType type = LanguageType.ChineseSimplified;
		LoadLanguageConfig(type);
		
//		EditorUtility.ClearProgressBar();
	}
#endif


	public static string Language;

	
    public static void LoadGalaSDKLanguageConfig(LanguageType type)
    {
        switch (type)
        {
            case LanguageType.English:
                GalaSDKLanguageService.Instance.SetLanguageType((GalaSDKLanguageService.LanguageType.EN));
                break;
            case LanguageType.ChineseSimplified:
                GalaSDKLanguageService.Instance.SetLanguageType((GalaSDKLanguageService.LanguageType.CN));
                break;
            case LanguageType.ChineseTraditionalHk:
                GalaSDKLanguageService.Instance.SetLanguageType((GalaSDKLanguageService.LanguageType.HK));
                break;
            case LanguageType.ChineseTraditionalTw:
                GalaSDKLanguageService.Instance.SetLanguageType((GalaSDKLanguageService.LanguageType.TW));
                break;
            default:
                GalaSDKLanguageService.Instance.SetLanguageType((GalaSDKLanguageService.LanguageType.CN));
                break;
        }
    }
	
	public static void LoadLanguageConfig(LanguageType type)
	{    		
		switch (type)
		{
			case LanguageType.ChineseSimplified:
				Language = "zh-cn";
				break;
			case LanguageType.ChineseTraditionalHk:
				Language = "zh-hk";
				break;
			case LanguageType.ChineseTraditionalTw:
				Language = "zh-tw";
				break;
			case LanguageType.English:
				Language = "en_us";
				break;
			default:
				Language = "zh-cn";
				break;
		}
		
		_languageDict = new Dictionary<string, string>();

		char[] separator = new char[] { '=' };
		
		string str = new AssetLoader().LoadTextSync(AssetLoader.GetLanguageDataPath(type));
		var strings = str.Split(new char[] { '\n'}, StringSplitOptions.RemoveEmptyEntries);
		int count = 0;
		foreach (var line in strings)
		{
			string trim = line.Trim();
			if (string.IsNullOrEmpty(trim) || line.StartsWith("//"))
				continue;

			count++;
			
			string[] arr = trim.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				_languageDict.Add(arr[0].Trim(), Regex.Unescape(arr[1].Trim()));
			}
			catch (Exception e)
			{
				Debug.LogError("I18NManager lint:" + count  +"  "+e.Message);
			}
		}
	}

	public static string Get(string key)
	{
		string value;
		if (_languageDict.TryGetValue(key, out value))
		{
			return value;
		}
		return null;
	}
	
	public static string Get(string key, params object[] strings)
	{
		string value;
		if (_languageDict.TryGetValue(key, out value))
		{
			return string.Format(value, strings);
		}
		
		return null;
	}
}
