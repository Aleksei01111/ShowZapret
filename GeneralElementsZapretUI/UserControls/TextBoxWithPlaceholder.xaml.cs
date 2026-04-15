using System.Windows;
using System.Windows.Controls;

namespace GeneralElementsZapretUI.UserControls;

public partial class TextBoxWithPlaceholder : UserControl
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(TextBoxWithPlaceholder));
    
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(TextBoxWithPlaceholder),
            new FrameworkPropertyMetadata(
                string.Empty, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public static readonly DependencyProperty IsReadOnlyProperty = 
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),typeof(TextBoxWithPlaceholder));
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
    
    public TextBoxWithPlaceholder()
    {
        InitializeComponent();
    }
}