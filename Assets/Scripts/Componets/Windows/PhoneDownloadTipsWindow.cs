using Assets.Scripts.Module.Download;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneDownloadTipsWindow : Window
{
    Toggle[] toggles;
    Button _okBtn;
    Button _cancelBtn;
    
    private void Awake()
    {
        toggles = new Toggle[4];
        for(int i=0;i<4;i++)
        {
            int index = i + 1;
            toggles[i] = transform.Find("ToggleGroup/Toggle" + index.ToString()).GetComponent<Toggle>();
            toggles[i].onValueChanged.AddListener(
                (b) => {
                    ToggleOnValueChanged(index, b);
                }          
             );
        }

        _cancelBtn = transform.GetButton("cancelBtn");
        _cancelBtn.onClick.AddListener(() =>
        {
            Close();
        });

        selectDownload = new List<int>();
        selectunfinishDownload = new List<int>();
        selectIndex = 0;
        _okBtn = transform.GetButton("okBtn");
        _okBtn.interactable = false;
        _okBtn.onClick.AddListener(() => {
            selectunfinishDownload.Clear();
            selectIndex = 0;
            for (int i=0;i< toggles.Length;i++)
            {
                Debug.Log(toggles[i].interactable +" " + toggles[i].isOn);
                if (toggles[i].interactable == true && toggles[i].isOn) 
                {
                    selectunfinishDownload.Add((i + 1));
                }
            }
            if(selectunfinishDownload.Count==0)
            {
                return;
            }
            CacheManager.DownloadPhoneCache(selectunfinishDownload[selectIndex], OnDownLoadDownFinish);
        });
    }

    private void ToggleOnValueChanged(int index,bool arg0)
    {
        Debug.Log("ToggleOnValueChanged index" + index + " bool " + arg0);
        if(arg0)
        {
            if(!selectDownload.Contains(index))
            {
                selectDownload.Add(index);
            }
        }
        else
        {
            if (selectDownload.Contains(index))
            {
                selectDownload.Remove(index);
            }
        }

        if(selectDownload.Count>0)
        {
            _okBtn.interactable = true;
        }
        else
        {
            _okBtn.interactable = false;
        }
    }

    List<int> selectDownload;
    List<int> selectunfinishDownload;
    int selectIndex;
    private void OnDownLoadDownFinish(string tag)
    {
        int index = selectunfinishDownload[selectIndex];

        toggles[index - 1].interactable = false;
        selectIndex++;
        if (selectIndex<selectunfinishDownload.Count) 
        {
            CacheManager.DownloadPhoneCache(selectunfinishDownload[selectIndex], OnDownLoadDownFinish);
        }
        else
        {
            _okBtn.interactable = false;
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        //  MaskAlpha = 0.7f;
        MaskColor = new Color(0, 0, 0, 0.8f);
        SetData();
    }

    public void SetData()
    {
        CacheVo vo = CacheManager.CheckPhoneCache();
        for (int i = 0; i < vo.sizeList.Count; i++)
        {
            //string NpcName = GlobalData.NpcModel.GetNpcById(i + 1).NpcName;
            PlayerPB playerPB = (PlayerPB)(i+1);
            string NpcName = Util.GetPlayerName(playerPB);
            long size = vo.sizeList[i] /( 1024*1024);
            int isize = int.Parse(size.ToString());
            toggles[i].transform.Find("Label").GetText().text = I18NManager.Get("Phone_DownloadContext", NpcName, isize);
        }
        for (int i = 0; i < 4; i++) 
        {
            int NpcId = i + 1;
            if(vo.ids.Contains(NpcId))
            {
                SetToggle(NpcId, false);
            }
            else
            {
                SetToggle(NpcId, true);
            }
        }
    }

    private void SetToggle(int id,bool isHasDownload)
    {
        toggles[id - 1].interactable = !isHasDownload;
        toggles[id - 1].isOn = isHasDownload;
   
    }
}
