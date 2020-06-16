using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class AchievementAwardPreviewItem : MonoBehaviour
{
    private Text _desc;
    private RawImage _item;
    private Text _num;
    private Text _name;

    private GameObject _finish;

    private AwardPreviewVo _data;
    
    private void Awake()
    {
        _desc = transform.GetText("Desc");
        _item = transform.GetRawImage("Reward/Item");
        _num = transform.GetText("Reward/Item/Num");
        _name = transform.GetText("Reward/Item/Name");
        _finish = transform.Find("FinishTask").gameObject;
        OnRewardClick();

    }

    private void OnRewardClick()
    {
        PointerClickListener.Get(_item.gameObject).onClick = go =>
        {
            var desc = ClientData.GetItemDescById(_data.Rewards[0].Id,_data.Rewards[0].Resource);
            if (desc!=null)
            {
                FlowText.ShowMessage(desc.ItemDesc);  
            }
        };
    }
    
    public void SetData(AwardPreviewVo vo)
    {
        _data = vo;
        _desc.text = vo.Desc;
        _item.texture = ResourceManager.Load<Texture>(vo.Rewards[0].IconPath);
        _num.text = vo.Rewards[0].Num.ToString();
        _name.text = vo.Rewards[0].Name;
        _finish.SetActive(vo.IsGet);

    }
     
}
