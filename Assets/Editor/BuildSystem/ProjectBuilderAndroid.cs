using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using FileUtil = UnityEditor.FileUtil;

class ProjectBuilderAndroid : Editor
{
    protected static string[] Scenes;
    protected static string ApplicationPath;
    protected static string OutputPath;
    protected static BuildTarget BuildTarget;
    protected static ScriptingImplementation ScriptingImplementation;
    protected static BuildOptions BuildOptions;
    protected static string TargetName;

    protected static string SdkbaseAssetsDir;
    protected static string SdkbaseDllDir;
    protected static string SdkbaseSoDir;
    protected static string SdkbaseJarsDir;

    protected static string AndroidBuildDir;
    protected static string GradleBatFile;
    protected static string BsdiffFile;
    protected static string HotfixDir;

    protected static string GpSdkDllDir;
    protected static string GpSdkAssetsDir;

    public static string[] FindEnabledEditorScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (PlayerSettings.Android.useAPKExpansionFiles == false &&
                scene.path.ToLower().IndexOf("obb", StringComparison.Ordinal) != -1)
            {
                continue;
            }

//            if (!scene.enabled) continue;

            scene.enabled = true;

            editorScenes.Add(scene.path);
            Debug.LogWarning("Scene Name-> " + scene.path);
        }

        Debug.LogWarning("Scene Num-> " + editorScenes.Count);
        return editorScenes.ToArray();
    }

    [InitializeOnLoadMethod]
    static void InitSetting()
    {
        Debug.Log("Init Build Setting");

        #region GetCommandLineArgs

        //        string[] args = System.Environment.GetCommandLineArgs();
        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            args[i] 

        #endregion

        ApplicationPath = Application.dataPath.Replace("/SuperStarGame/Assets", "");

        BuildTarget = BuildTarget.Android;
        ScriptingImplementation = ScriptingImplementation.IL2CPP;
        BuildOptions = BuildOptions.AcceptExternalModificationsToPlayer;
        OutputPath = ApplicationPath + "/AndroidProject";

        AndroidBuildDir = ApplicationPath + "/AndroidBuildBigstar";

        SdkbaseAssetsDir = AndroidBuildDir + "/SDKBase/src/assets";
        SdkbaseDllDir = AndroidBuildDir + "/SDKBase/src/assets/bin/Data/Managed";
        SdkbaseSoDir = AndroidBuildDir + "/SDKBase/src/main/jniLibs";
        SdkbaseJarsDir = AndroidBuildDir + "/SDKBase/libs";

        GradleBatFile = AndroidBuildDir + "/gradlew.bat";

        BsdiffFile = AndroidBuildDir + "/files/bsdiff.exe";

        HotfixDir = AndroidBuildDir + "/files/hotfix";


        GpSdkDllDir = AndroidBuildDir + "/gpsdk/src/assets/bin/Data/Managed";
        GpSdkAssetsDir = AndroidBuildDir + "/gpsdk/src/assets";


        TargetName = null;

        PlayerSettings.Android.useAPKExpansionFiles = false;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.galasports.bigstar");
        PlayerSettings.bundleVersion = "0.0.1";
        PlayerSettings.productName = "SuperStarGame";
    }

    [MenuItem(USE_BUNDLE, true)]
    static bool ScriptingDefineSymbolsCheck()
    {
        Menu.SetChecked(USE_BUNDLE, PlayerSettings
            .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
            .Contains("USE_BUNDLE"));

        return true;
    }

    public const string USE_BUNDLE = "Build System/ScriptingDefineSymbols/USE_BUNDLE";

    [MenuItem(USE_BUNDLE, false)]
    static void SetScriptingDefineSymbols()
    {
        if (PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup) ==
            "HOTFIX_ENABLE;USE_BUNDLE")
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                "HOTFIX_ENABLE");
        }
        else
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                "HOTFIX_ENABLE;USE_BUNDLE");
        }
    }

    [MenuItem("Build System/==Android Build==/Copy Project Files/copy DLL")]
    static void CopyDLLToSDKBase()
    {
        DirectoryInfo dirFrom =
            new DirectoryInfo(OutputPath + "/" + PlayerSettings.productName + "/src/main/assets/bin/Data/Managed");
        if (dirFrom.Exists == false)
        {
            Debug.LogError("DLL 文件不存在");
            return;
        }

        DirectoryInfo dirTo = new DirectoryInfo(SdkbaseDllDir);
        if (dirTo.Exists)
            dirTo.Delete(true);
        UnityEditor.FileUtil.CopyFileOrDirectory(dirFrom.ToString(), SdkbaseDllDir);

//        FileInfo[] files = di.GetFiles();
//        for (int i = 0; i < files.Length; i++)
//        {
//            File.Copy(files[i].ToString(), SdkbaseDllDir + "/" + files[i].Name);
//        }
    }

    [MenuItem("Build System/==Android Build==/Copy Project Files/copy Library(so, jar)")]
    protected static void CopySOToSDKBase()
    {
        DirectoryInfo dirFrom = new DirectoryInfo(OutputPath + "/" + PlayerSettings.productName + "/src/main/jniLibs");
        if (dirFrom.Exists == false)
        {
            Debug.LogError("SO 文件不存在");
            return;
        }

        DirectoryInfo dirTo = new DirectoryInfo(SdkbaseSoDir);
        if (dirTo.Exists)
            dirTo.Delete(true);
        UnityEditor.FileUtil.CopyFileOrDirectory(dirFrom.ToString(), SdkbaseSoDir);

        //不拷贝lib下文件
//        DirectoryInfo jarDir = new DirectoryInfo(SdkbaseJarsDir);
//        if (jarDir.Exists)
//            jarDir.Delete(true);
//
//        UnityEditor.FileUtil.CopyFileOrDirectory(OutputPath + "/" + PlayerSettings.productName + "/libs", SdkbaseJarsDir);
    }
    
    [MenuItem("Build System/==Android Build==/Copy Project Files/copy Whole Project")]
    public static void CopyWholeProjectToSDKBase()
    {
        Stopwatch sw = Stopwatch.StartNew();
        CopySOToSDKBase();
        //获取发布后的资源目录
        DirectoryInfo resourceDirInfo =
            new DirectoryInfo(OutputPath + "/" + PlayerSettings.productName + "/src/main/assets");
        //获取准备拷贝至的目录(android正式工程资源目录)
        DirectoryInfo dirTo = new DirectoryInfo(SdkbaseAssetsDir);
        //清除之前的数据并且重新创建文件夹
        if (dirTo.Exists)
            dirTo.Delete(true);
        dirTo.Create();
        //获取资源目录下的全部文件
        FileInfo[] allFilesTo = resourceDirInfo.GetFiles("*", SearchOption.AllDirectories);

        //临时变量
        DirectoryInfo dir;
        foreach (FileInfo info in allFilesTo)
        {
            //获取到应该拷贝至的文件夹
            //android正式工程目录+(源文件目录-资源目录)
            dir = new DirectoryInfo(SdkbaseAssetsDir + info.DirectoryName.Replace(resourceDirInfo.FullName, ""));
            //如果文件夹不存在则创建
            if (!dir.Exists)
            {
                dir.Create();
            }

            //拷贝文件
            File.Copy(info.FullName,
                SdkbaseAssetsDir + info.DirectoryName.Replace(resourceDirInfo.FullName, "") + "/" + info.Name, true);
        }

        //拷贝StreamingAssets中除AssetBundles以外文件
        //获取发布后的资源目录
        DirectoryInfo StreamingAssetsDirInfo =
            new DirectoryInfo(OutputPath.Replace("AndroidProject",PlayerSettings.productName) + "/StreamingAssets");
        //获取资源目录下的全部文件
        FileInfo[] allStreamingAssetsFilesTo = StreamingAssetsDirInfo.GetFiles("*", SearchOption.AllDirectories);

        foreach (FileInfo info in allStreamingAssetsFilesTo)
        {
            //获取到应该拷贝至的文件夹
            //android正式工程目录+(源文件目录-资源目录)
            dir = new DirectoryInfo(SdkbaseAssetsDir + info.DirectoryName.Replace(StreamingAssetsDirInfo.FullName, ""));

            //删除manifest文件
            if (info.Name.Contains(".manifest"))
            {
                info.Delete();
                continue;
            }
            
            //跳过ab文件夹
            if (info.DirectoryName.Contains("AssetBundles"))
            {
                continue;
            }
            
            //如果文件夹不存在则创建
            if (!dir.Exists)
            {
                dir.Create();
            }
            
            //拷贝文件
            File.Copy(info.FullName,
                SdkbaseAssetsDir + info.DirectoryName.Replace(StreamingAssetsDirInfo.FullName, "") + "/" + info.Name, true);
        }
        
        FileInfo cfgFile = new FileInfo(dirTo + "/game_config.properties");
        if (cfgFile.Exists)
        {
            cfgFile.Delete();
        }

        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.LogError("复制项目文件耗时：" + time + "ms");
    }
    
    
    [MenuItem("Build System/==Android Build==/Copy Project Files/copy Mono so", false)]
    public static void CopyEncryptSo()
    {
        FileInfo gpatchFrom = new FileInfo(AndroidBuildDir + "/files/libgpatch.so");
        FileInfo gpatchTo = new FileInfo(SdkbaseSoDir + "/armeabi-v7a/libgpatch.so");
        File.Copy(gpatchFrom.FullName, gpatchTo.FullName, true);

        FileInfo monoFrom = new FileInfo(AndroidBuildDir + "/files/libmono.so");
        FileInfo monoTo = new FileInfo(SdkbaseSoDir + "/armeabi-v7a/libmono.so");
        File.Copy(monoFrom.FullName, monoTo.FullName, true);
    }

    [MenuItem("Build System/==Android Build==/Copy Project Files/Encrypt Dll", false)]
    public static void EncryptDll()
    {
        FileInfo file = new FileInfo(SdkbaseDllDir + "/Assembly-CSharp.dll");
        if (file.Exists == false)
        {
            Debug.LogError("DLL文件不存在 " + file.FullName);
            throw new Exception("DLL文件不存在 " + file.FullName);
            return;
        }

        BuildUtil.processCommand(AndroidBuildDir + "/files/encrypt.exe", file.FullName + " " + file.FullName, false);
    }

    [MenuItem("Build System/==Android Build==/Build Project Mono2x", false, 103)]
    public static void BuildForAndroidProjectMono2x()
    {
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        ScriptingImplementation = ScriptingImplementation.Mono2x;
        doBuild();
    }

        
    [MenuItem("Build System/==Android Build==/Build Project Il2Cpp(32位)", false, 104)]
    public static void BuildForAndroidProjectIl2Cpp_32Bit()
    {
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        ScriptingImplementation = ScriptingImplementation.IL2CPP;
        doBuild();
    }
    
    [MenuItem("Build System/==Android Build==/Build Project Il2Cpp(32位及64位)", false, 105)]
    public static void BuildForAndroidProjectIl2Cpp_32A64Bit()
    {
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        ScriptingImplementation = ScriptingImplementation.IL2CPP;
        doBuild();
    }

    [MenuItem("Build System/==Android Build==/Build Apk Mono2x", false, 101)]
    static void BuildForAndroidApkMono2x()
    {
        TargetName = "BasketBallMono.apk";
        ScriptingImplementation = ScriptingImplementation.Mono2x;
        BuildOptions = BuildOptions.None;

        doBuild();
    }

    [MenuItem("Build System/==Android Build==/Build Apk Il2cpp", false, 102)]
    static void BuildForAndroidIl2Cpp()
    {
        TargetName = "NewSoccer.apk";
        BuildOptions = BuildOptions.None;

        doBuild();
    }

    protected static void doBuild()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
            "HOTFIX_ENABLE;USE_BUNDLE");
        
        CSObjectWrapEditor.Generator.GenAll();

        DirectoryInfo dir = new DirectoryInfo(OutputPath + "/" + PlayerSettings.productName);
        if (dir.Exists)
            dir.Delete(true);

        if (TargetName != null)
        {
            dir.Create();
            OutputPath += "/" + TargetName;
        }

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget);
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        PlayerSettings.SetPropertyInt("ScriptingBackend", (int) ScriptingImplementation, BuildTarget);

        EditorUserBuildSettings.buildScriptsOnly = true;

        Scenes = FindEnabledEditorScenes();
        BuildPipeline.BuildPlayer(Scenes, OutputPath, BuildTarget, BuildOptions);

        Debug.Log("Application.buildGUID===========>" + Application.buildGUID);
    }


    ////////////////////////////////////////////PlayerSettings.Android.useAPKExpansionFiles===OBB/////////////////////
    [MenuItem("Build System/==GooglePlay Build==/CompileObbProject", false, 120)]
    public static void CompileObbProject()
    {
        PlayerSettings.Android.useAPKExpansionFiles = true;
        PlayerSettings.productName = "SuperStarGame";
        BuildForAndroidProjectIl2Cpp_32A64Bit();
    }
}