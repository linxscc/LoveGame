using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


public class UD_ChangeFontType : Editor
{
	[MenuItem("Batch/字体改变为黑体")]
    
	static void Init()
	{
		Debug.Log("字体改变开始，请等待……");
		string kRootPath = "/Resources/";//这是Assest目录下的路径
		string kTexDir = Application.dataPath + kRootPath;
		string[] kFileList = Directory.GetFiles(kTexDir, "*.prefab", SearchOption.AllDirectories);
		Debug.Log("kFileList==>"+kFileList.Length);
		List<string> kList = new List<string>(kFileList);

		Font font = Resources.Load<Font>("Fonts/lantinghei");
		
		foreach (string file in kList)
		{
			int nPos = file.IndexOf("Assets");
			string kTexAssetPath = file.Substring(nPos);
			GameObject kUIObj = AssetDatabase.LoadAssetAtPath(kTexAssetPath, typeof(GameObject)) as GameObject;

			GameObject obj = PrefabUtility.InstantiatePrefab(kUIObj) as GameObject;
			Text[] childrenText = obj.transform.GetComponentsInChildren<Text>(true);
			for (int i = 0; i < childrenText.Length; i++)
			{
				childrenText[i].font = font;
			}

			try
			{
				PrefabUtility.ReplacePrefab(obj, kUIObj);
			}
			catch (System.Exception ex)
			{
				{
#if UNITY_EDITOR
					Debug.LogError(ex.ToString() + kUIObj.name);
#endif
				}

			}
			GameObject.DestroyImmediate(obj);
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		Debug.Log("字体改变结束");
	}
     
}
