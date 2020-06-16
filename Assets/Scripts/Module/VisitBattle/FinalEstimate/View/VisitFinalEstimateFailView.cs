using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using game.main;
using game.tools;
using Module.VisitBattle.Data;
using UnityEngine;
using UnityEngine.UI;

public class VisitFinalEstimateFailView : View {
	
	void Start () {


        var _hideBtn = transform.Find("HideBtn").gameObject;

        //点击其他地方关闭
        PointerClickListener.Get(_hideBtn).onClick = go =>
        {
            Hide();
            SendMessage(new Message(MessageConst.CMD_VISITBATTLE_FINISH));
        };

       transform.Find("Bg/Buttons/GotoCardCollention").GetComponent<Button>().onClick.AddListener(()=>
	    {
		    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD);
	    });
		transform.Find("Bg/Buttons/GotoSupporter").GetComponent<Button>().onClick.AddListener(()=>
		{
			ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SUPPORTER);
		});
		transform.Find("Bg/Buttons/GotoGiftPack").GetComponent<Button>().onClick.AddListener(()=>
		{
			FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));
		});
		
		string title = I18NManager.Get("Common_HelpExplain");
		string ruleDesc = I18NManager.Get("Battle_FailExplain");
		
		
		transform.Find("Bg/ExplainBtn").GetComponent<Button>().onClick.AddListener(()=>
		{
			PopupManager.ShowCommonRuleWindow(ruleDesc,title );
			//PopupManager.ShowRuleExplainWindow(title,ruleDesc,new Vector2(800,1000));
		});
    }

    public void SetData(VisitBattleResultData data, VisitLevelVo level)
    {
        transform.Find("Bg/StarAndGrade/Text/Text").GetComponent<Text>().text =  data.Cap.ToString();
        //transform.Find("TitleText").GetComponent<Text>().text = level.LevelName;
    }


    public override void Hide()
    {
        base.Hide();
    }

    //public void Show(float delay)
    //{
    //}

    //public void Hide()
    //{
       
    //}
	
}
