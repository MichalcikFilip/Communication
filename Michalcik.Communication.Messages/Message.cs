using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public abstract class Message : IMessage
    {
        public Guid MessageId { get; set; } = Guid.Empty;
    }
}
