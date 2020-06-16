using Assets.Scripts.Framework.GalaSports.Core;
 using Assets.Scripts.Framework.GalaSports.Interfaces;
 
 public class PayPanel : Panel
 {
     public override void Init(IModule module)
     {
         base.Init(module);
 
         PayView payview = (PayView) InstantiateWindow<PayView>("Pay/Prefabs/PayView");
         payview.WindowActionCallback = evt =>
         {
             ModuleManager.Instance.GoBack();
         }; 
     }
 
     public override void Hide()
     {
     }
 
     public override void Show(float delay)
     {
     }
 }