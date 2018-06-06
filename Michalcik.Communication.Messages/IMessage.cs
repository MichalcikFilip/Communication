using System;

namespace Michalcik.Communication.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; set; }
    }
}
