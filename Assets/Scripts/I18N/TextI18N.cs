using System;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Custom Text Control used for localization text.
/// </summary>
[AddComponentMenu("UI/Text_I18N", 10)]
[DisallowMultipleComponent]
public class TextI18N : Text
{
    protected override void Awake()
    {
        base.Awake();
        if (CustomFont != null)
        {
            font = CustomFont.UseFont;
        }
    }

    /// <summary>
    /// 文本的key
    /// </summary>
    public string Key;

    /// <summary>
    /// 自定义字体，方便后期替换
    /// </summary>
    public I18NFont CustomFont;

    /// <summary>
    /// 是否开启自身的本地化
    /// </summary>
    [SerializeField]
    public bool IsOpenLocalize = true;

    /// <summary>
    /// 重新本地化，用于游戏内切换语言时调用
    /// </summary>
    public void OnLocalize()
    {
        if (IsOpenLocalize)
        {
            text = I18NManager.Get(Key);
        }
    }


#if UNITY_EDITOR
    [SerializeField]
    private bool _useTestString = true;
    
    [TextArea(3, 10)]
    [SerializeField]
    private string _testString;
    public string TestString
    {
        get { return _testString; }
        set
        {
            _testString = value;
            if (!string.IsNullOrEmpty(Key))
            {
                if (m_Text != _testString)
                {
                    m_Text = _testString;
                    SetVerticesDirty();
                    SetLayoutDirty();
                }
            }
        }
    }
#endif
    
    public override string text
    {
        get
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                if (_useTestString)
                {
                    m_Text = _testString;
                }
                else if (IsOpenLocalize)
                {
                    m_Text = I18NManager.Get(Key);
                }
                return m_Text;
            }
#endif
            if (IsOpenLocalize && !string.IsNullOrEmpty(Key))
            {
                m_Text = I18NManager.Get(Key);
            }
            return m_Text;
        }
        set
        {
            if (IsOpenLocalize == false)
            {
                m_Text = value;
                SetVerticesDirty();
                SetLayoutDirty();
            }
            else if (string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(m_Text))
                    return;
                m_Text = "";
                SetVerticesDirty();
            }
            else if (m_Text != value)
            {
                m_Text = value;
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }
    }
}