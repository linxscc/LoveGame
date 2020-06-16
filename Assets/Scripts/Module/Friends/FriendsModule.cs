using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;

public class FriendsModule : ModuleBase
{
    private FriendsMainPanel _friendMainPanel;
    private MakeFriendsPanel _makeFriendsPanel;
    public override void Init()
    {
        _friendMainPanel = new FriendsMainPanel();
        RegisterModel<FriendModel>();
        _friendMainPanel.Init(this);
        _friendMainPanel.Show(0);
    }
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        switch (name)
        {
            case MessageConst.MODULE_FRIENDS_GOTO_MAKE_FRIENDS:
                if(_makeFriendsPanel==null)
                {
                    _makeFriendsPanel = new MakeFriendsPanel();
                    _makeFriendsPanel.Init(this);
                }
                _makeFriendsPanel.Show(0);
                _friendMainPanel.Hide();
                break;
            case MessageConst.MODULE_FRIENDS_GOBACK:
                _makeFriendsPanel.Hide();
                _friendMainPanel.GoBackRefresh();
                _friendMainPanel.Show(0);
                break;
        }
    }

    public override void Remove(float delay)
    {
        base.Remove(delay);
        _friendMainPanel.Destroy();
        if (_makeFriendsPanel!=null)
            _makeFriendsPanel.Destroy();
    }

    public void Close()
    {
    }
}
