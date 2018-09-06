using Michalcik.Communication.Factories;
using Michalcik.Communication.Messages;
using Michalcik.Communication.Messages.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Michalcik.Communication.Client
{
    public class ClientInst : IClientInst, IDisposable
    {
        private IConnection connection;
        private List<IResponseHandler> globalResponseHandlers = new List<IResponseHandler>();
        private Dictionary<Guid, IResponseHandler> responseHandlers = new Dictionary<Guid, IResponseHandler>();

        public event ErrorHandler ReadError;
        public event ErrorHandler WriteError;

        public IConnectionFactory ConnectionFactory { get; set; }
        public IEnumerable<IResponseHandler> ResponseHandlers { get { return globalResponseHandlers; } }

        public ClientInst()
        {
            ConnectionFactory = new BinaryConnectionFactory();
        }

        public void AddResponseHandler(IResponseHandler handler)
        {
            if (handler != null)
                globalResponseHandlers.Add(handler);
        }

        public void RemoveResponseHandler(IResponseHandler handler)
        {
            globalResponseHandlers.Remove(handler);
        }

        public void Connect(IPEndPoint endPoint)
        {
            Connect(new TcpClient(endPoint));
        }

        public void Connect(string hostname, int port)
        {
            Connect(new TcpClient(hostname, port));
        }

        private void Connect(TcpClient client)
        {
            connection = ConnectionFactory.GetConnection(client);

            connection.Open();
            connection.Received += message => ResponseReceived(message);
            connection.ReadError += exception => ReadError?.Invoke(exception);
            connection.WriteError += exception => WriteError?.Invoke(exception);
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void Authenticate(Messages.Security.ICredentials credentials)
        {
            if (credentials != null && connection.IsConnected)
                connection.Send(new Authentication(credentials));
        }

        public void Send(IMessage message)
        {
            if (message != null && connection.IsConnected)
            {
                message.Id = Guid.NewGuid();
                connection.Send(message);
            }
        }

        public void Send(IMessage message, IResponseHandler responseHandler)
        {
            if (message != null && connection.IsConnected)
            {
                message.Id = Guid.NewGuid();

                if (responseHandler != null)
                    responseHandlers.Add(message.Id, responseHandler);

                connection.Send(message);
            }
        }

        private void ResponseReceived(IMessage message)
        {
            if (message != null)
            {
                if (message is Heartbeat)
                    return;

                if (message is IResponse)
                {
                    IResponse response = (IResponse)message;

                    if (responseHandlers.ContainsKey(response.MessageId))
                    {
                        responseHandlers[response.MessageId].Handle(response);
                        responseHandlers.Remove(response.MessageId);
                    }

                    foreach (IResponseHandler handler in globalResponseHandlers)
                        handler.Handle(response);
                }
            }
        }
    }
}
