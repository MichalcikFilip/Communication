using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public class Heartbeat : Message, IMessage
    {
        public DateTime Time { get; }

        public Heartbeat()
        {
            Time = DateTime.Now;
        }
    }
}
