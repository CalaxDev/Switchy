using System.Runtime.InteropServices;

namespace Switchy.Utils
{
    public static class User32Helper
    {
        public const int SW_RESTORE = 9;
        [DllImport("User32.dll")]
        public static extern bool IsIconic(nint handle);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(nint handle);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(nint handle, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

    }
}