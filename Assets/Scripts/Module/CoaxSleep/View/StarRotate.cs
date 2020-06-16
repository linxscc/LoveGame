using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotate : MonoBehaviour
{


    public RectTransform Rect;
    public float Speed = 500f;


    private void Update()
    {
        Rect.Rotate(-Vector3.forward *Time.deltaTime *Speed);
    }
}
