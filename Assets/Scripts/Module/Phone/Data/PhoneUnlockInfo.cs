using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneUnlockInfo  {

    public int sceneId;//情景Id
    public string UnlcokDes;//解锁条件描述
    public string StartTips;//开头文案

    public PhoneUnlockInfo(string[] arr)
    {
        sceneId = int.Parse(arr[0]);
        UnlcokDes = arr[1];
        StartTips = arr[2];
    }
}
