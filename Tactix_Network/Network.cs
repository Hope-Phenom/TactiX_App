using Refit;

namespace TactiX_Network;

public class Network : INetwork
{
#if DEBUG
    private const string WEB_SERVER_URL = "http://127.0.0.1:5112";
#else
        private const string WebServerUrl = "https://api.east-unicorn.cn:8088";
#endif

    public Network()
    {
        Client = RestService.For<INetworkApi>(WEB_SERVER_URL);
    }

    public INetworkApi Client { get; }
}