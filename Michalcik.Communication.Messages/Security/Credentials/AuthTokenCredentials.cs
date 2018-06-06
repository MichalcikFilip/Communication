using System;

namespace Michalcik.Communication.Messages.Security.Credentials
{
    [Serializable]
    public class AuthTokenCredentials : BaseCredentials, ICredentials
    {
        public object Token { get; }

        public AuthTokenCredentials(object token)
            : base(AuthenticationTypes.AuthToken.ToString())
        {
            Token = token;
        }
    }
}
