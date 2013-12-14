using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Lab1.Core;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map _map;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            FindPathButton.Click += FindPathButtonOnClick;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _map = new Map(DrawCanvas);
        }

        private void DrawAreaButtonOnClick(object sender, RoutedEventArgs e)
        {
            _map.Init(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text));
        }

        private async void FindPathButtonOnClick(object sender, RoutedEventArgs e)
        {
            Title = "TotalCost = ?";
            _map.IsDiagonalMoveEnabled = DiagonalMoveCheckBox.IsChecked.HasValue && DiagonalMoveCheckBox.IsChecked.Value;
            _map.DelayTime = int.Parse(DelayTextBox.Text);
            var heurstic = (Map.HeuristicEnum)HeuristiComboBox.SelectedIndex;
            await _map.FindPath(heurstic);
            Title = "TotalCost = " + _map.TotalWayCost;
        }
    }
}
