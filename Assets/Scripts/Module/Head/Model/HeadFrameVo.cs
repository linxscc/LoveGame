using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;

public class HeadFrameVo
{
    public int Id;
    public string Path;
    public string Desc;
    public ElementTypePB ElementType;
    public ElementModulePB ElementModule;
    public string Name;
    public UnlockRulePB UnlockClaim;
    public bool IsUnlock=false;
    public bool IsShowRedDot=false;
    public string Key;
    public int Sort=0;
    
    
    
    public HeadFrameVo(ElementPB pb)
    {
        Id = pb.Id;
        ElementType = pb.ElementType;
        ElementModule = pb.ElementModule;
        Name = pb.Name;
        UnlockClaim = pb.UnlockClaim;
        Desc = pb.Desc;
        SetIsUnlock();
        Path = "HeadFrame/" + pb.Id;
        Key = GlobalData.PlayerModel.PlayerVo.UserId + Id+"";
        SetRedDot();

        if (!IsUnlock)
            Sort = 1;
    }

    private void SetRedDot()
    {
       
        var isKey = PlayerPrefs.HasKey(Key);
       // PlayerPrefs.DeleteKey(Key);
        if (!isKey&&IsUnlock)        
            IsShowRedDot = true;
        
    }


    private void SetIsUnlock()
    {
        IsUnlock = GlobalData.DiaryElementModel.IsUserElement(Id);

        if (IsUnlock)
            return;


        bool isFirst = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FIRST_CHARGE_DEFAULT_BOX_ID) == Id;
        if (!IsUnlock && isFirst)
        {
            var firstState = GlobalData.PlayerModel.PlayerVo.ExtInfo.FirstPrize;

            if (firstState != FirstPrizeStatusPB.FpNotInvolved)
                IsUnlock = true;
        }
    }


}