using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.UserControl;

public partial class TextBoxWithPlaceholder : System.Windows.Controls.UserControl, INotifyPropertyChanged
{
    private string _placeholder;
    private string _text;

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
    
    public TextBoxWithPlaceholder()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}