using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class LoveAppointmentGuidePanel : Panel
	{
		public override void Init(IModule module)
		{
			SetComplexPanel();//待研究这个方法
			base.Init(module);

			LoveAppointmentGuideView view = InstantiateView<LoveAppointmentGuideView>("Guide/Prefabs/ModuleView/LoveAppointment/LoveAppointmentGuideView");

			LoveAppointmentGuideController controller = new LoveAppointmentGuideController();
			controller.View = view;
			controller.Start();
            
			RegisterController(controller);
		}
	}
}