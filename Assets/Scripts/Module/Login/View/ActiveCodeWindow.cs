using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.main;
using UnityEngine.UI;

namespace Module.Login.View
{
    public class ActiveCodeWindow : Window
    {
        private void Awake()
        {
            transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                InputField input = transform.Find("InputField").GetComponent<InputField>();
                if (string.IsNullOrWhiteSpace(input.text))
                {
                    FlowText.ShowMessage(I18NManager.Get("Login_ActiveCodeWindowHint2"));
                    return;
                }
                EventDispatcher.TriggerEvent(EventConst.CheckActiveCode, input.text);
            });
        }
    }
}