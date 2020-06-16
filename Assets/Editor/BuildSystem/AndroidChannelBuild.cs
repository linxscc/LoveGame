using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;


class AndroidChannelBuild : AndroidAutoBuild
{
    /////////////////////////////内网包///////////////////////////////////////////
    [MenuItem("Build System/Android渠道打包/内网/官网内网包", false, 0)]
    static void BuildBigStarOfficialIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("official:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/一键打包官网内网包", false, 10)]
    static void BuildBigStarOfficialIntranetOnekey()
    {
        Publishil2cppProject_32Bit();
        BuildBigStarOfficialIntranet();
    }
    
    [MenuItem("Build System/Android渠道打包/内网/OPPO包", false, 11)]
    static void BuildBigStarOppoIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Oppo:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/美图包", false, 12)]
    static void BuildBigStarMeituIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Meitu:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/Bilibili包", false, 13)]
    static void BuildBigStarBilibiliIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Bilibili:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/小米包", false, 14)]
    static void BuildBigStarXiaomiIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Xiaomi:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/VIVO包", false, 15)]
    static void BuildBigStarVivoIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Vivo:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/百度包", false, 16)]
    static void BuildBigStarBaiduIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Baidu:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/360包", false, 17)]
    static void BuildBigStar360Intranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":360:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/4399包", false, 18)]
    static void BuildBigStar4399Intranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":4399:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/阿里九游包", false, 19)]
    static void BuildBigStarAli9youIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Ali9you:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/华为包", false, 20)]
    static void BuildBigStarHuaweiIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Huawei:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/快看包", false, 21)]
    static void BuildBigStarKuaikanIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Kuaikan:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/芒果TV包", false, 22)]
    static void BuildBigStarMgtvIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Mgtv:assembleIntranet");
    }
    
    [MenuItem("Build System/Android渠道打包/内网/应用宝包", false, 23)]
    static void BuildBigStarYsdkIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Ysdk:assembleIntranet");
    }
                                                     
    /////////////////////////////外网包///////////////////////////////////////////
    [MenuItem("Build System/Android渠道打包/正式服/官网包", false, 0)]
    static void BuildBigStarOfficialRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("official:assembleRelease");
    }

    [MenuItem("Build System/Android渠道打包/正式服/一键打包官网包", false, 10)]
    static void BuildBigStarOfficialReleaseOnekey()
    {
        Publishil2cppProject_32Bit();
        BuildBigStarOfficialRelease();
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/OPPO包", false, 11)]
    static void BuildBigStarOppoRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Oppo:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/美图包", false, 12)]
    static void BuildBigStarMeituRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Meitu:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/Bilibili包", false, 13)]
    static void BuildBigStarBilibiliRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Bilibili:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/小米包", false, 14)]
    static void BuildBigStarXiaomiRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Xiaomi:assembleRelease");
    }
   
    
    [MenuItem("Build System/Android渠道打包/正式服/VIVO包", false, 15)]
    static void BuildBigStarVivoRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Vivo:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/百度包", false, 16)]
    static void BuildBigStarBaiduRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Baidu:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/360包", false, 17)]
    static void BuildBigStar360Release()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":360:assembleRelease");
    }
   
    [MenuItem("Build System/Android渠道打包/正式服/4399包", false, 18)]
    static void BuildBigStar4399Release()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":4399:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/阿里九游包", false, 19)]
    static void BuildBigStarAli9youRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Ali9you:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/华为包", false, 20)]
    static void BuildBigStarHuaweiRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Huawei:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/快看包", false, 21)]
    static void BuildBigStarKuaikanRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Kuaikan:assembleRelease");
    }

    [MenuItem("Build System/Android渠道打包/正式服/芒果TV包", false, 22)]
    static void BuildBigStarMgtvRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Mgtv:assembleRelease");
    }
    
    [MenuItem("Build System/Android渠道打包/正式服/应用宝包", false, 23)]
    static void BuildBigStarYsdkRelease()
    {
        ChangeCGFile("简体");
        GradleBuildCommand(":Ysdk:assembleRelease");
    }
    
    /// <summary>
    /// 此方法是从Jenkins上接收数据并且开始打包的方法
    /// 使用该方法默认不需要使用ModifyVersionFromJenkins
    /// 修改gradle的运行时变量
    /// </summary>
    static void BuildFromJenkins()
    {
        Console.WriteLine("Clean Gradle");
        //先clean gradle
//        CleanAndroidProject();
        Console.WriteLine("BuildFromJenkins start");
        //获取目标包的target
        string PublishUnityProject = GetJenkinsParameter("PublishUnityProject");
        string PublishUnityBundles = GetJenkinsParameter("PublishUnityBundles");
        Console.WriteLine("发布工程:"+PublishUnityProject);
        Console.WriteLine("发布bundles:"+PublishUnityBundles);
        if (!"none".Equals(PublishUnityBundles))
        {
            PublishRes.OneKeyPublishAllBundle();
        }
        switch (PublishUnityProject)
        {
            case "None":
                break;
            case "Il2cpp":
                Publishil2cppProject_32A64Bit();
                break;
            case "Mono":
                PublishMono2xProject();
                break;
        }
        Console.WriteLine("BuildFromJenkins end");
    }
    
    /// <summary>
    /// 通过key匹配接收到的Jenkins传输的参数,返回value或者""
    /// </summary>
    static string GetJenkinsParameter(string key)
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.Contains(key))
            {
                string value = arg.Replace("{=}", "=").Split('=')[1];
                Console.WriteLine("key-->" + key + "    value-->" + value);
                return value;
            }
        }
        return "";
    }
    
    /**
    [MenuItem("Build System/Android渠道打包/发布所有渠道apk", false, 210)]
    public static void PublishAllApk()
    {
        PublishMono2xProject();

        GradleBuildCommand("assembleRelease");
    }
    */
    
    ////////////////////////////////////////////////批量Android打包//////////////////////////////////////////////////
    [MenuItem("Build System/Android渠道打包/正式服/===所有国内包===", false, 50)]
    public static void AndroidBatch()
    {
        Publishil2cppProject_32Bit();
        ChangeCGFile("简体");
        GradleBuildCommand(":Baidu:assembleRelease");
        GradleBuildCommand(":Xiaomi:assembleRelease");
        GradleBuildCommand("official:assembleRelease");
        GradleBuildCommand(":Mgtv:assembleRelease");
        GradleBuildCommand(":Ysdk:assembleRelease");
        GradleBuildCommand(":Kuaikan:assembleRelease");
        GradleBuildCommand(":Huawei:assembleRelease");
        GradleBuildCommand(":Oppo:assembleRelease");
        GradleBuildCommand(":Meitu:assembleRelease");
        GradleBuildCommand(":Bilibili:assembleRelease");
        
        GradleBuildCommand(":Vivo:assembleRelease");
        GradleBuildCommand(":360:assembleRelease");
        GradleBuildCommand(":Ali9you:assembleRelease");
        GradleBuildCommand(":4399:assembleRelease");
    }
    
}