using System;

namespace Michalcik.Communication.Messages.Security.Credentials
{
    [Serializable]
    public abstract class BaseCredentials : ICredentials
    {
        public string AuthenticationType { get; }

        public BaseCredentials(string authenticationType)
        {
            AuthenticationType = authenticationType;
        }
    }
}
