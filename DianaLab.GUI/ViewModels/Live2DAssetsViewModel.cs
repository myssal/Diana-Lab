using System.Collections.ObjectModel;
using System.IO;
using DianaLab.Core.Services;

namespace DianaLab.GUI.ViewModels
{
    public class Live2DAssetsViewModel : BaseViewModel
    {
        public ObservableCollection<L2DInfo> L2DAssets { get; set; }

        public Live2DAssetsViewModel()
        {
            L2DAssets = new ObservableCollection<L2DInfo>();
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
                    L2DAssets.Add(asset);
                }
            }
        }
    }
}