using System;
using System.Collections.Generic;
using System.IO;

public class BundleFilter
{
    public BlackList BlackList;

    public WhiteList WhiteList;

    public string DirPath;

    public string Id;

    public List<FileInfo> SearchFile()
    {
        FileInfo[] whiteFiles = null;
        FileInfo[] blackFiles = null;
        List<FileInfo> list = new List<FileInfo>();
        
        if (BlackList.List.Count == 0 && WhiteList.List.Count == 0)
        {
            string[] files = Directory.GetFiles(WhiteList.RootFolder, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.LastIndexOf(".manifest", StringComparison.Ordinal) != -1)
                    continue;
                list.Add(new FileInfo(file));
            }
        }
        else
        {
            blackFiles = BlackList.GetFiles();
            whiteFiles = WhiteList.GetFiles();
        }

        if (blackFiles != null) list.AddRange(blackFiles);

        if (whiteFiles != null) list.AddRange(whiteFiles);

        return list;
    }
}