using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace Switchy
{
    public class WindowHelper
    {
        private readonly Timer _myTimer = new();
        private LinkedListNode<Process>? _current = null;
        private readonly LinkedList<Process> _selectedItems;
        private readonly EventHandler _displayNextWindowHandler;

        public WindowHelper(LinkedList<Process> selectedItems)
        {
            _displayNextWindowHandler = new(DisplayNextWindow);
            _selectedItems = selectedItems;
        }

        private void BringProcessToFront(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (User32Helper.IsIconic(handle))
            {
                User32Helper.ShowWindow(handle, User32Helper.SW_RESTORE);
            }

            User32Helper.SetForegroundWindow(handle);
        }

        private void DisplayNextWindow(object? obj, EventArgs myEventArgs)
        {
            if (_current == null)
            {
                MessageBox.Show("Something went wrong. Please select processes again or restart the program!");
                Stop();
                return;
            }

            BringProcessToFront(Process.GetProcessById(Convert.ToInt32(_current.Value.Id)));

            if (_current.Next == null)
                _current = _selectedItems.EnumerateNodes().Where(x => !x.Value.HasExited).First();
            else
                _current = _current.Next;
        }

        public int Start(int timer)
        {
            EnsureTimerStopped();

            _current = EnsureItemsSelected();
            if (_current == null)
                return -1;

            var res = MessageBox.Show($"Switching between selected windows every {timer} seconds. Press OK to start!", "Confirm", MessageBoxButtons.OKCancel);
            if (res == DialogResult.Cancel)
            {
                MessageBox.Show("Timer canceled!");
                return -1;
            }

            _myTimer.Tick += _displayNextWindowHandler;
            _myTimer.Interval = timer * 1000;
            _myTimer.Start();
            DisplayNextWindow(null!, null!);
            return 0;
        }
        public void Stop()
        {
            _myTimer.Stop();
            _myTimer.Tick -= _displayNextWindowHandler;
        }
        private void EnsureTimerStopped()
        {
            if (_myTimer.Enabled)
                MessageBox.Show("Timer is already running!");
        }

        private LinkedListNode<Process>? EnsureItemsSelected()
        {
            var firstNode = _selectedItems.EnumerateNodes().Where(x => !x.Value.HasExited).FirstOrDefault();
            if (firstNode == null)
            {
                MessageBox.Show("No Items selected or all selected processes have already been closed! Please refresh the list and re-select desired processes.");
                return null;
            }
            return firstNode;
        }
    }
}
