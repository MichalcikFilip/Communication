using Michalcik.Communication.Messages.Security;
using Michalcik.Communication.Server.Security.Clients;

namespace Michalcik.Communication.Server.Security.Authenticators
{
    public interface IAuthenticator
    {
        string AuthenticationType { get; }

        IClient Authenticate(ICredentials credentials);
    }
}
