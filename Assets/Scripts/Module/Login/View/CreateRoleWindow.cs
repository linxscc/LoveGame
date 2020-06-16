using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.main;
using UnityEngine.UI;

namespace Module.Login.View
{
    public class CreateRoleWindow : Window
    {
        private InputField _inputField;

        private void Awake()
        {
            _inputField = transform.Find("InputField").GetComponent<InputField>();
            Button okBtn = transform.Find("OkBtn").GetComponent<Button>();
            okBtn.onClick.AddListener(OnOkBtnClick);
        }

        private void OnOkBtnClick()
        {
            EventDispatcher.TriggerEvent(EventConst.CreateRoleSubmit, _inputField.text);
            Close();
        }
    }
}