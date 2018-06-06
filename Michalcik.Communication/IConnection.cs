using Michalcik.Communication.Messages;
using System;

namespace Michalcik.Communication
{
    public interface IConnection : IDisposable
    {
        event MessageReceiveHandler Received;
        event ErrorHandler ReadError;
        event ErrorHandler WriteError;

        bool IsConnected { get; }

        void Open();
        void Close();
        void Send(IMessage message);
    }
}
