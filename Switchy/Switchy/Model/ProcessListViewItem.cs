using System.Diagnostics;

namespace Switchy.Model
{
    public class ProcessListViewItem
    {
        public Process Process { get; }
        public bool IsVisible { get; set; }

        public ProcessListViewItem(Process p)
        {
            Process = p;
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
    }
}
