using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Material.Icons;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactiX_I18N;
using TactiX_Logger;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.ViewModels.Popup
{
    public partial class TacticPlayWindowModel : ViewModelBase
    {
        #region DI容器注入
        public ILanguage Language { get; private set; }
        private readonly Logger _logger;
        private readonly IMessenger _messenger;
        private readonly L_Config _config;
        private readonly IOSes _oses;
        #endregion

        #region 变量/常量
        private const string WINDOW_NAME = "TacticPlayWindow";
        private const string TITLE_BAR_IMAGE = "titlebar.png";
        private readonly Point PLAYING_SIZE = new(700, 180);
        private readonly Point MINI_SIZE = new(700, 230);
        private readonly Point NORMAL_SIZE = new(700, 500);

        /// <summary>
        /// 当前装载的Mod
        /// </summary>
        private readonly ModPackage _modPackage;
        #endregion

        #region 数据绑定
        /// <summary>
        /// UI是否是准备模式
        /// </summary>
        public bool IsPrepare { get; private set; }
        /// <summary>
        /// UI是否是迷你模式
        /// </summary>
        private bool IsMini;
        /// <summary>
        /// UI的高度
        /// </summary>
        [ObservableProperty]
        public int uIHeight;
        /// <summary>
        /// 设置Group的分割线高度
        /// </summary>
        [ObservableProperty]
        public GridLength horizontalLineHeight;
        /// <summary>
        /// 设置Group的高度
        /// </summary>
        [ObservableProperty]
        public GridLength groupHeight;
        /// <summary>
        /// 下拉按钮的图标
        /// </summary>
        [ObservableProperty]
        public MaterialIconKind materialIconKind;
        /// <summary>
        /// TitleBarImage
        /// </summary>
        [ObservableProperty]
        public IImage titleBarImage;
        #endregion

        public TacticPlayWindowModel(ILang lang, ILoggerContainer loggerContainer, IMessenger messenger, 
            IOSTools oSTools) 
        {
            Language = lang.Language;
            _logger = loggerContainer.Builder.GetCurrentClassLogger();
            _messenger = messenger;
            _oses = oSTools.OSes;
            _config = _oses.LoadConfig();

            // 加载指定MOD
            _modPackage = new ModPackage(_config.CurrentlyEnabledMOD);

            // UI初始化
            IsPrepare = true;
            UIHeight = NORMAL_SIZE.Y;
            HorizontalLineHeight = new GridLength(10);
            GroupHeight = GridLength.Star;
            MaterialIconKind = MaterialIconKind.ArrowExpandUp;

            using var ms = new MemoryStream(_modPackage.ReadBinaryFile(TITLE_BAR_IMAGE));
            titleBarImage = new Bitmap(ms);
        }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
        public TacticPlayWindowModel() { } // 此构造函数仅用于保证可预览
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

        /// <summary>
        /// 关闭当前播放窗体
        /// </summary>
        [RelayCommand]
        public void CloseWindow()
        {
            _messenger.Send(new MB_WindowClose() 
            { 
                Name = WINDOW_NAME
            });

            _modPackage.Dispose();
        }

        /// <summary>
        /// 拉起当前窗体
        /// </summary>
        [RelayCommand]
        public void PullWindow()
        {
            UIHeight = IsMini
                ? NORMAL_SIZE.Y
                : MINI_SIZE.Y;

            HorizontalLineHeight = IsMini
                ? new GridLength(10)
                : new GridLength(0);

            GroupHeight = IsMini
                ? GridLength.Star
                : new GridLength(0);

            MaterialIconKind = IsMini
                ? MaterialIconKind.ArrowExpandUp
                : MaterialIconKind.ArrowExpandDown;

            IsMini = !IsMini;
        }

        public void PlayTactic()
        { 
        }
    }
}
