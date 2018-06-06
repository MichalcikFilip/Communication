namespace Michalcik.Communication.Server.Security.Clients
{
    public interface IClient
    {
        bool IsAuthenticated { get; }
    }
}
