using System;

namespace Michalcik.Communication.Messages
{
    [Serializable]
    public abstract class Response : Message, IResponse, IMessage
    {
        public Guid ResponseId { get; set; } = Guid.Empty;
    }
}
