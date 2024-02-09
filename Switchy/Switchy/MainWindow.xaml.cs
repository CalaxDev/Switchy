using Switchy.Model;
using Switchy.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text.RegularExpressions;
namespace Switchy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowHelper? _windowHelper;
        private DispatcherTimer? _textChangedTimer;
        [GeneratedRegex("[^0-9]")] private static partial Regex NumericRegex();

        public ProcessManager ProcessManager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ProcessManager = new ProcessManager();
            AvailableProcessListView.ItemsSource = ProcessManager.FilteredAvailableProcesses;
            AvailableProcessListView.SelectionMode = SelectionMode.Single;
            AvailableProcessListView.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Process.ProcessName", System.ComponentModel.ListSortDirection.Ascending));
            SelectedProcessListView.ItemsSource = ProcessManager.SelectedProcesses;
            SelectedProcessListView.SelectionMode = SelectionMode.Single;
            AlgoVersionSelector.ItemsSource = Enum.GetValues(typeof(WindowSwitcherType)).Cast<WindowSwitcherType>();
            AlgoVersionSelector.SelectedItem = WindowSwitcherType.Normal;
        }

        private void FilterAvailableProcesses(object? sender, TextChangedEventArgs e)
        {

            if (_textChangedTimer == null)
            {
                _textChangedTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100)
                };
                _textChangedTimer.Tick += new EventHandler(HandleTypingTimerTimeout);
            }

            _textChangedTimer.Stop();
            _textChangedTimer.Tag = (sender as TextBox)?.Text;
            _textChangedTimer.Start();
        }
        private void HandleTypingTimerTimeout(object? sender, EventArgs e)
        {
            if (sender is not DispatcherTimer timer)
                return;

            ProcessManager.FilterAvailableProcessList(timer.Tag as string);
            timer.Stop();
        }

        private void SelectItemsButton_Click(object? sender, RoutedEventArgs? e)
        {
            ProcessManager.AddSelectedItems(AvailableProcessListView.SelectedItems.Cast<ProcessListViewItem>());
        }

        private void AvailableProcessListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectItemsButton_Click(sender, e);
        }
        private void SelectedProcessListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveItemsFromSelection_Click(sender, e);
        }
        private void RemoveItemsFromSelection_Click(object? sender, RoutedEventArgs? e)
        {
            ProcessManager.RemoveSelectedItems(SelectedProcessListView.SelectedItems.Cast<ProcessListViewItem>());
        }

        private void RefreshProcessesButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessManager.UpdateAvailableProcessList();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_windowHelper != null)
            {
                MessageBox.Show("Window switching is already in progress! Please stop before starting again.");
                return;
            }

            if (!double.TryParse(TimerInSeconds.Text, out double parseResult))
            {
                MessageBox.Show("You need to enter a value for the window switching time in seconds!");
                return;
            }

            _windowHelper = new WindowHelper(ProcessManager.SelectedProcesses, parseResult, (WindowSwitcherType)Enum.Parse(typeof(WindowSwitcherType), AlgoVersionSelector.SelectedItem?.ToString() ?? nameof(WindowSwitcherType.Normal), true));
            if (_windowHelper.Start() == -1)
                StopButton_Click(sender, e);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _windowHelper?.Dispose();
            _windowHelper = null;
            MessageBox.Show("Switching windows stopped.");
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }
        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = NumericRegex();
            return reg.IsMatch(str);
        }

        private void MoveSelectedItemsUpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = SelectedProcessListView.SelectedItems;
            foreach (ProcessListViewItem item in selectedItems.Cast<ProcessListViewItem>().ToArray())
            {
                ProcessManager.TryMoveItemUp(item);
                SelectedProcessListView.SelectedItem = item;
            }
        }

        private void MoveSelectedItemsDownButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ProcessListViewItem item in SelectedProcessListView.SelectedItems.Cast<ProcessListViewItem>().ToArray())
            {
                ProcessManager.TryMoveItemDown(item);
                SelectedProcessListView.SelectedItem = item;
            }
        }
    }
}