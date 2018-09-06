using System;

namespace Michalcik.Communication.Messages
{
    public interface IResponse : IMessage
    {
        Guid MessageId { get; set; }
    }
}
