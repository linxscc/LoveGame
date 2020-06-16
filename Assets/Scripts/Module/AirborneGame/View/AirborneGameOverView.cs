using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class AirborneGameOverView : View {

    RawImage _over;
 
    private void Awake()
    {
        _over = transform.Find("GameOver/Image").GetComponent<RawImage>();
        GameObject obj = transform.Find("Panel").gameObject;
        UIEventListener.Get(obj).onClick = OnClickOutside;
    }

    private void OnClickOutside(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_SHOW_AWARD_PANEL, MessageReciverType.DEFAULT));
    }

    public void SetData(AirborneGameOverType overType)
    {
        string Path = "AirborneGame/GameOver" + overType;
        _over.texture= ResourceManager.Load<Texture>(Path, ModuleConfig.MODULE_AIRBORNEGAME);
    }
}
