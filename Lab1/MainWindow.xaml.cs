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
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _map = new Map(DrawCanvas);
        }

        private void DrawAreaButtonOnClick(object sender, RoutedEventArgs e)
        {
            _map.Init(int.Parse(WidthTextBox.Text), int.Parse(HeightTextBox.Text));
        }

        private void FindPathButtonOnClick(object sender, RoutedEventArgs e)
        {
            _map.FindPath((Map.HeuristicEnum) HeuristiComboBox.SelectedIndex);
        }
    }
}
