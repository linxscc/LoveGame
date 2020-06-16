using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDialogInfo  {
    public int CardId;
    public List<string> Dialog;

    public DrawDialogInfo(string[] strs)
    {
        CardId = int.Parse(strs[0]);
        Dialog = new List<string>();
        for (int i=1;i< strs.Length; i++)
        {
            Dialog.Add(strs[i]);
        }
    }
}
