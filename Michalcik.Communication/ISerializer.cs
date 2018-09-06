using Michalcik.Communication.Messages;

namespace Michalcik.Communication
{
    public interface ISerializer
    {
        byte[] Serialize(IMessage messsage);
        IMessage Deserialize(byte[] data);
    }
}
