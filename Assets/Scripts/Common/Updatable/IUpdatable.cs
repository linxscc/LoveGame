using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable  {
    /// <summary>
    /// 开始
    /// </summary>
    void Start();
    /// <summary>
    /// 暂停
    /// </summary>
    void Pause();
    /// <summary>
    /// 继续
    /// </summary>
    void Play();
    /// <summary>
    /// 更新
    /// </summary>
    void Update(float delaytime);
    /// <summary>
    /// 停止
    /// </summary>
    void Shutdown();
    /// <summary>
    /// 继续
    /// </summary>
}
