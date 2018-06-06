namespace Michalcik.Communication.Server.Security.Clients
{
    public class AuthTokenClient : Client, IClient
    {
        public object Token { get; }

        public AuthTokenClient(object token, bool isAuthenticated)
            : base(isAuthenticated)
        {
            Token = token;
        }
    }
}
