using Michalcik.Communication.Messages;

namespace Michalcik.Communication.Client
{
    public interface IResponseHandler
    {
        void Handle(IResponse response);
    }
}
