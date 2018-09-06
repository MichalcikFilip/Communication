using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public abstract class Response : Message, IResponse, IMessage
    {
        public Guid MessageId { get; set; } = Guid.Empty;
    }
}
