using System;
using System.Collections.Generic;
using Assets.Scripts.Module.Download;

/// <summary>
/// 资源文件压缩包
/// </summary>
[Serializable]
public class ResPack
{
    /// <summary>
    /// 解压相对路径
    /// </summary>
    public string releasePath;
    
    /// <summary>
    /// 服务器相对路径
    /// </summary>
    public string downloadPath;

    public string id;

    public FileType packageType;
    
    public string packageMd5;
    
    public long packageSize;
    
    /// <summary>
    /// bundle文件信息列表
    /// </summary>
    public List<ResItem> items;
}