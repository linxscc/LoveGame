using Assets.Scripts.Framework.GalaSports.Core;

public class PayModule : ModuleBase 
{	
	public override void Init()
	{
		PayPanel panel = new PayPanel();
		panel.Init(this);
		panel.Show(0);
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
