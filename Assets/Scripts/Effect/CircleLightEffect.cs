using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Effect
{
    public class CircleLightEffect : MonoBehaviour
    {
        Vector3[] corners = new Vector3[4];
        
        private Vector4 center;
        private Material material;
        private float current =0f;
        
        public RectTransform target;
        private RectTransform _oldTarget;
        private int _diameter = 200;

        private void DoStart()
        {
            Canvas canvas = Main.GuideCanvas;
            target.GetWorldCorners (corners);
 
            float x =corners [0].x + ((corners [3].x - corners [0].x) / 2f);
            float y =corners [0].y + ((corners [1].y - corners [0].y) / 2f);
 
            Vector3 center = new Vector3 (x, y, 0f);
            Vector2 position = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, center, canvas.GetComponent<Camera>(), out position);
 
            center = new Vector4 (position.x,position.y,0f,0f);
            material = GetComponent<Image>().material;
            material.SetVector ("_Center", center);
 
            (canvas.transform as RectTransform).GetWorldCorners (corners);
            for (int i = 0; i < corners.Length; i++) {
                current = Mathf.Max(Vector3.Distance (WordToCanvasPos(canvas,corners [i]), center),current);
            }

            
            material.SetFloat ("_Silder", _diameter);
        }
        
        public void SetSilder(int diameter)
        {
            material.SetFloat ("_Silder", diameter);
        }

        public void SetPostionOffset(Vector2 pos)
        {
            center.x += pos.x;
            center.y += pos.y;
            material.SetVector ("_Center", center);
        }
        
        public void SetTarget(RectTransform rectTransform)
        {
            _diameter = 100;
            Vector2 offset = Vector2.zero;
            SetTarget(rectTransform,_diameter,offset);
        }
        
        public void SetTarget(RectTransform rectTransform, int silder = 100)
        {
            _diameter = silder;
            Vector2 offset = Vector2.zero;
            SetTarget(rectTransform,_diameter,offset);
        }

        public void SetTarget(RectTransform rectTransform, int silder, Vector2 offset)
        {
            target = rectTransform;
            _diameter = silder;
            DoStart();
            SetPostionOffset(offset);
        }

        Vector2 WordToCanvasPos(Canvas canvas,Vector3 world){
            Vector2 position = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
            return position;
        }

        private void Update()
        {
            if (target != _oldTarget)
            {
                DoStart();
                _oldTarget = target;
            }
        }
    }
}