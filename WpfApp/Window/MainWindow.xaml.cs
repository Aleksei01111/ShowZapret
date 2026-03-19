using System.Windows;
using DB.Entities;

namespace WpfApp.Window;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        var authorizationWindow = new AuthorizationWindow(new User());
        
        var authorizationDialogResult = new AuthorizationWindow(new User()).ShowDialog();

        if (authorizationDialogResult != true)
        {
            Close();
        }

        authorizationWindow.Close();
        
        InitializeComponent();
    }
}