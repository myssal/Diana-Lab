using System.Windows.Controls;
using System.Windows;

namespace DianaLab.GUI.Views
{
    public partial class Live2DAssetsView : UserControl
    {
        public Live2DAssetsView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            L2DMetadataEditorWindow editorWindow = new L2DMetadataEditorWindow();
            editorWindow.ShowDialog(); // Use ShowDialog to make it a modal window
        }
    }
}
