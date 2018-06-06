using Michalcik.Communication.Messages;
using Michalcik.Communication.Server.Security.Clients;

namespace Michalcik.Communication.Server
{
    public interface IMessageHandler
    {
        IResponse Handle(IMessage message, IClient client);
    }
}
