using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public abstract class Message : IMessage
    {
        public Guid Id { get; set; } = Guid.Empty;
    }
}
