using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;

public class CoaxSleepOnPlayAudioController : Controller
{
    public CoaxSleepOnPlayAudioView View;


    public override void Start()
    {
        base.Start();
    }


    public override void Destroy()
    {
        base.Destroy();
    }

    public void GetCurData(MyCoaxSleepAudioData data)
    {

        var localPath = GetData<CoaxSleepModel>().GetLocalPath(data.AudioId);
        
        View.SetData(data,localPath);
    }
}
