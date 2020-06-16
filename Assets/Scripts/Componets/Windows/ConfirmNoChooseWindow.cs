using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class ConfirmNoChooseWindow : Window 
    {
        [SerializeField] private Button _okBtn;		
        [SerializeField] private Button _cancleBtn;
        [SerializeField] private Text _contenText;
        
        
        public string CancelText
        {
            get
            {
                return _cancleBtn.GetComponentInChildren<Text>().text;
            }
            set
            {
                _cancleBtn.GetComponentInChildren<Text>().text = value;
            }
        }
        
        
        public string Content
        {
            get { return _contenText.text; }
            set { _contenText.text = value; }
        }
        
        
        public string OkText
        {
            get
            {
                return _okBtn.GetComponentInChildren<Text>().text;
            }
            set
            {
                _okBtn.GetComponentInChildren<Text>().text = value;
            }
        }

        protected override void OnInit()
        {
            base.OnInit();			
            _contenText.text = "";
            
            _okBtn.onClick.AddListener(OnOkBtn);
            _cancleBtn.onClick.AddListener(OnCancelBtn);
        }
        
        protected override void OnClickOutside(GameObject go)
        {

        }
        
        protected virtual void OnOkBtn()
        {
            WindowEvent = WindowEvent.Ok;
            CloseAnimation();
        }
        
        
        protected void OnCancelBtn()
        {
            WindowEvent = WindowEvent.Cancel;
            Close();
        }

    }  
}


