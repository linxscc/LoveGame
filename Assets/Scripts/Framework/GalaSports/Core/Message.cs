using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public class Message : IMessage
    {
        public enum MessageReciverType
        {
            DEFAULT=0,
            VIEW=1,
            CONTROLLER=2,
            MODEL=3,
            UnvarnishedTransmission
        }
        public Message(string name)
            : this(name, null, MessageReciverType.DEFAULT)
        {
        }

        public Message(string name, object body)
            : this(name, body, MessageReciverType.DEFAULT)
        {
        }

        public Message(string name, object body, MessageReciverType type)
        {
            m_name = name;
            m_body = body;
            m_type = type;
        }

        public Message(string name, MessageReciverType type, params object[] paramObjects)
        {
            m_name = name;
            m_params = paramObjects;
            m_type = type;
        }

        /// <summary>
        /// Get the string representation of the <c>Notification instance</c>
        /// </summary>
        /// <returns>The string representation of the <c>Notification</c> instance</returns>
        public override string ToString()
        {
            string msg = "Notification Name: " + Name;
            msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
            msg += "\nType:" + Type.ToString();
            return msg;
        }

        /// <summary>
        /// The name of the <c>Notification</c> instance
        /// </summary>
        public virtual string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// The body of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual object Body
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_body;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_body = value;
            }
        }

        /// <summary>
        /// The type of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual MessageReciverType Type
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_type;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_type = value;
            }
        }

        public virtual object[] Params
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_params;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_params = value;
            }
        }

        /// <summary>
        /// The name of the notification instance 
        /// </summary>
        private string m_name;

        /// <summary>
        /// The type of the notification instance
        /// </summary>
        private MessageReciverType m_type;

        /// <summary>
        /// The body of the notification instance
        /// </summary>
        private object m_body;


        private object[] m_params;
    }
}

