using System.Windows;
using DianaLab.GUI.Views;

namespace DianaLab.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set the initial view
            MainContent.Content = new ExtractView();
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ExtractView();
        }

        private void Live2DAssetsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Live2DAssetsView();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AboutView();
        }
    }
}
