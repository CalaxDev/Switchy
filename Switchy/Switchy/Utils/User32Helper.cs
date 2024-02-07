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
    }
}