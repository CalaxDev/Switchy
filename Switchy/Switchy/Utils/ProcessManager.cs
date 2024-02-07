using Switchy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Switchy.Utils
{
    public class ProcessManager
    {
        private string? _lastFilterText;

        public ObservableCollection<ProcessListViewItem> AvailableProcesses { get; } = [];
        public ObservableCollection<ProcessListViewItem> FilteredAvailableProcesses { get; } = [];
        public ObservableCollection<ProcessListViewItem> SelectedProcesses { get; } = [];
        public ProcessManager()
        {
            UpdateAvailableProcessList();
            FilteredAvailableProcesses = new ObservableCollection<ProcessListViewItem>(AvailableProcesses);
        }

        public void UpdateAvailableProcessList()
        {
            AvailableProcesses.Clear();
            Process.GetProcesses()
                .Where(p => p.MainWindowHandle != 0)
                .Where(x => !SelectedProcesses.Any(y => y.Process.Id == x.Id))
                .Select(c => new ProcessListViewItem(c)).ToList().ForEach(x => AvailableProcesses.Add(x));

            FilterAvailableProcessList(_lastFilterText);
        }

        internal void AddSelectedItems(IEnumerable<ProcessListViewItem> selectedItems)
        {
            foreach (var item in selectedItems.ToArray())
            {
                if (SelectedProcesses.IndexOf(item) == -1)
                {
                    SelectedProcesses.Add(item);
                    AvailableProcesses.Remove(item);
                }
            }
            FilterAvailableProcessList(_lastFilterText);
        }

        internal void FilterAvailableProcessList(string? searchText)
        {
            _lastFilterText = searchText;
            var tempFiltered = AvailableProcesses.Where(i => string.IsNullOrEmpty(searchText) || i.Process.ProcessName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();

            for (int i = FilteredAvailableProcesses.Count - 1; i >= 0; i--)
            {
                var item = FilteredAvailableProcesses[i];
                if (!tempFiltered.Contains(item))
                {
                    FilteredAvailableProcesses.Remove(item);
                }
            }

            foreach (var item in tempFiltered)
            {
                if (!FilteredAvailableProcesses.Contains(item))
                {
                    FilteredAvailableProcesses.Add(item);
                }
            }
        }

        internal void RemoveSelectedItems(IEnumerable<ProcessListViewItem> selectedItemsToRemove)
        {
            foreach (var item in selectedItemsToRemove.ToArray()) { 
                SelectedProcesses.Remove(item);
                AvailableProcesses.Add(item);
            }

            FilterAvailableProcessList(_lastFilterText);
        }

        internal void TryMoveItemUp(ProcessListViewItem item)
        {
            var curIndex = SelectedProcesses.IndexOf(item);
            if (curIndex == 0) return;
            SelectedProcesses.Remove(item);
            SelectedProcesses.Insert(curIndex - 1, item);
        }

        internal void TryMoveItemDown(ProcessListViewItem item)
        {
            var curIndex = SelectedProcesses.IndexOf(item);
            if (curIndex == SelectedProcesses.Count - 1) return;
            SelectedProcesses.Remove(item);
            SelectedProcesses.Insert(curIndex + 1, item);
        }
    }
}
