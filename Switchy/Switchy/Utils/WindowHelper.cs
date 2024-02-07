using Switchy.Model;
using System.Diagnostics;
using System.Windows;


namespace Switchy.Utils
{
    public class WindowHelper(IEnumerable<ProcessListViewItem> selectedItems, double timeInSeconds) : IDisposable
    {
        private PeriodicTimer _myTimer;
        private ProcessListViewItem? _current = null;
        private List<ProcessListViewItem> _selectedItems = selectedItems.ToList();
        private double _timeInSeconds = timeInSeconds;

        private void BringProcessToFront(Process process)
        {
            nint handle = process.MainWindowHandle;
            if (User32Helper.IsIconic(handle))
            {
                User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
            }

            User32Helper.SetForegroundWindow(handle);
        }

        private void DisplayNextWindow()
        {
            if (_current == null) { throw new Exception("Something went wrong"); }

            BringProcessToFront(Process.GetProcessById(Convert.ToInt32(_current.Process.Id)));

            var curIndex = _selectedItems.IndexOf(_current);
            if (curIndex < _selectedItems.Count - 1)
                _current = _selectedItems[curIndex + 1];
            else
                _current = _selectedItems.First();
        }

        public void Start()
        {
            EnsureTimerStopped();

            if (!_selectedItems.Any())
                return;

            var res = MessageBox.Show($"Switching between selected windows every {_timeInSeconds} seconds. Press OK to start!", "Confirm", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.Cancel)
            {
                MessageBox.Show("Timer canceled!");
                return;
            }

            _current = _selectedItems.First();

            _ = RunInBackground(TimeSpan.FromSeconds(_timeInSeconds), () => DisplayNextWindow());
            DisplayNextWindow();
            return;
        }

        private void EnsureTimerStopped()
        {
            if (_myTimer != null)
                MessageBox.Show("Timer is already running!");
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
                _myTimer.Dispose();
            }
            catch
            {
                throw;
            }
        }
    }
}
