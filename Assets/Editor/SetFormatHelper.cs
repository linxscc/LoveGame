using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class SetFormatHelper : Editor
{

	//****************Texture*******************************
	
	//路径
	private const string CardPath = "Assets/BundleAssets/SingleFile/Card/";
	private const string FansPath = "Assets/BundleAssets/SingleFile/FansTexture/";
	private const string HeadPath = "Assets/BundleAssets/SingleFile/Head/";
	private const string Live2dPath = "Assets/BundleAssets/SingleFile/Live2d/";
	private const string PhonePath = "Assets/BundleAssets/SingleFile/Phone/";
	
	private const string StoryPath = "Assets/BundleAssets/Story/";
	private const string SingleFileBackground = "Assets/BundleAssets/SingleFile/Background/";
	
	
	//平台
	private static string iOS = "iPhone";   //苹果平台
	private static string Android = "Android";  //安卓平台
	
	
	
	//平台格式IOS
	private static TextureImporterFormat _iosSmallAlphaFormat = TextureImporterFormat.ASTC_RGBA_4x4;  //ios小图带透明
	private static TextureImporterFormat _iosBigAlphaFormat = TextureImporterFormat.ASTC_RGBA_8x8;   //ios大图带透明
	private static TextureImporterFormat _iosSmallFormat = TextureImporterFormat.ASTC_RGB_4x4;       //ios小图不带透明
	private static TextureImporterFormat _iosBigFormat = TextureImporterFormat.ASTC_RGB_8x8;         //ios大图不带透明                                                                                                            p


	private static TextureImporterFormat _androidSmallAlphaFormat = TextureImporterFormat.ETC2_RGBA8; //安卓小图带透明
	private static TextureImporterFormat _androidBigAlphaFormat = TextureImporterFormat.ETC2_RGBA8; //安卓大图带透明
	private static TextureImporterFormat _androidSmallFormat = TextureImporterFormat.ETC2_RGBA8;      //安卓小图不带透明
	private static TextureImporterFormat _androidBigFormat = TextureImporterFormat.ETC2_RGBA8;      //安卓大图不带透明
		
	//Size
	private const int Size = 2048;
	
	


	//****************Audio*******************************
	
	//路径
	private const string BackgroundAudioPath = "Assets/BundleAssets/Audio/";
	private const string DrawCardDialogsAudioPath = "Assets/BundleAssets/Audio/DrawCardDialogs/";
	private const string MainplayAudioPath = "Assets/BundleAssets/Audio/mainplay/";
	private const string PhoneDialogsAudioPath = "Assets/BundleAssets/Audio/PhoneDialogs/";
	private const string SoundEffectAudioPath = "Assets/BundleAssets/Audio/SoundEffect/";
	private const string DubbingAudioPath = "Assets/BundleAssets/Story/Dubbing/";
	private const string CoaxSleepAudioPath = "Assets/BundleAssets/Audio/CoaxSleepAudio/";
	//平台
	private static string Standalone = "Standalone";
	
	//格式
	private static  AudioClipLoadType LongAudio =  AudioClipLoadType.DecompressOnLoad;
	private static  AudioClipLoadType ShortAudio = AudioClipLoadType.Streaming; 

	#region 图片格式设置



    
    [MenuItem("SetFormat/Texture/SingleFile-Background")]
    static void SetBackgroundTexture()
    {
	    SetTexture(SingleFileBackground,false);
    }
	
  
	[MenuItem("SetFormat/Texture/Card")]
	static void SetCardTexture()
	{
		SetTexture(CardPath,false);
	}



	[MenuItem("SetFormat/Texture/Fans")]
	static void SetFansTexture()
	{
		SetTexture(FansPath,false);
	}
	

	[MenuItem("SetFormat/Texture/Head")]
	static void SetHeadTexture()
	{
		SetTexture(HeadPath,false);
	}
	

	[MenuItem("SetFormat/Texture/Live2d")]
	static void SetLive2DTexture()
	{
		SetTexture(Live2dPath, true);
	}
	

	[MenuItem("SetFormat/Texture/Phone")]
	static void SetPhoneTexture()
	{
		SetTexture(PhonePath,false);
	}


	[MenuItem("SetFormat/Texture/Story")]
	static void SetStory()
	{
		SetTexture(StoryPath,false);
	}

    private	static readonly List<string> _texturePaths =new List<string>{CardPath,FansPath,HeadPath,Live2dPath,PhonePath,StoryPath,SingleFileBackground};
	private static int _curSetTextureIndex = 0;
	
	[MenuItem("SetFormat/Texture/一键设置图片格式")]
	static void SetAllTexture()
	{

		ExecuteNextSetTextureMethod();
	}

	static void ExecuteNextSetTextureMethod()
	{		
	  var isSpriteAtlas = _texturePaths[_curSetTextureIndex]==Live2dPath;	 
	  SetTexture(_texturePaths[_curSetTextureIndex], isSpriteAtlas, (() =>
	  {
	    _curSetTextureIndex++;
		 if (_curSetTextureIndex>_texturePaths.Count-1)
		 {
			  _curSetTextureIndex = 0;
			return;  
		 }
		
		  ExecuteNextSetTextureMethod();
	  }));
	}
	


	
	
	static void SetTexture(string assetsPath,bool isSpriteAtlas,Action callBack=null)
	{
		if (!Directory.Exists(assetsPath))
		{
			Debug.LogError("assetsPathErro===>"+assetsPath);
		}
		
		DirectoryInfo di = new DirectoryInfo(assetsPath);
		
		FileInfo[] files = di.GetFiles("*", SearchOption.AllDirectories)
			.Where(s=>s.Name.IndexOf(".meta",StringComparison.Ordinal)==-1).ToArray();

		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Name.EndsWith(".png") || files[i].Name.EndsWith(".jpg"))
			{
				var filePath = files[i].FullName.Replace("\\", "/");
				string[] sArray = filePath.Split( new string[]{"SuperStarGame/"}, StringSplitOptions.RemoveEmptyEntries);
				
				string path = sArray[1];
				
				EditorUtility.DisplayProgressBar("处理中>>>", path, (float)i / files.Length);
				
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;	
									
				Texture tx = AssetDatabase.LoadAssetAtPath<Texture>(path);

				if (ti==null || tx ==null)
				{
					 Debug.LogError("此路径有问题===>"+path);
					 continue;
				}

				if (isSpriteAtlas)
				{

					var isSetIosSpriteTexture = IsSetIOSSpriteTexture(ti);
					var isSetAndroidSpriteTexture = IsSetAndroidSpriteTexture(ti);

					if (isSetIosSpriteTexture&&isSetAndroidSpriteTexture)
					{
						continue;
					}
					else
					{
						if (!isSetIosSpriteTexture)
						{
							SetSpriteAtlasTexture(ti, iOS, TextureImporterFormat.ASTC_RGBA_4x4);	
						}

						if (!isSetAndroidSpriteTexture)
						{
							SetSpriteAtlasTexture(ti, Android, TextureImporterFormat.ETC2_RGBA8);
						}
					}															
				}
				else
				{

					var isSetIos = IsSetIOSTexture(ti, tx);
					var isSetAndroid = IsSetAndroidTexture(ti, tx);

					if (isSetIos&&isSetAndroid)
					{
						 continue;
					}
					else
					{
						if (!isSetIos)
						{
							SetIOSTextureFormat(ti, tx);
						}

						if (!isSetAndroid)
						{
							SetAndroidTextureFormat(ti, tx);
						}
					}
				}
																
				Debug.Log("处理的图片===>"+path);
				
												
				AssetDatabase.ImportAsset(path);
				
			}
		}		
		EditorUtility.ClearProgressBar();
		callBack?.Invoke();
	}

	


	
	
	/// <summary>
	/// 是否设置IOS,live2d
	/// </summary>
	/// <param name="ti"></param>
	/// <returns></returns>
	static bool IsSetIOSSpriteTexture(TextureImporter ti)
	{
		bool isSet = false;
		var overridden = ti.GetPlatformTextureSettings(iOS).overridden;    //IOS 平台是否被设置过 

		if (overridden)
		{
			if ( ti.GetAutomaticFormat(iOS)== TextureImporterFormat.ASTC_RGBA_4x4 &&
            	 ti.textureType == TextureImporterType.Sprite &&
            	 ti.spriteImportMode == SpriteImportMode.Single &&
            	 ti.alphaSource == TextureImporterAlphaSource.FromInput &&
            	 ti.alphaIsTransparency && ti.sRGBTexture)
            {
            	return true;
            }
		}

		return isSet;
	}

	/// <summary>
	/// 是否设置Android,live2d
	/// </summary>
	/// <param name="ti"></param>
	/// <returns></returns>
	static bool IsSetAndroidSpriteTexture(TextureImporter ti)
	{
		bool isSet = false;
		var overridden = ti.GetPlatformTextureSettings(Android).overridden;
		if (overridden)
		{
			if ( ti.GetAutomaticFormat(Android)== TextureImporterFormat.ETC2_RGBA8 &&
			     ti.textureType == TextureImporterType.Sprite &&
			     ti.spriteImportMode == SpriteImportMode.Single &&
			     ti.alphaSource == TextureImporterAlphaSource.FromInput &&
			     ti.alphaIsTransparency && ti.sRGBTexture)
			{
				return true;
			}	
		}

		return isSet;
	}
	
	//设置live2d图集格式
	static void SetSpriteAtlasTexture(TextureImporter ti ,string platform,TextureImporterFormat format)
	{
		ti.SetPlatformTextureSettings(platform, Size, format);
		ti.textureType = TextureImporterType.Sprite;
		ti.spriteImportMode = SpriteImportMode.Single;
		ti.alphaSource = TextureImporterAlphaSource.FromInput;
		ti.alphaIsTransparency = true;
		ti.sRGBTexture = true;
	}
	
	
	// 是否大纹理
	static bool IsBigTexture(Texture tx)
	{
		if (tx.width>512 && tx.height >512)
		{
			return true;
		}

		return false;
	}
	
	//是否有透明
	static bool IsHaveAlpha(TextureImporter ti)
	{
		if (ti.DoesSourceTextureHaveAlpha())
		{
			return true;
		}

		return false;
	}


	/// <summary>
	/// 设置IOS图片格式
	/// </summary>
	/// <param name="ti"></param>
	/// <param name="tx"></param>
	static void SetIOSTextureFormat(TextureImporter ti, Texture tx)
	{
		var isBigTexture = IsBigTexture(tx);
		var isHaveAlpha = IsHaveAlpha(ti);
		
		ti.textureType = TextureImporterType.GUI;
		ti.npotScale = TextureImporterNPOTScale.ToNearest;
		
		if (isHaveAlpha)
		{
			ti.alphaSource = TextureImporterAlphaSource.FromInput;
			if(isBigTexture)
			{				
				ti.SetPlatformTextureSettings(iOS,Size,_iosBigAlphaFormat);				
			}
			else
			{
				ti.SetPlatformTextureSettings(iOS,Size,_iosSmallAlphaFormat);
			}
		}
		else
		{
			ti.alphaSource = TextureImporterAlphaSource.None;
			if(isBigTexture)
			{
				ti.SetPlatformTextureSettings(iOS,Size,_iosBigFormat);
			}
			else
			{				
				ti.SetPlatformTextureSettings(iOS,Size,_iosSmallFormat);
			}
		}
	}
	
	
	/// <summary>
	/// 设置安卓图片格式
	/// </summary>
	/// <param name="ti"></param>
	/// <param name="tx"></param>
	static void SetAndroidTextureFormat(TextureImporter ti, Texture tx)
	{
		var isBigTexture = IsBigTexture(tx);
		var isHaveAlpha = IsHaveAlpha(ti);
		
		ti.textureType = TextureImporterType.GUI;
		ti.npotScale = TextureImporterNPOTScale.ToNearest;
		
		if (isHaveAlpha)
		{
			ti.alphaSource = TextureImporterAlphaSource.FromInput;
			if(isBigTexture)
			{				
				ti.SetPlatformTextureSettings(Android,Size,_androidBigAlphaFormat);				
			}
			else
			{
				ti.SetPlatformTextureSettings(Android,Size,_androidSmallAlphaFormat);
			}
		}
		else
		{
			ti.alphaSource = TextureImporterAlphaSource.None;
			if(isBigTexture)
			{
				ti.SetPlatformTextureSettings(Android,Size,_androidBigFormat);
			}
			else
			{
				ti.SetPlatformTextureSettings(Android,Size,_androidSmallFormat);
			}
		}
	}
	
	
	
	

	/// <summary>
	/// 是否设置过IOS图片
	/// </summary>
	/// <param name="ti"></param>
	/// <param name="tx"></param>
	/// <returns></returns>
	static bool IsSetIOSTexture(TextureImporter ti,Texture tx)
	{
		bool isSet = false;
		var overridden = ti.GetPlatformTextureSettings(iOS).overridden;    //IOS 平台是否被设置过
		var isBigTexture = IsBigTexture(tx);
		var isHaveAlpha = IsHaveAlpha(ti);
		if (overridden)
		{
			if (isHaveAlpha)
			{
				if (isBigTexture)
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.FromInput &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(iOS) == _iosBigAlphaFormat)
					{
						isSet = true;
					}
				}
				else
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.FromInput &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(iOS) == _iosSmallAlphaFormat)
					{
						isSet = true;
					}
				}
			}
			else
			{
				if (isBigTexture)
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.None &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(iOS) == _iosBigFormat)
					{
						isSet = true;
					}
				}
				else
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.None &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(iOS) == _iosSmallFormat)
					{
						isSet = true;
					}
				}
			}
		}				
		return isSet;
	}
	
	
	/// <summary>
	/// 是否设置过Android图片
	/// </summary>
	/// <param name="ti"></param>
	/// <param name="tx"></param>
	/// <returns></returns>
	static bool IsSetAndroidTexture(TextureImporter ti,Texture tx)
	{
		bool isSet = false;
		var overridden = ti.GetPlatformTextureSettings(Android).overridden;    //IOS 平台是否被设置过
		var isBigTexture = IsBigTexture(tx);
		var isHaveAlpha = IsHaveAlpha(ti);
		if (overridden)
		{
			if (isHaveAlpha)
			{
				if (isBigTexture)
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.FromInput &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(Android) == _androidBigAlphaFormat)
					{
						isSet = true;
					}
				}
				else
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.FromInput &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(Android) == _androidSmallAlphaFormat)
					{
						isSet = true;
					}
				}
			}
			else
			{
				if (isBigTexture)
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.None &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(Android) == _androidBigFormat)
					{
						isSet = true;
					}
				}
				else
				{
					if (ti.textureType == TextureImporterType.GUI &&
					    ti.alphaSource == TextureImporterAlphaSource.None &&
					    ti.npotScale == TextureImporterNPOTScale.ToNearest &&
					    ti.GetAutomaticFormat(Android) == _androidSmallFormat)
					{
						isSet = true;
					}
				}
			}
		}				
		return isSet;
	}
	
	
	

	#endregion


	#region 音频格式设置

	[MenuItem("SetFormat/Audio/Standalone/CoaxSleep")]
	static void SetCoaxSleepAudio()
	{
		SetAudio(CoaxSleepAudioPath, true);
	}
	
		
	[MenuItem("SetFormat/Audio/Standalone/Background")]
	static void SetBackgroundAudio()
	{
		SetAudio(BackgroundAudioPath, true);
	}

	[MenuItem("SetFormat/Audio/Standalone/DrawCardDialogs")]
	static void SetDrawCardDialogsAudio()
	{
		SetAudio(DrawCardDialogsAudioPath);
	}

	[MenuItem("SetFormat/Audio/Standalone/Mainplay")]
	static void SetMainplayAudio()
	{
		SetAudio(MainplayAudioPath);
	}

	[MenuItem("SetFormat/Audio/Standalone/PhoneDialogs")]
	static void SetPhoneDialogsAudio()
	{
		SetAudio(PhoneDialogsAudioPath);
	}

	[MenuItem("SetFormat/Audio/Standalone/SoundEffect")]
	static void SetSoundEffectAudio()
	{
		SetAudio(SoundEffectAudioPath);
	}
	[MenuItem("SetFormat/Audio/Standalone/Dubbing")]
	static void SetDubbingAudio()
	{
		SetAudio(DubbingAudioPath);
	}

	private static readonly List<string> _audioPaths = new List<string> {BackgroundAudioPath,DrawCardDialogsAudioPath,MainplayAudioPath ,PhoneDialogsAudioPath,SoundEffectAudioPath,DubbingAudioPath,CoaxSleepAudioPath};
	private static int _curSetAudioIndex = 0;
	[MenuItem("SetFormat/Audio/Standalone/一键设置All 音频")]
	static void SetAllAudio()
	{
		ExecuteNextSetAudio();
	}

	static void ExecuteNextSetAudio()
	{
		var isLongAudio = _audioPaths[_curSetAudioIndex] == BackgroundAudioPath;	
		SetAudio(_audioPaths[_curSetAudioIndex], isLongAudio, () =>
		{
			_curSetAudioIndex++;
			if (_curSetAudioIndex>_audioPaths.Count-1)
			{
				_curSetAudioIndex = 0;
				return;
			}

			ExecuteNextSetAudio();
		});
	}
	
	
	
	static void SetAudio(string assetsPath,bool isLongAudio =false,Action callBack=null)
	{
		if (!Directory.Exists(assetsPath))
		{
			Debug.LogError("assetsPathErro===>"+assetsPath);
		}
		
		DirectoryInfo di = new DirectoryInfo(assetsPath);
		
		FileInfo[] files = di.GetFiles("*", SearchOption.TopDirectoryOnly)
			.Where(s=>s.Name.IndexOf(".meta",StringComparison.Ordinal)==-1).ToArray();

		for (int i = 0; i < files.Length ; i++)
		{
			var filePath = files[i].FullName.Replace("\\", "/");
			string[] sArray = filePath.Split( new string[]{"SuperStarGame/"}, StringSplitOptions.RemoveEmptyEntries);
				
			string path = sArray[1];
				
			EditorUtility.DisplayProgressBar("处理中>>>", path, (float)i / files.Length);
			
			AudioImporter ai = AssetImporter.GetAtPath(path) as AudioImporter;
			AudioImporterSampleSettings  ais =ai.defaultSampleSettings;

			if (isLongAudio)
			{
				if (IsAlreadySetLongAudio(ai,ais))
				{
					continue;					
				}

				SetLongAudio(ai, ais);
			}
			else
			{
				if (IsAlreadySetShortAudio(ai,ais))
				{
					continue;
				}
				SetShortAudio(ai,ais);
			}
			
			
			Debug.Log("处理的音频===>"+path);
			AssetDatabase.ImportAsset(path);
		}
		EditorUtility.ClearProgressBar();
		callBack?.Invoke();
		
	}

	static bool IsAlreadySetShortAudio(AudioImporter ai,AudioImporterSampleSettings  ais)
	{
		bool isSet = false;

		bool isOverride = ai.ContainsSampleSettingsOverride(Standalone);

		if (ais.loadType==ShortAudio && ai.loadInBackground && isOverride)
		{
			isSet = true;	
		}

		return isSet;
	}


	static void SetShortAudio(AudioImporter ai,AudioImporterSampleSettings  ais)
	{
		ai.loadInBackground = true;
		ais.loadType = ShortAudio;
		ai.defaultSampleSettings = ais;
		ai.SetOverrideSampleSettings(Standalone, ais);
	}
	
	static bool IsAlreadySetLongAudio(AudioImporter ai,AudioImporterSampleSettings  ais)
	{
		bool isSet = false;

		bool isOverride = ai.ContainsSampleSettingsOverride(Standalone);
		
		if (ais.loadType== LongAudio && ai.loadInBackground && isOverride)
		{
			isSet = true;
		}

		return isSet;
	}

	static void SetLongAudio(AudioImporter ai,AudioImporterSampleSettings  ais)
	{
		ai.loadInBackground = true;
		ais.loadType = LongAudio;
		ai.defaultSampleSettings = ais;
		ai.SetOverrideSampleSettings(Standalone, ais);
	}
	
	#endregion
}
