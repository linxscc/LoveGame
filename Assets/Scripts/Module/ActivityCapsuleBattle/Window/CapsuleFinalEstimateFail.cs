using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleFinalEstimateFail : Window
{

    private Button _goCardBtn;      //去星缘
    private Button _goSupporterBtn; //去应援会
    private Button _goGiftPackBtn;  //去甜蜜加速站
    private Button _explainBtn;      //说明按钮
    
    private bool _oneNum;
    private Text _scoreTxt;
    
    private void Awake()
    {
        _goCardBtn = transform.GetButton("Bg/Buttons/GotoCardCollention");
        _goSupporterBtn = transform.GetButton("Bg/Buttons/GotoSupporter");
        _goGiftPackBtn = transform.GetButton("Bg/Buttons/GotoGiftPack");
        _explainBtn = transform.GetButton("Bg/ExplainBtn");
        _scoreTxt = transform.GetText("Bg/StarAndGrade/Text/Text");
        
        string title = I18NManager.Get("Common_HelpExplain");
        string ruleDesc = I18NManager.Get("Battle_FailExplain");
        _explainBtn.onClick.AddListener(() =>
        {
            PopupManager.ShowCommonRuleWindow(ruleDesc,title );
        });
               
    }


    private void Start()
    {
        _oneNum = true;
        _goCardBtn.onClick.AddListener     (() => {EnterModule(ModuleConfig.MODULE_CARD);});
        _goSupporterBtn.onClick.AddListener(() => {EnterModule(ModuleConfig.MODULE_SUPPORTER);});
        _goGiftPackBtn.onClick.AddListener (() => {EnterModule(ModuleConfig.MODULE_SHOP); });
        
    }

    private void EnterModule(string moduleName)
    {        
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_FINISH, Message.MessageReciverType.DEFAULT, moduleName));
    }
    
    
    public void SetData(int score)
    {
        _scoreTxt.text = score.ToString();
    }
    
    protected override void OnClickOutside(GameObject go)
    {
        if (_oneNum)
        {         
           SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_FINISH));
            _oneNum = false;
        }
    }
}
