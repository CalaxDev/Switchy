using Switchy.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Switchy.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    [GeneratedRegex("[^0-9]")]
    private static partial Regex NumericRegex();

    private ProcessManager ProcessManager => (ProcessManager)DataContext;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ProcessManager();
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
        => e.Handled = NumericRegex().IsMatch(e.Text);
    private void ProcessSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
            ProcessManager.FilterAvailableProcessList(tb.Text);
    }
}