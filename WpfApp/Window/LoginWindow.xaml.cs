using System.ComponentModel;
using System.Windows;

namespace WpfApp.Window;

public partial class LoginWindow : System.Windows.Window
{
    private DB.Entities.User _user;
    
    public LoginWindow(DB.Entities.User user)
    {
        _user = user;
        
        InitializeComponent();
    }

    private void LoginWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        DialogResult = false;
    }

    private void Register_OnClick(object sender, RoutedEventArgs e)
    {
        new RegisterWindow(_user).ShowDialog();
    }
}