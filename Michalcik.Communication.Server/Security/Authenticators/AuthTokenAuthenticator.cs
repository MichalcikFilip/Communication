using Michalcik.Communication.Messages.Security;
using Michalcik.Communication.Messages.Security.Credentials;
using Michalcik.Communication.Server.Security.Clients;

namespace Michalcik.Communication.Server.Security.Authenticators
{
    public abstract class AuthTokenAuthenticator : IAuthenticator
    {
        public string AuthenticationType { get { return AuthenticationTypes.AuthToken.ToString(); } }

        public virtual IClient Authenticate(ICredentials credentials)
        {
            if (credentials != null && credentials is AuthTokenCredentials)
                return Authenticate(((AuthTokenCredentials)credentials).Token);

            return new Client(false);
        }

        protected abstract AuthTokenClient Authenticate(object token);
    }
}
