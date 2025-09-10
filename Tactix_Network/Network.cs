using Refit;

namespace TactiX_Network
{
    public class Network : INetwork
    {
#if DEBUG
        private const string WebServerUrl = "http://127.0.0.1:5112";
#else
        private const string WebServerUrl = "https://api.east-unicorn.cn";
#endif

        public Network()
        {
            Client = RestService.For<INetworkApi>(WebServerUrl);
        }

        public INetworkApi Client { get; private set; }
    }
}
