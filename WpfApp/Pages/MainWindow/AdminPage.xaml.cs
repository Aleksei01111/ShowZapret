using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using DB.Service;

namespace WpfApp.Pages.MainWindow;

public partial class AdminPage : Page, INotifyPropertyChanged
{
    private UsersService _usersService = new();
    
    private User _selectedUser;

    private string _newLoginSelectedUser;
    private string _newPasswordSelectedUser;
    private string _newAddressSelectedUser;
    private int _newRoleSelectedUser;

    public string NewLoginSelectedUser
    {
        get => _newLoginSelectedUser;
        set
        {
            _newLoginSelectedUser = value;
            OnPropertyChanged();
        }
    }

    public string NewPasswordSelectedUser
    {
        get => _newPasswordSelectedUser;
        set
        {
            _newPasswordSelectedUser = value;
            OnPropertyChanged();
        }
    }

    public string NewAddressSelectedUser
    {
        get => _newAddressSelectedUser;
        set
        {
            _newAddressSelectedUser = value;
            OnPropertyChanged();
        }
    }

    public int NewRoleSelectedUser
    {
        get => _newRoleSelectedUser;
        set
        {
            _newRoleSelectedUser = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<User> Users { get; }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();

            if (value == null)
                return;
            
            NewLoginSelectedUser = _selectedUser.Login;
            NewPasswordSelectedUser = _selectedUser.Password;
            NewAddressSelectedUser = _selectedUser.Address;
            NewRoleSelectedUser = (int)_selectedUser.Role;
        }
    }
    
    public AdminPage()
    {
        DataContext = this;

        Users = new ObservableCollection<User>(_usersService.GetUsers());
        
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SaveUser_OnClick(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Вы уверены?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.No)
            return;
        
        var userData = new User
        {
            Login = NewLoginSelectedUser,
            Password = NewPasswordSelectedUser,
            Address = NewAddressSelectedUser,
            Role = (User.UserRole)NewRoleSelectedUser
        };

        try
        {
            _usersService.SaveUser(SelectedUser, userData);
            MessageBox.Show("Сохранено");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
    {
        if (Users.Count == 1)
        {
            MessageBox.Show("Учетных записей слишком мало для удаления");
            return;
        }
        
        if (MessageBox.Show("Вы уверены?", "Вы уверены", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;

        try
        {
            _usersService.DeleteUser(SelectedUser);
            MessageBox.Show("Удалено");
            Users.Remove(SelectedUser);
            SelectedUser = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}