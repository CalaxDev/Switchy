using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Switchy
{
    public enum ControlState
    {
        ENABLED,
        DISABLED
    }
    public partial class Switchy_Main : Form
    {
        private LinkedList<Process> _selectedItems = new();
        private readonly WindowHelper _windowHelper;
        private readonly TypeAssistant _assistant;

        public Switchy_Main()
        {
            InitializeComponent();
            UpdateAvailableProcessList();
            progressBar1.Maximum = 100;
            _windowHelper = new WindowHelper(_selectedItems);
            _assistant = new TypeAssistant();
            _assistant.Idled += Assistant_Idled;
        }

        private void UpdateAvailableProcessList()
        {
            var items = Process.GetProcesses()
                            .Where(p => p.MainWindowHandle != 0)
                            .Where(x => _selectedItems.EnumerateNodes().FirstOrDefault(y => y.Value.Id == x.Id) == null)
                            .Where(i => string.IsNullOrEmpty(textBox1.Text) || i.ProcessName.Contains(textBox1.Text, StringComparison.CurrentCultureIgnoreCase))
                            .Select(c => new ProcessListViewItem(c)).ToArray();

            availableProcessList.Items.Clear();
            availableProcessList.Items.AddRange(items);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            _assistant.TextChanged();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            foreach (ProcessListViewItem item in availableProcessList.SelectedItems)
            {
                var newItem = new ProcessListViewItem(item.Process!);
                selectedProcessList.Items.Add(newItem);
                _selectedItems.AddLast(newItem.Process!);
            }
            UpdateAvailableProcessList();
        }

        private void RemoveFromSelectionBtn_Click(object sender, EventArgs e)
        {
            foreach (ProcessListViewItem item in selectedProcessList.SelectedItems)
            {
                _selectedItems.Remove(_selectedItems.EnumerateNodes().Where(x => x.Value.Id == item.Process!.Id).First());
                selectedProcessList.Items.Remove(item);
            }
            UpdateAvailableProcessList();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            EnableDisableControlsDuringExecution(ControlState.DISABLED);
            if (_windowHelper.Start(Convert.ToInt32(timerInSeconds.Value)) == -1)
            {
                StopBtn_Click(null!, null!);
                return;
            }
            progressTimer.Start();
            ProgressTimer_Tick(null!, null!);
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            _windowHelper.Stop();
            progressTimer.Stop();
            progressBar1.Value = 0;
            EnableDisableControlsDuringExecution(ControlState.ENABLED);
        }

        private void EnableDisableControlsDuringExecution(ControlState state)
        {
            var controlState = state switch
            {
                ControlState.ENABLED => true,
                ControlState.DISABLED => false,
                _ => throw new Exception("Oh ooh, something went critically wrong")
            };
            startBtn.Enabled = controlState;
            selectBtn.Enabled = controlState;
            moveSelectedItemUpBtn.Enabled = controlState;
            moveSelectedItemDownBtn.Enabled = controlState;
            removeFromSelectionBtn.Enabled = controlState;
            selectedProcessList.Enabled = controlState;
            availableProcessList.Enabled = controlState;
            refreshBtn.Enabled = controlState;
            timerInSeconds.Enabled = controlState;
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            foreach (ProcessListViewItem item in selectedProcessList.Items)
                if (item.Process!.HasExited)
                    selectedProcessList.Items.Remove(item);

            UpdateAvailableProcessList();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= 100)
                progressBar1.Value = 0;

            var nv = (int)Math.Ceiling(progressBar1.Maximum / timerInSeconds.Value);
            progressBar1.Value += progressBar1.Value + nv < progressBar1.Maximum ? nv : progressBar1.Maximum - progressBar1.Value;
        }

        private void AvailableProcessList_DoubleClick(object sender, EventArgs e)
        {
            SelectBtn_Click(null!, null!);
        }

        private void SelectedProcessList_DoubleClick(object sender, EventArgs e)
        {
            RemoveFromSelectionBtn_Click(null!, null!);
        }

        private void MoveSelectedItemUpBtn_Click(object sender, EventArgs e)
        {
            foreach (ProcessListViewItem item in selectedProcessList.SelectedItems)
            {
                int currentIndex = item.Index;
                ListViewItem otherItem = selectedProcessList.Items[currentIndex];
                LinkedListNode<Process>? curNode, prevNode;
                curNode = _selectedItems.EnumerateNodes().FirstOrDefault(x => x.Value.Id == item.Process!.Id);
                if (curNode == null || (prevNode = curNode.Previous) == null) return;
                _selectedItems.Remove(curNode);
                if (currentIndex > 0)
                {
                    selectedProcessList.Items.RemoveAt(currentIndex);
                    selectedProcessList.Items.Insert(currentIndex - 1, otherItem);
                }
                _selectedItems.AddBefore(prevNode, item.Process!);
            }
        }

        private void MoveSelectedItemDownBtn_Click(object sender, EventArgs e)
        {
            for (int i = selectedProcessList.SelectedItems.Count - 1; i >= 0; i--)
            {
                var item = (ProcessListViewItem)selectedProcessList.SelectedItems[i];
                int currentIndex = item.Index;
                ListViewItem otherItem = selectedProcessList.Items[currentIndex];
                LinkedListNode<Process>? curNode, nextNode;
                curNode = _selectedItems.EnumerateNodes().Where(x => x.Value.Id == item.Process!.Id).FirstOrDefault();
                if (curNode == null || (nextNode = curNode.Next) == null) return;
                _selectedItems.Remove(curNode);
                if (currentIndex < _selectedItems.Count)
                {
                    selectedProcessList.Items.RemoveAt(currentIndex);
                    selectedProcessList.Items.Insert(currentIndex + 1, otherItem);
                }
                _selectedItems.AddAfter(nextNode, item.Process!);
            }
        }

        private void Assistant_Idled(object? sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                UpdateAvailableProcessList();
            }));
        }
    }
}
