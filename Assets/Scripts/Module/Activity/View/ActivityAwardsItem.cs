
using Assets.Scripts.Module;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
namespace Module.Activity.View
{
	public class ActivityAwardsItem : MonoBehaviour
	{
		private Text _numTxt;
		private Text _nameTxt;
		private Frame _frame;

		private void Awake()
		{
			_numTxt = transform.GetText("Num");
			_nameTxt = transform.GetText("Name");
			_frame = transform.Find("BigFrame").GetComponent<Frame>();
		}



		public void SetData(RewardVo vo)
		{
			_numTxt.text = "x"+vo.Num;
			_nameTxt.text = vo.Name;			
			_frame.SetData(vo,ModuleConfig.MODULE_ACTIVITY);
		}
	}
}


