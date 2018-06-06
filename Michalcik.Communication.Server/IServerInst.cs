using Michalcik.Communication.Server.Security.Authenticators;
using System;
using System.Collections.Generic;

namespace Michalcik.Communication.Server
{
    public interface IServerInst : IDisposable
    {
        event ServerErrorHandler ReadError;
        event ServerErrorHandler WriteError;

        IConnectionFactory ConnectionFactory { get; set; }
        IAuthenticator Authenticator { get; set; }
        IEnumerable<IMessageHandler> MessageHandlers { get; }

        void Start();
        void Stop();
        void AddMessageHandler(IMessageHandler handler);
        void RemoveMessageHandler(IMessageHandler handler);
    }
}
