using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using HarfBuzzSharp;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Toasts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using TactiX_App.ViewModels;
using TactiX_App.ViewModels.Popup;
using TactiX_App.Views.Popup;
using TactiX_I18N;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_OS_Tools;

namespace TactiX_App.Views;

public partial class MainWindow : SukiWindow,
    IRecipient<MB_ToastPureText>, IRecipient<MB_ToastVersion>, IRecipient<MB_OpenTacticPlayWindow>,
    IRecipient<MB_WindowStatus>, IRecipient<MB_FileDialog>, IRecipient<MB_WindowTitle>,
    IRecipient<MB_FolderDialog>
{
    private readonly ILanguage _language;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOSes _oses;
    private readonly IMessenger _messenger;
    private readonly L_Config _config;

    private readonly string MAIN_WINDOW = "MainWindow";

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Opened += MainWindow_Opened;
        Closed += MainWindow_Closed;

        _serviceProvider = serviceProvider;
        _language = _serviceProvider.GetRequiredService<ILang>().Language;
        _oses = _serviceProvider.GetRequiredService<IOSTools>().OSes;
        _messenger = _serviceProvider.GetRequiredService<IMessenger>();
        _config = _oses.LoadConfig();

        _messenger.RegisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public MainWindow()        // 此构造函数仅用于保证窗体浏览正常
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        InitializeComponent();
    }
#endif

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        OsesSetHandle();

        _oses.RegisterWndProcHookCallback(this);

        _messenger.Send(new MB_CheckVersion());
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _oses.UnregisterAllHotkeys();
    }

    /// <summary>
    /// OSTools绑定窗体句柄
    /// </summary>
    private void OsesSetHandle()
    {
        var platformHandle = TryGetPlatformHandle();
        if (platformHandle != null)
        {
            _oses.SetMainWindowHandle(platformHandle.Handle);
        }
    }

    /// <summary>
    /// 注册所有的热键
    /// </summary>
    private bool RegisterAllHotkeys()
    {
        var final = true;

        foreach (var hotkeySetting in _config.Hotkeys)
        {
            var result = _oses.RegisterHotkey(
                hotkeySetting.Key,
                hotkeySetting.Modifiers,
                () => _messenger.Send(new MB_Hotkey(hotkeySetting.Hotkey)));

            if (!result)
            {
                _messenger.Send(new MB_ToastPureText()
                {
                    Message = string.Format(_language.TACTIC_PLAYING_HOTKEY_ALREADY_EXSITS,
                        hotkeySetting.Modifiers,
                        hotkeySetting.Key),
                    Title = _language.TOAST_TITLE_ERROR,
                    Type = MB_Enum_ToastType.Error
                });
            }

            final &= result;
        }

        return final;
    }

    #region MessageBus 消息处理

    /// <summary>
    /// 枚举转换
    /// </summary>
    private static NotificationType NotificationTypeConvert(MB_Enum_ToastType type)
    {
        NotificationType _type = type switch
        {
            MB_Enum_ToastType.Info => NotificationType.Information,
            MB_Enum_ToastType.Success => NotificationType.Success,
            MB_Enum_ToastType.Warn => NotificationType.Warning,
            MB_Enum_ToastType.Error => NotificationType.Error,
            _ => NotificationType.Information
        };

        return _type;
    }

    public void Receive(MB_ToastPureText msg)
    {
        Dispatcher.UIThread.InvokeAsync(new Action(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(15))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .Queue();
        }));
    }

    public void Receive(MB_ToastVersion msg)
    {
        Dispatcher.UIThread.InvokeAsync(new Action(() =>
        {
            ToastHost.Manager.CreateToast()
                .OfType(NotificationTypeConvert(msg.Type))
                .Dismiss().After(TimeSpan.FromSeconds(15))
                .Dismiss().ByClicking()
                .WithTitle(msg.Title)
                .WithContent(msg.Message)
                .WithActionButton(_language.BUTTON_TXT_SUBMIT, _ =>
                {
                    if (!string.IsNullOrEmpty(msg.Release_Url))
                    {
                        _oses.OpenUrl(msg.Release_Url);
                    }
                }, true)
                .Queue();
        }));
    }

    public void Receive(MB_OpenTacticPlayWindow message)
    {
        if (!RegisterAllHotkeys()) return;

        var tacPlayWindow = _serviceProvider.GetRequiredService<TacticPlayWindow>();
        tacPlayWindow.DataContext = _serviceProvider.GetRequiredService<TacticPlayWindowModel>();

        WindowState = WindowState.Minimized;

        tacPlayWindow.ShowDialog(this);
    }

    public void Receive(MB_WindowStatus message)
    {
        switch (message.WindowStatus)
        {
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Normal:
                WindowState = WindowState.Normal;
                break;
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Minimized:
                WindowState = WindowState.Minimized;
                break;
            case MB_WindowStatus.MB_ENUM_WINDOW_STATUS.Maximized:
                WindowState = WindowState.Maximized;
                break;
        }
    }

    public void Receive(MB_FileDialog message)
    {
        if (!string.IsNullOrEmpty(message.FilePath)) return;

        if (message.WindowName != MAIN_WINDOW) return;

        if (message.IsOpenMode)
        {
            OpenFileDialog(message);
        }
        else
        {
            SaveFileDialog(message);
        }
    }

    public void Receive(MB_WindowTitle message)
    {
        if (message.WindowName != MAIN_WINDOW) return;

        Title = message.Title;
    }

    public void Receive(MB_FolderDialog message)
    {
        if (message.WindowName != MAIN_WINDOW) return;
        if (!string.IsNullOrEmpty(message.FolderPath)) return;

        var folder = StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false
        }).Result;

        if (folder.Count == 0) return;

        _messenger.Send(new MB_FolderDialog()
        {
            WindowName = MAIN_WINDOW,
            Trigger = message.Trigger,
            FolderPath = folder.First().TryGetLocalPath()
        });
    }
    #endregion

    /// <summary>
    /// 打开文件选择弹窗
    /// </summary>
    private void OpenFileDialog(MB_FileDialog message)
    {
        if (string.IsNullOrEmpty(message.FileFilter) || string.IsNullOrEmpty(message.FileFilterName)) return;

        var storageFiles = StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType(message.FileFilterName) { Patterns = [message.FileFilter] }]
        }).Result;

        if (storageFiles.Count == 0) return;

        _messenger.Send(new MB_FileDialog()
        {
            WindowName = MAIN_WINDOW,
            FilePath = storageFiles.First().TryGetLocalPath(),
            IsOpenMode = true,
            Trigger = message.Trigger
        });
    }

    /// <summary>
    /// 打开文件保存弹窗
    /// </summary>
    private void SaveFileDialog(MB_FileDialog message)
    {
        if (string.IsNullOrEmpty(message.FileFilter) || string.IsNullOrEmpty(message.FileFilterName)) return;

        IStorageFolder? suggestFoldr = null;
        if (message.SuggestStartLocation != null)
        {
            suggestFoldr = StorageProvider
                .TryGetFolderFromPathAsync(message.SuggestStartLocation)
                .Result;
        }

        var storageFile = StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            FileTypeChoices = [new FilePickerFileType(message.FileFilterName) { Patterns = [message.FileFilter] }],
            ShowOverwritePrompt = true,
            SuggestedStartLocation = suggestFoldr
        }).Result;

        if (storageFile == null) return;

        _messenger.Send(new MB_FileDialog()
        {
            WindowName = MAIN_WINDOW,
            FilePath = storageFile.TryGetLocalPath(),
            IsOpenMode = false,
            Trigger = message.Trigger
        });
    }
}
