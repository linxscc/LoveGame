using System;
using UnityEngine;
 
 namespace game.main.Live2d
 {
     public class Live2dCanvas : MonoBehaviour
     {
         private Camera _l2dCamera;
         
         public Camera Live2dCamera => _l2dCamera;

         private void Awake()
         {
             _l2dCamera = GetComponent<Camera>();
         }
 
//         public L2DView CreateL2dView(string modelId)
//         {
//             L2DView l2dView = transform.gameObject.AddComponent<L2DView>();
//             l2dView.LoadModel(modelId);
//             return l2dView;
//         }

         private void OnDestroy()
         {
             RenderTexture tex = _l2dCamera.targetTexture;
             _l2dCamera.targetTexture = null;
             DestroyImmediate(tex);
         }
     }
 }