using Michalcik.Communication.Messages;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Michalcik.Communication
{
    public class Connection : IConnection, IDisposable
    {
        private const int THREAD_WAITING_TIME = 200;

        private bool running = false;
        private Queue<IMessage> messageBuffer = new Queue<IMessage>();

        private ISerializer serializer;
        private TcpClient client;
        private NetworkStream stream;

        public event MessageReceiveHandler Received;
        public event ErrorHandler ReadError;
        public event ErrorHandler WriteError;

        public Connection(ISerializer serializer, TcpClient client)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            stream = client.GetStream();
        }

        public bool IsConnected { get { return client.Connected; } }

        public void Open()
        {
            if (!running)
            {
                running = true;

                new Thread(ReadThread).Start();
                new Thread(WriteThread).Start();
            }
        }

        public void Close()
        {
            running = false;

            stream?.Close();
            client?.Close();
        }

        public void Send(IMessage message)
        {
            if (message != null)
                messageBuffer.Enqueue(message);
        }

        public void Dispose()
        {
            Close();
        }

        private void WriteThread()
        {
            while (running)
            {
                while (messageBuffer.Count > 0)
                {
                    byte[] data = serializer.Serialize(messageBuffer.Dequeue());
                    byte[] dataLength = BitConverter.GetBytes(data.Length);

                    try
                    {
                        stream.Write(dataLength, 0, 4);
                        stream.Write(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        WriteError?.Invoke(ex);
                    }
                }

                Thread.Sleep(THREAD_WAITING_TIME);
            }
        }

        private void ReadThread()
        {
            byte[] dataLength = new byte[4];

            while (running)
            {
                try
                {
                    stream.Read(dataLength, 0, 4);

                    int readedData = 0, totalData = BitConverter.ToInt32(dataLength, 0);
                    byte[] data = new byte[totalData];

                    do
                    {
                        readedData += stream.Read(data, readedData, totalData - readedData);
                    } while (readedData < totalData);

                    if (data.Length > 0)
                    {
                        Received?.Invoke(serializer.Deserialize(data));
                    }
                }
                catch (Exception ex)
                {
                    ReadError?.Invoke(ex);
                }

                Thread.Sleep(THREAD_WAITING_TIME);
            }
        }
    }
}
