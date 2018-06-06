using Michalcik.Communication.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Michalcik.Communication
{
    public class Connection : IConnection, IDisposable
    {
        private const int THREAD_WAITING_TIME = 200;

        private bool running = false;
        private Queue<IMessage> messageBuffer = new Queue<IMessage>();
        private BinaryFormatter formatter = new BinaryFormatter();

        private TcpClient client;
        private NetworkStream stream;

        public event MessageReceiveHandler Received;
        public event ErrorHandler ReadError;
        public event ErrorHandler WriteError;

        public Connection(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            this.client = client;
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
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        formatter.Serialize(memoryStream, messageBuffer.Dequeue());

                        byte[] data = memoryStream.ToArray();
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
                        using (MemoryStream memoryStream = new MemoryStream(data))
                        {
                            Received?.Invoke((IMessage)formatter.Deserialize(memoryStream));
                        }
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
