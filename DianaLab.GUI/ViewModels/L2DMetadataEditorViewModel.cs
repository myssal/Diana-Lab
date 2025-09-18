
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace DianaLab.GUI.ViewModels
{
    public class L2DMetadataEditorViewModel : BaseViewModel
    {
        // ComboBox properties
        public ObservableCollection<string> MetadataTypes { get; }
        private string m_SelectedMetadataType;
        public string SelectedMetadataType { get => m_SelectedMetadataType; set { m_SelectedMetadataType = value; OnPropertyChanged(); } }

        // Character properties
        private int m_CharacterId;
        public int CharacterId { get => m_CharacterId; set { m_CharacterId = value; OnPropertyChanged(); } }

        private string m_CharacterName;
        public string CharacterName { get => m_CharacterName; set { m_CharacterName = value; OnPropertyChanged(); } }

        private int m_CharacterRarity;
        public int CharacterRarity { get => m_CharacterRarity; set { m_CharacterRarity = value; OnPropertyChanged(); } }
        public ObservableCollection<int> Rarities { get; } = new ObservableCollection<int> { 4, 5 };


        // Costume properties
        private string m_CostumeCharacterSearch;
        public string CostumeCharacterSearch { get => m_CostumeCharacterSearch; set { m_CostumeCharacterSearch = value; OnPropertyChanged(); } }

        private int m_CostumeId;
        public int CostumeId { get => m_CostumeId; set { m_CostumeId = value; OnPropertyChanged(); } }

        private string m_CostumeName;
        public string CostumeName { get => m_CostumeName; set { m_CostumeName = value; OnPropertyChanged(); } }

        private string m_CostumeSpine;
        public string CostumeSpine { get => m_CostumeSpine; set { m_CostumeSpine = value; OnPropertyChanged(); } }

        private string m_CostumeCutscene;
        public string CostumeCutscene { get => m_CostumeCutscene; set { m_CostumeCutscene = value; OnPropertyChanged(); } }

        private DateTime? m_CostumeReleaseDate;
        public DateTime? CostumeReleaseDate { get => m_CostumeReleaseDate; set { m_CostumeReleaseDate = value; OnPropertyChanged(); } }

        // Special Guest properties
        private string m_GuestCharacterSearch;
        public string GuestCharacterSearch { get => m_GuestCharacterSearch; set { m_GuestCharacterSearch = value; OnPropertyChanged(); } }
        
        private DateTime? m_GuestReleaseDate;
        public DateTime? GuestReleaseDate { get => m_GuestReleaseDate; set { m_GuestReleaseDate = value; OnPropertyChanged(); } }

        private string m_GuestInteract;
        public string GuestInteract { get => m_GuestInteract; set { m_GuestInteract = value; OnPropertyChanged(); } }

        // Prestige Skin properties
        private string m_PrestigeCharacterSearch;
        public string PrestigeCharacterSearch { get => m_PrestigeCharacterSearch; set { m_PrestigeCharacterSearch = value; OnPropertyChanged(); } }

        private string m_PrestigeSkinName;
        public string PrestigeSkinName { get => m_PrestigeSkinName; set { m_PrestigeSkinName = value; OnPropertyChanged(); } }
        
        private DateTime? m_PrestigeReleaseDate;
        public DateTime? PrestigeReleaseDate { get => m_PrestigeReleaseDate; set { m_PrestigeReleaseDate = value; OnPropertyChanged(); } }

        private string m_PrestigeSpine;
        public string PrestigeSpine { get => m_PrestigeSpine; set { m_PrestigeSpine = value; OnPropertyChanged(); } }

        private string m_PrestigeInteract;
        public string PrestigeInteract { get => m_PrestigeInteract; set { m_PrestigeInteract = value; OnPropertyChanged(); } }


        // Commands
        public ICommand AddCharacterCommand { get; }
        public ICommand ResetCharacterCommand { get; }
        public ICommand AddCostumeCommand { get; }
        public ICommand ResetCostumeCommand { get; }
        public ICommand AddSpecialGuestCommand { get; }
        public ICommand ResetSpecialGuestCommand { get; }
        public ICommand AddPrestigeSkinCommand { get; }
        public ICommand ResetPrestigeSkinCommand { get; }

        public L2DMetadataEditorViewModel()
        {
            // Initialize commands
            AddCharacterCommand = new RelayCommand(AddCharacter);
            ResetCharacterCommand = new RelayCommand(ResetCharacter);
            AddCostumeCommand = new RelayCommand(AddCostume);
            ResetCostumeCommand = new RelayCommand(ResetCostume);
            AddSpecialGuestCommand = new RelayCommand(AddSpecialGuest);
            ResetSpecialGuestCommand = new RelayCommand(ResetSpecialGuest);
            AddPrestigeSkinCommand = new RelayCommand(AddPrestigeSkin);
            ResetPrestigeSkinCommand = new RelayCommand(ResetPrestigeSkin);
            
            MetadataTypes = new ObservableCollection<string> { "Character", "Costume", "Special Guest", "Prestige Skin" };
            SelectedMetadataType = "Character";
        }

        private void AddCharacter(object obj) { /* Add logic */ }
        private void ResetCharacter(object obj)
        {
            CharacterId = 0;
            CharacterName = string.Empty;
            CharacterRarity = 0;
        }

        private void AddCostume(object obj) { /* Add logic */ }
        private void ResetCostume(object obj)
        {
            CostumeCharacterSearch = string.Empty;
            CostumeId = 0;
            CostumeName = string.Empty;
            CostumeSpine = string.Empty;
            CostumeCutscene = string.Empty;
            CostumeReleaseDate = null;
        }

        private void AddSpecialGuest(object obj) { /* Add logic */ }
        private void ResetSpecialGuest(object obj)
        {
            
            GuestCharacterSearch = string.Empty;
            GuestReleaseDate = null;
            GuestInteract = string.Empty;
        }

        private void AddPrestigeSkin(object obj) { /* Add logic */ }
        private void ResetPrestigeSkin(object obj)
        {
            PrestigeCharacterSearch = string.Empty;
            PrestigeSkinName = string.Empty;
            PrestigeReleaseDate = null;
            PrestigeSpine = string.Empty;
            PrestigeInteract = string.Empty;
        }
    }
}
