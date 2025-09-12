
using System.Windows.Input;

namespace DianaLab.GUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExtractViewCommand { get; set; }
        public ICommand Live2DAssetsViewCommand { get; set; }
        public ICommand SettingsViewCommand { get; set; }
        public ICommand AboutViewCommand { get; set; }

        public MainViewModel()
        {
            _currentView = new ExtractViewModel();
            ExtractViewCommand = new RelayCommand(o => CurrentView = new ExtractViewModel());
            Live2DAssetsViewCommand = new RelayCommand(o => CurrentView = new Live2DAssetsViewModel());
            SettingsViewCommand = new RelayCommand(o => CurrentView = new SettingsViewModel());
            AboutViewCommand = new RelayCommand(o => CurrentView = new AboutViewModel());
        }
    }
}
