using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class DebugTool : MonoBehaviour
{

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    public float f_Fps;

    void Start()
    {
        //Application.targetFrameRate=60;
        f_LastInterval = Time.realtimeSinceStartup;
        i_Frames = 0;
    }

    void OnGUI()
    {
        var MonoUsedM = Profiler.GetMonoUsedSize() / 1000000;
        var AllMemory = Profiler.GetTotalAllocatedMemory() / 1000000;
        GUIStyle style = new GUIStyle();
        style.fontSize = 28;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        GUI.Box(new Rect(4,4,200,200), "AAA");
        
        GUI.Label(new Rect(4, 4, 200, 200), "FPS:" + f_Fps.ToString("f2") + "\n mono:" + MonoUsedM + "M" + "\nallMemo:" + AllMemory + "M", style);
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }

    static Vector3[] fourCorners = new Vector3[4];
    void OnDrawGizmos()
    {
        foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
        {
            if (g.raycastTarget)
            {
                RectTransform rectTransform = g.transform as RectTransform;
                rectTransform.GetWorldCorners(fourCorners);
                Gizmos.color = Color.blue;
                for (int i = 0; i < 4; i++)
                    Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);

            }
        }
    }
}