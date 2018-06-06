using System.Net.Sockets;

namespace Michalcik.Communication
{
    public class ConnectionFactory : IConnectionFactory
    {
        public virtual IConnection GetConnection(TcpClient client)
        {
            return new Connection(client);
        }
    }
}
