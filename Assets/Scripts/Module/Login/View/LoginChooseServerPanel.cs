using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto.Server;
using Assets.Scripts.Framework.GalaSports.Core.Events;

public class LoginChooseServerPanel : MonoBehaviour
{
    private LoginChooseServerItem _lastServer;

    private Transform _serverRoot;
    private Button _disableBtn;

    private void Awake()
    {
        _lastServer = transform.Find("ServerItem").GetComponent<LoginChooseServerItem>();
        _serverRoot = transform.Find("ServerList/Viewport/Content");
        _disableBtn = transform.Find("Disable").GetComponent<Button>();
        _disableBtn.onClick.AddListener(OnDisableBtnClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshServers();
    }

    private void RefreshServers()
    {
        GameServerInfoPB lastDate = null;
        ClearItems();
        for (int i = 0; i < NetWorkManager.Instance.serverIds.Count; ++i)
        {
            string serverId = NetWorkManager.Instance.serverIds[i];
            //string name = I18NManager.Get("Login_Server_" + (i + 1));

            GameServerInfoPB serverData = NetWorkManager.Instance.GetGameServerData(serverId);
            AddServerItem(serverData);

            if (lastDate == null)
                lastDate = serverData;
        }

        if (lastDate != null)
            _lastServer.SetData(lastDate);
    }

    private void AddServerItem(GameServerInfoPB date)
    {
        if (date == null) return;
        GameObject obj = Instantiate(_lastServer.gameObject);
        LoginChooseServerItem item = obj.GetComponent<LoginChooseServerItem>();
        item.SetData(date);
        obj.transform.SetParent(_serverRoot, false);
    }

    private void ClearItems()
    {
        foreach(Transform t in _serverRoot)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnDisableBtnClick()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
