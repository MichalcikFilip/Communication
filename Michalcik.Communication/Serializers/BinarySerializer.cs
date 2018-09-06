using Michalcik.Communication.Messages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Michalcik.Communication.Serializers
{
    public class BinarySerializer : ISerializer
    {
        private BinaryFormatter formatter = new BinaryFormatter();

        public byte[] Serialize(IMessage messsage)
        {
            if (messsage != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, messsage);
                    return memoryStream.ToArray();
                }
            }

            return null;
        }

        public IMessage Deserialize(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    return (IMessage)formatter.Deserialize(memoryStream);
                }
            }

            return null;
        }
    }
}
