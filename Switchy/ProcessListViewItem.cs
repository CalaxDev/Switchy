using System.Diagnostics;

namespace Switchy
{
    class ProcessListViewItem : ListViewItem, ICloneable
    {
        public Process? Process;

        public ProcessListViewItem(Process p) : base(new[] { p.ProcessName, p.Id.ToString() })
        {
            Process = p;
        }

        public ProcessListViewItem() : base()
        {

        }

        public override string ToString()
        {
            return Process?.ProcessName ?? "No Process";
        }
    }
}
