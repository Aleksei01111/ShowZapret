using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Pages;

public partial class LoginPage : Page, INotifyPropertyChanged
{
    private DB.Service.UsersService _usersService;
    private Action<DB.Entities.User> _onLoginDone;
    
    private DB.Entities.User _user;

    private string _login;
    private string _password;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }
    
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }
    
    public LoginPage(Action<DB.Entities.User> onLoginDone, DB.Service.UsersService usersService)
    {
        _usersService = usersService;
        _onLoginDone = onLoginDone;
        
        InitializeComponent();

        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Login_OnClick(object sender, RoutedEventArgs e)
    {
        var foundUser = _usersService.GetUserByLoginAndPassword(Login, Password);
        
        if (foundUser == null)
        {
            MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        _onLoginDone(foundUser);
    }
}