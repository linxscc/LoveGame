using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace game.tools
{
    public class UGuiUtil
    {
        public static void UpdateButtonListener(bool isAdd, Button btn, UnityAction listener)
        {
            if (isAdd)
            {
                btn.onClick.AddListener(listener);
            }
            else
            {
                btn.onClick.RemoveListener(listener);
            }
        }
        
        public static void UpdateEventTriggerListener(bool isAdd, GameObject go, UIEventListener.VoidDelegate listener)
        {
            if (isAdd)
            {
                UIEventListener.Get(go).onClick = listener;
            }
            else
            {
                UIEventListener.Get(go).onClick = null;
            }
        }

        public static void UpdatePointerClickListener(bool isAdd, GameObject go, PointerClickListener.VoidDelegate listener)
        {
            if (isAdd)
            {
                PointerClickListener.Get(go).onClick = listener;
            }
            else
            {
                PointerClickListener.Get(go).onClick = null;
            }
        }
    }
}
