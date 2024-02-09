using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Switchy.Model;
using Switchy.Utils;

namespace Switchy.ViewModel;

public partial class ProcessManager : ObservableObject
{
    private WindowHelper? _windowHelper;

    [ObservableProperty]
    private string _searchText = string.Empty;
    [ObservableProperty]
    private string _timerInSecondsText = "60";
    [ObservableProperty]
    private WindowSwitcherType _switcherType = WindowSwitcherType.Normal;

    public ObservableCollection<ProcessListViewItem> AvailableProcesses { get; } = [];
    public ObservableCollection<ProcessListViewItem> FilteredAvailableProcesses { get; } = [];
    public ObservableCollection<ProcessListViewItem> SelectedProcesses { get; } = [];

    public ProcessManager()
    {
        UpdateAvailableProcessList();
    }

    [RelayCommand]
    private void AddSelectedItem(ProcessListViewItem? item)
    {
        if (item == null)
            return;

        if (item.IsExited)
        {
            MessageBox.Show($"Processes has \"{item.Process.ProcessName}\" already exited! Please refresh the list and try again!");
            return;
        }

        if (!SelectedProcesses.Contains(item))
        {
            SelectedProcesses.Add(item);
            AvailableProcesses.Remove(item);
        }

        FilterAvailableProcessList();
    }

    [RelayCommand]
    private void RemoveSelectedItem(ProcessListViewItem? item)
    {
        if (item == null)
            return;

        SelectedProcesses.Remove(item);
        AvailableProcesses.Add(item);

        FilterAvailableProcessList();
    }

    [RelayCommand]
    public void MoveItemUp(ProcessListViewItem? item)
    {
        if (item == null)
            return;

        var curIndex = SelectedProcesses.IndexOf(item);
        if (curIndex > 0)
            SelectedProcesses.Move(curIndex, curIndex - 1);
    }

    [RelayCommand]
    public void MoveItemDown(ProcessListViewItem? item)
    {
        if (item == null)
            return;

        var curIndex = SelectedProcesses.IndexOf(item);
        if (curIndex < SelectedProcesses.Count - 1)
            SelectedProcesses.Move(curIndex, curIndex + 1);
    }

    [RelayCommand]
    public void UpdateAvailableProcessList()
    {
        AvailableProcesses.Clear();

        Process.GetProcesses()
            .Where(p => p.MainWindowHandle != 0)
            .Where(x => !SelectedProcesses.Any(y => y.Process.Id == x.Id))
            .Select(c => new ProcessListViewItem(c))
            .ToList()
            .ForEach(AvailableProcesses.Add);

        FilterAvailableProcessList();
    }

    [RelayCommand]
    public void Start()
    {
        if (_windowHelper != null)
        {
            MessageBox.Show("Window switching is already in progress! Please stop before starting again.");
            return;
        }

        if (!double.TryParse(TimerInSecondsText, out double timerInSeconds))
        {
            MessageBox.Show("You need to enter a value for the window switching time in seconds!");
            return;
        }

        _windowHelper = new WindowHelper(SelectedProcesses, timerInSeconds, SwitcherType);

        if (_windowHelper.Start() == -1)
            Stop();
    }

    [RelayCommand]
    public void Stop()
    {
        _windowHelper?.Dispose();
        _windowHelper = null;
        MessageBox.Show("Switching windows stopped.");
    }

    public void FilterAvailableProcessList(string? searchText = null)
    {
        searchText ??= SearchText;

        var filteredProcesses = AvailableProcesses.Where(i => i.Process.ProcessName.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();

        var itemsToRemove = FilteredAvailableProcesses.Except(filteredProcesses).ToList();
        var itemsToAdd = filteredProcesses.Except(FilteredAvailableProcesses).ToList();

        foreach (var item in itemsToRemove)
            FilteredAvailableProcesses.Remove(item);

        foreach (var item in itemsToAdd)
            FilteredAvailableProcesses.Add(item);
    }
}
