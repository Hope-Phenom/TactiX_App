using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Toasts;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views.Popup;
using TactiX_I18N;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow,
    IRecipient<MbToastPureText>, IRecipient<MbToastVersion>, IRecipient<MbOpenTacticPlayWindow>,
    IRecipient<MbWindowStatus>, IRecipient<MbFileDialog>, IRecipient<MbWindowTitle>,
    IRecipient<MbFolderDialog>
{
    private readonly LConfig _config;
    private readonly ILanguage _language;
    private readonly IMessenger _messenger;
    private readonly IoSes _oses;
    private readonly IServiceProvider _serviceProvider;

    private readonly string _mainWindow = "MainWindow";

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Opened += MainWindow_Opened;
        Closed += MainWindow_Closed;

        _serviceProvider = serviceProvider;
        _language = _serviceProvider.GetRequiredService<ILang>().Language;
        _oses = _serviceProvider.GetRequiredService<IosTools>().OSes;
        _messenger = _serviceProvider.GetRequiredService<IMessenger>();
        _config = _oses.LoadConfig();

        _messenger.RegisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public MainWindow() // 此构造函数仅用于保证窗体浏览正常
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        InitializeComponent();
    }
#endif

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        OsesSetHandle();

        _oses.RegisterWndProcHookCallback(this);

        _messenger.Send(new MbCheckVersion());
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _oses.UnregisterAllHotkeys();
    }

    /// <summary>
    ///     OSTools绑定窗体句柄
    /// </summary>
    private void OsesSetHandle()
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null) _oses.SetMainWindowHandle(platformHandle.Handle);
    }

    /// <summary>
    ///     注册所有的热键
    /// </summary>
    private bool RegisterAllHotkeys()
    {
        var final = true;

        foreach (var hotkeySetting in _config.Hotkeys)
        {
            var result = _oses.RegisterHotkey(
                hotkeySetting.Key,
                hotkeySetting.Modifiers,
                () => _messenger.Send(new MbHotkey(hotkeySetting.Hotkey)));

            if (!result)
                _messenger.Send(new MbToastPureText
                {
                    Message = string.Format(_language.TacticPlayingHotkeyAlreadyExsits,
                        hotkeySetting.Modifiers,
                        hotkeySetting.Key),
                    Title = _language.ToastTitleError,
                    Type = MbEnumToastType.Error
                });

            final &= result;
        }

        return final;
    }

    /// <summary>
    ///     打开文件选择弹窗
    /// </summary>
    private void OpenFileDialog(MbFileDialog message)
    {
        if (string.IsNullOrEmpty(message.FileFilter) || string.IsNullOrEmpty(message.FileFilterName)) return;

        var storageFiles = StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType(message.FileFilterName) { Patterns = [message.FileFilter] }]
        }).Result;

        if (storageFiles.Count == 0) return;

        _messenger.Send(new MbFileDialog
        {
            WindowName = _mainWindow,
            FilePath = storageFiles[0].TryGetLocalPath(),
            IsOpenMode = true,
            Trigger = message.Trigger
        });
    }

    /// <summary>
    ///     打开文件保存弹窗
    /// </summary>
    private void SaveFileDialog(MbFileDialog message)
    {
        if (string.IsNullOrEmpty(message.FileFilter) || string.IsNullOrEmpty(message.FileFilterName)) return;

        IStorageFolder? suggestFoldr = null;
        if (message.SuggestStartLocation != null)
            suggestFoldr = StorageProvider
                .TryGetFolderFromPathAsync(message.SuggestStartLocation)
                .Result;

        var storageFile = StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            FileTypeChoices = [new FilePickerFileType(message.FileFilterName) { Patterns = [message.FileFilter] }],
            ShowOverwritePrompt = true,
            SuggestedStartLocation = suggestFoldr
        }).Result;

        if (storageFile == null) return;

        _messenger.Send(new MbFileDialog
        {
            WindowName = _mainWindow,
            FilePath = storageFile.TryGetLocalPath(),
            IsOpenMode = false,
            Trigger = message.Trigger
        });
    }

    #region MessageBus 消息处理

    /// <summary>
    ///     枚举转换
    /// </summary>
    private static NotificationType NotificationTypeConvert(MbEnumToastType enumToastType)
    {
        var type = enumToastType switch
        {
            MbEnumToastType.Info => NotificationType.Information,
            MbEnumToastType.Success => NotificationType.Success,
            MbEnumToastType.Warn => NotificationType.Warning,
            MbEnumToastType.Error => NotificationType.Error,
            _ => NotificationType.Information
        };

        return type;
    }

    public void Receive(MbToastPureText msg)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(15))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .Queue();
        });
    }

    public void Receive(MbToastVersion msg)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(15))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .WithActionButton(_language.ButtonTxtSubmit, _ =>
                {
                    if (!string.IsNullOrEmpty(msg.ReleaseUrl)) _oses.OpenUrl(msg.ReleaseUrl);
                }, true)
                .Queue();
        });
    }

    public void Receive(MbOpenTacticPlayWindow message)
    {
        if (!RegisterAllHotkeys()) return;

        var tacPlayWindow = _serviceProvider.GetRequiredService<TacticPlayWindow>();
        tacPlayWindow.DataContext = _serviceProvider.GetRequiredService<TacticPlayWindowModel>();

        WindowState = WindowState.Minimized;

        tacPlayWindow.ShowDialog(this);
    }

    public void Receive(MbWindowStatus message)
    {
        switch (message.WindowStatus)
        {
            case MbWindowStatus.MbEnumWindowStatus.Normal:
                WindowState = WindowState.Normal;
                break;
            case MbWindowStatus.MbEnumWindowStatus.Minimized:
                WindowState = WindowState.Minimized;
                break;
            case MbWindowStatus.MbEnumWindowStatus.Maximized:
                WindowState = WindowState.Maximized;
                break;
        }
    }

    public void Receive(MbFileDialog message)
    {
        if (!string.IsNullOrEmpty(message.FilePath)) return;

        if (message.WindowName != _mainWindow) return;

        if (message.IsOpenMode)
            OpenFileDialog(message);
        else
            SaveFileDialog(message);
    }

    public void Receive(MbWindowTitle message)
    {
        if (message.WindowName != _mainWindow) return;

        Title = message.Title;
    }

    public void Receive(MbFolderDialog message)
    {
        if (message.WindowName != _mainWindow) return;
        if (!string.IsNullOrEmpty(message.FolderPath)) return;

        var folder = StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false
        }).Result;

        if (folder.Count == 0) return;

        _messenger.Send(new MbFolderDialog
        {
            WindowName = _mainWindow,
            Trigger = message.Trigger,
            FolderPath = folder[0].TryGetLocalPath()
        });
    }

    #endregion
}