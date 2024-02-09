using Switchy.Model;
using System.Diagnostics;
using System.Windows;

namespace Switchy.Utils;

public enum WindowSwitcherType
{
    Normal,
    SwitchToThisWindow
}

public class WindowHelper(IEnumerable<ProcessListViewItem> selectedItems, double timeInSeconds, WindowSwitcherType switcherType) : IDisposable
{
    private PeriodicTimer? _myTimer;
    private ProcessListViewItem? _current = null;
    private readonly double _timeInSeconds = timeInSeconds;
    private readonly WindowSwitcherType _switchType = switcherType;
    private readonly List<ProcessListViewItem> _initalSelected = selectedItems.ToList();
    private readonly uint _ownThreadId = User32Helper.GetCurrentThreadId();

    public List<ProcessListViewItem> SelectedItems
        => _initalSelected.FindAll(x => !x.IsExited);

    private static void BringProcessToFront(Process process)
    {
        nint handle = process.MainWindowHandle;
        if (User32Helper.IsIconic(handle))
        {
            User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
        }

        User32Helper.SetForegroundWindow(handle);
    }
    private static void SwitchToThisWindow(Process process)
    {
        nint handle = process.MainWindowHandle;

        if (User32Helper.IsIconic(handle))
            User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);

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
            case WindowSwitcherType.SwitchToThisWindow:
                {
                    SwitchToThisWindow(Process.GetProcessById(Convert.ToInt32(_current.Process.Id)));
                    break;
                }
        }

        var curIndex = SelectedItems.IndexOf(_current);
        if (curIndex < SelectedItems.Count - 1)
            _current = SelectedItems[curIndex + 1];
        else
            _current = SelectedItems.First();
    }

    public int Start()
    {
        if (SelectedItems.Count == 0)
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

        _current = SelectedItems.First();

        foreach (var item in SelectedItems)
        {
            uint foregroundThreadId = User32Helper.GetWindowThreadProcessId(item.Process.MainWindowHandle, out var _);
            User32Helper.AttachThreadInput(_ownThreadId, foregroundThreadId, true);
        }

        _ = RunInBackground(TimeSpan.FromSeconds(_timeInSeconds), () => DisplayNextWindow());
        DisplayNextWindow();
        return 0;
    }

    private async Task RunInBackground(TimeSpan timeSpan, Action action)
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
            _myTimer?.Dispose();

            foreach (var item in SelectedItems)
            {
                uint foregroundThreadId = User32Helper.GetWindowThreadProcessId(item.Process.MainWindowHandle, out var _);
                User32Helper.AttachThreadInput(_ownThreadId, foregroundThreadId, false);
            }
            GC.SuppressFinalize(this);
        }
        catch
        {
            throw;
        }
    }
}
