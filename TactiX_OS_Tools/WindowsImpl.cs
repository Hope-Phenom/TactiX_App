using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Newtonsoft.Json;
using TactiX_Exception;
using TactiX_I18N;
using TactiX_Models;

namespace TactiX_OS_Tools;

public class WindowsImpl : IOSes
{
    public WindowsImpl(ILanguage language, ITactiXExceptionFactory tactiXExceptionFactory)
    {
        Language = language;
        TactiXExceptionFactory = tactiXExceptionFactory;

        AppDataFolderPath = GetAppDataFolderPath();
        Config = LoadConfig();
    }

    private ILanguage Language { get; }
    private ITactiXExceptionFactory TactiXExceptionFactory { get; }

    private string AppName => "TactiX";
    private string ConfigName => ".config";
    public L_Config Config { get; }
    public string AppDataFolderPath { get; }

    public bool IsSingleton
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name ?? string.Empty;
            var app = Process.GetProcessesByName(assemblyName);
            if (app.Length > 1)
                throw TactiXExceptionFactory.Create(TactiXErrorCodes.ERROR_MUILT_PROCESS, Language.ERROR_MUILT_PROCESS);

            return true;
        }
    }

    public void SetMouseTransport(bool enable)
    {
        /**
         * 虽然设计上来说已经通过接口进行了区分，非Windows时不会进入此处
         * 但是为了避免将来可能出现的奇怪的问题还是用预编译彻底排除代码
         **/
#if OS_WINDOWS
        // 获取当前扩展样式
        var style = GetWindowLong(TacticPlayingWindowHwnd, GWL_EXSTYLE);

        // 添加透明和分层样式

        if (enable)
            SetWindowLong(TacticPlayingWindowHwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        else
            SetWindowLong(TacticPlayingWindowHwnd, GWL_EXSTYLE, style & ~WS_EX_TRANSPARENT);
#endif
    }

    public L_Config LoadConfig()
    {
        if (Config != null) return Config;

        L_Config _conf;
        var filePath = Path.Combine(AppDataFolderPath, ConfigName);
        if (!File.Exists(filePath))
        {
            _conf = new L_Config();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(_conf, Formatting.Indented));
        }

        var confText = File.ReadAllText(filePath);
        _conf = JsonConvert.DeserializeObject<L_Config>(confText) ?? new L_Config();

        return _conf;
    }

    public void SaveConfig()
    {
        var filePath = Path.Combine(AppDataFolderPath, ConfigName);
        File.WriteAllText(filePath, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }

    public void OpenUrl(string url)
    {
#if OS_WINDOWS
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
#endif
    }

    public void CheckOrCreateDir(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    private string GetAppDataFolderPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var folderPath = Path.Combine(localPath, AppName);

        CheckOrCreateDir(folderPath);

        return folderPath;
    }

    #region 实现鼠标穿透功能的Win32API

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int WS_EX_LAYERED = 0x00080000;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow(); //获得本窗体的句柄

    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern bool SetForegroundWindow(IntPtr hWnd); //设置此窗体为活动窗体

    private IntPtr TacticPlayingWindowHwnd { get; set; }

    public void SetTacticPlayingWindowHandle(IntPtr hwnd)
    {
        TacticPlayingWindowHwnd = hwnd;
    }

    #endregion

    #region 实现全局快捷键的Win32API

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("kernel32.dll")]
    private static extern uint GetLastError();

    // Windows API 常量
    private const int WM_HOTKEY = 0x0312;
    private const int ERROR_HOTKEY_ALREADY_REGISTERED = 1409;

    // 修饰键常量
    private const uint MOD_NONE = 0x0000;
    private const uint MOD_ALT = 0x0001;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint MOD_WIN = 0x0008;

    private int _nextHotkeyId = 1;
    private readonly Dictionary<int, HotkeyInfo> _hotkeys = new();
    private IntPtr MainWindowHwnd { get; set; }

    public void SetMainWindowHandle(IntPtr hwnd)
    {
        MainWindowHwnd = hwnd;
    }

    public bool RegisterHotkey(Key key, KeyModifiers modifiers, Action action)
    {
        if (MainWindowHwnd == IntPtr.Zero) return false;

        var id = _nextHotkeyId++;
        var winModifiers = ConvertModifiers(modifiers);
        var winKey = ConvertKey(key);

        if (winKey == 0) return false; // 不支持的键

        var success = RegisterHotKey(MainWindowHwnd, id, winModifiers, winKey);

        if (!success)
        {
            var error = GetLastError();
            if (error == ERROR_HOTKEY_ALREADY_REGISTERED) return false; // 热键已被占用
            return false; // 其他错误
        }

        _hotkeys[id] = new HotkeyInfo
        {
            Action = action,
            Key = key,
            Modifiers = modifiers
        };

        return true;
    }

    public void UnregisterAllHotkeys()
    {
        foreach (var id in _hotkeys.Keys) UnregisterHotKey(MainWindowHwnd, id);
        _hotkeys.Clear();
    }

    public bool IsHotkeyAvailable(Key key, KeyModifiers modifiers)
    {
        if (MainWindowHwnd == IntPtr.Zero) return false;

        var winModifiers = ConvertModifiers(modifiers);
        var winKey = ConvertKey(key);

        if (winKey == 0) return false; // 不支持的键

        // 使用临时ID测试注册
        var testId = -9999;
        var success = RegisterHotKey(MainWindowHwnd, testId, winModifiers, winKey);

        if (success)
        {
            UnregisterHotKey(MainWindowHwnd, testId);
            return true;
        }

        var error = GetLastError();
        return error != ERROR_HOTKEY_ALREADY_REGISTERED;
    }

    private uint ConvertModifiers(KeyModifiers modifiers)
    {
        var winModifiers = MOD_NONE;

        if (modifiers.HasFlag(KeyModifiers.Control))
            winModifiers |= MOD_CONTROL;
        if (modifiers.HasFlag(KeyModifiers.Alt))
            winModifiers |= MOD_ALT;
        if (modifiers.HasFlag(KeyModifiers.Shift))
            winModifiers |= MOD_SHIFT;
        if (modifiers.HasFlag(KeyModifiers.Meta))
            winModifiers |= MOD_WIN;

        return winModifiers;
    }

    private uint ConvertKey(Key key)
    {
        // 将 Avalonia 键值转换为 Windows 虚拟键码
        // 这里只列出常用键，可根据需要扩展
        return key switch
        {
            Key.A => 0x41,
            Key.B => 0x42,
            Key.C => 0x43,
            Key.D => 0x44,
            Key.E => 0x45,
            Key.F => 0x46,
            Key.G => 0x47,
            Key.H => 0x48,
            Key.I => 0x49,
            Key.J => 0x4A,
            Key.K => 0x4B,
            Key.L => 0x4C,
            Key.M => 0x4D,
            Key.N => 0x4E,
            Key.O => 0x4F,
            Key.P => 0x50,
            Key.Q => 0x51,
            Key.R => 0x52,
            Key.S => 0x53,
            Key.T => 0x54,
            Key.U => 0x55,
            Key.V => 0x56,
            Key.W => 0x57,
            Key.X => 0x58,
            Key.Y => 0x59,
            Key.Z => 0x5A,

            Key.D0 => 0x30,
            Key.D1 => 0x31,
            Key.D2 => 0x32,
            Key.D3 => 0x33,
            Key.D4 => 0x34,
            Key.D5 => 0x35,
            Key.D6 => 0x36,
            Key.D7 => 0x37,
            Key.D8 => 0x38,
            Key.D9 => 0x39,

            Key.F1 => 0x70,
            Key.F2 => 0x71,
            Key.F3 => 0x72,
            Key.F4 => 0x73,
            Key.F5 => 0x74,
            Key.F6 => 0x75,
            Key.F7 => 0x76,
            Key.F8 => 0x77,
            Key.F9 => 0x78,
            Key.F10 => 0x79,
            Key.F11 => 0x7A,
            Key.F12 => 0x7B,

            Key.Space => 0x20,
            Key.Enter => 0x0D,
            Key.Escape => 0x1B,
            Key.Tab => 0x09,
            Key.Back => 0x08,
            Key.Insert => 0x2D,
            Key.Delete => 0x2E,
            Key.Home => 0x24,
            Key.End => 0x23,
            Key.PageUp => 0x21,
            Key.PageDown => 0x22,

            Key.Left => 0x25,
            Key.Up => 0x26,
            Key.Right => 0x27,
            Key.Down => 0x28,

            Key.OemPlus => 0xBB,
            Key.OemMinus => 0xBD,
            Key.OemComma => 0xBC,
            Key.OemPeriod => 0xBE,

            _ => 0 // 不支持的键
        };
    }

    /// <summary>
    ///     Callback for registering a global hotkey press.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="msg">The returned message.</param>
    /// <param name="wParam">The parameters of the message.</param>
    private IntPtr HotKeyCallback(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case WM_HOTKEY:
                if (_hotkeys.TryGetValue((int)wParam, out var hotkeyInfo))
                    // 在UI线程执行操作
                    Dispatcher.UIThread.Post(() => hotkeyInfo?.Action?.Invoke());
                handled = true;
                break;
            default:
                handled = false;
                break;
        }

        return IntPtr.Zero;
    }

    public void RegisterWndProcHookCallback(TopLevel topLevel)
    {
        Win32Properties.AddWndProcHookCallback(topLevel, HotKeyCallback);
    }

    private class HotkeyInfo
    {
        public Action? Action { get; set; }
        public Key Key { get; set; }
        public KeyModifiers Modifiers { get; set; }
    }

    #endregion
}