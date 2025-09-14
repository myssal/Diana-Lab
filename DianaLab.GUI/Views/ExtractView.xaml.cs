using System.Windows.Controls;
using DianaLab.GUI.ViewModels;

namespace DianaLab.GUI.Views
{
    public partial class ExtractView : UserControl
    {
        public ExtractView()
        {
            InitializeComponent();
            DataContext = new ExtractViewModel();
        }
    }
}
