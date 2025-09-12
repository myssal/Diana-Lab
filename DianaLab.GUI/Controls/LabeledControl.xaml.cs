using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PGRT.GUI.Controls;

public partial class LabeledControl : UserControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledControl));

    public static readonly DependencyProperty InputContentProperty =
        DependencyProperty.Register(nameof(InputContent), typeof(UIElement), typeof(LabeledControl));

    public static readonly DependencyProperty LabelWidthProperty =
        DependencyProperty.Register(nameof(LabelWidth), typeof(GridLength), typeof(LabeledControl),
            new PropertyMetadata(GridLength.Auto));

    // FontSize for label
    public static readonly DependencyProperty LabelFontSizeProperty =
        DependencyProperty.Register(nameof(LabelFontSize), typeof(double), typeof(LabeledControl),
            new PropertyMetadata(12.0));

    public double LabelFontSize
    {
        get => (double)GetValue(LabelFontSizeProperty);
        set => SetValue(LabelFontSizeProperty, value);
    }

    // FontFamily for label
    public static readonly DependencyProperty LabelFontFamilyProperty =
        DependencyProperty.Register(nameof(LabelFontFamily), typeof(FontFamily), typeof(LabeledControl),
            new PropertyMetadata(SystemFonts.MessageFontFamily));

    public FontFamily LabelFontFamily
    {
        get => (FontFamily)GetValue(LabelFontFamilyProperty);
        set => SetValue(LabelFontFamilyProperty, value);
    }

    // FontWeight for label
    public static readonly DependencyProperty LabelFontWeightProperty =
        DependencyProperty.Register(nameof(LabelFontWeight), typeof(FontWeight), typeof(LabeledControl),
            new PropertyMetadata(FontWeights.Normal));

    public FontWeight LabelFontWeight
    {
        get => (FontWeight)GetValue(LabelFontWeightProperty);
        set => SetValue(LabelFontWeightProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public UIElement InputContent
    {
        get => (UIElement)GetValue(InputContentProperty);
        set => SetValue(InputContentProperty, value);
    }

    public GridLength LabelWidth
    {
        get => (GridLength)GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public LabeledControl()
    {
        InitializeComponent();
    }
}
