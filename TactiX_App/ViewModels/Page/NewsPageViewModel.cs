using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using NLog;
using TactiX_Localization;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Models.Network;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page;

public partial class NewsPageViewModel : ViewModelBase
{
    public NewsPageViewModel(ILocalizationService localizationService, IosTools oSTools, ILoggerContainer loggerContainer, INetwork network,
        IMessenger messenger)
    {
        _localizationService = localizationService;
        _oses = oSTools.OSes;
        _logger = loggerContainer.Builder.GetCurrentClassLogger();
        _network = network;
        _messenger = messenger;

        ListTopics = new AvaloniaList<NForumTopic>();
        ListSysNews = new AvaloniaList<NNewsSys>();
        ListVideos = new AvaloniaList<NVideoInfo>();

        Task.Run(UpdateNews);
    }

    public IAvaloniaList<NForumTopic> ListTopics { get; }
    public IAvaloniaList<NNewsSys> ListSysNews { get; }
    public IAvaloniaList<NVideoInfo> ListVideos { get; }

    [RelayCommand]
    public async Task OpenWeb(string url)
    {
        await Task.Run(() => { _oses.OpenUrl(url); });
    }

    /// <summary>
    ///     更新新闻信息
    /// </summary>
    public async Task UpdateNews()
    {
        try
        {
            var news = await _network.Client.GetNews();
            if (news.Count == 0) return;

            var topicsNews = news
                .First(t => t.Type == 0);
            await UpdateTopics(topicsNews);

            var videosNews = news
                .First(t => t.Type == 1);
            await UpdateVideos(videosNews);

            var newsSys = await _network.Client.GetN_NewsSys();
            await UpdateNewsSys(newsSys);
        }
        catch (Exception ex)
        {
            _logger.Error($"NewsPageViewModel.UpdateNews Error: {ex.Message}");
            _messenger.Send(new MbToastPureText
            {
                Message = $"{_localizationService.GetString("NewsPageErrorNetwork")}，错误信息：{ex.Message}",
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
    }

    /// <summary>
    ///     更新热帖
    /// </summary>
    private async Task UpdateTopics(NNews? news)
    {
        await Task.Run(() =>
        {
            if (news == null) return;
            var topics = JsonConvert.DeserializeObject<List<NForumTopic>>(news.Json);
            if (topics == null) return;

            ListTopics.Clear();

            for (var i = 0; i < topics.Count; i++)
            {
                var topic = topics[i];

                var displayText = topic.Title;
                if (displayText.Length > 30)
                    displayText = string.Concat("● ", displayText.AsSpan(0, 27), "...");
                else
                    displayText = string.Concat("● ", displayText);

                Dispatcher.UIThread.Post(() =>
                {
                    ListTopics.Add(new NForumTopic
                    {
                        Date = topic.Date,
                        Title = displayText,
                        Url = topic.Url
                    });
                });
            }
        });
    }

    /// <summary>
    ///     更新视频信息
    /// </summary>
    private async Task UpdateVideos(NNews? news)
    {
        await Task.Run(async () =>
        {
            if (news == null) return;
            var videos = JsonConvert.DeserializeObject<List<NVideoInfo>>(news.Json);
            if (videos == null) return;

            ListVideos.Clear();

            for (var i = 0; i < videos.Count; i++)
            {
                var video = videos[i];
                video.ImageObj = await LoadFromWeb(new Uri(video.CoverUrl));
            }

            Dispatcher.UIThread.Post(() => ListVideos.AddRange(videos));
        });
    }

    /// <summary>
    ///     更新系统公告
    /// </summary>
    public async Task UpdateNewsSys(List<NNewsSys> newsSys)
    {
        await Task.Run(() =>
        {
            foreach (var v in newsSys)
            {
                var displayText = v.Title;
                
                displayText = displayText.Length > 30 
                    ? string.Concat("● ", displayText.AsSpan(0, 27), "...") 
                    : string.Concat("● ", displayText);

                var news = new NNewsSys
                {
                    DateTime = v.DateTime,
                    Link = v.Link,
                    Title = "● " + displayText
                };

                Dispatcher.UIThread.Post(() => ListSysNews.Add(news));
            }
        });
    }

    public async Task<Bitmap?> LoadFromWeb(Uri url)
    {
        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsByteArrayAsync();
            return new Bitmap(new MemoryStream(data));
        }
        catch (HttpRequestException ex)
        {
            _logger.Error($"An error occurred while downloading image '{url}' : {ex.Message}");
            return null;
        }
    }

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly IoSes _oses;
    private readonly Logger _logger;
    private readonly INetwork _network;
    private readonly IMessenger _messenger;

    #endregion
}