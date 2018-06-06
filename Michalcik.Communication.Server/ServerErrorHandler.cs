using Michalcik.Communication.Server.Security.Clients;
using System;

namespace Michalcik.Communication.Server
{
    public delegate void ServerErrorHandler(Exception exception, IConnection connection, IClient client);
}
