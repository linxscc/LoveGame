using System;
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using game.tools;
using UnityEngine.UI;


public class SupportStrengthView : View
{
    private int _power = 0;
    private void Awake()
    {
        PointerClickListener.Get(gameObject).onClick = (go) =>
        {
            //ShowNextView();
            SendMessage(new Message(MessageConst.CMD_BATTLE_NEXT,Message.MessageReciverType.DEFAULT,_power));
        };
    }

    public void SetData()
    {
        _power = 0;
        foreach (var VARIABLE in GlobalData.DepartmentData.MyDepartments)
        {
            switch (VARIABLE.UserDepartmentPb.DepartmentType)
            {
                case DepartmentTypePB.Active:
                    transform.Find("Container/Active/Text").GetComponent<Text>().text = VARIABLE.RulePb.Exp.ToString();
                    _power += VARIABLE.RulePb.Power;
                    break;
                case DepartmentTypePB.Financial:
                    transform.Find("Container/FinancialStrength/Text").GetComponent<Text>().text = VARIABLE.RulePb.Exp.ToString();
                    _power += VARIABLE.RulePb.Power;
                    break;
                case DepartmentTypePB.Resource:
                    transform.Find("Container/Resource/Text").GetComponent<Text>().text = VARIABLE.RulePb.Exp.ToString();
                    _power += VARIABLE.RulePb.Power;
                    break;
                case DepartmentTypePB.Transmission:
                    transform.Find("Container/Propagation/Text").GetComponent<Text>().text = VARIABLE.RulePb.Exp.ToString();
                    _power += VARIABLE.RulePb.Power;
                    break;
                case DepartmentTypePB.Support:
                    _power += VARIABLE.RulePb.Power;
                    break;
            }
        }
        
        var textStrength = transform.Find("ScoreBg/StrengthTxt").GetComponent<Text>();
        textStrength.text = "+0";
        Util.TweenTextNum(textStrength,0.6f,_power,"+", "", () =>
        {
            SendMessage(new Message(MessageConst.CMD_BATTLE_CHANGE_POWER, Message.MessageReciverType.DEFAULT, _power));
        });
        
        
    }
}
