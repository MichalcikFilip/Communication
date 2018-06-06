using System;

namespace Michalcik.Communication.Messages.Security
{
    [Serializable]
    public class Authentication : Message, IMessage
    {
        public ICredentials Credentials { get; }

        public Authentication(ICredentials credentials)
        {
            Credentials = credentials;
        }
    }
}
