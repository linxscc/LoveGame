using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Proto.Server;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;

public class LoginChooseServerItem : MonoBehaviour
{
    private Text _nameText;

    private GameObject _hotStatus;
    private GameObject _newStatus;
    private GameObject _closeStatus;

    private Button _btn;

    GameServerInfoPB _date;

    private void Awake()
    {
        _nameText = transform.Find("name").GetComponent<Text>();
        _hotStatus = transform.Find("status/hot").gameObject;
        _newStatus = transform.Find("status/new").gameObject;
        _closeStatus = transform.Find("status/close").gameObject;

        _btn = transform.GetComponent<Button>();
        _btn.onClick.AddListener(onBtnClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(GameServerInfoPB date)
    {
        _date = date;
        _nameText.text = date.Name;

        _hotStatus.SetActive(false);
        _newStatus.SetActive(false);
        _closeStatus.SetActive(false);
        switch (date.Status) {
            case NetWorkManager.SERVER_HOT:
                _hotStatus.SetActive(true);
                break;
            case NetWorkManager.SERVER_NEW:
                _newStatus.SetActive(true);
                break;
            case NetWorkManager.SERVER_CLOSE:
                _closeStatus.SetActive(true);
                break;
        }

    }

    private void onBtnClick()
    {
        if (_date == null) return;
        AppConfig.Instance.logicServer = "http://" + _date.Addr + ":" + _date.Port + "/";

        NetWorkManager.Instance.SetServer(AppConfig.Instance.logicServer);

        AppConfig.Instance.serverId = _date.ServerId;
        AppConfig.Instance.serverName = _date.Name;
        EventDispatcher.TriggerEvent(Common.EventConst.OnChooseServer);
    }
}
