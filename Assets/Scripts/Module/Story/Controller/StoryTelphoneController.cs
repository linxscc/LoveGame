using Assets.Scripts.Framework.GalaSports.Core;
using game.main;

public class StoryTelphoneController : Controller
{
    public StoryTelephoneView View;
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
        View.Reset();
        _storyModel.LoadTelphoneById(id, vo => { View.SetData(vo, switchover); });
    }
}