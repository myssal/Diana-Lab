
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DianaLab.Core.Model;
using DianaLab.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;

namespace DianaLab.GUI.ViewModels
{
    public class ExtractViewModel : BaseViewModel
    {
        private Config m_Config;

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

        private string m_BundlesLocationError;
        public string BundlesLocationError
        {
            get => m_BundlesLocationError;
            set
            {
                m_BundlesLocationError = value;
                OnPropertyChanged();
            }
        }

        private string m_OutputLocationError;
        public string OutputLocationError
        {
            get => m_OutputLocationError;
            set
            {
                m_OutputLocationError = value;
                OnPropertyChanged();
            }
        }

        private string m_TempLocationError;
        public string TempLocationError
        {
            get => m_TempLocationError;
            set
            {
                m_TempLocationError = value;
                OnPropertyChanged();
            }
        }

        private string m_CliLocationError;
        public string CliLocationError
        {
            get => m_CliLocationError;
            set
            {
                m_CliLocationError = value;
                OnPropertyChanged();
            }
        }

        private string m_EndDateError;
        public string EndDateError
        {
            get => m_EndDateError;
            set
            {
                m_EndDateError = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }

        public ExtractViewModel()
        {
            FilterTypes = new ObservableCollection<string> { "Texture2D", "TextAsset", "Both" };
            SelectedFilterType = "Texture2D";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            StartCommand = new RelayCommand(Start);
            Preload();
        }

        private void Preload()
        {
            m_Config = AssetService.GetConfigureData(new NullLogger<ExtractViewModel>());
            if (m_Config != null)
            {
                BundlesLocation = m_Config.Input;
                TempLocation = m_Config.Temp;
                OutputLocation = m_Config.Output;
                if (DateTime.TryParse(m_Config.StartDate, out var startDate))
                {
                    StartDate = startDate;
                }
                if (DateTime.TryParse(m_Config.EndDate, out var endDate))
                {
                    EndDate = endDate;
                }
                CliLocation = m_Config.AssetStudio;
                UnityVersion = m_Config.UnityVersion;
                CopyBundles = m_Config.IsCopyToTemp;

                if (m_Config.Types != null)
                {
                    bool hasTexture2D = m_Config.Types.Contains("Texture2D");
                    bool hasTextAsset = m_Config.Types.Contains("TextAsset");

                    if (hasTexture2D && hasTextAsset)
                    {
                        SelectedFilterType = "Both";
                    }
                    else if (hasTexture2D)
                    {
                        SelectedFilterType = "Texture2D";
                    }
                    else if (hasTextAsset)
                    {
                        SelectedFilterType = "TextAsset";
                    }
                }
                
                // Job section
                ExtractAsset = m_Config.ExtractAsset;
                DeleteRedundant = m_Config.DeleteRedundant;
                RenameSpine = m_Config.RenameSpine;
                SortAsset = m_Config.SortAsset;
                SortSpine = m_Config.SortSpine;
                OrganizeSpine = m_Config.OrganizeSpine;
                ResizeSpineTextures = m_Config.ResizeSpineTextures;
                SortAtlas = m_Config.SortAtlas;
                NormalizeCostumeName = m_Config.NormalizeCostumeName;
                
            }
        }

        private void Start(object obj)
        {
            if (InputValiate())
            {
                
            }
        }

        public bool InputValiate()
        {
            BundlesLocationError = null;
            OutputLocationError = null;
            TempLocationError = null;
            CliLocationError = null;
            EndDateError = null;

            if (string.IsNullOrEmpty(BundlesLocation))
            {
                BundlesLocationError = "Bundles location cannot be empty.";
            }
            else if (!Directory.Exists(BundlesLocation))
            {
                BundlesLocationError = "Folder does not exist.";
            }

            if (string.IsNullOrEmpty(OutputLocation))
            {
                OutputLocationError = "Output location cannot be empty.";
            }
            else if (!Directory.Exists(OutputLocation))
            {
                OutputLocationError = "Folder does not exist.";
            }

            if (string.IsNullOrEmpty(CliLocation))
            {
                CliLocationError = "CLI location cannot be empty.";
            }
            else if (!File.Exists(CliLocation))
            {
                CliLocationError = "File does not exist.";
            }

            if (StartDate > EndDate)
            {
                EndDateError = "End date cannot be earlier than start date.";
            }

            if (CopyBundles)
            {
                if (string.IsNullOrEmpty(TempLocation))
                {
                    TempLocationError = "Temp location cannot be empty.";
                }
                else if (!Directory.Exists(TempLocation))
                {
                    TempLocationError = "Folder does not exist.";
                }
            }

            return string.IsNullOrEmpty(BundlesLocationError) &&
                   string.IsNullOrEmpty(OutputLocationError) &&
                   string.IsNullOrEmpty(TempLocationError) &&
                   string.IsNullOrEmpty(CliLocationError) &&
                   string.IsNullOrEmpty(EndDateError);
        }
    }
}
