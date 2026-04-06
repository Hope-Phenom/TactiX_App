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

    #region 战术大厅 API

    /// <summary>
    ///     搜索战术文件
    /// </summary>
    [Get("/api/TacticsHall/Search")]
    Task<NTacticsSearchResult> SearchTactics(
        [Query] string? keyword,
        [Query] string? race,
        [Query] long? uploaderId,
        [Query] string sortBy,
        [Query] int page,
        [Query] int pageSize);

    /// <summary>
    ///     获取战术文件详情
    /// </summary>
    [Get("/api/TacticsHall/Detail/{shareCode}")]
    Task<NTacticsDetail> GetTacticsDetail(string shareCode);

    /// <summary>
    ///     下载战术文件
    /// </summary>
    [Get("/api/TacticsHall/Download/{shareCode}")]
    Task<HttpResponseMessage> DownloadTactics(string shareCode);

    /// <summary>
    ///     获取版本列表
    /// </summary>
    [Get("/api/TacticsHall/Versions/{shareCode}")]
    Task<NTacticsVersionsResult> GetTacticsVersions(string shareCode);

    #endregion

    #region 战术互动 API

    /// <summary>
    ///     点赞/取消点赞
    /// </summary>
    [Post("/api/TacticsInteraction/Like/{shareCode}")]
    Task<NLikeResult> ToggleLike(string shareCode);

    /// <summary>
    ///     收藏/取消收藏
    /// </summary>
    [Post("/api/TacticsInteraction/Favorite/{shareCode}")]
    Task<NFavoriteResult> ToggleFavorite(string shareCode);

    /// <summary>
    ///     获取我点赞的文件
    /// </summary>
    [Get("/api/TacticsInteraction/Liked")]
    Task<NTacticsSearchResult> GetMyLiked([Query] int page, [Query] int pageSize);

    /// <summary>
    ///     获取我的收藏
    /// </summary>
    [Get("/api/TacticsInteraction/Favorites")]
    Task<NTacticsSearchResult> GetMyFavorites([Query] int page, [Query] int pageSize);

    /// <summary>
    ///     添加评论
    /// </summary>
    [Post("/api/TacticsInteraction/Comment/{shareCode}")]
    Task<NTacticsComment> AddComment(string shareCode, [Body] NAddCommentReq request);

    /// <summary>
    ///     获取评论列表
    /// </summary>
    [Get("/api/TacticsInteraction/Comments/{shareCode}")]
    Task<NTacticsCommentResult> GetComments(string shareCode, [Query] int page = 1, [Query] int pageSize = 20);

    /// <summary>
    ///     删除评论
    /// </summary>
    [Delete("/api/TacticsInteraction/Comment/{id}")]
    Task<bool> DeleteComment(long id);

    #endregion

    #region 排行榜 API

    /// <summary>
    ///     获取热门战术排行榜
    /// </summary>
    [Get("/api/Leaderboard/HotFiles")]
    Task<NHotFilesResult> GetHotFiles(
        [Query] string period = "weekly",
        [Query] string? race = null,
        [Query] string sortBy = "downloads",
        [Query] int page = 1,
        [Query] int pageSize = 20);

    /// <summary>
    ///     获取贡献者排行榜
    /// </summary>
    [Get("/api/Leaderboard/TopUploaders")]
    Task<NTopUploadersResult> GetTopUploaders(
        [Query] string period = "monthly",
        [Query] int page = 1,
        [Query] int pageSize = 20);

    #endregion

    #region 认证 API

    /// <summary>
    ///     获取OAuth登录URL
    /// </summary>
    [Get("/api/Auth/Login/{provider}")]
    Task<NLoginUrlResp> GetLoginUrl(string provider, [Query] string? redirectUri = null);

    /// <summary>
    ///     OAuth回调处理
    /// </summary>
    [Get("/api/Auth/Callback/{provider}")]
    Task<NUserSession> HandleOAuthCallback(string provider, [Query] string code, [Query] string? state = null);

    /// <summary>
    ///     开发模式登录
    /// </summary>
    [Get("/api/Auth/DevLogin")]
    Task<NUserSession> DevLogin([Query] string? devUserId = null, [Query] string? state = null);

    /// <summary>
    ///     获取用户等级信息（需要认证）
    /// </summary>
    [Get("/api/Auth/LevelInfo")]
    Task<NUserLevelInfo> GetLevelInfo();

    #endregion
}