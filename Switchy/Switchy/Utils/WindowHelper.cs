using Switchy.Model;
using System.Diagnostics;
using System.Windows;

namespace Switchy.Utils
{
    public enum WindowSwitcherType
    {
        Normal,
        WithThreadAttach,
        SwitchToThisWindow
    }
    public class WindowHelper(IEnumerable<ProcessListViewItem> selectedItems, double timeInSeconds, WindowSwitcherType switcherType) : IDisposable
    {
        private PeriodicTimer? _myTimer;
        private ProcessListViewItem? _current = null;
        private double _timeInSeconds = timeInSeconds;
        private readonly WindowSwitcherType _switchType = switcherType;
        private List<ProcessListViewItem> _selectedItemsBackingField = selectedItems.ToList();
        private List<ProcessListViewItem> _selectedItems { get => _selectedItemsBackingField.FindAll(x => !x.HasExited); set => _selectedItemsBackingField = value; }

        private void BringProcessToFront(Process process)
        {
            nint handle = process.MainWindowHandle;
            if (User32Helper.IsIconic(handle))
            {
                User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
            }

            User32Helper.SetForegroundWindow(handle);
        }

        private void SwitchWithThreadAttach(Process process)
        {
            IntPtr foregroundWindowHandle = User32Helper.GetForegroundWindow();

            nint handle = process.MainWindowHandle;
            if (User32Helper.IsIconic(handle))
            {
                User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
            }

            uint currentThreadId = User32Helper.GetCurrentThreadId();
            uint temp;
            uint foregroundThreadId = User32Helper.GetWindowThreadProcessId(foregroundWindowHandle, out temp);
            User32Helper.AttachThreadInput(currentThreadId, foregroundThreadId, true);
            User32Helper.SetForegroundWindow(handle);
            User32Helper.AttachThreadInput(currentThreadId, foregroundThreadId, false);
        }

        private void SwitchToThisWindow(Process process)
        {
            nint handle = process.MainWindowHandle;
            if (User32Helper.IsIconic(handle))
            {
                User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
            }

            User32Helper.SwitchToThisWindow(handle, true);
        }

        private void DisplayNextWindow()
        {
            if (_current == null) { throw new Exception("Something went wrong"); }

            switch (_switchType)
            {
                case WindowSwitcherType.Normal:
                    {
                        BringProcessToFront(Process.GetProcessById(Convert.ToInt32(_current.Process.Id)));
                        break;
                    }
                case WindowSwitcherType.WithThreadAttach:
                    {
                        SwitchWithThreadAttach(Process.GetProcessById(Convert.ToInt32(_current.Process.Id)));
                        break;
                    }
                case WindowSwitcherType.SwitchToThisWindow:
                    {
                        SwitchToThisWindow(Process.GetProcessById(Convert.ToInt32(_current.Process.Id)));
                        break;
                    }
            }

            var curIndex = _selectedItems.IndexOf(_current);
            if (curIndex < _selectedItems.Count - 1)
                _current = _selectedItems[curIndex + 1];
            else
                _current = _selectedItems.First();
        }

        public int Start()
        {
            if (!_selectedItems.Any())
            {
                MessageBox.Show("You currently have no windows selected or all selected process have already exited!");
                return -1;
            }

            if (_timeInSeconds <= 0)
            {
                MessageBox.Show($"Your time in seconds might be off - it is currently set to {_timeInSeconds} seconds");
                return -1;
            }

            var res = MessageBox.Show($"Switching between selected windows every {_timeInSeconds} seconds. Press OK to start!", "Confirm", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.Cancel)
            {
                MessageBox.Show("Timer canceled!");
                return -1;
            }

            _current = _selectedItems.First();

            _ = RunInBackground(TimeSpan.FromSeconds(_timeInSeconds), () => DisplayNextWindow());
            DisplayNextWindow();
            return 0;
        }

        async Task RunInBackground(TimeSpan timeSpan, Action action)
        {
            _myTimer = new PeriodicTimer(timeSpan);
            while (await _myTimer.WaitForNextTickAsync())
            {
                action();
            }
        }

        public void Dispose()
        {
            try
            {
                if (_myTimer != null)
                    _myTimer.Dispose();
            }
            catch
            {
                throw;
            }
        }
    }
}
