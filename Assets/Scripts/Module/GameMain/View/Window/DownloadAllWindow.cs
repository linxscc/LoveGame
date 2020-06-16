using game.main;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using UnityEngine;
using UnityEngine.UI;


public class DownloadAllWindow : Window
{

     private Text _contentText;
     private Transform _finish;
     private Transform _noFinish;

     private Button _downloadBtn;
     private Button _cancelBtn;
     private Button _okBtn;

     private Text _sizeText;
     
     private void Awake()
     {
         _contentText = transform.GetText("ContentText");
         _noFinish = transform.Find("NoFinish");
         _finish = transform.Find("Finish");
        

         _downloadBtn = _noFinish.GetButton("DownloadBtn");
         _cancelBtn = _noFinish.GetButton("CancelBtn");
         _okBtn = _finish.GetButton("OkBtn");

         _sizeText = _noFinish.GetText("Size");
         
         _downloadBtn.onClick.AddListener((() =>
         {
             WindowEvent = WindowEvent.Yes;
             Close();
         }));
         
         
         _cancelBtn.onClick.AddListener((() =>
         {
             WindowEvent = WindowEvent.Cancel;
             Close();
         }));
         
         _okBtn.onClick.AddListener((() =>
         {
             WindowEvent = WindowEvent.Ok;
             Close();             
         }));
     }


     public void SetData(bool isDownload,string size="")
     {      
         if (isDownload)
         {
             _finish.gameObject.Show();
             _contentText.text = I18NManager.Get("GameMain_DownloadWindow2"); 
             
         }
         else
         {
             _noFinish.gameObject.Show();
             _contentText.text = I18NManager.Get("GameMain_DownloadWindow1");
             _sizeText.text = size;            
         }         
     }


     protected override void OnClickOutside(GameObject go)
     {
       
     }
}
