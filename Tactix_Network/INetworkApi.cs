using Refit;
using TactiX_Models.Network;

namespace TactiX_Network;

[Headers("Content-Type: application/json")]
public interface INetworkApi
{
    /// <summary>
    ///     Post上报异常报告
    /// </summary>
    /// <param name="reportModel">异常报告</param>
    /// <returns></returns>
    [Post("/api/Stats/PostExceptionReport")]
    Task<HttpResponseMessage> PostExceptionReportModel([Body] NExceptionReportModel reportModel);

    /// <summary>
    ///     Post请求最新的版本控制信息
    /// </summary>
    /// <param name="versionControlReq">版本控制请求</param>
    /// <returns></returns>
    [Post("/api/Stats/PostVersionControl")]
    Task<NVersionControlResp> PostVersionControlReq([Body] NVersionControlReq versionControlReq);

    [Get("/api/News/GetNews")]
    Task<List<NNews>> GetNews();

    [Get("/api/News/GetNewsSys")]
    Task<List<NNewsSys>> GetN_NewsSys();
}