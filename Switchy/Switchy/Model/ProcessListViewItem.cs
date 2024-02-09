using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Switchy.Model;

public partial class ProcessListViewItem(Process p) : ObservableObject
{
    private Process _process = p;
    [ObservableProperty]
    private bool _isExited;
    public Process Process
    {
        get
        {
            return _process;
        }
        private set
        {
            _process = value;

            try //Needed to prevent application crash when executing switchy without debug mode or admin rights; Effectively disables process exit listview binding however.
            {
                _process.EnableRaisingEvents = true;
                _process.Exited += P_Exited;
            }
            catch { }
        }
    }

    private void P_Exited(object? sender, EventArgs e)
        => IsExited = true;

    public override string ToString()
        => Process?.ProcessName ?? "No Process";

    public override bool Equals(object? obj)
        => obj is ProcessListViewItem it && Process.Id.Equals(it.Process.Id);

    public override int GetHashCode()
        => HashCode.Combine(Process.Id);
}
