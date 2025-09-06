using Refit;

using TactiX_Models;

namespace TactiX_Network
{
    [Headers("Content-Type: application/json")]
    public interface INetworkApi
    {
        /// <summary>
        /// Post上报异常报告
        /// </summary>
        /// <param name="reportModel">异常报告</param>
        /// <returns></returns>
        [Post("/api/Stats/PostExceptionReport")]
        Task<HttpResponseMessage> PostExceptionReportModel([Body] NExceptionReportModel reportModel);
    }
}
