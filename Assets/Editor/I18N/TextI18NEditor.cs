/*
 * ==============================================================================
 * File Name: LocalizationTextEditor.cs
 * Description: 用来扩展Text，增加显示属性
 * ==============================================================================
*/

using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(TextI18N))]
public class TextI18NEditor : UnityEditor.UI.TextEditor
{

    private SerializedProperty m_Text;
    private SerializedProperty m_FontData;
    private SerializedProperty _useTestString;
    private SerializedProperty _testString;
    private SerializedProperty _isOpenLocalize;


    protected override void OnEnable()
    {
        base.OnEnable();
        this.m_Text = this.serializedObject.FindProperty("m_Text");
        this.m_FontData = this.serializedObject.FindProperty("m_FontData");
        this._useTestString = this.serializedObject.FindProperty("_useTestString");
        this._testString = this.serializedObject.FindProperty("_testString");
        this._isOpenLocalize = this.serializedObject.FindProperty("IsOpenLocalize");
    }
    
    public override void OnInspectorGUI()
    {
        TextI18N component = (TextI18N)target;
        
        serializedObject.Update();
        
        component.Key = EditorGUILayout.TextField("Key", component.Key);
        
        EditorGUILayout.PropertyField(_testString);
        EditorGUILayout.PropertyField(_useTestString);

        
        component.CustomFont = (I18NFont)EditorGUILayout.ObjectField("Custom Font", component.CustomFont, typeof(I18NFont), true);

        EditorGUILayout.PropertyField(_isOpenLocalize);

        EditorGUILayout.PropertyField(m_Text);
        EditorGUILayout.PropertyField(m_FontData);
        AppearanceControlsGUI();
        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
        
        
    }
}
