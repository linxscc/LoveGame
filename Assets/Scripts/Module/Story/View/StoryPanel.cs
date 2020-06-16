using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;

public class StoryPanel : ReturnablePanel
{
    private StoryController control;

    public int[] AppointmentId={0,0};
    
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<StoryView>("Story/Prefabs/StoryView");
        
        control = new StoryController();
        control.View = viewScript;
        
        RegisterController(control);
        
        HideBackBtn();
    }

    public override void OnBackClick()
    {
        PopupManager.ShowConfirmWindow(I18NManager.Get("Story_QuitTip")).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                base.OnBackClick();
            }
        };
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        control.View.gameObject.Show();
    }
    public void Show(bool continueAutoPlay)
    {
        Show(0);
        control.View.SetAutoPlay(continueAutoPlay);
    }
    
    public void ForceAutoPlay()
    {
        control.View.ForceAutoPlay();
    }

    public override void Hide()
    {
        base.Hide();
        control.View.gameObject.Hide();
    }

    public void Start(string storyId)
    {
        control.Start();
        control.LoadStoryById(storyId);
        control.Appointmentdata = AppointmentId;
    }

    public void NeedToHideSkip(bool enable)
    {
        control.View.ShowSkip(enable);
    }


    public override void Destroy()
    {
        base.Destroy();
        control.Destroy();
    }

    public void HideAllBtn()
    {
        control.View.HideAllBtn();
    }
}
