    using System;
    using System.Collections.Generic;
    using System.IO;

    public class BlackList : NameList
    {
        public FileInfo[] GetFiles()
        {
            string[] files = Directory.GetFiles(RootFolder, "*", SearchOption.AllDirectories);

            List<FileInfo> fileList = new List<FileInfo>();
            if(List.Count > 0)
            {
                foreach (var str in files)
                {
                    if (str.LastIndexOf(".manifest", StringComparison.Ordinal) != -1)
                        continue;
                    
                    string path = str.Replace("\\", "/");
                    bool hasKey = false;
                    foreach (var name in List)
                    {
                        if (path.Contains(name))
                        {
                            hasKey = true;
                            break;
                        }
                    }
                    if(hasKey == false)
                    {
                        fileList.Add(new FileInfo(path));
                    }
                }
            }

            return fileList.ToArray();
        }
    }
