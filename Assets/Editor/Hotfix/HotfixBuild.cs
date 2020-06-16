using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AssetBundleTool;
using UnityEditor;
using UnityEngine;
using FileUtil = Framework.Utils.FileUtil;

public class HotfixBuild
{
    public static string HotfixDir = Application.dataPath.Replace("SuperStarGame/Assets", "Hotfix");

    public static string BundleFilesDir
    {
        get
        {
#if UNITY_EDITOR_OSX
            string platform = "iOS";
#elif UNITY_EDITOR
            string platform = "Android";
#endif
            return HotfixDir + "/AssetBundles/" + platform;
        }
    }

    public static string LuaFilesDir = HotfixDir + "/LuaFiles";

    public static int GetHotfixVersion()
    {
        string vFile = HotfixDir + "/version.txt";
        string str = FileUtil.ReadFileText(vFile);
        int ret = int.Parse(str);

        return ret;
    }
    
    [MenuItem("Hotfix/加密Lua",false,10)]
    public static void SignatureLua()
    {
        Process process = new Process();

        string path = Application.dataPath.Replace("Assets", "Tools");
        process.StartInfo.FileName = path + "/FilesSignature.exe";
        process.StartInfo.WorkingDirectory = path;
        process.StartInfo.Arguments = PackageManager.I18NDataPath + "/LuaFiles " +
                                      AssetBundleHelper.GetOuterStreamingAssetsPath() + "/LuaFiles";
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
        process.WaitForExit();
    }

    [MenuItem("Hotfix/初始化热更目录", false, 200)]
    public static void InitHotfixDir()
    {
        if (Directory.Exists(HotfixDir) == false)
        {
            Directory.CreateDirectory(HotfixDir);
        }

        if (Directory.Exists(BundleFilesDir) == false)
        {
            Directory.CreateDirectory(BundleFilesDir);
        }

        if (Directory.Exists(LuaFilesDir) == false)
        {
            Directory.CreateDirectory(LuaFilesDir);
        }

        GenerateVersionFile();

        System.Diagnostics.Process.Start(HotfixDir);
    }

    [MenuItem("Hotfix/打开版本控制文件", false, 100)]
    public static void OpenVersionFile()
    {
        System.Diagnostics.Process.Start(GenerateVersionFile());
    }

    private static string GenerateVersionFile()
    {
        string vFile = HotfixDir + "/version.txt";

        if (File.Exists(vFile) == false)
        {
            FileStream fs = File.Create(vFile);
            byte[] bytes = Encoding.ASCII.GetBytes("1");
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        return vFile;
    }

    [MenuItem("Hotfix/打开热更目录", false, 110)]
    public static void OpenHotfixDir()
    {
        System.Diagnostics.Process.Start(HotfixDir);
    }
}