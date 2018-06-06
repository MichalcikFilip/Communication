using Michalcik.Communication.Messages.Security;
using Michalcik.Communication.Messages.Security.Credentials;
using Michalcik.Communication.Server.Security.Clients;

namespace Michalcik.Communication.Server.Security.Authenticators
{
    public abstract class UserPasswordAuthenticator : IAuthenticator
    {
        public string AuthenticationType { get { return AuthenticationTypes.UserPassword.ToString(); } }

        public virtual IClient Authenticate(ICredentials credentials)
        {
            if (credentials != null && credentials is UserPasswordCredentials)
                return Authenticate(((UserPasswordCredentials)credentials).Username, ((UserPasswordCredentials)credentials).Password);

            return new Client(false);
        }

        protected abstract UserPasswordClient Authenticate(string username, string password);
    }
}
