using System.Windows;
using System.Windows.Controls;

namespace WpfApp.UserControl;

public partial class MultilineTextWithPlaceholder
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(MultilineTextWithPlaceholder));
    
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(MultilineTextWithPlaceholder),
            new FrameworkPropertyMetadata(
                string.Empty, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
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
    
    public MultilineTextWithPlaceholder()
    {
        InitializeComponent();
    }
}