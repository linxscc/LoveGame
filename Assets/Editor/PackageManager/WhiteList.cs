using System;
using System.Collections.Generic;
using System.IO;

public class WhiteList : NameList
{
    public FileInfo[] GetFiles()
    {
        string[] files = Directory.GetFiles(RootFolder, "*", SearchOption.AllDirectories);

        List<FileInfo> fileList = new List<FileInfo>();
        if(List.Count > 0)
        {
            foreach (var key in List)
            {
                foreach (var file in files)
                {
                    if (file.LastIndexOf(".manifest", StringComparison.Ordinal) != -1)
                        continue;
                    
                    string path = file.Replace("\\", "/");
                    if (path.Contains(key))
                    {
                        fileList.Add(new FileInfo(path));
                        break;
                    }
                }
            }
        }

        return fileList.ToArray();
    }
}