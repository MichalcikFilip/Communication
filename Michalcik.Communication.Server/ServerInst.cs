using Michalcik.Communication.Factories;
using Michalcik.Communication.Messages;
using Michalcik.Communication.Messages.Security;
using Michalcik.Communication.Server.Security.Authenticators;
using Michalcik.Communication.Server.Security.Clients;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Michalcik.Communication.Server
{
    public class ServerInst : IServerInst, IDisposable
    {
        private const int HEARTBEAT_INTERVAL = 500;

        private bool running = false;
        private List<IMessageHandler> messageHandlers = new List<IMessageHandler>();
        private List<ClientConnection> connections = new List<ClientConnection>();

        private TcpListener listener;

        public event ServerErrorHandler ReadError;
        public event ServerErrorHandler WriteError;

        public IConnectionFactory ConnectionFactory { get; set; }
        public IAuthenticator Authenticator { get; set; }
        public IEnumerable<IMessageHandler> MessageHandlers { get { return messageHandlers; } }

        public ServerInst(IPEndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException(nameof(endPoint));

            listener = new TcpListener(endPoint);
            ConnectionFactory = new BinaryConnectionFactory();
        }

        public ServerInst(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
                throw new ArgumentNullException(nameof(ipAddress));

            listener = new TcpListener(ipAddress, port);
            ConnectionFactory = new BinaryConnectionFactory();
        }

        public void AddMessageHandler(IMessageHandler handler)
        {
            if (handler != null)
                messageHandlers.Add(handler);
        }

        public void RemoveMessageHandler(IMessageHandler handler)
        {
            messageHandlers.Remove(handler);
        }

        public void Start()
        {
            if (!running)
            {
                running = true;

                new Thread(ListenThread).Start();
                new Thread(HeartbeatThread).Start();
            }
        }

        public void Stop()
        {
            running = false;

            listener?.Stop();

            foreach (ClientConnection clientConnection in connections)
                clientConnection.Connection.Close();
        }

        public void Dispose()
        {
            Stop();
        }

        private void MessageReceived(IMessage message, ClientConnection clientConnection)
        {
            if (message != null)
            {
                if (message is Authentication)
                {
                    Messages.Security.ICredentials credentials = ((Authentication)message).Credentials;

                    if (Authenticator != null && credentials != null && credentials.AuthenticationType == Authenticator.AuthenticationType)
                    {
                        IClient client = Authenticator.Authenticate(credentials);
                        clientConnection.Client = client != null ? client : new Client(false);
                    }

                    return;
                }

                foreach (IMessageHandler handler in messageHandlers)
                {
                    IResponse response = handler.Handle(message, clientConnection.Client);

                    if (response != null)
                    {
                        response.Id = Guid.NewGuid();
                        response.MessageId = message.Id;

                        clientConnection.Connection.Send(response);
                    }
                }
            }
        }

        private void ListenThread()
        {
            listener.Start();

            while (running)
            {
                TcpClient client = listener.AcceptTcpClient();
                ClientConnection clientConnection = new ClientConnection(ConnectionFactory.GetConnection(client), new Client(false));

                clientConnection.Connection.Received += message => MessageReceived(message, clientConnection);
                clientConnection.Connection.ReadError += exception => ReadError?.Invoke(exception, clientConnection.Connection, clientConnection.Client);
                clientConnection.Connection.WriteError += exception => WriteError?.Invoke(exception, clientConnection.Connection, clientConnection.Client);
                clientConnection.Connection.Open();

                connections.Add(clientConnection);
            }
        }

        private void HeartbeatThread()
        {
            while (running)
            {
                foreach (ClientConnection clientConnection in connections.ToArray())
                {
                    if (clientConnection.Connection.IsConnected)
                    {
                        clientConnection.Connection.Send(new Heartbeat());
                    }
                    else
                    {
                        clientConnection.Connection.Close();
                        connections.Remove(clientConnection);
                    }
                }

                Thread.Sleep(HEARTBEAT_INTERVAL);
            }
        }
    }
}
