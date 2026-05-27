using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DB.Service;
using GeneralElementsUI.Entities;
using GenericLibraryUtil;
using WpfApp.Pages.MainWindow;
using User = DB.Entities.User;

namespace WpfApp.Window;

public partial class MainWindow : INotifyPropertyChanged
{
    private User _user = new();
    private MainPageViewModel _selectedPage;

    private UsersService _usersService = new();

    public ObservableCollection<MainPageViewModel> Pages { get; } = new();

    public MainPageViewModel SelectedPage
    {
        get => _selectedPage;
        set
        {
            _selectedPage = value;
            OnPropertyChanged();
        }
    }

    private void InitAuthWindow()
    {
        var loginFields = Fields.GetLoginFields();
        var registerFields = Fields.GetRegisterFields();
        
        var authWindow = new GeneralElementsUI.Views.AuthorizationWindow(registerFields, loginFields, OnAuthEnd, CheckRegister, CheckLogin, true);
        authWindow.ShowDialog();
    }

    private bool CheckLogin(GeneralElementsUI.Entities.User arg)
    {
        var convertedUser = Converter.Convert(arg);
        var foundUser = _usersService.GetUserByLoginAndPassword(convertedUser.Login, convertedUser.Password);
        
        if (foundUser == null)
        {
            MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
        
        return true;
    }

    private bool CheckRegister(GeneralElementsUI.Entities.User user)
    {
        var userAsDBUser = Converter.Convert(user);
        return TryRegisterUser(userAsDBUser);
    }

    private void OnAuthEnd(GeneralElementsUI.Entities.User user)
    {
        if (user.Role == GeneralElementsUI.Entities.User.DefaultRoles.NonAuthorized)
        {
            _user = _usersService.GetUserByLoginAndPassword("", "", true);
        }
        else
        {
            var convertedUser = Converter.Convert(user);

            _user = _usersService.GetUserByLoginAndPassword(convertedUser.Login, convertedUser.Password);
        }
    }

    public MainWindow()
    {
        InitAuthWindow();
        
        InitializeComponent();
        DataContext = this;
        
        if(_user.Role == User.UserRole.Admin)
            Pages.Add(new MainPageViewModel(new AdminPage(), "Пользователи"));
        
        if(_user.Role is User.UserRole.Client or User.UserRole.Guest)
            Pages.Add(new MainPageViewModel(new InspectPhrase(_user), "Проверить текст"));

        if (_user.Role is User.UserRole.Client)
            Pages.Add(new MainPageViewModel(new NotesHistoryPage(_user), "История записей"));
        
        if(_user.Role is User.UserRole.RKNEmployee)
            Pages.Add(new MainPageViewModel(new RKNEmployeePage(_user), "Для РКН работников"));

        if (_user.Role == User.UserRole.Mizulina)
            Pages.Add(new MainPageViewModel(new MizulinaPage(_user), "Мизулина"));
        
        if(Pages.Count > 0)
            SelectedPage = Pages[0];
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
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class MainPageViewModel
{
    public Page TabPage { get; set; }
    public string Header { get; set; }

    public MainPageViewModel(Page page, string header)
    {
        TabPage = page;
        Header = header;
    }
}