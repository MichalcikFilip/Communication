using System;

namespace Michalcik.Communication.Messages
{
    public interface IResponse : IMessage
    {
        Guid ResponseId { get; set; }
    }
}
