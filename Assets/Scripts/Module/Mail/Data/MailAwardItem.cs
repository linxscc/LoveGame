using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using game.tools;

public class MailAwardItem : MonoBehaviour
{


    private Frame _frame;
    private Text _numText;
    private Text _nameText;
    private GameObject _onClick;

    private void Awake()
    {
        _numText = transform.Find("PropImage/PropNumText").GetComponent<Text>();
        _nameText = transform.Find("PropImage/PropNameText").GetComponent<Text>();
        _frame = transform.Find("SmallFrame").GetComponent<Frame>();
    }

    public void SetData(MailAwardVO vO)
    {
        _frame.SetData(vO.Reward);
        _numText.text = vO.Reward.Num.ToString();
        _nameText.text = vO.Reward.Name;
    }


}
