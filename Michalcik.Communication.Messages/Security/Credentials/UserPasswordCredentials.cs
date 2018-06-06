using System;

namespace Michalcik.Communication.Messages.Security.Credentials
{
    [Serializable]
    public class UserPasswordCredentials : BaseCredentials, ICredentials
    {
        public string Username { get; }
        public string Password { get; }

        public UserPasswordCredentials(string username, string password)
            : base(AuthenticationTypes.UserPassword.ToString())
        {
            Username = username;
            Password = password;
        }
    }
}
