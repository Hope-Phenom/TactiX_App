using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TactiX_App.Service;
using TactiX_App.Views;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models.MessageBus;
using TactiX_Models.Network;
using TactiX_Network;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Page
{
    public partial class NewsPageViewModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        private readonly IOSes _oses;
        private readonly Logger _logger;
        private readonly INetwork _network;
        private readonly IMessenger _messenger;
        #endregion

        public IAvaloniaList<N_ForumTopic> List_Topics { get; private set; }
        public IAvaloniaList<N_VideoInfo> List_SysNews { get; private set; }
        public IAvaloniaList<N_VideoInfo> List_Videos { get; private set; }

        public NewsPageViewModel(ILang lang, IServiceProvider serviceProvider, IOSTools oSTools,
            ILoggerContainer loggerContainer, INetwork network,
            IMessenger messenger)
        {
            Language = lang.Language;

            _oses = oSTools.OSes;
            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _network = network;
            _messenger = messenger;

            List_Topics = new AvaloniaList<N_ForumTopic>();
            List_SysNews = new AvaloniaList<N_VideoInfo>();
            List_Videos = new AvaloniaList<N_VideoInfo>();

            Task.Run(UpdateNews);
        }

        [RelayCommand]
        public async Task OpenWeb(string url)
        {
            await Task.Run(() => 
            {
                _oses.OpenWeb(url);
            });
        }

        /// <summary>
        /// 更新新闻信息
        /// </summary>
        public async Task UpdateNews()
        {
            try
            {
                var news = await _network.Client.GetNews();
                if (news == null || news.Count == 0) return;

                var topicsNews = news
                    .Where(t => t.Type == 0)
                    .First();
                await UpdateTopics(topicsNews);

                var videosNews = news
                    .Where(t => t.Type == 1)
                    .First();
                await UpdateVideos(videosNews);


            }
            catch (Exception ex)
            {
                _logger.Error($"NewsPageViewModel.UpdateNews Error: {ex.Message}");
                _messenger.Send(new MB_ToastPureText() 
                { 
                    Message = $"{Language.NEWS_PAGE_ERROR_NETWORK}，错误信息：{ex.Message}",
                    Title = Language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });
            }
        }
        /// <summary>
        /// 更新热帖
        /// </summary>
        public async Task UpdateTopics(N_News news)
        {
            await Task.Run(() =>
            {
                if (news == null) return;
                var topics = JsonConvert.DeserializeObject<List<N_ForumTopic>>(news.Json);
                if (topics == null) return;

                List_Topics.Clear();

                for (int i = 0; i < topics.Count; i++)
                {
                    var topic = topics[i];

                    var displayText = topic.Title;
                    if (displayText.Length > 30)
                    {
                        displayText = displayText.Substring(0, 27) + "...";
                    }

                    Dispatcher.UIThread.Post(() =>
                    {
                        List_Topics.Add(new N_ForumTopic()
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
        /// 更新视频信息
        /// </summary>
        public async Task UpdateVideos(N_News news)
        {
            await Task.Run(async () =>
            {
                if (news == null) return;
                var videos = JsonConvert.DeserializeObject<List<N_VideoInfo>>(news.Json);
                if (videos == null) return;

                List_Videos.Clear();

                for (int i = 0; i < videos.Count; i++)
                {
                    var video = videos[i];
                    video.ImageObj = await LoadFromWeb(new Uri(video.CoverUrl));
                }

                Dispatcher.UIThread.Post(() => List_Videos.AddRange(videos));
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
    }
}
