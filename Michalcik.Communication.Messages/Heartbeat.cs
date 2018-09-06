using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public class Heartbeat : Message, IMessage
    {
        public DateTime Timestamp { get; }

        public Heartbeat()
        {
            Timestamp = DateTime.Now;
        }
    }
}
