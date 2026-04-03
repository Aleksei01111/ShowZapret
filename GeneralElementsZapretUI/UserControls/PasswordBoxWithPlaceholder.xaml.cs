using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeneralElementsZapretUI.UserControls;

public partial class PasswordBoxWithPlaceholder : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
        nameof(Placeholder), typeof(string), typeof(PasswordBoxWithPlaceholder), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password), typeof(string), typeof(PasswordBoxWithPlaceholder), new PropertyMetadata(default(string)));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public PasswordBoxWithPlaceholder()
    {
        InitializeComponent();
    }

    private void PasswordPB_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = ((PasswordBox)sender).Password;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ShowPassword_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        PasswordText.Visibility = Visibility.Visible;
    }

    private void HidePassword_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        PasswordText.Visibility = Visibility.Hidden;
    }
}