using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NLog;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Network;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page;

public partial class TacticsHallPageViewModel : ViewModelBase
{
    #region 构造函数

    public TacticsHallPageViewModel(
        ILocalizationService localizationService,
        ILoggerContainer loggerContainer,
        INetwork network,
        IMessenger messenger,
        IosTools oSTools)
    {
        _localizationService = localizationService;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _network = network;
        _messenger = messenger;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();

        // 初始化选项列表
        RaceOptions = new AvaloniaList<string> { "全部", "神族", "人族", "虫族" };
        SortOptions = new AvaloniaList<string> { "最新", "热门", "下载量" };
        SelectedRace = RaceOptions[0];
        SelectedSort = SortOptions[0];

        // 检查登录状态
        if (_config.UserSession != null && _config.UserSession.IsLoggedIn)
        {
            UserSession = _config.UserSession;
            IsUserLoggedIn = true;
        }

        // 加载初始数据
        Task.Run(LoadInitialData);
    }

#pragma warning disable CS8618
    public TacticsHallPageViewModel()
#pragma warning restore CS8618
    {
    }

    #endregion

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly Logger _logger;
    private readonly INetwork _network;
    private readonly IMessenger _messenger;
    private readonly IoSes _oses;
    private readonly LConfig _config;

    #endregion

    #region 常量

    private const int PAGE_SIZE = 20;

    #endregion

    #region 状态变量

    private int _currentPage = 1;
    private bool _isLoading = false;
    private bool _hasMorePages = true;
    private string? _lastKeyword = null;
    private string? _lastRace = null;
    private string _lastSort = "latest";

    #endregion

    #region 数据绑定 - 搜索与筛选

    /// <summary>搜索关键词</summary>
    [ObservableProperty] private string _searchKeyword = string.Empty;

    /// <summary>种族筛选选项</summary>
    public AvaloniaList<string> RaceOptions { get; }

    /// <summary>排序选项</summary>
    public AvaloniaList<string> SortOptions { get; }

    /// <summary>选中的种族</summary>
    [ObservableProperty] private string _selectedRace = "全部";

    /// <summary>选中的排序</summary>
    [ObservableProperty] private string _selectedSort = "最新";

    /// <summary>当前页码显示文本</summary>
    [ObservableProperty] private string _pageInfoText = "第 1 页";

    /// <summary>是否正在加载</summary>
    [ObservableProperty] private bool _isLoadingData;

    /// <summary>加载状态文本</summary>
    [ObservableProperty] private string _loadingText = string.Empty;

    #endregion

    #region 数据绑定 - 战术列表

    /// <summary>战术文件列表</summary>
    public IAvaloniaList<NTacticsDetail> TacticsList { get; } = new AvaloniaList<NTacticsDetail>();

    /// <summary>热门排行榜</summary>
    public IAvaloniaList<NHotFile> HotFilesList { get; } = new AvaloniaList<NHotFile>();

    /// <summary>贡献者排行榜</summary>
    public IAvaloniaList<NTopUploader> TopUploadersList { get; } = new AvaloniaList<NTopUploader>();

    /// <summary>选中的战术详情</summary>
    [ObservableProperty] private NTacticsDetail? _selectedTactics;

    /// <summary>是否显示详情弹窗</summary>
    [ObservableProperty] private bool _showDetailDialog;

    /// <summary>详情弹窗中的评论列表</summary>
    public IAvaloniaList<NTacticsComment> DetailComments { get; } = new AvaloniaList<NTacticsComment>();

    /// <summary>详情弹窗中的版本列表</summary>
    public IAvaloniaList<NTacticsVersion> DetailVersions { get; } = new AvaloniaList<NTacticsVersion>();

    /// <summary>新评论内容</summary>
    [ObservableProperty] private string _newCommentContent = string.Empty;

    /// <summary>用户是否已登录</summary>
    [ObservableProperty] private bool _isUserLoggedIn;

    /// <summary>用户会话信息</summary>
    [ObservableProperty] private NUserSession? _userSession;

    #endregion

    #region Commands

    /// <summary>搜索战术</summary>
    [RelayCommand]
    private async Task Search()
    {
        _currentPage = 1;
        _hasMorePages = true;
        TacticsList.Clear();

        _lastKeyword = SearchKeyword?.Trim();
        _lastRace = ConvertRaceFilter(SelectedRace);
        _lastSort = ConvertSortFilter(SelectedSort);

        await LoadTacticsList();
    }

    /// <summary>刷新列表</summary>
    [RelayCommand]
    private async Task Refresh()
    {
        _currentPage = 1;
        _hasMorePages = true;
        TacticsList.Clear();

        await LoadTacticsList();
        await LoadLeaderboards();
    }

    /// <summary>加载下一页</summary>
    [RelayCommand]
    private async Task LoadMore()
    {
        if (!_hasMorePages || _isLoading) return;

        _currentPage++;
        await LoadTacticsList();
    }

    /// <summary>显示战术详情</summary>
    [RelayCommand]
    private async Task ShowDetail(NTacticsDetail? tactics)
    {
        if (tactics == null) return;

        SelectedTactics = tactics;
        ShowDetailDialog = true;

        // 加载详情页数据
        await LoadDetailData(tactics.ShareCode);
    }

