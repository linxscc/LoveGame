using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class AndroidAutoBuild : ProjectBuilderAndroid
{

    [MenuItem("Build System/==Android Build==/Publish Mono2x Project %#F12", false, 0)]
    public static void PublishMono2xProject()
    {
        BuildUtil.ClearConsole();
        BuildForAndroidProjectMono2x();      
        CopyWholeProjectToSDKBase();
        //CopyEncryptSo();
        //EncryptDll();
        
    }

    [MenuItem("Build System/==Android Build==/Publish Mono2x Project(不加密)", false, 20)]
    public static void PublishMono2xProjectNoEncrypt()
    {
        BuildUtil.ClearConsole();
        BuildForAndroidProjectMono2x();
        CopyWholeProjectToSDKBase();
    }
    
    [MenuItem("Build System/==Android Build==/Publish il2cpp Project(32位)", false, 1)]
    public static void Publishil2cppProject_32Bit()
    {
        BuildUtil.ClearConsole();
        BuildForAndroidProjectIl2Cpp_32Bit();
        CopyWholeProjectToSDKBase();
    }
    
    [MenuItem("Build System/==Android Build==/Publish il2cpp Project(32和64位) %#_p", false, 2)]
    public static void Publishil2cppProject_32A64Bit()
    {
        BuildUtil.ClearConsole();
        BuildForAndroidProjectIl2Cpp_32A64Bit();
        CopyWholeProjectToSDKBase();
    }
    
    [MenuItem("Build System/==Android Build==/Publish Obb Project(Il2Cpp)", false, 3)]
    public static void PublishIl2CppObbProject()
    {
        BuildUtil.ClearConsole();
        CompileObbProject();
        CopyWholeProjectToSDKBase();
    }
    

    public static void GradleBuildCommand(string cmd, bool isSowLog = true)
    {
        BuildUtil.processCommand(GradleBatFile, cmd, isSowLog);
    }

    [MenuItem("Build System/==Android Build==/Publish Mono2x Project(测试)", false, 21)]
    public static void PublishMono2xProjectDebug()
    {
        BuildOptions =
            BuildOptions.AllowDebugging |
            BuildOptions.Development |
            BuildOptions.ConnectWithProfiler |
            BuildOptions.AcceptExternalModificationsToPlayer;
        
        BuildUtil.ClearConsole();
        BuildForAndroidProjectMono2x();
        CopyWholeProjectToSDKBase();
    }

    [MenuItem("Build System/==Android Build==/CleanAndroidProject",false,5000)]
    public static void CleanAndroidProject()
    {
        GradleBuildCommand("clean");
    }

    public static void ChangeCGFile(string type)
    {
        FileInfo cgFileInfo;
        switch (type)
        {  
            case "繁体":
                cgFileInfo = new FileInfo(AndroidBuildDir + "/SDKBase/cgFile/HK/video.mp4");
                File.Copy(cgFileInfo.FullName,AndroidBuildDir + "/SDKBase/src/main/res/raw/video.mp4", true);
                break;
            case "简体":
                cgFileInfo = new FileInfo(AndroidBuildDir + "/SDKBase/cgFile/CN/video.mp4");
                File.Copy(cgFileInfo.FullName,AndroidBuildDir + "/SDKBase/src/main/res/raw/video.mp4", true);
                break;
        }
    }
    
    
    
    

}

