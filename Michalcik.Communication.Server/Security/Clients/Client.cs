namespace Michalcik.Communication.Server.Security.Clients
{
    public class Client : IClient
    {
        public bool IsAuthenticated { get; }

        public Client(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }
}
