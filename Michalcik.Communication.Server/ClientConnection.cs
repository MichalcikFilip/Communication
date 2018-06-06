using Michalcik.Communication.Server.Security.Clients;

namespace Michalcik.Communication.Server
{
    class ClientConnection
    {
        public IConnection Connection { get; }
        public IClient Client { get; set; }

        public ClientConnection(IConnection connection, IClient client)
        {
            Connection = connection;
            Client = client;
        }
    }
}
