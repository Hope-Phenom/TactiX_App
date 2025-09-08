using Refit;

namespace TactiX_Network
{
    public class Network
    {
        #region 单例模式
        private static readonly Lazy<Network> _lazyInstance = new Lazy<Network>(() => new Network(), isThreadSafe: true);
        public static Network Instance => _lazyInstance.Value;
        private Network()
        {
            Client = RestService.For<INetworkApi>(WebServerUrl);
        }
        #endregion

#if DEBUG
        private const string WebServerUrl = "http://127.0.0.1:5112";
#else
        private const string WebServerUrl = "https://api.east-unicorn.cn";
#endif

        public INetworkApi Client { get; private set; }
    }
}
