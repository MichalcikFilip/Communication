using Michalcik.Communication.Messages;
using System;
using System.Collections.Generic;
using System.Net;

namespace Michalcik.Communication.Client
{
    public interface IClientInst : IDisposable
    {
        event ErrorHandler ReadError;
        event ErrorHandler WriteError;

        IConnectionFactory ConnectionFactory { get; set; }
        IEnumerable<IResponseHandler> ResponseHandlers { get; }

        void Connect(IPEndPoint endPoint);
        void Connect(string hostname, int port);
        void Disconnect();
        void AddResponseHandler(IResponseHandler handler);
        void RemoveResponseHandler(IResponseHandler handler);
        void Authenticate(Messages.Security.ICredentials credentials);
        void Send(IMessage message);
        void Send(IMessage message, IResponseHandler responseHandler);
    }
}
