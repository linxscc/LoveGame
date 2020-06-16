using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace game.tools
{
    public class PointerClickListener : MonoBehaviour,IPointerClickHandler,IPointerDownHandler, IPointerUpHandler  
    {
        [CSharpCallLua]
        public delegate void VoidDelegate(GameObject go);
        
        public VoidDelegate onClick;  
        public object parameter;  
  
        static public PointerClickListener Get(GameObject go)  
        {  
            PointerClickListener listener = go.GetComponent<PointerClickListener>();  
            if (listener == null) listener = go.AddComponent<PointerClickListener>();  
            return listener;  
        }  
  
        public  void OnPointerClick(PointerEventData eventData)  
        {  
            if (onClick != null)  
            {  
                onClick(gameObject);  
            }  
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }
    }  
}