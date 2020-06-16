using Assets.Scripts.Framework.GalaSports.Core;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{

    public interface IMessage
    {
        string Name { get; }

        object Body { get; set; }

        Message.MessageReciverType Type { get; set; }

        string ToString();
    }

}

