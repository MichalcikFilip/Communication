using Michalcik.Communication.Serializers;
using System.Net.Sockets;

namespace Michalcik.Communication.Factories
{
    public class BinaryConnectionFactory : IConnectionFactory
    {
        public IConnection GetConnection(TcpClient client)
        {
            return new Connection(new BinarySerializer(), client);
        }
    }
}