    /// <summary>关闭详情弹窗</summary>
    [RelayCommand]
    private void CloseDetail()
    {
        ShowDetailDialog = false;
        SelectedTactics = null;
        DetailComments.Clear();
        DetailVersions.Clear();
        NewCommentContent = string.Empty;
    }

    /// <summary>下载战术文件</summary>
    [RelayCommand]
    private async Task DownloadTactics(NTacticsDetail? tactics)
    {
        if (tactics == null) return;

        IsLoadingData = true;
        LoadingText = _localizationService.GetString("TacticsHallDownloading");

        try
        {
            var response = await _network.Client.DownloadTactics(tactics.ShareCode);
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();

            // 确保目录存在
            var targetDir = Path.Combine(Environment.CurrentDirectory, _config.TacticsDir);
            _oses.CheckOrCreateDir(targetDir);

            // 生成文件名
            var fileName = $"{tactics.Name ?? tactics.ShareCode}.tactix";
            var filePath = Path.Combine(targetDir, fileName);

            // 写入文件
            File.WriteAllBytes(filePath, bytes);

            _messenger.Send(new MbToastPureText
            {
                Message = string.Format(_localizationService.GetString("TacticsHallDownloadSuccess"), fileName),
                Title = _localizationService.GetString("ToastTitleSuccess"),
                Type = MbEnumToastType.Success
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Download tactics error: {ex.Message}");
            _messenger.Send(new MbToastPureText
            {
                Message = string.Format(_localizationService.GetString("TacticsHallDownloadError"), ex.Message),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
        finally
        {
            IsLoadingData = false;
            LoadingText = string.Empty;
        }
    }

    /// <summary>点赞</summary>
    [RelayCommand]
    private async Task ToggleLike()
    {
        if (SelectedTactics == null || !IsUserLoggedIn)
        {
            ShowLoginRequiredToast();
            return;
        }

        try
        {
            var result = await _network.Client.ToggleLike(SelectedTactics.ShareCode);

            // 更新本地状态
            Dispatcher.UIThread.Post(() =>
            {
                SelectedTactics.IsLikedByUser = result.IsLiked;
                SelectedTactics.LikeCount = result.LikeCount;
                OnPropertyChanged(nameof(SelectedTactics));
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Toggle like error: {ex.Message}");
            ShowLoginRequiredToast();
        }
    }

    /// <summary>收藏</summary>
    [RelayCommand]
    private async Task ToggleFavorite()
    {
        if (SelectedTactics == null || !IsUserLoggedIn)
        {
            ShowLoginRequiredToast();
            return;
        }

        try
        {
            var result = await _network.Client.ToggleFavorite(SelectedTactics.ShareCode);

            // 更新本地状态
            Dispatcher.UIThread.Post(() =>
            {
                SelectedTactics.IsFavoritedByUser = result.IsFavorited;
                SelectedTactics.FavoriteCount = result.FavoriteCount;
                OnPropertyChanged(nameof(SelectedTactics));
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Toggle favorite error: {ex.Message}");
            ShowLoginRequiredToast();
        }
    }

    /// <summary>添加评论</summary>
    [RelayCommand]
    private async Task AddComment()
    {
        if (SelectedTactics == null || !IsUserLoggedIn)
        {
            ShowLoginRequiredToast();
            return;
        }

        if (string.IsNullOrWhiteSpace(NewCommentContent)) return;

        try
        {
            var comment = await _network.Client.AddComment(SelectedTactics.ShareCode,
                new NAddCommentReq { Content = NewCommentContent.Trim() });

            Dispatcher.UIThread.Post(() =>
            {
                DetailComments.Add(comment);
                NewCommentContent = string.Empty;
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Add comment error: {ex.Message}");
            ShowLoginRequiredToast();
        }
    }

    /// <summary>登录（根据编译模式自动切换）</summary>
    [RelayCommand]
    private async Task Login()
    {
#if DEBUG
        // 开发环境：使用 DevLogin
        await DevLogin(null);
#else
        // 生产环境：使用 OAuth 登录
        await LoginWithProvider("qq");
#endif
    }

    /// <summary>OAuth 登录（生产环境）</summary>
    private async Task LoginWithProvider(string provider)
    {
        try
        {
            // 获取登录 URL
            var loginResp = await _network.Client.GetLoginUrl(provider);

            // 打开浏览器进行授权
            _oses.OpenUrl(loginResp.LoginUrl);

            // 提示用户授权完成后刷新
            _messenger.Send(new MbToastPureText
            {
                Message = _localizationService.GetString("TacticsHallLoginPrompt"),
                Title = _localizationService.GetString("ToastTitleInfo"),
                Type = MbEnumToastType.Info
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"OAuth login error: {ex.Message}");
            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
    }

    /// <summary>开发模式登录</summary>
    [RelayCommand]
    private async Task DevLogin(string? devUserId)
    {
        try
        {
            var session = await _network.Client.DevLogin(devUserId ?? "test");
            UserSession = session;
            IsUserLoggedIn = session.IsLoggedIn;

            // 保存到配置
            _config.UserSession = session;
            _oses.SaveConfig();

            _messenger.Send(new MbToastPureText
            {
                Message = string.Format(_localizationService.GetString("TacticsHallLoginSuccess"), session.Nickname),
                Title = _localizationService.GetString("ToastTitleSuccess"),
                Type = MbEnumToastType.Success
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Dev login error: {ex.Message}");
            _messenger.Send(new MbToastPureText
            {
                Message = ex.Message,
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
    }

    /// <summary>登出</summary>
    [RelayCommand]
    private void Logout()
    {
        UserSession = null;
        IsUserLoggedIn = false;

        // 清除配置中的登录信息
        _config.UserSession = null;
        _oses.SaveConfig();

        _messenger.Send(new MbToastPureText
        {
            Message = _localizationService.GetString("TacticsHallLogoutSuccess"),
            Title = _localizationService.GetString("ToastTitleInfo"),
            Type = MbEnumToastType.Info
        });
    }

    /// <summary>打开战术目录</summary>
    [RelayCommand]
    private void OpenTacticsFolder()
    {
        var path = Path.Combine(Environment.CurrentDirectory, _config.TacticsDir);
        _oses.CheckOrCreateDir(path);
        _oses.OpenUrl(path);
    }

    #endregion

    #region 私有方法

    /// <summary>加载初始数据</summary>
    private async Task LoadInitialData()
    {
        await LoadTacticsList();
        await LoadLeaderboards();
    }

    /// <summary>加载战术列表</summary>
    private async Task LoadTacticsList()
    {
        if (_isLoading) return;

        _isLoading = true;
        IsLoadingData = true;
        LoadingText = _localizationService.GetString("TacticsHallLoading");

        try
        {
            var result = await _network.Client.SearchTactics(
                _lastKeyword, _lastRace, null, _lastSort, _currentPage, PAGE_SIZE);

            // Calculate pagination state
            _hasMorePages = result.Page * PAGE_SIZE < result.TotalCount;
            var totalPages = (int)Math.Ceiling(result.TotalCount / (double)PAGE_SIZE);

            Dispatcher.UIThread.Post(() =>
            {
                TacticsList.AddRange(result.Files);
                PageInfoText = string.Format(_localizationService.GetString("TacticsHallPageInfo"),
                    result.Page, totalPages);
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Load tactics list error: {ex.Message}");
            _messenger.Send(new MbToastPureText
            {
                Message = string.Format(_localizationService.GetString("TacticsHallLoadError"), ex.Message),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
        finally
        {
            _isLoading = false;
            IsLoadingData = false;
            LoadingText = string.Empty;
        }
    }

    /// <summary>加载排行榜</summary>
    private async Task LoadLeaderboards()
    {
        try
        {
            // Parallelize independent API calls
            var hotFilesTask = _network.Client.GetHotFiles();
            var topUploadersTask = _network.Client.GetTopUploaders();
            await Task.WhenAll(hotFilesTask, topUploadersTask);

            var hotFilesResult = await hotFilesTask;
            var topUploadersResult = await topUploadersTask;

            Dispatcher.UIThread.Post(() =>
            {
                HotFilesList.Clear();
                HotFilesList.AddRange(hotFilesResult.Files.Take(10));

                TopUploadersList.Clear();
                TopUploadersList.AddRange(topUploadersResult.Uploaders.Take(10));
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Load leaderboards error: {ex.Message}");
        }
    }

    /// <summary>加载详情页数据</summary>
    private async Task LoadDetailData(string shareCode)
    {
        try
        {
            // Parallelize independent API calls
            var versionsTask = _network.Client.GetTacticsVersions(shareCode);
            var commentsTask = _network.Client.GetComments(shareCode);
            await Task.WhenAll(versionsTask, commentsTask);

            var versionsResult = await versionsTask;
            var commentsResult = await commentsTask;

            Dispatcher.UIThread.Post(() =>
            {
                DetailVersions.Clear();
                DetailVersions.AddRange(versionsResult.Versions);

                DetailComments.Clear();
                DetailComments.AddRange(commentsResult.Comments.Where(c => !c.IsDeleted));
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Load detail data error: {ex.Message}");
        }
    }

    /// <summary>转换种族筛选参数</summary>
    private static string? ConvertRaceFilter(string selected)
    {
        return selected switch
        {
            "神族" => "P",
            "人族" => "T",
            "虫族" => "Z",
            _ => null
        };
    }

    /// <summary>转换排序参数</summary>
    private static string ConvertSortFilter(string selected)
    {
        return selected switch
        {
            "最新" => "latest",
            "热门" => "popular",
            "下载量" => "downloads",
            _ => "latest"
        };
    }

    /// <summary>显示需要登录提示</summary>
    private void ShowLoginRequiredToast()
    {
        _messenger.Send(new MbToastPureText
        {
            Message = _localizationService.GetString("TacticsHallLoginRequired"),
            Title = _localizationService.GetString("ToastTitleInfo"),
            Type = MbEnumToastType.Info
        });
    }

    #endregion
}