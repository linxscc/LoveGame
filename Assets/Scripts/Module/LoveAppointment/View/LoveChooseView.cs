using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class LoveChooseView : View
{
    private Transform _rolesContent;

    private void Awake()
    {
        _rolesContent = transform.Find("Viewport/Content");

//        for (int i = 0; i < _rolesContent.childCount; i++)
//        {
//            _rolesContent.GetChild(i).gameObject.Hide();
//        }
    }

    private void GoToJournal(GameObject go)
    {
        //发送消息给Controller要打开JournalView，并且要发送Id.
        int vo=(int)PointerClickListener.Get(go).parameter;
        //Debug.LogError(vo);
        SendMessage(new Message(MessageConst.CMD_APPOINTMENT_SHOW_JOURNALCHOOSE,vo));
        
    }

    public void SetData(List<AppointmentRuleVo> appointmentRuleVos,AppointmentModel appointmentModel)
    {
        //参考GamePlay并且看看CMD的协议
        //第一步，读取拉下来的恋爱对象数据
        //第二步点击不同的角色进入不同的日记UI。
        int[] ids = {
            PropConst.CardEvolutionPropChi, PropConst.CardEvolutionPropQin, PropConst.CardEvolutionPropTang,
            PropConst.CardEvolutionPropYan      
        };
        
        //要优化一下算法了，因为有三重循环在，效率非常低下！
        if (ids.Length <= 4)
        {
            for (int i = 0; i < ids.Length ; i++)
            {
                //_rolesContent.GetChild(i).gameObject.Show();
                Image image = _rolesContent.GetChild(i).GetComponent<Image>();
                image.alphaHitTestMinimumThreshold = 0.1f;
                GameObject redpoint = _rolesContent.GetChild(i).Find("RedPoint").gameObject;
                
                PointerClickListener.Get(_rolesContent.GetChild(i).gameObject).parameter = ids[i];//appointmentRuleVos[i];
                PointerClickListener.Get(_rolesContent.GetChild(i).gameObject).onClick = GoToJournal;
                
                //GetTargetData.先获取到AppointmentRule,然后在判断是否有userAppintment在其中，然后每个appointment是否有可解锁的。
                //最好的办法就是做一个全局的可解锁条件和全部通关未拍照的条件。

                var roleAppointmentRule = appointmentModel.GetTargetData(ids[i]);
                bool showredpoint=false;
                foreach (var ruleVo in roleAppointmentRule)
                {
                    var userappointment = appointmentModel.GetUserAppointment(ruleVo.Id);
                    //这个要抽出来做成全局通用的判断！！
                    if (userappointment == null) continue;
                    //有这张卡并且有红点的时候有两种情况：1.没有激活这张卡。2.有可以解锁的关卡。3.有新的卡
                    foreach (var v in ruleVo.ActiveCards)
                    {
                        showredpoint=appointmentModel.NeedSetRedPoint(userappointment, v);   
                        //Debug.LogError("WHY NO SHOW?");
                        if (showredpoint)
                        {
                            break;
                        }
                    }
                    if (showredpoint)
                    {
                        break;
                    }

                }
                redpoint.gameObject.SetActive(showredpoint);
            }            
            
        }
        else
        {
            Debug.LogError(appointmentRuleVos.Count);
        }        
    }

    
}