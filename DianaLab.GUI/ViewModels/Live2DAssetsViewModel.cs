using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using DianaLab.Core.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace DianaLab.GUI.ViewModels
{
    public class Live2DAssetsViewModel : BaseViewModel
    {
        private readonly ObservableCollection<L2DInfo> _allL2DAssets;
        private ObservableCollection<L2DInfo> _l2dAssets;
        public ObservableCollection<L2DInfo> L2DAssets
        {
            get => _l2dAssets;
            set
            {
                _l2dAssets = value;
                OnPropertyChanged();
            }
        }

        private bool _shouldUpdateSuggestions = true;
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    if (_shouldUpdateSuggestions)
                    {
                        UpdateSuggestions();
                    }
                }
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand SelectSuggestionCommand { get; }

        public ObservableCollection<string> SearchSuggestions { get; } = new();

        private bool _isSuggestionsOpen;
        public bool IsSuggestionsOpen
        {
            get => _isSuggestionsOpen;
            set
            {
                _isSuggestionsOpen = value;
                OnPropertyChanged();
            }
        }

        public Live2DAssetsViewModel()
        {
            _allL2DAssets = new ObservableCollection<L2DInfo>();
            _l2dAssets = new ObservableCollection<L2DInfo>();
            SearchCommand = new RelayCommand(PerformSearch);
            SelectSuggestionCommand = new RelayCommand(SelectSuggestion);
            LoadAssets();
        }

        private void LoadAssets()
        {
            string characterJsonPath = @"F:\FullSetC\Game\Active\BrownDust\BrownDust2\CharInfo.json";
            if (File.Exists(characterJsonPath))
            {
                var characters = CharacterService.LoadCharacters(characterJsonPath);
                var l2dAssets = L2DManager.GetL2DAssets(characters);

                foreach (var asset in l2dAssets)
                {
                    _allL2DAssets.Add(asset);
                }

                PerformSearch(null);
            }
        }

        private void PerformSearch(object parameter)
        { 
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                L2DAssets = new ObservableCollection<L2DInfo>(_allL2DAssets);
            }
            else
            {
                var searchTextLower = SearchText.ToLower();
                var filteredAssets = _allL2DAssets.Where(asset =>
                    (asset.id?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.name?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.l2d?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.l2dTags?.Any(tag => tag.ToString().ToLower().Contains(searchTextLower)) ?? false)
                );
                L2DAssets = new ObservableCollection<L2DInfo>(filteredAssets);
            }
        }

        private void UpdateSuggestions()
        {
            SearchSuggestions.Clear();
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IsSuggestionsOpen = false;
                return;
            }

            var searchTextLower = SearchText.ToLower();
            var suggestions = _allL2DAssets
                .Where(asset =>
                    (asset.id?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.name?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.l2d?.ToLower().Contains(searchTextLower) ?? false) ||
                    (asset.l2dTags?.Any(tag => tag.ToString().ToLower().Contains(searchTextLower)) ?? false)
                )
                .Select(asset => asset.name)
                .Distinct()
                .Take(10)
                .ToList();

            foreach (var suggestion in suggestions)
            {
                SearchSuggestions.Add(suggestion);
            }

            IsSuggestionsOpen = SearchSuggestions.Any();
        }

        private void SelectSuggestion(object parameter)
        {
            if (parameter is string suggestion)
            {
                _shouldUpdateSuggestions = false;
                SearchText = suggestion;
                _shouldUpdateSuggestions = true;

                IsSuggestionsOpen = false;
                PerformSearch(null);
            }
        }
    }
}