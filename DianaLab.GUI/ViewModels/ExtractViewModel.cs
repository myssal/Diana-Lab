using System.Collections.ObjectModel;
using System.Windows.Input;
using DianaLab.Core.Services;
using DianaLab.Core.Model;
using static DianaLab.Core.Utils.Helper;

namespace DianaLab.GUI.ViewModels
{
    public class ExtractViewModel : BaseViewModel
    {
        private bool m_CopyToTempFolder;
        public bool CopyToTempFolder
        {
            get => m_CopyToTempFolder;
            set
            {
                m_CopyToTempFolder = value;
                OnPropertyChanged();
            }
        }

        private string m_TempLocation;
        public string TempLocation
        {
            get => m_TempLocation;
            set
            {
                m_TempLocation = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> m_FilterTypes;
        public ObservableCollection<string> FilterTypes
        {
            get => m_FilterTypes;
            set
            {
                m_FilterTypes = value;
                OnPropertyChanged();
            }
        }

        private string m_SelectedFilterType;
        public string SelectedFilterType
        {
            get => m_SelectedFilterType;
            set
            {
                m_SelectedFilterType = value;
                OnPropertyChanged();
            }
        }

        private string m_BundlesLocation;
        public string BundlesLocation
        {
            get => m_BundlesLocation;
            set
            {
                m_BundlesLocation = value;
                OnPropertyChanged();
            }
        }

        private string m_OutputLocation;
        public string OutputLocation
        {
            get => m_OutputLocation;
            set
            {
                m_OutputLocation = value;
                OnPropertyChanged();
            }
        }

        private string m_CliLocation;
        public string CliLocation
        {
            get => m_CliLocation;
            set
            {
                m_CliLocation = value;
                OnPropertyChanged();
            }
        }

        private string m_UnityVersion;
        public string UnityVersion
        {
            get => m_UnityVersion;
            set
            {
                m_UnityVersion = value;
                OnPropertyChanged();
            }
        }

        private bool m_CopyBundles;
        public bool CopyBundles
        {
            get => m_CopyBundles;
            set
            {
                m_CopyBundles = value;
                OnPropertyChanged();
            }
        }

        private bool m_ExtractAsset;
        public bool ExtractAsset
        {
            get => m_ExtractAsset;
            set
            {
                m_ExtractAsset = value;
                OnPropertyChanged();
            }
        }

        private bool m_DeleteRedundant;
        public bool DeleteRedundant
        {
            get => m_DeleteRedundant;
            set
            {
                m_DeleteRedundant = value;
                OnPropertyChanged();
            }
        }

        private bool m_RenameSpine;
        public bool RenameSpine
        {
            get => m_RenameSpine;
            set
            {
                m_RenameSpine = value;
                OnPropertyChanged();
            }
        }

        private bool m_SortAsset;
        public bool SortAsset
        {
            get => m_SortAsset;
            set
            {
                m_SortAsset = value;
                OnPropertyChanged();
            }
        }

        private bool m_SortSpine;
        public bool SortSpine
        {
            get => m_SortSpine;
            set
            {
                m_SortSpine = value;
                OnPropertyChanged();
            }
        }

        private bool m_OrganizeSpine;
        public bool OrganizeSpine
        {
            get => m_OrganizeSpine;
            set
            {
                m_OrganizeSpine = value;
                OnPropertyChanged();
            }
        }

        private bool m_ResizeSpineTextures;
        public bool ResizeSpineTextures
        {
            get => m_ResizeSpineTextures;
            set
            {
                m_ResizeSpineTextures = value;
                OnPropertyChanged();
            }
        }

        private bool m_SortAtlas;
        public bool SortAtlas
        {
            get => m_SortAtlas;
            set
            {
                m_SortAtlas = value;
                OnPropertyChanged();
            }
        }

        private bool m_NormalizeCostumeName;
        public bool NormalizeCostumeName
        {
            get => m_NormalizeCostumeName;
            set
            {
                m_NormalizeCostumeName = value;
                OnPropertyChanged();
            }
        }

        private DateTime m_StartDate;
        public DateTime StartDate
        {
            get => m_StartDate;
            set
            {
                m_StartDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime m_EndDate;
        public DateTime EndDate
        {
            get => m_EndDate;
            set
            {
                m_EndDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }

        public ExtractViewModel()
        {
            FilterTypes = new ObservableCollection<string> { "Texture2D", "TextAsset", "Both" };
            SelectedFilterType = "Texture2D";

            StartCommand = new RelayCommand(Start);
        }

        private void Start(object obj)
        {
            
        }

        public void ParseConfig()
        {
            Config config = new Config();
        }
    }
}
