using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using Module;

public class StorySmsController : Controller 
{
	public StorySmsView View;
    private StoryModel _storyModel;

	/// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            
        }
    }

    public void LoadJson(string id, bool switchover)
    {
        _storyModel = GetData<StoryModel>();
        if (switchover)
        {
            View.ResetView();
            _storyModel.LoadSmsById(id, vo => { View.SetData(vo, true); });
        }
        else
        {
            View.IsWait = true;
            _storyModel.LoadSmsById(id, vo => { View.Append(vo); });
        }
        
    }
}
