using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainlineTipWindow : Window
{
    public void SetData(int requireLevel)
    {
        transform.GetText("contentText").text = I18NManager.Get("Guide_MainLineTipText", requireLevel);
        Button button = transform.GetButton("gotoTaskBtn");
        Button cancelBtn = transform.GetButton("cancelBtn");
        
        button.onClick.AddListener(() =>
        {
            Close();
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MISSION, false, true);
            if (GlobalData.PlayerModel.PlayerVo.Level < 4)
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_ClickGo_Mission);
            }
        });

        if(GlobalData.PlayerModel.PlayerVo.Level < 4)
        {
            cancelBtn.gameObject.Hide();
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, rect.anchoredPosition.y);
            
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Go_Chapter3);
        }
        
        cancelBtn.onClick.AddListener(Close);
    }

    protected override void OnClickOutside(GameObject go)
    {
        if(GlobalData.PlayerModel.PlayerVo.Level < 4)
            return;
        
        base.OnClickOutside(go);
    }
}