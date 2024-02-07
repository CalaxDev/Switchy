using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Switchy.Model
{
    public class ProcessListViewItem : INotifyPropertyChanged
    {
        public Process Process { get; }
        public bool HasExited { get; internal set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ProcessListViewItem(Process p)
        {
            Process = p;
            p.EnableRaisingEvents = true;
            p.Exited += P_Exited;
        }

        private void P_Exited(object? sender, EventArgs e)
        {
            this.HasExited = true;
            NotifyPropertyChanged(nameof(Process.HasExited));
        }

        public override string ToString()
        {
            return Process?.ProcessName ?? "No Process";
        }

        public override bool Equals(object? obj)
        {
            if (obj is ProcessListViewItem it)
                return Process.Id.Equals(it.Process.Id);

            return false;
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
