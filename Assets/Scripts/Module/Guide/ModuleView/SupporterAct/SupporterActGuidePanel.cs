using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;

namespace Module.Guide.ModuleView.Supporter
{
	public class SupporterActGuidePanel : Panel
	{

		private SupporterActGuideController _supporterActGuideController;
		
		public override void Init(IModule module)
		{
			base.Init(module);

			SupporterActGuideView guideView =
				(SupporterActGuideView) InstantiateView<SupporterActGuideView>(
					"Guide/Prefabs/ModuleView/SupporterActivity/SupporterActGuideView");
			_supporterActGuideController=new SupporterActGuideController();
			RegisterController(_supporterActGuideController);
			_supporterActGuideController._SupporterActGuideView = guideView;
		}

		
		public override void Hide()
		{
        
		}

		public override void Show(float delay)
		{
        
		}

	}
}
