namespace Michalcik.Communication.Server.Security.Clients
{
    public class UserPasswordClient : Client, IClient
    {
        public string Username { get; }

        public UserPasswordClient(string username, bool isAuthenticated)
            : base(isAuthenticated)
        {
            Username = username;
        }
    }
}
