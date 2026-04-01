using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Window;

namespace WpfApp.Pages.CreateDonos;

public partial class ConfirmLoginStepPage : IStepPage, INotifyPropertyChanged
{
    private string? _userName;
    private string? _password;

    public string? UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged(nameof(UserName));
        }
    }

    public string? Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public IStepPage.OnStepDialogDoneDelegate OnStepDialogDone { get; }
    public IStepPage? NextPage { get; }
    public Page ThisPage => this;

    public ConfirmLoginStepPage(IStepPage.OnStepDialogDoneDelegate onStepDialogDone, IStepPage? nextPage)
    {
        OnStepDialogDone = onStepDialogDone;
        NextPage = nextPage;
        
        InitializeComponent();

        DataContext = this;
    }

    private void Done_OnClick(object sender, RoutedEventArgs e)
    {
        if (_userName is null || _password is null || _userName.Length == 0 || _password.Length == 0)
        {
            MessageBox.Show("НЕТ!");
            return;
        }
        OnStepDialogDone(this);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}