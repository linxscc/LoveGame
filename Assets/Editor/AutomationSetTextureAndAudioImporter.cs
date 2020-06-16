using System;
using UnityEditor;
using UnityEngine;



	public class AutomationSetTextureAndAudioImporter : AssetPostprocessor 
	{
        //音频路径
		private const string BackgroundAudio = "Assets/BundleAssets/Audio/";  
		private const string DrawCardDialogsAudio = "Assets/BundleAssets/Audio/DrawCardDialogs/"; 
		private const string MainPlayAudio = "Assets/BundleAssets/Audio/mainplay/";
		private const string PhoneDialogsAudio = "Assets/BundleAssets/Audio/PhoneDialogs/";
		private const string SoundEffectAudio = "Assets/BundleAssets/Audio/SoundEffect/";
		private const string DubbingAudio = "Assets/BundleAssets/Story/Dubbing/";
						
		//音频平台
		private static string Standalone = "Standalone";
		
		//音频格式
		private static  AudioClipLoadType LongAudio =  AudioClipLoadType.DecompressOnLoad;
		private static  AudioClipLoadType ShortAudio = AudioClipLoadType.Streaming;


		private const string Root = "Assets/BundleAssets/";
		private static bool IsNeedSet = false;
		
		//图片路径
		private const string Card = "Assets/BundleAssets/SingleFile/Card/";
		private const string FansTexture = "Assets/BundleAssets/SingleFile/FansTexture/";
		private const string HeadPath = "Assets/BundleAssets/SingleFile/Head/";
		private const string Live2DPath = "Assets/BundleAssets/SingleFile/Live2d/";
		private const string PhonePath = "Assets/BundleAssets/SingleFile/Phone/";		
		private const string StoryPath = "Assets/BundleAssets/Story/";
		//Prop路径暂时不处理
		
		//图片平台
		private const string iOS = "iPhone";     //苹果平台
		private const string Android = "Android";//安卓平台
		
		//图片Size
		private const int Size = 2048;
		
		//平台格式IOS
		private static TextureImporterFormat _iosSmallAlphaFormat = TextureImporterFormat.ASTC_RGBA_4x4;  //ios小图带透明
		private static TextureImporterFormat _iosBigAlphaFormat = TextureImporterFormat.ASTC_RGBA_8x8;   //ios大图带透明
		private static TextureImporterFormat _iosSmallFormat = TextureImporterFormat.ASTC_RGB_4x4;       //ios小图不带透明
		private static TextureImporterFormat _iosBigFormat = TextureImporterFormat.ASTC_RGB_8x8;         //ios大图不带透明                                                                                                            p


		private static TextureImporterFormat _androidSmallAlphaFormat = TextureImporterFormat.ETC2_RGBA8; //安卓小图带透明
		private static TextureImporterFormat _androidBigAlphaFormat = TextureImporterFormat.ETC2_RGBA8; //安卓大图带透明
		private static TextureImporterFormat _androidSmallFormat = TextureImporterFormat.ETC2_RGBA8;      //安卓小图不带透明
		private static TextureImporterFormat _androidBigFormat = TextureImporterFormat.ETC2_RGBA8;      //安卓大图不带透明
				
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,string[] movedFromPath)			
		{
			
			
			foreach (var path in importedAssets)
			{				
				var disposePath = DisposePath(path);

				if (disposePath.StartsWith(Root))
				{
					IsNeedSet = true;				
				}
				else
				{
					IsNeedSet = false;
					continue;
				}				
				Debug.LogError("开始自动化");				
				if (disposePath==BackgroundAudio)
				{
					SetLongAudio(path);
				}
				else if(disposePath==DrawCardDialogsAudio|| disposePath== MainPlayAudio|| disposePath== PhoneDialogsAudio||disposePath== SoundEffectAudio||disposePath== DubbingAudio)
				{
					SetShortAudio(path);
				}
				else if(path.Contains(Card)||path.Contains(FansTexture)||path.Contains(HeadPath)||path.Contains(PhonePath)||path.Contains(StoryPath))
				{
					SetNormalTexture(path);
				}
				else if (path.Contains(Live2DPath) )
				{
					SetSpecialTexture(path);
				}
				
				AssetDatabase.WriteImportSettingsIfDirty(path);
				
			}

			if (IsNeedSet)
			{
				AssetDatabase.Refresh();
				AssetDatabase.SaveAssets();
			}
			
		}

		//处理后的路径，不含文件名的
		static string DisposePath(string path)
		{
			var starIndex = path.LastIndexOf("/", StringComparison.Ordinal)+1;
			var endIndex = path.Length - starIndex;				
			return path.Remove(starIndex, endIndex);			
		}

		//设置长音频格式
		static void SetLongAudio(string path)
		{		
		    AudioImporter ai = AssetImporter.GetAtPath(path) as AudioImporter;
		    if (ai != null)
		    {
			  AudioImporterSampleSettings ais = ai.defaultSampleSettings;
			  	ai.loadInBackground = true;
			    ais.loadType = LongAudio;
			    ai.defaultSampleSettings = ais;
			    ai.SetOverrideSampleSettings(Standalone, ais);						    			 
			    Debug.Log("处理的长音频===>"+path);
			}
			else
		    {
				Debug.LogError("导入此路径的文件不是音频类型===>"+path);						
			}
		}

		//设置短音频格式
		static void SetShortAudio(string path)
		{			
			AudioImporter ai = AssetImporter.GetAtPath(path) as AudioImporter;
			if (ai != null)
			{
				AudioImporterSampleSettings ais = ai.defaultSampleSettings;
				ai.loadInBackground = true;
				ais.loadType = ShortAudio;
				ai.defaultSampleSettings = ais;
				ai.SetOverrideSampleSettings(Standalone, ais);
							
				Debug.Log("处理的短音频===>"+path);
			}
			else
			{
				Debug.LogError("导入此路径的文件不是音频类型===>"+path);						
			}		
		}
		
		//设置特殊（图集）图片
		static void SetSpecialTexture(string path)
		{
			if (path.EndsWith(".png") || path.EndsWith(".jpg"))
			{
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if (ti != null)
				{
#pragma warning disable 618
					ti.SetPlatformTextureSettings(iOS, Size, TextureImporterFormat.ASTC_RGBA_4x4);
#pragma warning restore 618
					ti.textureType = TextureImporterType.Sprite;
					ti.spriteImportMode = SpriteImportMode.Single;
					ti.alphaSource = TextureImporterAlphaSource.FromInput;
					ti.alphaIsTransparency = true;
					ti.sRGBTexture = true;
							
					Debug.Log("处理的图片资源路径===>"+path);		
				}
			}
		}
						
		//设置普通图片
		static void SetNormalTexture(string path)
		{
			if (path.EndsWith(".png") || path.EndsWith(".jpg"))
			{
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				Texture tx = AssetDatabase.LoadAssetAtPath<Texture>(path);
				if (tx!=null && ti!=null)
				{
					var isBig = IsBigTexture(tx);
					var isAlpha = IsHaveAlpha(ti);
							
					ti.textureType = TextureImporterType.GUI;
					ti.npotScale = TextureImporterNPOTScale.ToNearest;
							
					if (isAlpha)
					{
						ti.alphaSource = TextureImporterAlphaSource.FromInput;
#pragma warning disable 618
						ti.SetPlatformTextureSettings(iOS, Size, isBig ? _iosBigAlphaFormat : _iosSmallAlphaFormat);
						ti.SetPlatformTextureSettings(Android,Size,isBig?_androidBigAlphaFormat:_androidSmallAlphaFormat);
#pragma warning restore 618
					}
					else
					{
						ti.alphaSource = TextureImporterAlphaSource.None;
#pragma warning disable 618
						ti.SetPlatformTextureSettings(iOS, Size, isBig ? _iosBigFormat : _iosSmallFormat);
						ti.SetPlatformTextureSettings(Android,Size,isBig?_androidBigFormat:_androidSmallFormat);
#pragma warning restore 618
					}
					
					Debug.Log("处理的图片资源路径===>"+path);					
				}
			}
		}
					
		//是否是大的图
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
	}
	
	



