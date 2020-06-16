using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NormalAwardItem : MonoBehaviour
{


    private Text numText;
    private Text nameText;
    private RawImage image;

    private void Awake()
    {
        numText = transform.Find("PropImage/PropNumText").GetComponent<Text>();
        nameText = transform.Find("PropImage/PropNameText").GetComponent<Text>();
        image =transform.Find("PropImage").GetComponent<RawImage>();
    }


    public void SetData(RewardVo vO)
    {
        numText.text = vO.Num.ToString();
        nameText.text = vO.Name;
        image.texture = ResourceManager.Load<Texture>(vO.IconPath, ModuleConfig.MODULE_ACTIVITY);
    }
}
