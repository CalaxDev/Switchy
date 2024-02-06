using System.Runtime.InteropServices;

namespace Switchy
{
    public static class User32Helper
    {
        public const int SW_RESTORE = 9;
        [DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);
    }
}