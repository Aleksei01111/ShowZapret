using System.Windows;
using System.Windows.Controls;

namespace GeneralElementsZapretUI.UserControls;

public partial class CheckBoxWithPlaceholder : UserControl
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(CheckBoxWithPlaceholder));
    
    public static readonly DependencyProperty IsCheckedProperty =
        DependencyProperty.Register(
            nameof(IsChecked),
            typeof(bool?),
            typeof(CheckBoxWithPlaceholder),
            new FrameworkPropertyMetadata(
                false, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool? IsChecked
    {
        get => (bool?)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public CheckBoxWithPlaceholder()
    {
        InitializeComponent();
    }
}