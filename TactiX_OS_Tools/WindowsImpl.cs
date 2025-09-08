using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

using TactiX_Exception;
using TactiX_I18N;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace TactiX_OS_Tools
{
    public class WindowsImpl : IOSes
    {
        #region 实现鼠标穿透功能的Win32API

#if OS_WINDOWS
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_LAYERED = 0x00080000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const Int32 MY_HOTKEY1 = 0x9999;
        private const Int32 MY_HOTKEY2 = 0x9998;
        private const Int32 MY_HOTKEY3 = 0x9997;
        private const Int32 MY_HOTKEY4 = 0x9996;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow(); //获得本窗体的句柄

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体
        public IntPtr han;                                         //定义变量,句柄类型
#endif

        #endregion

        private ILanguage Language => I18N.Instance.Language;
        private string AppName => "TactiX";
        public string UserDataPath { get; private set; }
        private IntPtr hwnd {  get; set; }

        /// <summary>
        /// 鼠标穿透模式，为true时启用穿透
        /// </summary>
        private bool _tr = false;

        public WindowsImpl()
        {
            var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            UserDataPath = Path.Combine(localPath, AppName);
        }

        public void SetHandle(IntPtr hwnd)
        {
            this.hwnd = hwnd;
        }

        public bool IsSingleton
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string assemblyName = assembly.GetName().Name ?? string.Empty;
                Process[] app = Process.GetProcessesByName(assemblyName);
                if (app.Length > 1)
                {
                    throw new TactiXException(TactiXErrorCodes.ERROR_MUILT_PROCESS, Language.ERROR_MUILT_PROCESS);
                }
                else
                {
                    return true;
                }
            }
        }

        public void SetMouseTransport()
        {
            /**
             * 虽然设计上来说已经通过接口进行了区分，非Windows时不会进入此处
             * 但是为了避免将来可能出现的奇怪的问题还是用预编译彻底排除代码
             **/
#if OS_WINDOWS
            // 获取当前扩展样式
            int style = GetWindowLong(hwnd, GWL_EXSTYLE);

            // 添加透明和分层样式

            if (!_tr)
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
            }
            else
            {
                SetWindowLong(hwnd, GWL_EXSTYLE, style & ~WS_EX_TRANSPARENT);
            }

            _tr = !_tr;
#endif
        }
    }
}
