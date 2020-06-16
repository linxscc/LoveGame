using System;
using Assets.Scripts.Module.Download;

/// <summary>
/// 单个bundle文件
/// </summary>
[Serializable]
public class ResItem : IDownloadItem
{
    public long Size { get; set; }
    public string Path { get; set; }
    public string Md5 { get; set; }
    public FileType FileType { get; set; }
}