using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using Debug = UnityEngine.Debug;

class BuildUtil : UnityEditor.Editor
{

    public static bool GlobalSwitch = false;

    [MenuItem("Build System/Clear Console %#DOWN")]
    public static void ClearConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type logEntries = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
    }


    public static void processCommand(string command, string argument, bool isShowLog = true)
    {
        ProcessStartInfo start = new ProcessStartInfo(command);
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = false;
//        start.WindowStyle = ProcessWindowStyle.Hidden;

        if (File.Exists(command))
            start.WorkingDirectory = Directory.GetParent(command).FullName;

        if (start.UseShellExecute)
        {
            start.RedirectStandardOutput = false;
            start.RedirectStandardError = false;
            start.RedirectStandardInput = false;
        }
        else
        {
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.StandardOutputEncoding = System.Text.Encoding.UTF8;
            start.StandardErrorEncoding = System.Text.Encoding.UTF8;
        }

        Process p = Process.Start(start);

        string info2 = p.StandardOutput.ReadToEnd();
        string info = p.StandardError.ReadToEnd();

        if (!start.UseShellExecute && isShowLog)
        {
            string path = start.WorkingDirectory.ToString() + "/build.log";
            File.WriteAllText(path, info2);

            System.Diagnostics.Process.Start(path);

//            FileInfo fi = new FileInfo(start.WorkingDirectory.ToString()+"/build.log");
//            FileStream fs = fi.OpenWrite();
//            fs.Write();

//            if (isShowInfoWindow)
//            {
//                BuildInfoWindow.InitWindow(info2 + "\n" + info, "Gradle Log");
//            }
        }
        
        p.WaitForExit();
        p.Close();
    }
}