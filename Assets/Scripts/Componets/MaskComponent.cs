using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI Mask组件
/// </summary>
[AddComponentMenu("UI/MaskComponent")]
[RequireComponent(typeof(RawImage))]
public class MaskComponent : MonoBehaviour {

    /// <summary>
    /// 遮罩贴图
    /// </summary>
    [Header("遮罩贴图")]
    public Texture Mask;
    /// <summary>
    /// 偏移原始大小 (0,0);
    /// </summary>
    [Header("底图偏移")]
    public Vector2 Offset = Vector2.zero;
    /// <summary>
    /// 遮罩缩放 原始大小 (1,1);
    /// </summary>
    [Header("遮罩缩放")]
    public Vector2  Scale = Vector2.one;
    //public Vector2  Size = Vector2.one;
    // [SerializeField] Shader _kernels;
    private Material _material;

    private void Start()
    {
        Debug.LogError("Start .......");
        HandleMask();
    }

    void HandleMask()
    {
        var img = GetComponent<RawImage>().texture;
        if (img == null)
        {
            return;
        }
        if (_material == null)
        {
            var shader = Shader.Find("ImageEffect/UIMaskShader");
            _material = new Material(shader);
            _material.hideFlags = HideFlags.HideAndDontSave;
        }
        RectTransform rect = transform.GetRectTransform();
        float XOffset = Offset.x / rect.sizeDelta.x;
        float YOffset = Offset.y / rect.sizeDelta.y;
        _material.SetTexture("_Mask", Mask);
        Scale.x = Scale.x == 0 ? 0.0001f : Scale.x;
        Scale.y = Scale.y == 0 ? 0.0001f : Scale.y;
        float ScaleW = 0;
        float ScaleH = 0;
        if (Mask != null)
        {
            ScaleW = img.width / (Mask.width * Scale.x);
            ScaleH = img.height / (Mask.height * Scale.y);
        }
  
        _material.SetFloat("_XOffset", XOffset);
        _material.SetFloat("_YOffset", YOffset);
        _material.SetFloat("_ScaleX", ScaleW);
        _material.SetFloat("_ScaleY", ScaleH);
        transform.GetComponent<RawImage>().material = _material;
    }

    void OnDestroy()
    {
        ReleaseObject(_material); _material = null;
    }
    void ReleaseObject(Object o)
    {
        if (o != null)
            if (Application.isPlaying)
                Destroy(o);
            else
                DestroyImmediate(o);
    }

    private void OnValidate()
    {
        HandleMask();
    }

}
