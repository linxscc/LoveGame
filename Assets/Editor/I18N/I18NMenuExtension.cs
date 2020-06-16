using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class I18NMenuExtension : MonoBehaviour 
{
    private const string kUILayerName = "UI";
    private const float  kWidth       = 160f;
    private const float  kThickHeight = 30f;
    private const float  kThinHeight  = 20f;
    private const string kStandardSpritePath           = "UI/Skin/UISprite.psd";
    private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
    private const string kInputFieldBackgroundPath     = "UI/Skin/InputFieldBackground.psd";
    private const string kKnobPath                     = "UI/Skin/Knob.psd";
    private const string kCheckmarkPath                = "UI/Skin/Checkmark.psd";

    private static Vector2 s_ThickGUIElementSize    = new Vector2(kWidth, kThickHeight);
    private static Vector2 s_ThinGUIElementSize     = new Vector2(kWidth, kThinHeight);
    private static Vector2 s_ImageGUIElementSize    = new Vector2(100f, 100f);
    private static Color   s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
    private static Color   s_PanelColor             = new Color(1f, 1f, 1f, 0.392f);
    private static Color   s_TextColor              = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
    
    
    [MenuItem("GameObject/UI/Text_I18N", false, 2000)]
    public static void AddText(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Text");
        TextI18N txt = go.AddComponent<TextI18N>();
        txt.fontSize = 30;

//        string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/mainfont.bytes");
        string path = "Assets/BundleAssets/SingleFile/Fonts/MainFont.prefab";
        txt.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(path);
        
        path = "Assets/BundleAssets/SingleFile/Fonts/lantingHei.TTF";
        txt.font = AssetDatabase.LoadAssetAtPath<Font>(path);

        PlaceUIElementRoot(go, menuCommand);
        InitValue(txt);
    }
    
    
    [MenuItem("GameObject/UI/Button_I18N", false, 2001)]
    static public void AddButton(MenuCommand menuCommand)
    {
        GameObject buttonRoot = CreateUIElementRoot("Button", menuCommand, s_ThickGUIElementSize);

        GameObject childText = new GameObject("Text");
        GameObjectUtility.SetParentAndAlign(childText, buttonRoot);

        RectTransform rect = buttonRoot.transform as RectTransform;
        rect.sizeDelta = new Vector2(300,100);

        Image image = buttonRoot.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        image.type = Image.Type.Sliced;
        image.color = s_DefaultSelectableColor;

        Button bt = buttonRoot.AddComponent<Button>();
        SetDefaultColorTransitionValues(bt);

        TextI18N text = childText.AddComponent<TextI18N>();
        text.text = "Button";
        text.alignment = TextAnchor.MiddleCenter;
        SetDefaultTextValues(text);
        
        text.fontSize = 30;
        text.TestString = "Button";
        
        string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/mainfont.bytes");
        text.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
        
        bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/lantinghei.bytes");
        text.font = AssetDatabase.LoadAssetAtPath<Font>(bundles[0]);

        RectTransform textRectTransform = childText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
    }
    
    private static GameObject CreateUIElementRoot(string name, MenuCommand menuCommand, Vector2 size)
    {
        GameObject parent = menuCommand.context as GameObject;
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            parent = GetOrCreateCanvasGameObject();
        }
        GameObject child = new GameObject(name);

        Undo.RegisterCreatedObjectUndo(child, "Create " + name);
        Undo.SetTransformParent(child.transform, parent.transform, "Parent " + child.name);
        GameObjectUtility.SetParentAndAlign(child, parent);

        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        if (parent != menuCommand.context) // not a context click, so center in sceneview
        {
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), rectTransform);
        }
        Selection.activeGameObject = child;
        return child;
    }
    private static void SetDefaultColorTransitionValues(Selectable slider)
    {
        ColorBlock colors = slider.colors;
        colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
        colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
        colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
    }
    private static void SetDefaultTextValues(Text lbl)
    {
        // Set text values we want across UI elements in default controls.
        // Don't set values which are the same as the default values for the Text component,
        // since there's no point in that, and it's good to keep them as consistent as possible.
        lbl.color = s_TextColor;
    }
    
    
    /// <summary>
    /// 初始化值为了和Text的初始值保持一致
    /// </summary>
    /// <param name="txt"></param>
    private static void InitValue(TextI18N txt)
    {
        txt.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
        RectTransform contentRT = txt.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(200f, 50);
        txt.gameObject.layer = LayerMask.NameToLayer(kUILayerName);
    }


    #region 为了能够扩展，这里是从UGUI 源码抠出来的代码,有改动
    private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
    {
        GameObject parent = menuCommand.context as GameObject;
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            parent = GetOrCreateCanvasGameObject();
        }

        string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
        element.name = uniqueName;
        Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
        Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
        GameObjectUtility.SetParentAndAlign(element, parent);
        if (parent != menuCommand.context) // not a context click, so center in sceneview
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

        Selection.activeGameObject = element;
    }

    // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
    static public GameObject GetOrCreateCanvasGameObject()
    {
        GameObject selectedGo = Selection.activeGameObject;

        // Try to find a gameobject that is the selected GO or one if its parents.
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in selection or its parents? Then use just any canvas..
        canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in the scene at all? Then create a new one.
        return CreateNewUI();
    }

    static public GameObject CreateNewUI()
    {
        // Root for the UI
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

        // if there is no event system add one...
        CreateEventSystem(false);
        return root;
    }

    private static void CreateEventSystem(bool select)
    {
        CreateEventSystem(select, null);
    }

    private static void CreateEventSystem(bool select, GameObject parent)
    {
        var esys = FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null)
        {
            Selection.activeGameObject = esys.gameObject;
        }
    }

    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        // Find the best scene view
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null && SceneView.sceneViews.Count > 0)
            sceneView = SceneView.sceneViews[0] as SceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }
    #endregion

}
