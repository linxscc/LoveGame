using System.IO;
using UnityEditor;

class AndroidGooglePlayBuild  : AndroidAutoBuild
{

    [MenuItem("Build System/==GooglePlay Build==/一键打包/正式包/香港台湾包", false, 120)]
    static void BuildOneKeyHK()
    {
        PublishIl2CppObbProject();
        BuildHK();
    }
        
    [MenuItem("Build System/==GooglePlay Build==/一键打包/正式包/马来西亚", false, 120)]
    static void BuildOneKeyMY()
    {
        PublishIl2CppObbProject();
        BuildMY();
    }

    [MenuItem("Build System/==GooglePlay Build==/一键打包/正式包/香港台湾包(不拆包)", false, 120)]
    static void BuildOneKeyHK_NoObb()
    {
        Publishil2cppProject_32A64Bit();
        BuildHK_NoObb();
    }
    [MenuItem("Build System/==GooglePlay Build==/一键打包/正式包/马来西亚(不拆包)", false, 120)]
    static void BuildOneKeyMY_NoObb()
    {
        Publishil2cppProject_32A64Bit();
        BuildMY_NoObb();
    }

    [MenuItem("Build System/==GooglePlay Build==/正式包/打包香港台湾", false, 120)]
    static void BuildHK()
    {
        ChangeCGFile("繁体");
        GradleBuildCommand("Oversea:assembleHktwWithobb --info");
    }
    [MenuItem("Build System/==GooglePlay Build==/正式包/打包马来西亚", false, 120)]
    static void BuildMY()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("Oversea:assembleMyWithobb --info");
    }

    [MenuItem("Build System/==GooglePlay Build==/正式包/打包香港台湾(不拆包)", false, 120)]
    static void BuildHK_NoObb()
    {
        ChangeCGFile("繁体");
        GradleBuildCommand("Oversea:assembleHktwRelease --info");
    }
    [MenuItem("Build System/==GooglePlay Build==/正式包/打包马来西亚(不拆包)", false, 120)]
    static void BuildMY_NoObb()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("Oversea:assembleMyRelease --info");
    }
        
    
    
    
    [MenuItem("Build System/==GooglePlay Build==/一键打包/内网//香港台湾包(不拆包)", false, 120)]
    static void BuildOneKeyHK_NoObbIntranet()
    {
        Publishil2cppProject_32A64Bit();
        BuildHK_NoObbIntranet();
    }
    [MenuItem("Build System/==GooglePlay Build==/一键打包/内网//马来西亚(不拆包)", false, 120)]
    static void BuildOneKeyMY_NoObbIntranet()
    {
        Publishil2cppProject_32A64Bit();
        BuildMY_NoObbIntranet();
    }
    

    [MenuItem("Build System/==GooglePlay Build==/一键打包/外网//香港台湾包(不拆包)", false, 120)]
    static void BuildOneKeyHK_NoObbDevelop()
    {
        Publishil2cppProject_32A64Bit();
        BuildHK_NoObbDevelop();
    }
    [MenuItem("Build System/==GooglePlay Build==/一键打包/外网//马来西亚(不拆包)", false, 120)]
    static void BuildOneKeyMY_NoObbDevelop()
    {
        Publishil2cppProject_32A64Bit();
        BuildMY_NoObbDevelop();
    }
    
    [MenuItem("Build System/==GooglePlay Build==/内网包/香港台湾(不拆包)", false, 120)]
    static void BuildHK_NoObbIntranet()
    {
        ChangeCGFile("繁体");
        GradleBuildCommand("Oversea:assembleHktwIntranet --info");
    }
    [MenuItem("Build System/==GooglePlay Build==/内网包/马来西亚(不拆包)", false, 120)]
    static void BuildMY_NoObbIntranet()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("Oversea:assembleMyIntranet --info");
    }
    
    [MenuItem("Build System/==GooglePlay Build==/外网包/香港台湾(不拆包)", false, 120)]
    static void BuildHK_NoObbDevelop()
    {
        ChangeCGFile("繁体");
        GradleBuildCommand("Oversea:assembleHktwDevelop --info");
    }
    [MenuItem("Build System/==GooglePlay Build==/外网包/马来西亚(不拆包)", false, 120)]
    static void BuildMY_NoObbDevelop()
    {
        ChangeCGFile("简体");
        GradleBuildCommand("Oversea:assembleMyDevelop --info");
    }

}
