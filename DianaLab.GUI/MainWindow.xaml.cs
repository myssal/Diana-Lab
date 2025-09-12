using System.Windows;
using DianaLab.GUI.ViewModels;

namespace DianaLab.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
