using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using UnityEngine;

public class UpdateModule : ModuleBase 
{
    private UpdatePanel _panel;

    LoginCallbackType _loginCallbackType = LoginCallbackType.None;
    public override void Init()
    {
        _panel = new UpdatePanel();
        AssetManager.Instance.LoadAtlas("UIAtlas_Login");
        _panel.SetData(_loginCallbackType);

        _panel.Init(this);
        _panel.Show(0);
    }

    public override void SetData(params object[] paramsObjects)
    {
        _loginCallbackType  = LoginCallbackType.None; 
        if(paramsObjects.Length>0)
        {
            _loginCallbackType = (LoginCallbackType)paramsObjects[0];
        } 
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            
        }
    }
	
    public void Destroy()
    {
		
    }
}
