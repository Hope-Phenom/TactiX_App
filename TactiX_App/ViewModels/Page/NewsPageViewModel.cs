using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public IAvaloniaList<TextBlock> List_SysNews { get; private set; }
        public IAvaloniaList<TextBlock> List_Videos { get; private set; }

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
            List_SysNews = new AvaloniaList<TextBlock>();
            List_Videos = new AvaloniaList<TextBlock>();

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
                if (topicsNews == null) return;
                var topics = JsonConvert.DeserializeObject<List<N_ForumTopic>>(topicsNews.Json);
                if (topics == null) return;

                var videosNews = news
                    .Where(t => t.Type == 1)
                    .First();
                if (videosNews == null) return;
                var videos = JsonConvert.DeserializeObject<List<N_VideoInfo>>(videosNews.Json);
                if (videos == null) return;

                List_Topics.Clear();
                List_Videos.Clear();

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
    }
}
