using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Pages;

public partial class RegistrationPage : Page, INotifyPropertyChanged
{
    private readonly DB.Service.UsersService _usersService;
    private readonly Action<DB.Entities.User> _onRegisterDone;

    private string? _login;
    private string? _password;
    private string? _passwordRepeat;
    private string? _address;

    public string? Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }

    public string? Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string? PasswordRepeat
    {
        get => _passwordRepeat;
        set
        {
            _passwordRepeat = value;
            OnPropertyChanged();
        }
    }

    public string? Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged();
        }
    }

    public RegistrationPage(Action<DB.Entities.User> onRegisterDone, DB.Service.UsersService usersService)
    {
        _usersService = usersService;
        _onRegisterDone = onRegisterDone;

        InitializeComponent();

        DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void RegisterDone_OnClick(object sender, RoutedEventArgs e)
    {
        if (!CheckCorrect())
            return;

        var user = new DB.Entities.User
        {
            Login = Login!,
            Password = Password!,
            Address = Address!,
        };

        if (TryRegisterUser(user))
            _onRegisterDone(user);
    }

    private bool CheckCorrect()
    {
        if (Login == null || Password == null || PasswordRepeat == null || Address == null)
        {
            MessageBox.Show("Заполнены не все поля");
            return false;
        }
        
        if (Password != PasswordRepeat)
        {
            MessageBox.Show("Пароли не совпадают!!!!");
            return false;
        }

        return true;
    }

    private bool TryRegisterUser(DB.Entities.User user)
    {
        try
        {
            _usersService.RegisterNewUser(user);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }

        return true;
    }
}