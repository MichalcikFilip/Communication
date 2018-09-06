using System;

namespace Michalcik.Communication.Messages
{
    public interface IMessage
    {
        Guid Id { get; set; }
    }
}
