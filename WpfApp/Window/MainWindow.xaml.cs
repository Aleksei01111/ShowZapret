using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DB.Entities;
using WpfApp.Pages.MainWindow;

namespace WpfApp.Window;

public partial class MainWindow : INotifyPropertyChanged
{
    private User _user = new();
    private MainPageViewModel _selectedPage;
    
    public User User => _user;

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
    
    public MainWindow()
    {
        var authorizationWindow = new AuthorizationWindow(OnAuthorizationDone);
        if (authorizationWindow.ShowDialog() != true)
            Close();
        authorizationWindow.Close();
        
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

    private void OnAuthorizationDone(User foundUser)
    {
        _user = foundUser;
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