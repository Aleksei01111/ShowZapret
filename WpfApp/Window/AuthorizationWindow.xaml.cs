using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DB.Entities;

namespace WpfApp.Window;

public partial class AuthorizationWindow : System.Windows.Window
{
    private DB.Service.UsersService _usersService;
    private DB.Entities.User _user;

    private readonly Pages.LoginPage _loginPage;
    private readonly Pages.RegistrationPage _registrationPage;

    public AuthorizationWindow(DB.Entities.User user)
    {
        _user = user;
        _usersService = new();

        InitializeComponent();

        _loginPage = new Pages.LoginPage(OnLoginDone, _usersService);
        _registrationPage = new Pages.RegistrationPage(OnRegisterDone, _usersService);

        MainFrame.Navigate(_loginPage);
    }

    private void Register_OnClick(object sender, RoutedEventArgs e)
    {
        var btn = (Button)sender;
        if (btn.Content.ToString() == "Войти")
        {
            btn.Content = "Зарегистрироваться";
            MainFrame.Navigate(_loginPage);
        }
        else if (btn.Content.ToString() == "Зарегистрироваться")
        {
            btn.Content = "Войти";
            MainFrame.Navigate(_registrationPage);
        }
    }

    private void OnLoginDone(User foundUser)
    {
        MessageBox.Show($"Вы вошли как: {foundUser.Login}");
        DialogResult = true;
    }

    private void OnRegisterDone(User user)
    {
        MessageBox.Show($"Вы зарегистрировались: {user.Login}");
        DialogResult = true;
    }
}