using System.Net.Sockets;

namespace Michalcik.Communication
{
    public interface IConnectionFactory
    {
        IConnection GetConnection(TcpClient client);
    }
}
