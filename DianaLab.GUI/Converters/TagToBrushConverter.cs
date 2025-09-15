using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DianaLab.Core.Services;

namespace DianaLab.GUI.Converters
{
    public class TagToBrushConverter : IValueConverter
    {
        private readonly Dictionary<L2DTag, SolidColorBrush> _tagColors = new Dictionary<L2DTag, SolidColorBrush>
        {
            { L2DTag.Prestige_Interact, new SolidColorBrush(Colors.Gold) },
            { L2DTag.Prestige_Skin, new SolidColorBrush(Colors.Goldenrod) },
            { L2DTag.Dating, new SolidColorBrush(Colors.DeepPink) },
            { L2DTag.Prestige_Idle, new SolidColorBrush(Colors.Gold) },
            { L2DTag.Costume, new SolidColorBrush(Colors.SkyBlue) },
            { L2DTag.Costume_Idle, new SolidColorBrush(Colors.SkyBlue) },
            { L2DTag.Costume_Cutscene, new SolidColorBrush(Colors.MediumPurple) },
            { L2DTag.SpecialIllust, new SolidColorBrush(Colors.Tomato) }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is L2DTag tag && _tagColors.TryGetValue(tag, out var brush))
            {
                return brush;
            }
            return Brushes.LightGray; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
