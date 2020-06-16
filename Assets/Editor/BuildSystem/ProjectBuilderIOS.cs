using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

class ProjectBuilderIOS : ProjectBuilderAndroid
{
    protected new static void InitSetting()
    {
        ApplicationPath = Application.dataPath.Replace("/Assets", "");
        Scenes = FindEnabledEditorScenes();
        BuildTarget = BuildTarget.iOS;
        ScriptingImplementation = ScriptingImplementation.IL2CPP;
        BuildOptions = BuildOptions.AcceptExternalModificationsToPlayer;
        OutputPath = ApplicationPath + "/IOSProject";

        PlayerSettings.applicationIdentifier = "com.galasports.appstore";
        PlayerSettings.bundleVersion = "0.0.1";

        PlayerSettings.productName = "il2cpp";

		EditorUserBuildSettings.symlinkLibraries = false;
    }

#if UNITY_IOS

    [MenuItem("Build System/Build IOS Project Il2Cpp", false, 21)]
    static void BuildForIosProjectIl2Cpp()
    {
        InitSetting();
        doBuild();
    }

#endif
}