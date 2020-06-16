using System;
using System.Collections.Generic;


/// <summary>
/// 资源模块，对应功能模块
/// </summary>
[Serializable]
public class ResIndex
{
    /// <summary>
    /// 归属于模块
    /// </summary>
    public string belong;

    public string language;

    public Dictionary<string, ResPack> packageDict;
}