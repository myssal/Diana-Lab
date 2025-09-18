using System.Windows;

namespace DianaLab.GUI.Views
{
    /// <summary>
    /// Interaction logic for L2DMetadataEditorWindow.xaml
    /// </summary>
    public partial class L2DMetadataEditorWindow : Window
    {
        public L2DMetadataEditorWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.L2DMetadataEditorViewModel();
        }
    }
}